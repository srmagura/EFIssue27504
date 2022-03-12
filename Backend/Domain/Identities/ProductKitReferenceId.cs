namespace Identities;

public record ProductKitReferenceId : Identity
{
    public ProductKitReferenceId() { }
    public ProductKitReferenceId(Guid guid) : base(guid) { }
    public ProductKitReferenceId(Guid? guid) : base(guid ?? Guid.Empty) { }
}
