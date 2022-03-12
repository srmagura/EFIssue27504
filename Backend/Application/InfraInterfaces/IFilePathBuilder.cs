using Identities;

namespace InfraInterfaces;

public interface IFilePathBuilder
{
    string ForProjectPhoto(FileId fileId);
    string ForProductPhoto(FileId fileId);
    string ForPage(FileId fileId);
    string ForImport(FileId fileId);
    string ForThumbnail(FileId fileId);
    string ForReport(FileId fileId);
    string ForTermsDocument(FileId fileId);
    string ForLogo(FileId fileId);
}
