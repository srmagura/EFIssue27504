namespace AppDTOs;

public class SymbolDto
{
    public SymbolDto(SymbolId id, OrganizationId organizationId, string name, string svgText)
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
        OrganizationId = organizationId ?? throw new ArgumentNullException(nameof(organizationId));
        Name = name ?? throw new ArgumentNullException(nameof(name));
        SvgText = svgText ?? throw new ArgumentNullException(nameof(svgText));
    }

    public SymbolId Id { get; set; }
    public OrganizationId OrganizationId { get; set; }

    public string Name { get; set; }
    public string SvgText { get; set; }

    public bool IsActive { get; set; }
}
