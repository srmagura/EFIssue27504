namespace DataInterfaces.Queries;

public interface IProductKitReferenceQueries
{
    Task<ProductKitReferenceSummaryDto[]> ListAsync(ProjectId projectId);
}
