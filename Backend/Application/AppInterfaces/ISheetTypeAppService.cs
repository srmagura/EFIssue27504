namespace AppInterfaces;

public interface ISheetTypeAppService
{
    Task<SheetTypeSummaryDto[]> ListAsync(OrganizationId organizationId);

    Task<SheetTypeId> AddAsync(
        OrganizationId organizationId,
        string sheetNumberPrefix,
        string sheetNamePrefix
    );

    Task SetPrefixesAsync(SheetTypeId id, string sheetNumberPrefix, string sheetNamePrefix);
    Task SetActiveAsync(SheetTypeId id, bool active);
}
