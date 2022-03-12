namespace Identities;

public record ComponentId : Identity
{
    public ComponentId() { }
    public ComponentId(Guid guid) : base(guid) { }
    public ComponentId(Guid? guid) : base(guid ?? Guid.Empty) { }
}
