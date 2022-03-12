namespace Identities;

public record TermsDocumentId : Identity
{
    public TermsDocumentId() { }
    public TermsDocumentId(Guid guid) : base(guid) { }
    public TermsDocumentId(Guid? guid) : base(guid ?? Guid.Empty) { }
}
