namespace GenerateTypeScript
{
    internal static class PathUtil
    {
        private static string GetRepositoryPath()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory;

            while (true)
            {
                if (Directory.Exists(Path.Combine(path, ".git")))
                    return path;

                var newPath = Path.GetFullPath(Path.Combine(path, ".."));

                if (path == newPath)
                    throw new Exception("Reached root directory and did not find .git.");

                path = newPath;
            }
        }

        // Returns the absolute path of react-app/src/models/generated
        public static string GetModelsGeneratedPath()
        {
            return Path.Combine(GetRepositoryPath(), "UI/react-app/src/models/generated");
        }
    }
}
