namespace Identities;

public record ProductRequirementId : Identity
{
    public ProductRequirementId() { }
    public ProductRequirementId(Guid guid) : base(guid) { }
    public ProductRequirementId(Guid? guid) : base(guid ?? Guid.Empty) { }
}
