namespace Identities
{
    public record FileId : Identity
    {
        // DO NOT use the default behavior (SequentialGuid.Next) for file IDs since this
        // messes up Azure Blob load balancing
        public FileId() : base(Guid.NewGuid()) { }
        public FileId(Guid guid) : base(guid) { }
        public FileId(Guid? guid) : base(guid ?? Guid.Empty) { }
    }
}
