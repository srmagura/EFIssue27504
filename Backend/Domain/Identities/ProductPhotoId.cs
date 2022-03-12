namespace Identities
{
    public record ProductPhotoId : Identity
    {
        public ProductPhotoId() { }
        public ProductPhotoId(Guid guid) : base(guid) { }
        public ProductPhotoId(Guid? guid) : base(guid ?? Guid.Empty) { }
    }
}
