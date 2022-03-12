namespace Entities;

public class Symbol : AggregateRoot
{
    public Symbol(OrganizationId organizationId, string name, string svgText)
    {
        OrganizationId = organizationId;

        SetName(name);
        SetSvgText(svgText);
    }

    public SymbolId Id { get; protected set; } = new();

    public OrganizationId OrganizationId { get; protected set; }

    public string Name { get; protected set; }
    public string SvgText { get; protected set; }

    public bool IsActive { get; protected set; } = true;

    [MemberNotNull(nameof(Name))]
    public void SetName(string name)
    {
        Require.HasValue(name, "Name is required.");
        Name = name;
    }

    [MemberNotNull(nameof(SvgText))]
    public void SetSvgText(string svgText)
    {
        Require.HasValue(svgText, "SVG text is required.");
        SvgText = svgText;
    }

    public void SetActive(bool isActive)
    {
        IsActive = isActive;
    }
}
