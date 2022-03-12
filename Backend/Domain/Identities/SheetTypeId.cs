namespace Identities;

public record SheetTypeId : Identity
{
    public SheetTypeId() { }
    public SheetTypeId(Guid guid) : base(guid) { }
    public SheetTypeId(Guid? guid) : base(guid ?? Guid.Empty) { }
}
