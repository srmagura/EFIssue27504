namespace Identities;

public record OrganizationId : Identity
{
    public OrganizationId() { }
    public OrganizationId(Guid guid) : base(guid) { }
    public OrganizationId(Guid? guid) : base(guid ?? Guid.Empty) { }
}
