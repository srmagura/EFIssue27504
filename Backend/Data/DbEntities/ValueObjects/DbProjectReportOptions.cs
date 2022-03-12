using ITI.DDD.Domain;
using ValueObjects;

namespace DbEntities.ValueObjects;

public record DbProjectReportOptions : DbValueObject
{
    public DbProjectReportOptions(
        string signeeName,
        string preparerName,
        Guid logoSetId,
        Guid termsDocumentId
    )
    {
        SigneeName = signeeName ?? throw new ArgumentNullException(nameof(signeeName));
        PreparerName = preparerName ?? throw new ArgumentNullException(nameof(preparerName));
        LogoSetId = logoSetId;
        TermsDocumentId = termsDocumentId;
    }

    public string SigneeName { get; protected init; }
    public string PreparerName { get; protected init; }

    public int TitleBlockSheetNameFontSize { get; protected init; }

    public bool IncludeCompassInFooter { get; protected init; }
    public int CompassAngle { get; protected init; }

    public CompanyContactInfo? CompanyContactInfo { get; protected init; }

    public Guid LogoSetId { get; protected init; }
    public DbLogoSet? LogoSet { get; protected init; }

    public Guid TermsDocumentId { get; protected init; }
    public DbTermsDocument? TermsDocument { get; protected init; }
}
