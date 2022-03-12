using Reports.Shared;

namespace Reports.Title;

public class ProjectPhotoSection : IHtmlBuilder
{
    private const double SectionWidth = 765.8;
    private const double SectionHeight = 726;
    private const double SectionPaddingTop = 33;

    public List<IHtmlBuilder> Children { get; } = new List<IHtmlBuilder>();
    public string? Style { get; }

    public string ProjectPhotoPath { get; }

    public ProjectPhotoSection(string projectPhotoPath)
    {
        ProjectPhotoPath = projectPhotoPath ?? throw new ArgumentNullException(nameof(projectPhotoPath));
    }

    public string Render()
    {
        return $@"
<div style=""width: {SectionWidth}pt; padding-top: {SectionPaddingTop}pt; display: flex; justify-content: flex-end;"">
	<div>
		<img style=""max-width: {SectionWidth}pt; max-height: {SectionHeight}pt;"" src=""{ProjectPhotoPath}"" />
	</div>
</div>
";
    }
}
