namespace Identities;

public record NotForConstructionDisclaimerTextId : Identity
{
    public NotForConstructionDisclaimerTextId() { }
    public NotForConstructionDisclaimerTextId(Guid guid) : base(guid) { }
    public NotForConstructionDisclaimerTextId(Guid? guid) : base(guid ?? Guid.Empty) { }
}
