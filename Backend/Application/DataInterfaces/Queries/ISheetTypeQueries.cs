namespace DataInterfaces.Queries;

public interface ISheetTypeQueries
{
    Task<SheetTypeSummaryDto[]> ListAsync(OrganizationId organizationId);
}
