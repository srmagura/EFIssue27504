namespace Identities;

public record CategoryId : Identity
{
    public CategoryId() { }
    public CategoryId(Guid guid) : base(guid) { }
    public CategoryId(Guid? guid) : base(guid ?? Guid.Empty) { }
}
