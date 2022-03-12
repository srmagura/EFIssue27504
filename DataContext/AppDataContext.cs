namespace DataContext;

public class AppDataContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Windows authentication. If you are using Linux, need to tweak this
        var connectionString = "Server=localhost;Database=Slateplan;TrustServerCertificate=True;MultipleActiveResultSets=True;Trusted_Connection=True;";

        optionsBuilder
            .EnableSensitiveDataLogging()
            .UseSqlServer(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        DbOrganization.OnModelCreating(modelBuilder);

        DbProject.OnModelCreating(modelBuilder);
    }

    public DbSet<DbOrganization> Organizations => Set<DbOrganization>();

    public DbSet<DbProject> Projects => Set<DbProject>();
    public DbSet<DbImport> Imports => Set<DbImport>();
}
