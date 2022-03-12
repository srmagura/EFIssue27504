using System.Text;
using AppDTOs;
using Reports.Shared;

namespace Reports.Title;

public class ProjectInfoSection : IHtmlBuilder
{
    private const double SectionWidth = 238.5;
    private const double SectionPaddingTop = 37.5;
    private const double SectionMarginLeft = 33.8;

    private const double HeaderFontSize = 7;
    private const double HeaderLetterSpacing = 0.2;
    private const double HeaderMarginBottom = 10;

    private const double NameFontSize = 17;
    private const double NameLetterSpacing = 0.8;

    private const double DescriptionFontSize = 7;
    private const double DescriptionLineHeight = 12;

    public List<IHtmlBuilder> Children { get; } = new List<IHtmlBuilder>();
    public string? Style { get; }

    public ProjectDto Project { get; }
    public List<(string? Number, string? Name)> SheetIndex { get; }

    public ProjectInfoSection(ProjectDto project, List<(string? Number, string? Name)> sheetIndex)
    {
        Project = project ?? throw new ArgumentNullException(nameof(project));
        SheetIndex = sheetIndex ?? throw new ArgumentNullException(nameof(sheetIndex));
    }

    public string Render()
    {
        var projectInfoSection = new StringBuilder();

        projectInfoSection.Append($@"
<div style=""width: {SectionWidth}pt; padding-top: {SectionPaddingTop}pt; margin-left: {SectionMarginLeft}pt; color: {ReportConstants.TextColorDark};"">
	<div style=""font-weight: bold; font-size: {HeaderFontSize}pt; letter-spacing: {HeaderLetterSpacing}pt; margin-bottom: {HeaderMarginBottom}pt"">PROJECT NAME</div>
	<div style=""font-size: {NameFontSize}pt; letter-spacing: {NameLetterSpacing}pt;"">{Project.ShortName}</div>
");

        var projectAddress = Project.Address.ToValueObject().GetCityStateString();
        if (projectAddress != null)
        {
            projectInfoSection.Append($"<div style=\"font-size: {NameFontSize}pt; letter-spacing: {NameLetterSpacing}pt; \">{projectAddress}</div>");
        }

        projectInfoSection.Append($@"
	<br />
	<br />
	<div style=""font-weight: bold; font-size: {HeaderFontSize}pt; letter-spacing: {HeaderLetterSpacing}pt; margin-bottom: {HeaderMarginBottom}pt"">PROJECT INFORMATION</div>
	<div style=""font-size: {DescriptionFontSize}pt; line-height: {DescriptionLineHeight}pt; text-align: justify; white-space: pre-line; letter-spacing: 0;"">{Project.Description}</div>
	<br />
	<br />
	<br />
	<div style=""font-weight: bold; font-size: {HeaderFontSize}pt; letter-spacing: {HeaderLetterSpacing}pt; margin-bottom: {HeaderMarginBottom}pt"">SHEET INDEX</div>
");

        // Sheet Index

        var sheetIndexSection = new SheetIndexSection(SheetIndex);
        projectInfoSection.Append(sheetIndexSection.Render());

        //

        projectInfoSection.Append("</div>");

        return projectInfoSection.ToString();
    }
}
