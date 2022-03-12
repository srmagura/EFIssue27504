namespace DataInterfaces.Repositories;

public interface IPageRepository
{
    void Add(Page page);
    void Add(List<Page> pages);
    Task<Page?> GetAsync(PageId id);

    Task SetIndexAsync(PageId id, bool decreaseIndex);
    Task SetIndexBeforeSetActiveAsync(PageId id, bool isActive);
    Task IncrementIndicesAsync(ProjectId projectId, int beginIndex);
}
