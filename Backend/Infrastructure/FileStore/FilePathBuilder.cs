using Identities;
using InfraInterfaces;

namespace FileStore;

public class FilePathBuilder : IFilePathBuilder
{
    private static string PathFor(string type, FileId fileId)
    {
        return $"{type}/{fileId.Guid}";
    }

    public string ForProjectPhoto(FileId fileId)
    {
        return PathFor("projectPhotos", fileId);
    }

    public string ForProductPhoto(FileId fileId)
    {
        return PathFor("productPhotos", fileId);
    }

    public string ForPage(FileId fileId)
    {
        return PathFor("pages", fileId);
    }

    internal const string ImportsPath = "imports";

    public string ForImport(FileId fileId)
    {
        return PathFor(ImportsPath, fileId);
    }

    public string ForThumbnail(FileId fileId)
    {
        return PathFor("thumbnails", fileId);
    }

    public string ForReport(FileId fileId)
    {
        return PathFor("reports", fileId);
    }

    public string ForTermsDocument(FileId fileId)
    {
        return PathFor("termsDocuments", fileId);
    }

    public string ForLogo(FileId fileId)
    {
        return PathFor("logos", fileId);
    }
}
