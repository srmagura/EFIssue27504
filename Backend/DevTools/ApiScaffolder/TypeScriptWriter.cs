using System.Text;
using Namotion.Reflection;

namespace ApiScaffolder;

internal class TypeScriptWriter : BaseWriter
{
    private readonly AppInterfaceModel _model;

    public TypeScriptWriter(AppInterfaceModel model)
    {
        _model = model;
    }

    internal static string MapType(ContextualType contextualType, bool allowNull)
    {
        var type = contextualType.Type;
        var typeName = contextualType.Type.Name;

        var rewrites = new Dictionary<string, string>
        {
            ["String"] = "string",
            ["Boolean"] = "boolean",
            ["Int32"] = "number",
            ["Int64"] = "number",
            ["Float"] = "number",
            ["Single"] = "number",
            ["Double"] = "number",
            ["Decimal"] = "number",
            ["DateTime"] = "moment.Moment",
            ["DateTimeOffset"] = "moment.Moment",
            ["TimeSpan"] = "string",
            ["Guid"] = "string"
        };

        if (rewrites.ContainsKey(typeName))
        {
            typeName = rewrites[typeName];
        }

        if (type.FullName != null && type.FullName.Contains("System.Threading.Tasks.Task"))
        {
            if (contextualType.GenericArguments.Any())
            {
                typeName = MapType(contextualType.GenericArguments.Single(), allowNull);
            }
            else
            {
                typeName = "void";
            }
        }
        else if (type.FullName != null && type.FullName.Contains("System.Collections.Generic.List"))
        {
            var genericArgumentTypeName = MapType(contextualType.GenericArguments.Single(), allowNull);

            if (genericArgumentTypeName.Contains(' '))
            {
                typeName = $"({genericArgumentTypeName})[]";
            }
            else
            {
                typeName = genericArgumentTypeName + "[]";
            }
        }
        else if (type.IsGenericType)
        {
            var genericArgumentNames = contextualType.GenericArguments.Select(t => MapType(t, allowNull));

            typeName = typeName.Split('`')[0];
            typeName += "<" + string.Join(", ", genericArgumentNames) + ">";
        }

        if (contextualType.Nullability == Nullability.Nullable)
        {
            if (allowNull) typeName += " | null";
            typeName += " | undefined";
        }

        return typeName;
    }

    private static string GetGetOrPostCall(MethodModel method)
    {
        var genericParam = "<" + MapType(method.ReturnType, allowNull: true) + ">";

        return method.HttpMethod switch
        {
            HttpMethod.Get => "get" + genericParam,
            HttpMethod.Post => "post" + genericParam,
            _ => throw new ArgumentOutOfRangeException(nameof(method)),
        };
    }

    private string GetUrl(MethodModel method)
    {
        var entityName = VarUtil.ToCamelCase(_model.EntityName);
        var methodName = VarUtil.ToCamelCase(method.NameNoAsync);

        return $"/api/{entityName}/{methodName}";
    }

    private void WriteMethod(MethodModel method)
    {
        var arguments = method.Parameters.Select(p =>
        {
            var name = VarUtil.ToCamelCase(p.Name!);
            var type = MapType(p, allowNull: false);

            var orUndefined = "";
            if (p.Type.Name.Contains("Nullable"))
                orUndefined = " | undefined";

            return $"{name}: {type} {orUndefined}\n";
        });

        AppendLine(VarUtil.ToCamelCase(method.Name.Replace("Async", "")));
        AppendLine(": (options: {");
        Append(string.Join("", arguments));
        AppendLine("}) => ");

        AppendLine(GetGetOrPostCall(method));
        AppendLine($"('{GetUrl(method)}', options),");
        AppendLine();
    }

    private void WriteApiClient()
    {
        AppendLine($"export function {VarUtil.ToCamelCase(_model.EntityName)}Api({{get, post}}: ApiMethods) {{");
        AppendLine("return {");

        foreach (var method in _model.Methods)
        {
            WriteMethod(method);
        }

        AppendLine("}");
        AppendLine("}");
    }

    // Returns file path
    public string Write(string outputDir)
    {
        StringBuilder = new StringBuilder();
        WriteApiClient();

        var path = Path.Combine(outputDir, VarUtil.ToCamelCase(_model.EntityName) + "Api.ts");
        WriteFile(path);

        return path;
    }
}
