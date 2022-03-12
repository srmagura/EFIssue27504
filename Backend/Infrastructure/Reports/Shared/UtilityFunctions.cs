namespace Reports.Shared;

public static class UtilityFunctions
{
    public static string ColorBlendFromHex(string topColor, string bottomColor, double opacity)
    {
        var top = System.Drawing.ColorTranslator.FromHtml(topColor);
        var bot = System.Drawing.ColorTranslator.FromHtml(bottomColor);

        int r = (int)Math.Round((opacity * top.R) + ((1 - opacity) * bot.R));
        int g = (int)Math.Round((opacity * top.G) + ((1 - opacity) * bot.G));
        int b = (int)Math.Round((opacity * top.B) + ((1 - opacity) * bot.B));

        return System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.FromArgb(r, g, b));
    }
}
