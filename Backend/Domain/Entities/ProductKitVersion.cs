using Enumerations;
using ValueObjects;

namespace Entities;

public class ProductKitVersion : AggregateRoot
{
    [Obsolete("For use by AutoMapper only.")]
    public ProductKitVersion(
        OrganizationId organizationId,
        ProductKitId productKitId,
        string name,
        string description,
        string versionName,
        Money sellPrice,
        SymbolId symbolId,
        ComponentVersionId mainComponentVersionId,
        List<ProductKitComponentMap> componentMaps
    )
    {
        OrganizationId = organizationId;
        ProductKitId = productKitId;
        Name = name;
        Description = description;
        SymbolId = symbolId;
        MainComponentVersionId = mainComponentVersionId;
        VersionName = versionName;
        SellPrice = sellPrice;
        ComponentMaps = componentMaps;
    }

    public ProductKitVersion(
        OrganizationId organizationId,
        ProductKitId productKitId,
        string name,
        string description,
        string versionName,
        Symbol symbol,
        ProductPhoto? productPhoto,
        ComponentVersionId mainComponentVersionId,
        List<ProductKitComponentMap> componentMaps,
        IReadOnlyList<MeasurementType> componentMeasurementTypes,
        MeasurementType expectedMeasurementType,
        ComponentVersion[] componentVersions
    )
    {
        Require.NotNull(organizationId, "Organization ID is required.");
        OrganizationId = organizationId;

        Require.NotNull(productKitId, "Product kit ID is required.");
        ProductKitId = productKitId;

        Require.NotNull(mainComponentVersionId, "Main component version ID is required.");
        Require.IsTrue(
            componentMaps.Any(p => p.ComponentVersionId == mainComponentVersionId),
            "The main component does not correspond to one of the product kit's components."
        );
        MainComponentVersionId = mainComponentVersionId;

        Require.HasValue(versionName, "Version name is required.");
        VersionName = versionName;

        SetComponentMaps(componentMaps, componentVersions);

        var newMeasurementType = ProductKit.GetMeasurementType(componentMeasurementTypes);
        Require.IsTrue(newMeasurementType == expectedMeasurementType, "A product kit's measurement type cannot change.");

        SetName(name);
        SetDescription(description);
        SetSymbol(symbol);
        SetProductPhoto(productPhoto);
        SetSellPrice(componentVersions);
    }

    public ProductKitVersionId Id { get; protected set; } = new();

    public OrganizationId OrganizationId { get; protected set; }

    public ProductKitId ProductKitId { get; protected set; }

    public string Name { get; protected set; }
    public string Description { get; protected set; }
    public string VersionName { get; protected set; }
    public Money SellPrice { get; protected set; }

    public SymbolId SymbolId { get; protected set; }
    public ProductPhotoId? ProductPhotoId { get; protected set; }

    public ComponentVersionId MainComponentVersionId { get; protected set; }
    public List<ProductKitComponentMap> ComponentMaps { get; protected set; }

    [MemberNotNull(nameof(Name))]
    public void SetName(string name)
    {
        Require.HasValue(name, "Name is required.");
        Name = name;
    }

    [MemberNotNull(nameof(Description))]
    public void SetDescription(string description)
    {
        Require.NotNull(description, "Description cannot be null.");
        Description = description;
    }

    [MemberNotNull(nameof(SymbolId))]
    public void SetSymbol(Symbol symbol)
    {
        Require.NotNull(symbol, "Symbol is required.");
        Require.IsTrue(OrganizationId == symbol.OrganizationId, "Symbol must belong to the same organization.");
        SymbolId = symbol.Id;
    }

    public void SetProductPhoto(ProductPhoto? productPhoto)
    {
        if (productPhoto != null)
        {
            Require.IsTrue(
                OrganizationId == productPhoto.OrganizationId,
                "Product photo must belong to the same organization."
            );
            ProductPhotoId = productPhoto.Id;
        }
    }

    [MemberNotNull(nameof(SellPrice))]
    private void SetSellPrice(ComponentVersion[] componentVersions)
    {
        var sellPrice = 0m;

        var componentVersionDict = componentVersions.ToDictionary(v => v.Id);

        foreach (var map in ComponentMaps)
        {
            var componentVersion = componentVersionDict[map.ComponentVersionId];
            sellPrice += componentVersion.SellPrice.Value * map.Count;
        }

        SellPrice = new Money(sellPrice);
    }

    [MemberNotNull(nameof(ComponentMaps))]
    private void SetComponentMaps(
        List<ProductKitComponentMap> componentMaps,
        ComponentVersion[] componentVersions
    )
    {
        Require.IsTrue(
            componentMaps
                .Select(m => m.ComponentVersionId)
                .ToHashSet()
                .SetEquals(componentVersions.Select(v => v.Id)),
            "Component version mismatch."
        );

        var componentMapComponentIds = componentMaps
            .Select(map => componentVersions.First(v => v.Id == map.ComponentVersionId).ComponentId)
            .ToHashSet();

        Require.IsTrue(
            componentMapComponentIds.Count == componentMaps.Count,
            "Component maps contain multiple entries for the same component."
        );
        Require.IsTrue(componentMaps.Count > 0, "At least one component map is required.");
        Require.IsTrue(
            componentVersions.All(p => OrganizationId == p.OrganizationId),
            "Components must belong to the same organization."
        );
        ComponentMaps = componentMaps;
    }
}
