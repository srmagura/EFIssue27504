using ITI.Baseline.Util;

namespace AppServices;

public class ReportAppService : ApplicationService, IReportAppService
{
    private readonly IAppPermissions _perms;
    private readonly IReportQueries _queries;
    private readonly IReportRepository _repo;
    private readonly IProjectRepository _projectRepo;
    private readonly IFileStore _fileStore;
    private readonly IFilePathBuilder _path;

    public ReportAppService(
        IUnitOfWorkProvider uowp,
        ILogger logger,
        IAppAuthContext auth,
        IAppPermissions perms,
        IReportQueries queries,
        IReportRepository repo,
        IProjectRepository projectRepo,
        IFileStore fileStore,
        IFilePathBuilder path
    ) : base(uowp, logger, auth)
    {
        _perms = perms;
        _queries = queries;
        _repo = repo;
        _projectRepo = projectRepo;
        _fileStore = fileStore;
        _path = path;
    }

    public Task<ReportDto?> GetAsync(ReportId id)
    {
        return QueryAsync(
            Authorize.AuthorizedBelow,
            async () =>
            {
                var report = await _queries.GetAsync(id);
                if (report == null) return null;

                Authorize.Require(await _perms.CanViewReportsAsync(report.OrganizationId));

                return report;
            }
        );
    }

    public Task<string?> GetPdfAsync(ReportId id, Stream outputStream)
    {
        return QueryAsync(
            Authorize.AuthorizedBelow,
            async () =>
            {
                var report = await _queries.GetSystemAsync(id);
                if (report == null) return null;

                Authorize.Require(await _perms.CanViewReportsAsync(report.OrganizationId));

                var file = report.File;
                if (file == null) return null;

                await _fileStore.GetAsync(_path.ForReport(file.FileId), outputStream);

                return file.FileType;
            }
        );
    }

    public Task<ReportId> RequestDraftAsync(ProjectId projectId, ReportType reportType)
    {
        return CommandAsync(
            Authorize.AuthorizedBelow,
            async () =>
            {
                var project = await _projectRepo.GetAsync(projectId);
                Require.NotNull(project, "Project could not be found.");
                Authorize.Require(await _perms.CanViewReportsAsync(project.OrganizationId));

                var report = new Report(project, projectPublication: null, reportType);
                _repo.Add(report);

                return report.Id;
            }
        );
    }
}
