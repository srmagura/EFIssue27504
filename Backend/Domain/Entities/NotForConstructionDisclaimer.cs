namespace Entities;

public class NotForConstructionDisclaimer : AggregateRoot
{
    public NotForConstructionDisclaimer(OrganizationId organizationId, string text)
    {
        Require.NotNull(organizationId, "Organization ID is required.");
        OrganizationId = organizationId;

        SetText(text);
    }

    public DefaultProjectDescriptionId Id { get; protected set; } = new();

    public OrganizationId OrganizationId { get; protected set; }

    public string Text { get; protected set; }

    [MemberNotNull(nameof(Text))]
    public void SetText(string disclaimer)
    {
        Require.NotNull(disclaimer, "Disclaimer is required.");
        Text = disclaimer;
    }
}
