namespace DbApplicationImpl.Util;

internal static class VersionNameUtil
{
    public static string GetNewVersionName(IReadOnlyList<string> versionNames, string nowName)
    {
        if (versionNames.Count == 0)
            return nowName;

        var i = 1;
        while (true)
        {
            var newName = i == 1 ? nowName : $"{nowName} ({i})";

            if (!versionNames.Any(p => p.Equals(newName, StringComparison.OrdinalIgnoreCase)))
                return newName;

            i++;
        }
    }
}
