namespace DataContext;

public class DbProject : DbEntity
{
    [MaxLength(128)]
    public string Name { get; set; }

    [MaxLength(64)]
    public string ShortName { get; set; }

    public string Description { get; set; }

    [MaxLength(128)]
    public string CustomerName { get; set; }

    public int EstimatedSquareFeet { get; set; }

    public DbFileRef? Photo { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTimeOffset? DesignerLockedUtc { get; set; }

    public List<DbImport> Imports { get; set; } = new();
}
