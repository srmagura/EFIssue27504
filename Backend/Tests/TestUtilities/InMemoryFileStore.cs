using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using InfraInterfaces;

namespace TestUtilities
{
    public class InMemoryFileStore : IFileStore
    {
        private static IFileSystem _fileSystem = new MockFileSystem();

        public static void Clear()
        {
            _fileSystem = new MockFileSystem();
        }

        public async Task PutAsync(string path, Stream inputStream)
        {
            EnsureDirectory(path);

            using var file = _fileSystem.File.Create(path);
            await inputStream.CopyToAsync(file);
        }

        public Task<bool> ExistsAsync(string path)
        {
            return Task.FromResult(_fileSystem.File.Exists(path));
        }

        public async Task GetAsync(string path, Stream outputStream)
        {
            using var file = _fileSystem.File.OpenRead(path);
            await file.CopyToAsync(outputStream);
        }

        public Task RemoveAsync(string path)
        {
            _fileSystem.File.Delete(path);
            return Task.CompletedTask;
        }

        private static void EnsureDirectory(string filePath)
        {
            var dir = _fileSystem.Path.GetDirectoryName(filePath);
            if (!_fileSystem.Directory.Exists(dir))
            {
                _fileSystem.Directory.CreateDirectory(dir);
            }
        }

        public Task CopyAsync(string fromPath, string toPath)
        {
            EnsureDirectory(toPath);
            _fileSystem.File.Copy(fromPath, toPath, true);
            return Task.CompletedTask;
        }

        public static List<string> ListFiles(string path)
        {
            return _fileSystem.Directory.EnumerateFiles(path).ToList();
        }
    }
}
