using System.ComponentModel.DataAnnotations.Schema;

namespace DataContext;

public abstract class DbEntity
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public Guid Id { get; set; } = Guid.NewGuid();

    public DateTimeOffset DateCreatedUtc { get; set; } = DateTimeOffset.UtcNow;
}
