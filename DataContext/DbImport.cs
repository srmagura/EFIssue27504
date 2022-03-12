namespace DataContext;

public class DbImport
{
    public long Id { get; set; }

    public long ProjectId { get; protected set; }
    public DbProject? Project { get; protected set; }

    public DbFileRef File { get; protected set; }
}
