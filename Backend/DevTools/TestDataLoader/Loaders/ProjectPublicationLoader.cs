using TestDataLoader.Helpers;

namespace TestDataLoader.Loaders;

internal class ProjectPublicationLoader
{
    private readonly IProjectPublicationAppService _projectPublicationAppService;
    private readonly TestDataMonitor _monitor;
    private readonly Func<AppDataContext> _dbFactory;

    public ProjectPublicationLoader(
        IProjectPublicationAppService projectPublicationAppService,
        TestDataMonitor monitor,
        Func<AppDataContext> dbFactory
    )
    {
        _projectPublicationAppService = projectPublicationAppService;
        _monitor = monitor;
        _dbFactory = dbFactory;
    }

    public async Task AddProjectPublicationsAsync()
    {
        ProjectId projectId;
        using (var db = _dbFactory())
        {
            projectId = await LoaderQueries.GetFirstHostOrganizationProjectAsync(db);
        }

        await _projectPublicationAppService.PublishAsync(projectId);

        _monitor.WriteCompletedMessage("Added project publications.");
    }
}
