namespace DbEntities;

public class DbProductRequirement : DbEntity
{
    public DbProductRequirement(string label, string svgText)
    {
        Label = label ?? throw new ArgumentNullException(nameof(label));
        SvgText = svgText ?? throw new ArgumentNullException(nameof(svgText));
    }

    public Guid OrganizationId { get; set; }
    public DbOrganization? Organization { get; set; }

    [MaxLength(FieldLengths.ProductRequirement.Label)]
    public string Label { get; set; }

    public string SvgText { get; set; }

    public int Index { get; set; }

    public static void OnModelCreating(ModelBuilder mb)
    {
        // For convention
    }
}
