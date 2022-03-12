using AppInterfaces.System;
using ITI.Baseline.Util;
using ValueObjects;

namespace AppServices.System;

public class ImportSystemAppService : SystemApplicationService, IImportSystemAppService
{
    private readonly IImportQueries _queries;
    private readonly IImportRepository _repo;

    public ImportSystemAppService(
        IUnitOfWorkProvider uowp,
        ILogger logger,
        IAppAuthContext auth,
        IImportQueries queries,
        IImportRepository repo
    ) : base(uowp, logger, auth)
    {
        _queries = queries;
        _repo = repo;
    }

    public Task<ImportDto?> GetAsync(ImportId id)
    {
        return QueryAsync(() => _queries.GetAsync(id));
    }

    private async Task<Import> GetDomainEntityAsync(ImportId id)
    {
        var import = await _repo.GetAsync(id);
        Require.NotNull(import, "Could not find import.");

        return import;
    }

    public Task BeginProcessingAsync(ImportId id)
    {
        return CommandAsync(async () =>
        {
            (await GetDomainEntityAsync(id)).BeginProcessing();
        });
    }

    public Task SetPercentCompleteAsync(ImportId id, decimal percentComplete)
    {
        return CommandAsync(async () =>
        {
            (await GetDomainEntityAsync(id)).SetPercentComplete(new Percentage(percentComplete));
        });
    }

    public Task SetCompletedAsync(ImportId id)
    {
        return CommandAsync(async () =>
        {
            (await GetDomainEntityAsync(id)).SetCompleted();
        });
    }

    public Task SetErrorAsync(ImportId id, string errorMessage)
    {
        return CommandAsync(async () =>
        {
            (await GetDomainEntityAsync(id)).SetError(errorMessage);
        });
    }

    public Task SetCanceledAsync(ImportId id)
    {
        return CommandAsync(async () =>
        {
            (await GetDomainEntityAsync(id)).SetCanceled();
        });
    }
}
