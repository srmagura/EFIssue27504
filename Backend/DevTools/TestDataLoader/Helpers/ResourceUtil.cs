using System.Reflection;

namespace TestDataLoader.Helpers
{
    public static class ResourceUtil
    {
        public static Stream GetResourceStream(string filename)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourcePath = $"{assembly.GetName().Name}.Resources.{filename}";

            return assembly.GetManifestResourceStream(resourcePath)
                ?? throw new NullReferenceException($"Could not load resource {filename}.");
        }

        public static string GetResourceText(string filename)
        {
            using var stream = GetResourceStream(filename);
            using var reader = new StreamReader(stream);

            return reader.ReadToEnd();
        }
    }
}
