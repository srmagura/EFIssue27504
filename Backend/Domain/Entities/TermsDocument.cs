using ValueObjects;

namespace Entities;

public class TermsDocument : AggregateRoot
{
    public TermsDocument(OrganizationId organizationId, int number, FileRef file)
    {
        Require.NotNull(organizationId, "Organization ID is required.");
        OrganizationId = organizationId;

        Require.IsTrue(number >= 1, "Number must be greater than 0.");
        Number = number;

        Require.NotNull(file, "File is required.");
        File = file;
    }

    public TermsDocumentId Id { get; protected set; } = new();

    public OrganizationId OrganizationId { get; protected set; }

    public int Number { get; protected set; }

    public FileRef File { get; protected set; }

    public bool IsActive { get; protected set; } = true;

    public void SetActive(bool isActive)
    {
        IsActive = isActive;
    }
}
