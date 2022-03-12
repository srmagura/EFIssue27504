namespace DataContext;

public class DbImport : DbEntity
{
    public Guid ProjectId { get; protected set; }
    public DbProject? Project { get; protected set; }

    public DbFileRef File { get; protected set; }
}
