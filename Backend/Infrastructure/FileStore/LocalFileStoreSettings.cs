namespace FileStore;

public class LocalFileStoreSettings
{
    public string BasePath { get; set; } =
        Path.Combine(Path.GetTempPath(), "SlateplanLocalFileStore");
}
