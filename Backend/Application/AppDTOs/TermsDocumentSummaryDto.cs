namespace AppDTOs;

public class TermsDocumentSummaryDto
{
    public TermsDocumentSummaryDto(TermsDocumentId id)
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
    }

    public TermsDocumentId Id { get; set; }

    public DateTimeOffset DateCreatedUtc { get; set; }
    public bool IsActive { get; set; }

    public int Number { get; set; }
}
