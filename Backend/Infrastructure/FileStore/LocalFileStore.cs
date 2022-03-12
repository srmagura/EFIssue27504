using InfraInterfaces;

namespace FileStore;

public class LocalFileStore : IFileStore
{
    private readonly LocalFileStoreSettings _settings;

    public LocalFileStore(LocalFileStoreSettings settings)
    {
        _settings = settings;
    }

    public Task<bool> ExistsAsync(string path)
    {
        var filePath = BuildPath(path);

        return Task.FromResult(File.Exists(filePath));
    }

    public async Task GetAsync(string path, Stream outputStream)
    {
        var filePath = BuildPath(path);

        using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        await fs.CopyToAsync(outputStream, 65535);
    }

    public async Task PutAsync(string path, Stream inputStream)
    {
        var filePath = BuildPath(path);

        using var fs = new FileStream(filePath, FileMode.Create);
        await inputStream.CopyToAsync(fs);
    }

    public async Task RemoveAsync(string path)
    {
        if (await ExistsAsync(path))
        {
            var filePath = BuildPath(path);

            File.Delete(filePath);
        }
    }

    public Task CopyAsync(string fromPath, string toPath)
    {
        var fromFilePath = BuildPath(fromPath);
        var toFilePath = BuildPath(toPath);

        File.Copy(fromFilePath, toFilePath);
        return Task.CompletedTask;
    }

    private string BuildPath(string path)
    {
        var filePath = Path.Combine(_settings.BasePath, path);
        var dirPath = Path.GetDirectoryName(filePath);

        if (dirPath == null)
            throw new Exception($"GetDirectoryName failed for file {filePath}.");

        if (!Directory.Exists(dirPath))
        {
            Directory.CreateDirectory(dirPath);
        }

        return filePath;
    }

    public void DeleteAllFiles()
    {
        if (Directory.Exists(_settings.BasePath))
            Directory.Delete(_settings.BasePath, recursive: true);
    }
}
