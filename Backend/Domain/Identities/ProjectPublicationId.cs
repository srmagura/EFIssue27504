namespace Identities;

public record ProjectPublicationId : Identity
{
    public ProjectPublicationId() { }
    public ProjectPublicationId(Guid guid) : base(guid) { }
    public ProjectPublicationId(Guid? guid) : base(guid ?? Guid.Empty) { }
}
