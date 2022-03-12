namespace Identities;

public record LogoSetId : Identity
{
    public LogoSetId() { }
    public LogoSetId(Guid guid) : base(guid) { }
    public LogoSetId(Guid? guid) : base(guid ?? Guid.Empty) { }
}
