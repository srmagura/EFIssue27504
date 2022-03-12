namespace AppInterfaces;

public interface IDesignerAppService
{
    Task<DesignerDataDto[]> ListAsync(ProjectId projectId);

    Task SetAsync(PageId pageId, DesignerDataType type, string json);
}
