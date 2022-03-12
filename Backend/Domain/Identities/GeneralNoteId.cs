namespace Identities;

public record GeneralNoteId : Identity
{
    public GeneralNoteId() { }
    public GeneralNoteId(Guid guid) : base(guid) { }
    public GeneralNoteId(Guid? guid) : base(guid ?? Guid.Empty) { }
}
