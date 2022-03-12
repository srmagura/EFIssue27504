using Enumerations;

namespace DataInterfaces.Repositories;

public interface IDesignerRepository
{
    Task AddOrUpdateAsync(PageId pageId, DesignerDataType type, string json);
}
