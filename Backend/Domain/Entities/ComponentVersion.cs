using ITI.DDD.Core;
using ValueObjects;

namespace Entities;

public class ComponentVersion : AggregateRoot
{
    [Obsolete("AutoMapper only.")]
    public ComponentVersion(
        OrganizationId organizationId,
        ComponentId componentId,
        string displayName,
        string versionName,
        Money sellPrice,
        string make,
        string model,
        string vendorPartNumber
    )
    {
        OrganizationId = organizationId;
        ComponentId = componentId;
        DisplayName = displayName;
        VersionName = versionName;
        SellPrice = sellPrice;
        Make = make;
        Model = model;
        VendorPartNumber = vendorPartNumber;
    }

    public ComponentVersion(
        OrganizationId organizationId,
        Component component,
        string displayName,
        string versionName,
        Money sellPrice,
        Url? url,
        string make,
        string model,
        string vendorPartNumber,
        string? organizationPartNumber,
        string? whereToBuy,
        string? style,
        string? color,
        string? internalNotes
    )
    {
        Require.NotNull(organizationId, "Organization ID is required.");
        OrganizationId = organizationId;

        Require.NotNull(component, "Component is required.");
        Require.IsTrue(organizationId == component.OrganizationId, "Component must belong to the same organization.");
        ComponentId = component.Id;

        Require.HasValue(versionName, "Version name is required.");
        VersionName = versionName;

        Require.IsTrue(sellPrice > 0, "Sell price must be greater than 0.");
        SellPrice = sellPrice;

        SetDisplayName(displayName);
        SetUrl(url);
        SetMake(make);
        SetModel(model);
        SetVendorPartNumber(vendorPartNumber);
        SetOrganizationPartNumber(organizationPartNumber);
        SetWhereToBuy(whereToBuy);
        SetStyle(style);
        SetColor(color);
        SetInternalNotes(internalNotes);
    }

    public ComponentVersionId Id { get; protected set; } = new();

    public ComponentId ComponentId { get; protected set; }

    public OrganizationId OrganizationId { get; protected set; }

    public string DisplayName { get; protected set; }
    public string VersionName { get; protected set; }

    public Money SellPrice { get; protected set; }
    public Url? Url { get; protected set; }

    public string Make { get; protected set; }
    public string Model { get; protected set; }
    public string VendorPartNumber { get; protected set; }
    public string? OrganizationPartNumber { get; protected set; }
    public string? WhereToBuy { get; protected set; }
    public string? Style { get; protected set; }
    public string? Color { get; protected set; }
    public string? InternalNotes { get; protected set; }

    [MemberNotNull(nameof(DisplayName))]
    public void SetDisplayName(string displayName)
    {
        Require.HasValue(displayName, "Display name is required.");
        DisplayName = displayName;
    }

    public void SetUrl(Url? url)
    {
        Url = url;
    }

    [MemberNotNull(nameof(Make))]
    public void SetMake(string make)
    {
        Require.HasValue(make, "Make is required.");
        Make = make;
    }

    [MemberNotNull(nameof(Model))]
    public void SetModel(string model)
    {
        Require.HasValue(model, "Model is required.");
        Model = model;
    }

    [MemberNotNull(nameof(VendorPartNumber))]
    public void SetVendorPartNumber(string vendorPartNumber)
    {
        Require.HasValue(vendorPartNumber, "Vendor part number is required.");
        VendorPartNumber = vendorPartNumber;
    }

    public void SetOrganizationPartNumber(string? organizationPartNumber)
    {
        OrganizationPartNumber = organizationPartNumber.HasValue() ? organizationPartNumber : null;
    }

    public void SetWhereToBuy(string? whereToBuy)
    {
        WhereToBuy = whereToBuy.HasValue() ? whereToBuy : null;
    }

    public void SetStyle(string? style)
    {
        Style = style.HasValue() ? style : null;
    }

    public void SetColor(string? color)
    {
        Color = color.HasValue() ? color : null;
    }

    public void SetInternalNotes(string? internalNotes)
    {
        InternalNotes = internalNotes.HasValue() ? internalNotes : null;
    }
}
