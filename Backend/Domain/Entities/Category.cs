namespace Entities;

public class Category : AggregateRoot
{
    [Obsolete("AutoMapper only")]
    public Category(OrganizationId organizationId, string name)
    {
        OrganizationId = organizationId;
        Name = name;
    }

    public Category(
        OrganizationId organizationId,
        string name,
        Category? parent,
        int index,
        SymbolId? symbolId,
        string? color,
        bool isActive = true
    )
    {
        Require.NotNull(organizationId, "Organization ID is required.");
        OrganizationId = organizationId;

        if (parent != null && !parent.IsActive)
            Require.IsTrue(!isActive, "A category must be inactive if its parent is inactive.");

        SetName(name);
        SetParentSymbolAndColor(parent, index, symbolId, color);
        SetActive(isActive);
    }

    public CategoryId Id { get; protected set; } = new();

    public OrganizationId OrganizationId { get; protected set; }

    public string Name { get; protected set; }

    public CategoryId? ParentId { get; protected set; }
    public int Index { get; protected set; }

    public SymbolId? SymbolId { get; protected set; }
    public string? Color { get; protected set; }

    public bool IsActive { get; protected set; } = true;

    public void SetId(CategoryId id)
    {
        Require.NotNull(id, "ID is required.");
        Id = id;
    }

    [MemberNotNull(nameof(Name))]
    public void SetName(string name)
    {
        Require.HasValue(name, "Name is required.");
        Name = name;
    }

    public void SetParentSymbolAndColor(Category? newParent, int index, SymbolId? symbolId, string? color)
    {
        if (newParent != null)
            Require.IsTrue(OrganizationId == newParent.OrganizationId, "Parent category must be in the same organization.");

        ParentId = newParent?.Id;

        SetIndex(index);
        SetSymbol(symbolId);
        SetColor(color);
    }

    protected void SetIndex(int index)
    {
        Require.IsTrue(index >= 0, "Index must be 0 or greater.");
        Index = index;
    }

    protected void SetSymbol(SymbolId? symbolId)
    {
        if (ParentId == null)
            Require.NotNull(symbolId, "Top-level categories require a symbol.");
        else
            Require.IsTrue(symbolId == null, "Child categories cannot have a symbol.");

        SymbolId = symbolId;
    }

    protected void SetColor(string? color)
    {
        if (ParentId == null)
            Require.NotNull(color, "Top-level categories require a color.");
        else
            Require.IsTrue(color == null, "Child categories cannot have a color.");

        Color = color;
    }

    protected void SetActive(bool active)
    {
        IsActive = active;
    }

    public void UpdateFrom(Category other, Category? parent)
    {
        Require.NotNull(other, "Other category cannot be null.");
        if (parent != null && !parent.IsActive)
            Require.IsTrue(!other.IsActive, "A category must be inactive if its parent is inactive.");

        SetParentSymbolAndColor(parent, other.Index, other.SymbolId, other.Color);
        SetName(other.Name);
        SetActive(other.IsActive);
    }
}
