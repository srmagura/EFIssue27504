namespace Identities;

public record ProductKitComponentMapId : Identity
{
    public ProductKitComponentMapId() { }
    public ProductKitComponentMapId(Guid guid) : base(guid) { }
    public ProductKitComponentMapId(Guid? guid) : base(guid ?? Guid.Empty) { }
}
