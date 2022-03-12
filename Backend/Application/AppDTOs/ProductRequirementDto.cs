namespace AppDTOs;

public class ProductRequirementDto
{
    public ProductRequirementDto(ProductRequirementId id, OrganizationId organizationId, string label, string svgText)
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
        OrganizationId = organizationId ?? throw new ArgumentNullException(nameof(organizationId));
        Label = label ?? throw new ArgumentNullException(nameof(label));
        SvgText = svgText ?? throw new ArgumentNullException(nameof(svgText));
    }

    public ProductRequirementId Id { get; set; }

    public OrganizationId OrganizationId { get; set; }

    public string Label { get; set; }
    public string SvgText { get; set; }
}
