namespace Identities;

public record ImportId : Identity
{
    public ImportId() { }
    public ImportId(Guid guid) : base(guid) { }
    public ImportId(Guid? guid) : base(guid ?? Guid.Empty) { }
}
