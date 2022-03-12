namespace Identities;

public record ComponentTypeId : Identity
{
    public ComponentTypeId() { }
    public ComponentTypeId(Guid guid) : base(guid) { }
    public ComponentTypeId(Guid? guid) : base(guid ?? Guid.Empty) { }
}
