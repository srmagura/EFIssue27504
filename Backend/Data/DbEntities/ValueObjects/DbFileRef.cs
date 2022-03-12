using ITI.DDD.Domain;

namespace DbEntities.ValueObjects
{
    public record DbFileRef : DbValueObject
    {
        public DbFileRef(Guid fileId, string fileType)
        {
            FileId = fileId;
            FileType = fileType ?? throw new ArgumentNullException(nameof(fileType));
        }

        public Guid FileId { get; protected init; }

        [MaxLength(FieldLengths.FileRef.FileType)]
        public string FileType { get; protected init; }
    }
}
