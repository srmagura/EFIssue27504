using Namotion.Reflection;

namespace ApiScaffolder;

internal static class VarUtil
{
    public static string ToPascalCase(string typeName)
    {
        var chars = typeName.ToCharArray();
        chars[0] = char.ToUpperInvariant(chars[0]);
        return new string(chars);
    }

    public static string ToCamelCase(string typeName)
    {
        var chars = typeName.ToCharArray();
        chars[0] = char.ToLowerInvariant(chars[0]);
        return new string(chars);
    }

    public static string GetCsTypeName(ContextualType contextualType)
    {
        var type = contextualType.Type;
        var typeName = type.Name;

        var rewrites = new Dictionary<string, string>
        {
            ["String"] = "string",
            ["Boolean"] = "bool",
            ["Int32"] = "int",
            ["Int64"] = "long"
        };

        if (rewrites.ContainsKey(typeName))
        {
            typeName = rewrites[typeName];
        }

        if (type.IsGenericType)
        {
            var genericArgumentNames = type.GetGenericArguments().Select(t => GetCsTypeName(t.ToContextualType()));

            typeName = typeName.Split('`')[0];
            typeName += "<" + string.Join(", ", genericArgumentNames) + ">";
        }

        if (contextualType.Nullability == Nullability.Nullable)
            typeName += "?";

        return typeName;
    }
}
