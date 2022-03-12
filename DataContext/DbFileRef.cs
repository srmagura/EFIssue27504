namespace DataContext;

[Owned]
public record DbFileRef
{
    public Guid FileId { get; protected init; }

    [MaxLength(32)]
    public string FileType { get; protected init; }
}
