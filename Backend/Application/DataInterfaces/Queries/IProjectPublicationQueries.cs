namespace DataInterfaces.Queries;

public interface IProjectPublicationQueries
{
    Task<ProjectPublicationSummaryDto?> GetAsync(ProjectPublicationId id);
    Task<ProjectPublicationSummaryDto[]> ListAsync(ProjectId projectId);
    Task<int> CountAsync(ProjectId id);
}
