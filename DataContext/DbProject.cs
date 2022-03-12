namespace DataContext;

public class DbProject
{
    public long Id { get; set; }

    [MaxLength(128)]
    public string Name { get; set; }

    public List<DbImport> Imports { get; set; } = new();
}
