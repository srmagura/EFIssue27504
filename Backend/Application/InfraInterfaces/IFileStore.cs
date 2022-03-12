namespace InfraInterfaces
{
    public interface IFileStore
    {
        Task PutAsync(string path, Stream inputStream);
        Task GetAsync(string path, Stream outputStream);
        Task<bool> ExistsAsync(string path);
        Task RemoveAsync(string path);
        Task CopyAsync(string fromPath, string toPath);
    }
}
