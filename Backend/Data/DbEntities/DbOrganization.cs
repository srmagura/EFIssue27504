using ValueObjects;

namespace DbEntities;

public class DbOrganization : DbEntity
{
#nullable disable
    [Obsolete("Entity Framework only.")]
    protected DbOrganization() { }
#nullable enable

    public DbOrganization(string name, OrganizationShortName shortName)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        ShortName = shortName ?? throw new ArgumentNullException(nameof(shortName));
    }

    public bool IsHost { get; set; }

    [MaxLength(FieldLengths.Organization.Name)]
    public string Name { get; set; }

    public OrganizationShortName ShortName { get; set; }

    public bool IsActive { get; set; } = true;

    public List<DbUser> Users { get; protected set; } = new();

    public static void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<DbOrganization>()
            .HasIndex(p => p.Name)
            .IsUnique();

        mb.Entity<DbOrganization>()
            .OwnsOne(p => p.ShortName, sn =>
            {
                sn.HasIndex(p => p.Value)
                    .IsUnique(true);
            });

        mb.Entity<DbOrganization>()
            .HasMany(o => o.Users)
            .WithOne(u => u.Organization)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
