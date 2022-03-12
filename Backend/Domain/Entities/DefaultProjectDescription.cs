namespace Entities;

public class DefaultProjectDescription : AggregateRoot
{
    public DefaultProjectDescription(OrganizationId organizationId, string text)
    {
        Require.NotNull(organizationId, "Organization ID is required.");
        OrganizationId = organizationId;

        SetText(text);
    }

    public DefaultProjectDescriptionId Id { get; protected set; } = new();

    public OrganizationId OrganizationId { get; protected set; }

    public string Text { get; protected set; }

    [MemberNotNull(nameof(Text))]
    public void SetText(string text)
    {
        Require.NotNull(text, "Text is required.");
        Text = text;
    }
}
