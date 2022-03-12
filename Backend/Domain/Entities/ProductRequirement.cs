namespace Entities;

public class ProductRequirement : AggregateRoot
{
    public ProductRequirement(OrganizationId organizationId, string label, string svgText, int index)
    {
        Require.NotNull(organizationId, "Organization ID is requried.");
        OrganizationId = organizationId;

        Require.IsTrue(index >= 0, "Index must be 0 or greater.");
        Index = index;

        SetLabel(label);
        SetSvgText(svgText);
    }

    public ProductRequirementId Id { get; protected set; } = new();

    public OrganizationId OrganizationId { get; protected set; }

    public string Label { get; protected set; }

    public string SvgText { get; protected set; }

    public int Index { get; protected set; }

    [MemberNotNull(nameof(Label))]
    public void SetLabel(string label)
    {
        Require.HasValue(label, "Label is required.");
        Label = label;
    }

    [MemberNotNull(nameof(SvgText))]
    public void SetSvgText(string svgText)
    {
        Require.HasValue(svgText, "SVG text is required.");
        SvgText = svgText;
    }
}
