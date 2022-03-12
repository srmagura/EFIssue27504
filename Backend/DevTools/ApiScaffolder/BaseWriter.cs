using System.Text;

namespace ApiScaffolder;

internal abstract class BaseWriter
{
    protected StringBuilder StringBuilder { get; set; } = new();

    protected void Append(string s)
    {
        StringBuilder.Append(s);
    }

    protected void AppendLine()
    {
        StringBuilder.AppendLine();
    }

    protected void AppendLine(string s)
    {
        StringBuilder.AppendLine(s);
    }

    protected void WriteFile(string path)
    {
        var s = StringBuilder.Replace("\r", "").ToString();
        File.WriteAllText(path, s);
    }
}
