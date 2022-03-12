using CommandLine;

namespace ApiScaffolder;

internal static class ApiScaffolderProgram
{
    public class Options
    {
        [Value(0)]
        public string? AppInterfaceName { get; set; }
    }

    public static void Main(string[] args)
    {
        Parser.Default.ParseArguments<Options>(args)
            .WithParsed(Run);
    }

    private static void Run(Options options)
    {
        if (options.AppInterfaceName == null)
            throw new Exception("AppInterfaceName is null.");

        TheApiScaffolder.Run(options.AppInterfaceName);
    }
}
