using AppDTOs.Enumerations;

namespace DataInterfaces.Queries;

public interface IPageQueries
{
    Task<PageDto?> GetAsync(PageId id);

    Task<List<PageSummaryDto>> ListAsync(ProjectId id, ActiveFilter activeFilter);
}
