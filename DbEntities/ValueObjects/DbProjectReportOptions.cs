using ValueObjects;

namespace DbEntities.ValueObjects;

public record DbProjectReportOptions : DbValueObject
{
    public DbProjectReportOptions(
        string signeeName,
        string preparerName
    )
    {
        SigneeName = signeeName ?? throw new ArgumentNullException(nameof(signeeName));
        PreparerName = preparerName ?? throw new ArgumentNullException(nameof(preparerName));
    }

    public string SigneeName { get; protected init; }
    public string PreparerName { get; protected init; }

    public int TitleBlockSheetNameFontSize { get; protected init; }

    public bool IncludeCompassInFooter { get; protected init; }
    public int CompassAngle { get; protected init; }

    public CompanyContactInfo? CompanyContactInfo { get; protected init; }
}
