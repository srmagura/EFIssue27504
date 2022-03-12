using AppInterfaces.System;
using ITI.Baseline.Util;
using ValueObjects;

namespace AppServices.System;

public class ReportSystemAppService : SystemApplicationService, IReportSystemAppService
{
    private readonly IReportQueries _queries;
    private readonly IReportRepository _repo;
    private readonly IProjectRepository _projectRepo;
    private readonly IProjectPublicationRepository _projectPublicationRepo;

    public ReportSystemAppService(
        IUnitOfWorkProvider uowp,
        ILogger logger,
        IAppAuthContext auth,
        IReportQueries queries,
        IReportRepository repo,
        IProjectRepository projectRepo,
        IProjectPublicationRepository projectPublicationRepo
    ) : base(uowp, logger, auth)
    {
        _queries = queries;
        _repo = repo;
        _projectRepo = projectRepo;
        _projectPublicationRepo = projectPublicationRepo;
    }

    public Task<ReportSystemDto?> GetAsync(ReportId id)
    {
        return QueryAsync(() => _queries.GetSystemAsync(id));
    }

    private async Task<Report> GetDomainEntityAsync(ReportId id)
    {
        var report = await _repo.GetAsync(id);
        Require.NotNull(report, "Could not find report.");

        return report;
    }

    public Task RequestAsync(ProjectId projectId, ProjectPublicationId? projectPublicationId, ReportType type)
    {
        return CommandAsync(async () =>
        {
            var project = await _projectRepo.GetAsync(projectId);
            Require.NotNull(project, "Project could not be found.");

            ProjectPublication? projectPublication = null;
            if (projectPublicationId != null)
            {
                // Event handlers must be idempotent
                if (await _queries.AnyAsync(projectPublicationId, type))
                    return;

                projectPublication = await _projectPublicationRepo.GetAsync(projectPublicationId);
                Require.NotNull(projectPublication, "Project publication could not be found.");
            }

            _repo.Add(new Report(project, projectPublication, type));
        });
    }

    public Task BeginProcessingAsync(ReportId id)
    {
        return CommandAsync(async () =>
        {
            (await GetDomainEntityAsync(id)).BeginProcessing();
        });
    }

    public Task SetPercentCompleteAsync(ReportId id, decimal percentComplete)
    {
        return CommandAsync(async () =>
        {
            (await GetDomainEntityAsync(id)).SetPercentComplete(new Percentage(percentComplete));
        });
    }

    public Task SetCompletedAsync(ReportId id, FileId fileId, string fileType)
    {
        return CommandAsync(async () =>
        {
            var report = await GetDomainEntityAsync(id);

            var project = await _projectRepo.GetAsync(report.ProjectId);
            Require.NotNull(project, "Project could not be found.");

            ProjectPublication? projectPublication = null;
            if (report.ProjectPublicationId != null)
            {
                projectPublication = await _projectPublicationRepo.GetAsync(report.ProjectPublicationId);
                Require.NotNull(projectPublication, "Project publication could not be found.");
            }

            report.SetCompleted(new FileRef(fileId, fileType), project.ShortName, projectPublication?.RevisionNumber);
        });
    }

    public Task SetCanceledAsync(ReportId id)
    {
        return CommandAsync(async () =>
        {
            (await GetDomainEntityAsync(id)).SetCanceled();
        });
    }

    public Task SetErrorAsync(ReportId id, string errorMessage)
    {
        return CommandAsync(async () =>
        {
            (await GetDomainEntityAsync(id)).SetError(errorMessage);
        });
    }
}
