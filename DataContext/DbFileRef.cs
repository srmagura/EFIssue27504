namespace DataContext;

[Owned]
public record DbFileRef
{
    public Guid FileId { get; protected init; }
}
