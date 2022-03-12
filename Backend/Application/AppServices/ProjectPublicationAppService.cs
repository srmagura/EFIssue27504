using ITI.Baseline.Util;

namespace AppServices;

public class ProjectPublicationAppService : ApplicationService, IProjectPublicationAppService
{
    private readonly IAppPermissions _perms;
    private readonly IAppAuthContext _auth;
    private readonly IProjectPublicationRepository _repo;
    private readonly IProjectPublicationQueries _queries;
    private readonly IProjectRepository _projectRepo;

    public ProjectPublicationAppService(
        IUnitOfWorkProvider uowp,
        ILogger logger,
        IAppAuthContext auth,
        IAppPermissions perms,
        IProjectPublicationRepository repo,
        IProjectPublicationQueries queries,
        IProjectRepository projectRepo
    ) : base(uowp, logger, auth)
    {
        _perms = perms;
        _auth = auth;
        _repo = repo;
        _queries = queries;
        _projectRepo = projectRepo;
    }

    public Task<ProjectPublicationSummaryDto[]> ListAsync(ProjectId projectId)
    {
        return QueryAsync(
            async () => Authorize.Require(await _perms.CanViewAsync(projectId)),
            () => _queries.ListAsync(projectId)
        );
    }

    public Task PublishAsync(ProjectId projectId)
    {
        return CommandAsync(
            Authorize.AuthorizedBelow,
            async () =>
            {
                Require.NotNull(_auth.UserId, "User is not authenticated.");

                var project = await _projectRepo.GetAsync(projectId);
                Require.NotNull(project, "Project not found.");
                Authorize.Require(await _perms.CanManageProjectsAsync(project.OrganizationId));

                var count = await _queries.CountAsync(projectId);

                var projectPublication = new ProjectPublication(project, _auth.UserId, count + 1, false);
                _repo.Add(projectPublication);
            }
        );
    }

    private async Task<ProjectPublication> GetDomainEntityAsync(ProjectPublicationId id)
    {
        var projectPublication = await _repo.GetAsync(id);
        Require.NotNull(projectPublication, "Project publication could not be found.");
        Authorize.Require(await _perms.CanViewProjectPublicationsAsync(projectPublication.OrganizationId));

        return projectPublication;
    }

    public Task SetReportsSentToCustomerAsync(ProjectPublicationId id, bool reportsSentToCustomer)
    {
        return CommandAsync(
            Authorize.AuthorizedBelow,
            async () => (await GetDomainEntityAsync(id)).SetReportsSentToCustomer(reportsSentToCustomer)
        );
    }
}
