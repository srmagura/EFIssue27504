using DbEntities.ValueObjects;

namespace DbEntities;

public class DbPage : DbEntity
{
#nullable disable
    [Obsolete("Entity Framework only.")]
    protected DbPage() { }
#nullable enable

    public DbFileRef Pdf { get; set; }
    public DbFileRef Thumbnail { get; set; }

    public Guid OrganizationId { get; set; }
    public DbOrganization? Organization { get; set; }

    public Guid ProjectId { get; set; }
    public DbProject? Project { get; set; }

    public int Index { get; set; }

    public Guid? SheetTypeId { get; set; }
    public DbSheetType? SheetType { get; set; }

    [MaxLength(FieldLengths.Page.SheetNumberSuffix)]
    public string? SheetNumberSuffix { get; set; }

    [MaxLength(FieldLengths.Page.SheetNameSuffix)]
    public string? SheetNameSuffix { get; set; }

    public bool IsActive { get; set; } = true;

    public List<DbDesignerData> DesignerData { get; set; } = new();

    public static void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<DbPage>()
            .HasMany(p => p.DesignerData)
            .WithOne(d => d.Page)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
