namespace Identities;

public record ProductKitId : Identity
{
    public ProductKitId() { }
    public ProductKitId(Guid guid) : base(guid) { }
    public ProductKitId(Guid? guid) : base(guid ?? Guid.Empty) { }
}
