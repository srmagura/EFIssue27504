namespace AppInterfaces;

public interface IProjectPublicationAppService
{
    Task<ProjectPublicationSummaryDto[]> ListAsync(ProjectId projectId);

    Task PublishAsync(ProjectId projectId);
    Task SetReportsSentToCustomerAsync(ProjectPublicationId id, bool reportsSentToCustomer);
}
