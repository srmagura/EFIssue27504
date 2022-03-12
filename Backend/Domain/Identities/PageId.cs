namespace Identities
{
    public record PageId : Identity
    {
        public PageId() { }
        public PageId(Guid guid) : base(guid) { }
        public PageId(Guid? guid) : base(guid ?? Guid.Empty) { }
    }
}
