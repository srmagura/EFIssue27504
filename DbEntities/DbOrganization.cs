namespace DbEntities;

public class DbOrganization : DbEntity
{
    public bool IsHost { get; set; }

    [MaxLength(64)]
    public string Name { get; set; }

    public bool IsActive { get; set; } = true;

    public static void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<DbOrganization>()
            .HasIndex(p => p.Name)
            .IsUnique();
    }
}
