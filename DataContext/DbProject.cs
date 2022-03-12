namespace DataContext;

public class DbProject : DbEntity
{
    [MaxLength(128)]
    public string Name { get; set; }

    public List<DbImport> Imports { get; set; } = new();
}
