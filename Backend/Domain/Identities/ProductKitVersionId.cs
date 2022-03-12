namespace Identities;

public record ProductKitVersionId : Identity
{
    public ProductKitVersionId() { }
    public ProductKitVersionId(Guid guid) : base(guid) { }
    public ProductKitVersionId(Guid? guid) : base(guid ?? Guid.Empty) { }
}
