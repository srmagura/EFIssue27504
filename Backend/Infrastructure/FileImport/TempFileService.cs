using InfraInterfaces;

namespace FileImport
{
    public class TempFileService : ITempFileService
    {
        private static readonly string BasePath = Path.Combine(Path.GetTempPath(), "SlateplanTempFiles");

        public string GetBasePath()
        {
            return BasePath;
        }

        public string GetTempDirectory(string directoryName)
        {
            var path = Path.Combine(BasePath, directoryName);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            return path;
        }

        // Services using temp files should clean up their own files whenever possible,
        // but there are some situations in which this is not possible:
        // - FileUploadService: WriteChunkAsync called but Cleanup never called
        internal static void DeleteOldFiles(TimeSpan deleteAge)
        {
            if (!Directory.Exists(BasePath)) return;

            var filePaths = Directory.EnumerateFiles(BasePath, "*", new EnumerationOptions { RecurseSubdirectories = true });

            foreach (var path in filePaths)
            {
                if (DateTimeOffset.UtcNow.Subtract(File.GetLastWriteTimeUtc(path)).TotalSeconds > deleteAge.TotalSeconds)
                {
                    File.Delete(path);
                }
            }
        }

        public void DeleteOldFiles()
        {
            DeleteOldFiles(TimeSpan.FromHours(1));
        }
    }
}
