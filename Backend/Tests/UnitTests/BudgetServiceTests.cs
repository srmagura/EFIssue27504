using AppDTOs.Designer;
using Entities;
using Enumerations;
using Identities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Services;
using Settings;
using ValueObjects;

namespace UnitTests;

[TestClass]
public class BudgetServiceTests
{
    [TestMethod]
    public void BudgetIsCalculatedCorrectly()
    {
        var budgetService = new BudgetService();

        var organizationId = new OrganizationId();

        var project = new Project(
            organizationId,
            "name",
            "shortname",
            "description",
            new PartialAddress(),
            "customerName",
            "signeeName",
            new LogoSetId(),
            new TermsDocumentId(),
            "preparerName",
            1000,
            new ProjectSettings() { DefaultDepositPercentage = 0.25m }
        );

        var category1 = new Category(organizationId, "Category1", null, 1, new SymbolId(), "blue");
        var category2 = new Category(organizationId, "Category2", null, 1, new SymbolId(), "red");
        var categories = new Category[2]
        {
            category1,
            category2
        };


        var productKit1 = new ProductKit(
            organizationId,
            category1,
            MeasurementType.Normal,
            null
        );
        var productKit2 = new ProductKit(
            organizationId,
            category1,
            MeasurementType.Linear,
            null
        );
        var productKit3 = new ProductKit(
            organizationId,
            category2,
            MeasurementType.Normal,
            null
        );
        var productKits = new ProductKit[3]
        {
            productKit1,
            productKit2,
            productKit3
        };

        // Service isn't using anything other than sell price and name
#pragma warning disable CS0618 // Type or member is obsolete
        var productKitVersion1 = new ProductKitVersion(
            organizationId,
            productKit1.Id,
            "name",
            "description",
            "1",
            new Money(55m),
            new SymbolId(),
            new ComponentVersionId(),
            new List<ProductKitComponentMap>()
        );
        var productKitVersion2 = new ProductKitVersion(
            organizationId,
            productKit2.Id,
            "name",
            "description",
            "1",
            new Money(42m),
            new SymbolId(),
            new ComponentVersionId(),
            new List<ProductKitComponentMap>()
        );
        var productKitVersion3 = new ProductKitVersion(
            organizationId,
            productKit3.Id,
            "name",
            "description",
            "1",
            new Money(97m),
            new SymbolId(),
            new ComponentVersionId(),
            new List<ProductKitComponentMap>()
        );
        var productKitVersions = new ProductKitVersion[3]
        {
            productKitVersion1,
            productKitVersion2,
            productKitVersion3
        };
#pragma warning restore CS0618 // Type or member is obsolete

        var placedProductKit1 = new PlacedProductKitDto(
            Guid.NewGuid(),
            productKit1.Id,
            new double[] { 0, 0 },
            0,
            null
        );
        var placedProductKit2 = new PlacedProductKitDto(
            Guid.NewGuid(),
            productKit1.Id,
            new double[] { 0, 0 },
            0,
            null
        );
        var placedProductKit3 = new PlacedProductKitDto(
            Guid.NewGuid(),
            productKit2.Id,
            new double[] { 0, 0 },
            0,
            10
        );
        var placedProductKit4 = new PlacedProductKitDto(
            Guid.NewGuid(),
            productKit2.Id,
            new double[] { 0, 0 },
            0,
            10
        );
        var placedProductKit5 = new PlacedProductKitDto(
            Guid.NewGuid(),
            productKit2.Id,
            new double[] { 0, 0 },
            0,
            15
        );
        var placedProductKit6 = new PlacedProductKitDto(
            Guid.NewGuid(),
            productKit3.Id,
            new double[] { 0, 0 },
            0,
            null
        );
        var placedProductKits = new PlacedProductKitDto[6]
        {
            placedProductKit1,
            placedProductKit2,
            placedProductKit3,
            placedProductKit4,
            placedProductKit5,
            placedProductKit6,
        };

        var budget = budgetService.CalculateBudget(project, placedProductKits, categories, productKits, productKitVersions);
        Assert.IsNotNull(budget);

        Assert.AreEqual(project.Id, budget.ProjectId);
        Assert.AreEqual(1677m, budget.TotalCost);
        Assert.AreEqual(419.25m, budget.Deposit);
        Assert.AreEqual(1.677m, budget.PricePerSquareFoot);

        Assert.AreEqual(categories.Length, budget.Categories.Count);
        Assert.AreEqual(4, budget.Categories.Sum(c => c.ProductKits.Count));

        budget.Categories.ForEach(c =>
        {
            Assert.IsNotNull(c);

            var category = categories.First(p => c.CategoryId == p.Id);
            Assert.AreEqual(category.Name, c.Name);
            Assert.AreEqual(category.Color, c.Color);

            c.ProductKits.ForEach(p =>
            {
                Assert.IsNotNull(p);

                var productKit = productKits.First(q => p.ProductKitId == q.Id);
                var productKitVersion = productKitVersions.First(q => p.ProductKitId == q.ProductKitId);
                var placedProductKitQuantity = placedProductKits.Count(q => p.ProductKitId == q.ProductKitId && p.Length == q.LengthInches);
                Assert.AreEqual(productKit.CategoryId, p.CategoryId);
                Assert.AreEqual(productKitVersion.Name, p.Name);
                Assert.AreEqual(placedProductKitQuantity, p.Quantity);
                Assert.AreEqual(productKitVersion.SellPrice.Value, p.SellPrice);
                if (productKit.MeasurementType == MeasurementType.Normal) Assert.IsNull(p.Length);
                else Assert.IsNotNull(p.Length);
            });
        });
    }
}
