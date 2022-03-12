namespace Identities;

public record ComponentVersionId : Identity
{
    public ComponentVersionId() { }
    public ComponentVersionId(Guid guid) : base(guid) { }
    public ComponentVersionId(Guid? guid) : base(guid ?? Guid.Empty) { }
}
