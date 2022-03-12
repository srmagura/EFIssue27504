using ITI.Baseline.Util;

namespace AppServices;

public class SheetTypeAppService : ApplicationService, ISheetTypeAppService
{
    private readonly IAppPermissions _perms;
    private readonly ISheetTypeQueries _queries;
    private readonly ISheetTypeRepository _repo;

    public SheetTypeAppService(
        IUnitOfWorkProvider uowp,
        ILogger logger,
        IAppAuthContext auth,
        IAppPermissions perms,
        ISheetTypeQueries queries,
        ISheetTypeRepository repo
    ) : base(uowp, logger, auth)
    {
        _perms = perms;
        _queries = queries;
        _repo = repo;
    }

    public Task<SheetTypeSummaryDto[]> ListAsync(OrganizationId organizationId)
    {
        return QueryAsync(
            async () => Authorize.Require(await _perms.CanViewSheetTypesAsync(organizationId)),
            () => _queries.ListAsync(organizationId)
        );
    }

    public Task<SheetTypeId> AddAsync(OrganizationId organizationId, string sheetNumberPrefix, string sheetNamePrefix)
    {
        return CommandAsync(
            async () => Authorize.Require(await _perms.CanManageSheetTypesAsync(organizationId)),
            () =>
            {
                var sheetType = new SheetType(organizationId, sheetNumberPrefix, sheetNamePrefix);
                _repo.Add(sheetType);

                return Task.FromResult(sheetType.Id);
            }
        );
    }

    private async Task<SheetType> GetDomainEntityAsync(SheetTypeId id)
    {
        var sheetType = await _repo.GetAsync(id);
        Require.NotNull(sheetType, "Sheet type not found.");
        Authorize.Require(await _perms.CanManageSheetTypesAsync(sheetType.OrganizationId));

        return sheetType;
    }

    public Task SetPrefixesAsync(SheetTypeId id, string sheetNumberPrefix, string sheetNamePrefix)
    {
        return CommandAsync(
            Authorize.AuthorizedBelow,
            async () =>
            {
                var sheetType = await GetDomainEntityAsync(id);
                sheetType.SetSheetNamePrefix(sheetNamePrefix);
                sheetType.SetSheetNumberPrefix(sheetNumberPrefix);
            }
        );
    }

    public Task SetActiveAsync(SheetTypeId id, bool active)
    {
        return CommandAsync(
            Authorize.AuthorizedBelow,
            async () => (await GetDomainEntityAsync(id)).SetActive(active)
        );
    }
}
