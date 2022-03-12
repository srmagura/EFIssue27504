using ITI.Baseline.Passwords;
using ITI.Baseline.ValueObjects;
using ValueObjects;

namespace DbEntities;

public class DbUser : DbEntity
{
#nullable disable
    [Obsolete("Entity Framework only.")]
    protected DbUser() { }
#nullable enable

    public DbUser(EmailAddress email, PersonName name)
    {
        Email = email ?? throw new ArgumentNullException(nameof(email));
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }

    public bool IsActive { get; set; } = true;

    public Guid OrganizationId { get; set; }
    public DbOrganization? Organization { get; set; }

    public EmailAddress Email { get; set; }
    public PersonName Name { get; set; }
    public UserRole Role { get; set; }

    public EncodedPassword? EncodedPassword { get; set; }

    public static void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<DbUser>()
            .OwnsOne(p => p.Email, email =>
            {
                email.HasIndex(p => p.Value)
                    .IsUnique();
            });
    }
}
