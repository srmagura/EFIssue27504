using ITI.Baseline.Util;
using ValueObjects;

namespace AppServices;

public class ImportAppService : ApplicationService, IImportAppService
{
    private readonly IAppPermissions _perms;
    private readonly IImportRepository _repo;
    private readonly IImportQueries _queries;
    private readonly IProjectQueries _projectQueries;
    private readonly IFileStore _fileStore;
    private readonly IFilePathBuilder _path;

    public ImportAppService(
        IUnitOfWorkProvider uowp,
        ILogger logger,
        IAppAuthContext auth,
        IAppPermissions perms,
        IImportRepository repo,
        IImportQueries queries,
        IProjectQueries projectQueries,
        IFileStore fileStore,
        IFilePathBuilder path
    ) : base(uowp, logger, auth)
    {
        _perms = perms;
        _repo = repo;
        _queries = queries;
        _projectQueries = projectQueries;
        _fileStore = fileStore;
        _path = path;
    }

    public Task<ImportDto[]> ListAsync(ProjectId projectId, int skip, int take)
    {
        return QueryAsync(
            async () => Authorize.Require(await _perms.CanViewAsync(projectId)),
            () => _queries.ListAsync(projectId, skip, take)
        );
    }

    public async Task<ImportId> ImportAsync(ProjectId projectId, string filename, Stream stream)
    {
        var file = new FileRef(new FileId(), "application/pdf");
        var filePath = _path.ForImport(file.FileId);

        try
        {
            return await CommandAsync(
                Authorize.AuthorizedBelow,
                async () =>
                {
                    var project = await _projectQueries.GetAsync(projectId);
                    Require.NotNull(project, "Could not find project.");
                    Authorize.Require(await _perms.CanManagePagesAsync(project.OrganizationId));

                    await _fileStore.PutAsync(filePath, stream);

                    var import = new Import(project.OrganizationId, projectId, filename, file);
                    _repo.Add(import);

                    return import.Id;
                }
            );
        }
        catch
        {
            if (await _fileStore.ExistsAsync(filePath))
                await _fileStore.RemoveAsync(filePath);

            throw;
        }
    }
}
