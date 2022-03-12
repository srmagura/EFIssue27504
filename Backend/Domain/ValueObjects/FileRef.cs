using Identities;
using ITI.Baseline.Util;

namespace ValueObjects;

public record FileRef
{
    public FileRef(FileId fileId, string fileType)
    {
        Require.NotNull(fileId, "File ID is null.");
        FileId = fileId;

        Require.HasValue(fileType, "File type is required.");
        FileType = fileType;
    }

    public FileId FileId { get; protected init; }
    public string FileType { get; protected init; }
}
