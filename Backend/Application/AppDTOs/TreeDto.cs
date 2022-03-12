namespace AppDTOs;

public class TreeDto
{
    public CategoryDto? Category { get; set; }

    public List<TreeDto> Children { get; set; } = new();
}
