using System.Text;
using Namotion.Reflection;

namespace ApiScaffolder;

internal class FunctionWriter : BaseWriter
{
    private readonly AppInterfaceModel _model;

    public FunctionWriter(AppInterfaceModel model)
    {
        _model = model;
    }

    private const string Tab = "    ";
    private string ClassName => $"{_model.EntityName}Api";

    private void WriteClassStart()
    {
        AppendLine("namespace FunctionApp.Api;");
        AppendLine();
        AppendLine($"public class {ClassName} : ApiFunction");
        AppendLine("{");
    }

    private string InterfaceMemberName => "_" + VarUtil.ToCamelCase(_model.InterfaceName[1..]);

    private void WriteConstructor()
    {
        var interfaceArgumentName = InterfaceMemberName.Replace("_", "");

        AppendLine($"private readonly {_model.InterfaceName} {InterfaceMemberName};");
        AppendLine();
        AppendLine($"public {ClassName}(");
        AppendLine($"IAppAuthContext auth,");
        AppendLine($"Bugsnag.IClient bugsnag,");
        AppendLine($"{_model.InterfaceName} {interfaceArgumentName}");
        AppendLine($") : base(auth, bugsnag)");
        AppendLine("{");
        AppendLine($"{InterfaceMemberName} = {interfaceArgumentName};");
        AppendLine("}");
        AppendLine();
    }

    private static string GetRequestParamsClassName(MethodModel method) => $"{method.NameNoAsync}RequestParams";
    private static string GetRequestBodyRecordName(MethodModel method) => $"{method.NameNoAsync}RequestBody";

    private void WriteRequestParamsClass(MethodModel method)
    {
        AppendLine($"public class {GetRequestParamsClassName(method)} {{");

        foreach (var parameter in method.Parameters)
        {
            var typeName = VarUtil.GetCsTypeName(parameter);
            if (typeName.EndsWith("Id")) typeName = "Guid";
            if (parameter.Type.IsEnum) typeName = "int";
            if (!typeName.EndsWith("?")) typeName += "?";

            var parameterName = VarUtil.ToPascalCase(parameter.Name);

            AppendLine(Tab + $"public {typeName} {parameterName} {{ get; set; }}");
        }

        AppendLine("}");
        AppendLine();
    }

    private void WriteRequestBodyRecord(MethodModel method)
    {
        AppendLine($"public record {GetRequestBodyRecordName(method)}(");

        for (var i = 0; i < method.Parameters.Count; i++)
        {
            var parameter = method.Parameters[i];
            var typeName = VarUtil.GetCsTypeName(parameter);
            var comma = i < method.Parameters.Count - 1 ? "," : "";

            var parameterName = VarUtil.ToPascalCase(parameter.Name!);

            AppendLine(Tab + $"{typeName} {parameterName}{comma}");
        }

        AppendLine(");");
        AppendLine();
    }

    private void WriteMethod(MethodModel method, bool isLast)
    {
        var entityName = VarUtil.ToCamelCase(_model.EntityName);
        var methodNameCamelCase = VarUtil.ToCamelCase(method.Name.Replace("Async", ""));

        AppendLine($"[FunctionName(\"api_{entityName}_{methodNameCamelCase}\")]");
        AppendLine($"public Task<IActionResult> {method.Name}(");

        var getOrPost = method.HttpMethod == HttpMethod.Get ? "get" : "post";
        AppendLine(Tab + Tab + $"[HttpTrigger(AuthorizationLevel.Anonymous, \"{getOrPost}\", Route = \"{entityName}/{methodNameCamelCase}\")]");

        if (method.HttpMethod == HttpMethod.Get)
        {
            AppendLine(Tab + Tab + $"{GetRequestParamsClassName(method)} @params");
        }
        else
        {
            AppendLine(Tab + Tab + $"{GetRequestBodyRecordName(method)} body");
        }

        AppendLine(")");
        AppendLine("{");
        AppendLine("return HandleRequestAsync(async () =>");
        AppendLine("{");

        if (method.HttpMethod == HttpMethod.Get)
        {
            foreach (var parameter in method.Parameters)
            {
                if (parameter.Nullability == Nullability.Nullable) continue;
                var parameterName = "@params." + VarUtil.ToPascalCase(parameter.Name);
                AppendLine($"RequireParam({parameterName}, nameof({parameterName}));");
            }

            AppendLine();
        }

        var argumentPrefix = method.HttpMethod == HttpMethod.Get ? "@params." : "body.";
        var appServiceArguments = method.Parameters.Select(p => argumentPrefix + VarUtil.ToPascalCase(p.Name));
        var @return = method.ReturnType.Type.Name == "Task" ? "" : "return ";

        AppendLine($"{@return}await {InterfaceMemberName}.{method.Name}({string.Join(", ", appServiceArguments)});");
        AppendLine("});");
        AppendLine("}");

        if (!isLast) AppendLine();
    }

    private void WriteClass()
    {
        // Not generating any using statements - just use Visual Studio to add them all
        WriteClassStart();
        WriteConstructor();

        for (var i = 0; i < _model.Methods.Count; i++)
        {
            var method = _model.Methods[i];

            if (method.HttpMethod == HttpMethod.Get)
            {
                WriteRequestParamsClass(method);
            }
            else
            {
                WriteRequestBodyRecord(method);
            }

            WriteMethod(method, isLast: i == _model.Methods.Count - 1);
        }

        AppendLine("}");
    }

    // Returns file path
    public string Write(string outputDir)
    {
        StringBuilder = new StringBuilder();
        WriteClass();

        var path = Path.Combine(outputDir, ClassName + ".cs");
        WriteFile(path);

        return path;
    }
}
