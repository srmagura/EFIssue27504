using System.Reflection;
using AppInterfaces;
using Namotion.Reflection;

namespace ApiScaffolder;

internal static class AppServiceReader
{
    private static MethodModel ReadMethod(MethodInfo method)
    {
        return new MethodModel(
            method.Name,
            method.GetParameters().Select(p => p.ToContextualParameter()).ToList(),
            method.ReturnType.ToContextualType()
        );
    }

    private static List<MethodModel> ReadMethods(Type appInterface)
    {
        return appInterface.GetMethods().Select(ReadMethod).ToList();
    }

    internal static AppInterfaceModel Read(string interfaceName)
    {
        var types = typeof(IUserAppService).Assembly.GetTypes();

        var appInterface = types.SingleOrDefault(t =>
            t.Name.Equals(interfaceName, StringComparison.OrdinalIgnoreCase)
        );
        if (appInterface == null)
            throw new Exception($"Could not find interface {interfaceName}.");

        var methodModels = ReadMethods(appInterface);
        return new AppInterfaceModel(appInterface.Name, methodModels);
    }
}
