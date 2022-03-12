namespace Settings
{
    public class PageSettings
    {
        public double WidthInches { get; set; } = 17.0;
        public double HeightInches { get; set; } = 11.0;
        public double DimensionMarginOfErrorPercent { get; set; } = 0.01;

        public double WidthMarginOfError => WidthInches * DimensionMarginOfErrorPercent;
        public double HeightMarginOfError => HeightInches * DimensionMarginOfErrorPercent;
    }
}
