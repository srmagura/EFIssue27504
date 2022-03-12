using Identities;
using ITI.Baseline.Util;

namespace ValueObjects;

public record ProjectReportOptions
{
    public ProjectReportOptions(
        string signeeName,
        string preparerName,
        LogoSetId logoSetId,
        TermsDocumentId termsDocumentId,
        int titleBlockSheetNameFontSize = 20, // TODO determine real default value through experimentation
        bool includeCompassInFooter = true,
        int compassAngle = 0,
        CompanyContactInfo? companyContactInfo = null
    )
    {
        Require.NotNull(signeeName, "Signee name is required.");
        Require.NotNull(preparerName, "Preparer name is required.");
        Require.NotNull(logoSetId, "Logo set ID is required.");
        Require.NotNull(termsDocumentId, "Terms document ID is required.");

        SigneeName = signeeName;
        PreparerName = preparerName;
        LogoSetId = logoSetId;
        TermsDocumentId = termsDocumentId;
        TitleBlockSheetNameFontSize = titleBlockSheetNameFontSize;
        IncludeCompassInFooter = includeCompassInFooter;
        CompassAngle = compassAngle;
        CompanyContactInfo = companyContactInfo;
    }

    public string SigneeName { get; protected init; }
    public string PreparerName { get; protected init; }

    public int TitleBlockSheetNameFontSize { get; protected init; }

    public bool IncludeCompassInFooter { get; protected init; }
    public int CompassAngle { get; protected init; }

    public CompanyContactInfo? CompanyContactInfo { get; protected init; }

    public LogoSetId LogoSetId { get; protected init; }

    public TermsDocumentId TermsDocumentId { get; protected init; }
}
