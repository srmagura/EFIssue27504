using Namotion.Reflection;

namespace ApiScaffolder;

internal class AppInterfaceModel
{
    public AppInterfaceModel(
        string interfaceName,
        List<MethodModel> methods
    )
    {
        InterfaceName = interfaceName ?? throw new ArgumentNullException(nameof(interfaceName));
        Methods = methods ?? throw new ArgumentNullException(nameof(methods));
    }

    public string InterfaceName { get; set; }
    public List<MethodModel> Methods { get; set; }

    public string EntityName => InterfaceName[1..].Replace("AppService", "");
}

internal enum HttpMethod
{
    Get,
    Post
}

internal class MethodModel
{
    public MethodModel(string name, List<ContextualParameterInfo> parameters, ContextualType returnType)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Parameters = parameters ?? throw new ArgumentNullException(nameof(parameters));
        ReturnType = returnType ?? throw new ArgumentNullException(nameof(returnType));
    }

    public string Name { get; set; }
    public string NameNoAsync => Name.Replace("Async", "");
    public HttpMethod HttpMethod { get; set; }

    public List<ContextualParameterInfo> Parameters { get; set; }
    public ContextualType ReturnType { get; set; }
}
