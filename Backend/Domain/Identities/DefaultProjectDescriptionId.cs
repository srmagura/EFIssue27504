namespace Identities;

public record DefaultProjectDescriptionId : Identity
{
    public DefaultProjectDescriptionId() { }
    public DefaultProjectDescriptionId(Guid guid) : base(guid) { }
    public DefaultProjectDescriptionId(Guid? guid) : base(guid ?? Guid.Empty) { }
}
