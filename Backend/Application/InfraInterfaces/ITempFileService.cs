namespace InfraInterfaces
{
    public interface ITempFileService
    {
        string GetBasePath();
        string GetTempDirectory(string directoryName);
        void DeleteOldFiles();
    }
}
