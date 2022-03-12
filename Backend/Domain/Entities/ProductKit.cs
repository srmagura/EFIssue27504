using Enumerations;

namespace Entities;

public class ProductKit : AggregateRoot
{
    [Obsolete("AutoMapper only.")]
    public ProductKit(OrganizationId organizationId, CategoryId categoryId, ProductFamilyId? productFamilyId)
    {
        OrganizationId = organizationId;
        CategoryId = categoryId;
        ProductFamilyId = productFamilyId;
    }

    public ProductKit(OrganizationId organizationId, Category category, MeasurementType measurementType, ProductFamily? productFamily)
    {
        Require.NotNull(organizationId, "Organization ID is required.");
        OrganizationId = organizationId;

        MeasurementType = measurementType;

        SetCategory(category);
        SetProductFamily(productFamily);
    }

    public ProductKitId Id { get; protected set; } = new();

    public OrganizationId OrganizationId { get; protected set; }

    public CategoryId CategoryId { get; protected set; }

    public MeasurementType MeasurementType { get; protected set; }

    public ProductFamilyId? ProductFamilyId { get; protected set; }

    public bool IsActive { get; protected set; } = true;

    public void SetActive(bool active)
    {
        IsActive = active;
    }

    [MemberNotNull(nameof(CategoryId))]
    public void SetCategory(Category category)
    {
        Require.NotNull(category, "Category is required.");
        Require.IsTrue(OrganizationId == category.OrganizationId, "Category must belong to the same organization.");
        CategoryId = category.Id;
    }

    public void SetProductFamily(ProductFamily? productFamily)
    {
        if (productFamily != null)
        {
            Require.IsTrue(
                OrganizationId == productFamily.OrganizationId,
                "Product family must belong to the same organization."
            );
            ProductFamilyId = productFamily.Id;
        }
        else
        {
            ProductFamilyId = null;
        }
    }

    public static MeasurementType GetMeasurementType(IReadOnlyList<MeasurementType> measurementTypes)
    {
        return measurementTypes.Any(m => m == MeasurementType.Linear)
            ? MeasurementType.Linear
            : MeasurementType.Normal;
    }

    public static bool IsVideoDisplay(IReadOnlyList<bool> componentsIsVideoDisplay)
    {
        return componentsIsVideoDisplay.Any(b => b);
    }
}
