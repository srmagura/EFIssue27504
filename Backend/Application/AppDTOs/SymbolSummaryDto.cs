namespace AppDTOs
{
    public class SymbolSummaryDto
    {
        public SymbolSummaryDto(SymbolId id, string name, string svgText)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            SvgText = svgText ?? throw new ArgumentNullException(nameof(svgText));
        }

        public SymbolId Id { get; set; }

        public string Name { get; set; }
        public string SvgText { get; set; }

        public bool IsActive { get; set; }
    }
}
