namespace AppDTOs;

public class CategoryDto
{
    public CategoryDto(CategoryId id, string name)
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }

    public CategoryId Id { get; set; }

    public string Name { get; set; }

    public SymbolId? SymbolId { get; set; }
    public string? Color { get; set; }

    public bool IsActive { get; set; }
}
