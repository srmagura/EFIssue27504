namespace Identities;

public record ProductFamilyId : Identity
{
    public ProductFamilyId() { }
    public ProductFamilyId(Guid guid) : base(guid) { }
    public ProductFamilyId(Guid? guid) : base(guid ?? Guid.Empty) { }
}
