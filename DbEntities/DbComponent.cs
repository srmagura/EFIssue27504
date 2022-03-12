namespace DbEntities;

public class DbComponent : DbEntity
{
    public Guid OrganizationId { get; set; }
    public DbOrganization? Organization { get; set; }

    public Guid ComponentTypeId { get; set; }
    public DbComponentType? ComponentType { get; set; }

    public MeasurementType MeasurementType { get; set; }

    public bool IsVideoDisplay { get; set; }

    public bool IsActive { get; set; }

    public bool VisibleToCustomer { get; set; }

    public List<DbComponentVersion> Versions { get; set; } = new List<DbComponentVersion>();

    public static void OnModelCreating(ModelBuilder mb)
    {
        // placeholder method
    }
}
