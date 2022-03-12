using ValueObjects;

namespace Entities;

public class ProductPhoto : AggregateRoot
{
    [Obsolete("Serialization only", true)]
    protected ProductPhoto() { }

    public ProductPhoto(OrganizationId organizationId, string name, FileRef photo)
    {
        OrganizationId = organizationId;
        Photo = photo;

        SetName(name);
    }

    public ProductPhotoId Id { get; protected set; } = new();

    public OrganizationId OrganizationId { get; protected set; }

    public string Name { get; protected set; }
    public FileRef Photo { get; protected set; }

    public bool IsActive { get; protected set; } = true;

    public void SetName(string name)
    {
        Require.HasValue(name, "Name is required.");
        Name = name;
    }

    public void SetActive(bool isActive)
    {
        IsActive = isActive;
    }

    public void SetPhoto(FileRef photo)
    {
        Require.NotNull(photo, "Photo is required.");
        Photo = photo;
    }
}
