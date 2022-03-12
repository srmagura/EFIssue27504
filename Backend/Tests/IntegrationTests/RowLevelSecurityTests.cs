using DataContext;
using DbEntities;
using DbEntities.ValueObjects;
using Microsoft.EntityFrameworkCore;
using ValueObjects;

namespace IntegrationTests;

[TestClass]
public class RowLevelSecurityTests : IntegrationTest
{
    [TestMethod]
    public async Task SymbolTests()
    {
        var organizationId = await AddOrganizationAsync();

        var dbSymbol = new DbSymbol(organizationId.Guid, "mySymbol", "mySvgText");

        using (new TestOrganizationSecurityScope(organizationId))
        using (var db = Container.Resolve<AppDataContext>())
        {
            db.Symbols.Add(dbSymbol);
            await db.SaveChangesAsync();
        }

        using (new TestOrganizationSecurityScope(new OrganizationId()))
        {
            using (var db = Container.Resolve<AppDataContext>())
            {
                Assert.IsFalse(db.Symbols.Any());

                var newDbSymbol = new DbSymbol(organizationId.Guid, "mySymbol2", "mySvgText2");

                db.Symbols.Add(newDbSymbol);
                var e = Assert.ThrowsException<DbUpdateException>(() => db.SaveChanges());
                Assert.IsTrue(e.InnerException?.Message.Contains("has a block predicate that conflicts with this operation"));
            }

            using (var db = Container.Resolve<AppDataContext>())
            {
                db.Symbols.Update(dbSymbol);
                Assert.ThrowsException<DbUpdateConcurrencyException>(() => db.SaveChanges());
            }

            using (var db = Container.Resolve<AppDataContext>())
            {
                db.Symbols.Remove(dbSymbol);
                Assert.ThrowsException<DbUpdateConcurrencyException>(() => db.SaveChanges());
            }
        }
    }

    [TestMethod]
    public async Task ProjectTests()
    {
        LogoSetId logoSetId;
        TermsDocumentId termsDocumentId;
        using (new TestOrganizationSecurityScope(HostOrganizationId))
        {
            logoSetId = await AddLogoSetAsync(HostOrganizationId);
            termsDocumentId = await AddTermsDocumentAsync(HostOrganizationId);
        }

        var dbProjectReportOptions = new DbProjectReportOptions(
            "signeeName",
            "preparerName",
            logoSetId.Guid,
            termsDocumentId.Guid
        );

        var dbProject = new DbProject(
            name: "myProject",
            shortName: "myProject",
            description: "myDescription",
            address: new PartialAddress(),
            customerName: "myCustomer",
            budgetOptions: new ProjectBudgetOptions(new Percentage(0m), new Percentage(0.1m), true, true),
            reportOptions: dbProjectReportOptions
        )
        {
            OrganizationId = HostOrganizationId.Guid
        };

        using (new TestOrganizationSecurityScope(HostOrganizationId))
        using (var db = Container.Resolve<AppDataContext>())
        {
            db.Projects.Add(dbProject);
            await db.SaveChangesAsync();
        }

        using (new TestOrganizationSecurityScope(new OrganizationId()))
        {
            using (var db = Container.Resolve<AppDataContext>())
            {
                Assert.IsFalse(db.Projects.Any());

                var newDbProject = new DbProject(
                    name: "myProject2",
                    shortName: "myProject2",
                    description: "myDescription",
                    address: new PartialAddress(),
                    customerName: "myCustomer",
                    budgetOptions: new ProjectBudgetOptions(new Percentage(0m), new Percentage(0.1m), true, true),
                    reportOptions: dbProjectReportOptions
                )
                {
                    OrganizationId = HostOrganizationId.Guid
                };

                db.Projects.Add(newDbProject);
                var e = Assert.ThrowsException<DbUpdateException>(() => db.SaveChanges());
                Assert.IsTrue(e.InnerException?.Message.Contains("has a block predicate that conflicts with this operation"));
            }

            using (var db = Container.Resolve<AppDataContext>())
            {
                db.Projects.Update(dbProject);
                Assert.ThrowsException<DbUpdateConcurrencyException>(() => db.SaveChanges());
            }

            using (var db = Container.Resolve<AppDataContext>())
            {
                db.Projects.Remove(dbProject);
                Assert.ThrowsException<DbUpdateConcurrencyException>(() => db.SaveChanges());
            }
        }
    }

    [TestMethod]
    public async Task ComponentTests()
    {
        ComponentTypeId componentTypeId;

        using (new TestOrganizationSecurityScope(HostOrganizationId))
        {
            componentTypeId = await AddComponentTypeAsync(HostOrganizationId);
        }

        var dbComponent = new DbComponent
        {
            OrganizationId = HostOrganizationId.Guid,
            ComponentTypeId = componentTypeId.Guid
        };

        using (new TestOrganizationSecurityScope(HostOrganizationId))
        using (var db = Container.Resolve<AppDataContext>())
        {
            db.Components.Add(dbComponent);
            await db.SaveChangesAsync();
        }

        using (new TestOrganizationSecurityScope(new OrganizationId()))
        {
            using (var db = Container.Resolve<AppDataContext>())
            {
                Assert.IsFalse(db.Components.Any());

                var newDbComponent = new DbComponent
                {
                    OrganizationId = HostOrganizationId.Guid,
                    ComponentTypeId = componentTypeId.Guid
                };

                db.Components.Add(newDbComponent);
                var e = Assert.ThrowsException<DbUpdateException>(() => db.SaveChanges());
                Assert.IsTrue(e.InnerException?.Message.Contains("has a block predicate that conflicts with this operation"));
            }

            using (var db = Container.Resolve<AppDataContext>())
            {
                db.Components.Update(dbComponent);
                Assert.ThrowsException<DbUpdateConcurrencyException>(() => db.SaveChanges());
            }

            using (var db = Container.Resolve<AppDataContext>())
            {
                db.Components.Remove(dbComponent);
                Assert.ThrowsException<DbUpdateConcurrencyException>(() => db.SaveChanges());
            }
        }
    }
}
