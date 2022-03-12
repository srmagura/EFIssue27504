namespace Reports.Shared;

public interface IHtmlBuilder
{
    public List<IHtmlBuilder> Children { get; }
    public string? Style { get; }
    public string Render();
}
