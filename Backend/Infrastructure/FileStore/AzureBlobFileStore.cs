using Azure.Storage.Blobs;
using InfraInterfaces;
using Settings;

namespace FileStore
{
    public class AzureBlobFileStore : IFileStore
    {
        private static readonly object _lock = new();
        private static bool _isInitialized = false;

        private readonly ConnectionStrings _settings;

        public AzureBlobFileStore(ConnectionStrings settings)
        {
            _settings = settings;
        }

        public async Task PutAsync(string path, Stream inputStream)
        {
            var blob = GetBlob(path);
            await blob.UploadAsync(inputStream, overwrite: true);
        }

        public async Task<bool> ExistsAsync(string path)
        {
            var blob = GetBlob(path);
            return await blob.ExistsAsync();
        }

        public async Task GetAsync(string path, Stream outputStream)
        {
            var blob = GetBlob(path);
            await blob.DownloadToAsync(outputStream);
        }

        public async Task RemoveAsync(string path)
        {
            var blob = GetBlob(path);
            await blob.DeleteIfExistsAsync();
        }

        private static BlobContainerClient CreateContainerIfNotExists(string connectionString)
        {
            var container = new BlobContainerClient(connectionString, "slateplan");

            if (!_isInitialized)
            {
                lock (_lock)
                {
                    if (!_isInitialized)
                    {
                        container.CreateIfNotExists();
                    }

                    _isInitialized = true;
                }
            }

            return container;
        }

        private BlobClient GetBlob(string blobPath)
        {
            var container = CreateContainerIfNotExists(_settings.AzureBlobStorage);

            var blob = container.GetBlobClient(blobPath);
            return blob;
        }

        public async Task CopyAsync(string fromPath, string toPath)
        {
            var fromBlob = GetBlob(fromPath);
            using var stream = fromBlob.OpenRead();

            var toBlob = GetBlob(toPath);
            await toBlob.UploadAsync(stream, overwrite: true);
        }
    }
}
