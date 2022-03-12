namespace DataContext;

public class DbProject
{
    public long Id { get; set; }

    public List<DbImport> Imports { get; set; } = new();
}
