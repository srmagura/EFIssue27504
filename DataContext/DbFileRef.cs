namespace DataContext;

[Owned]
public record DbFileRef
{
    public DbFileRef(Guid fileId, string fileType)
    {
        FileId = fileId;
        FileType = fileType ?? throw new ArgumentNullException(nameof(fileType));
    }

    public Guid FileId { get; protected init; }

    [MaxLength(32)]
    public string FileType { get; protected init; }
}
