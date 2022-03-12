namespace ApiScaffolder;

internal static class TheApiScaffolder
{
    private static void AskForHttpMethod(MethodModel method)
    {
        var paramaterNames = method.Parameters.Select(p => p.Name);
        var signature = VarUtil.GetCsTypeName(method.ReturnType) + " ";
        signature += method.Name + "(" + string.Join(", ", paramaterNames) + ")";

        Console.Write($"{signature}: ");

        var keyChar = Console.ReadKey().KeyChar;
        keyChar = char.ToLowerInvariant(keyChar);

        Console.WriteLine();

        switch (keyChar)
        {
            case 'g':
                method.HttpMethod = HttpMethod.Get;
                break;
            case 'p':
                method.HttpMethod = HttpMethod.Post;
                break;
            default:
                Console.WriteLine("Invalid input. Try again.");
                AskForHttpMethod(method);
                break;
        }
    }

    private static void AskForHttpMethods(List<MethodModel> methods)
    {
        Console.WriteLine("You must select GET or POST for each of the following methods.");
        Console.WriteLine("Type g for GET or p for POST.\n");

        foreach (var method in methods)
        {
            AskForHttpMethod(method);
        }

        Console.WriteLine();
    }

    private static void Write(AppInterfaceModel model)
    {
        var outputDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ApiScaffolderOutput");
        Directory.CreateDirectory(outputDir);

        var functionPath = new FunctionWriter(model).Write(outputDir);
        var typeScriptPath = new TypeScriptWriter(model).Write(outputDir);

        Console.WriteLine($"Wrote {Path.GetFileName(functionPath)} and {Path.GetFileName(typeScriptPath)} in {outputDir}.");
        Console.WriteLine();
    }

    internal static void Run(string interfaceName)
    {
        var model = AppServiceReader.Read(interfaceName);
        AskForHttpMethods(model.Methods);
        Write(model);
    }
}
