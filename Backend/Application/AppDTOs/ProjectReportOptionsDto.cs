using ValueObjects;

namespace AppDTOs;

public class ProjectReportOptionsDto
{
    public ProjectReportOptionsDto(
        string signeeName,
        string preparerName,
        LogoSetId logoSetId,
        TermsDocumentId termsDocumentId,
        int termsDocumentNumber,
        int titleBlockSheetNameFontSize,
        bool includeCompassInFooter,
        int compassAngle,
        CompanyContactInfoDto? companyContactInfo
    )
    {
        SigneeName = signeeName;
        PreparerName = preparerName;
        TitleBlockSheetNameFontSize = titleBlockSheetNameFontSize;
        IncludeCompassInFooter = includeCompassInFooter;
        CompassAngle = compassAngle;
        CompanyContactInfo = companyContactInfo;
        LogoSetId = logoSetId;
        TermsDocumentId = termsDocumentId;
        TermsDocumentNumber = termsDocumentNumber;
    }

    public string SigneeName { get; set; }
    public string PreparerName { get; set; }

    public int TitleBlockSheetNameFontSize { get; set; }

    public bool IncludeCompassInFooter { get; set; }
    public int CompassAngle { get; set; }

    public CompanyContactInfoDto? CompanyContactInfo { get; set; }

    public LogoSetId LogoSetId { get; set; }

    public TermsDocumentId TermsDocumentId { get; set; }
    public int TermsDocumentNumber { get; set; }

    public ProjectReportOptions ToValueObject()
    {
        return new ProjectReportOptions(
            SigneeName,
            PreparerName,
            LogoSetId,
            TermsDocumentId,
            TitleBlockSheetNameFontSize,
            IncludeCompassInFooter,
            CompassAngle,
            CompanyContactInfo?.ToValueObject()
        );
    }
}
