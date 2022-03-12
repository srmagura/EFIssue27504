namespace Identities
{
    public record ProjectId : Identity
    {
        public ProjectId() { }
        public ProjectId(Guid guid) : base(guid) { }
        public ProjectId(Guid? guid) : base(guid ?? Guid.Empty) { }
    }
}
