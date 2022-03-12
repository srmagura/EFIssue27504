namespace AppDTOs.Designer;

public class DesignerDataDto
{
    public DesignerDataDto(PageId pageId, DesignerDataType type, string json)
    {
        PageId = pageId ?? throw new ArgumentNullException(nameof(pageId));
        Type = type;
        Json = json ?? throw new ArgumentNullException(nameof(json));
    }

    public PageId PageId { get; set; }
    public DesignerDataType Type { get; set; }
    public string Json { get; set; }
}
