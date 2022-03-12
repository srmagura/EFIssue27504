using AppDTOs.Report;

namespace DataInterfaces.Queries;

public interface IProductKitQueries
{
    Task<ProductKitDto?> GetAsync(ProductKitId id);

    Task<ProductKitVersionDto?> GetVersionAsync(ProductKitVersionId id);

    Task<ProductKitSummaryDto[]> ListAsync(OrganizationId organizationId);
    Task<ProductKitDesignerDto[]> ListForDesignerAsync(ProjectId projectId);
    Task<ProductKitReportDto[]> ListForReportAsync(ProjectId projectId);
    Task<ProductKitVersionReferenceDto[]> ListForComponentAsync(ComponentId componentId);
    Task<ProductKitVersionReferenceDto[]> ListForComponentVersionAsync(ComponentVersionId componentVersionId);
    Task<string> GetNewVersionNameAsync(ProductKitId id);
}
