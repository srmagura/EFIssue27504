using DataContext;
using EventHandlers;
using Events;
using FileStore;
using ITI.DDD.Application.DomainEvents;
using ITI.DDD.Core;
using ValueObjects;

namespace IntegrationTests;

[TestClass]
public class ImportTests : IntegrationTest
{
    [TestMethod]
    public async Task ItCanBeCanceledViaToken()
    {
        var builder = new ContainerBuilder();
        AddRegistrations(builder);
        builder.RegisterType<NullDomainEventPublisher>().As<IDomainEventPublisher>();

        Container.Dispose();
        Container = builder.Build();

        var importSvc = Container.Resolve<IImportAppService>();
        var importEventHandler = Container.Resolve<ImportEventHandler>();

        using var _ = new SystemSecurityScope();

        var projectId = await AddProjectAsync(HostOrganizationId);

        var filename = "floorplan.pdf";
        ImportId importId;

        using (var stream = GetResourceStream(filename))
        {
            importId = await importSvc.ImportAsync(projectId, filename, stream);
        }

        using var cts = new CancellationTokenSource();
        cts.Cancel();

        await importEventHandler.HandleAsync(new ImportAddedEvent(importId), cts.Token);

        var import = (await importSvc.ListAsync(projectId, skip: 0, take: 1)).Single();
        Assert.AreEqual(ImportStatus.Canceled, import.Status);
        Assert.IsNull(import.ErrorMessage);
        Assert.IsTrue(import.PercentComplete < new Percentage(1));

        Assert.AreEqual(0, InMemoryFileStore.ListFiles(FilePathBuilder.ImportsPath).Count);
    }

    [TestMethod]
    public async Task ItValidatesPageDimensions()
    {
        var builder = new ContainerBuilder();
        AddRegistrations(builder);
        builder.RegisterType<NullDomainEventPublisher>().As<IDomainEventPublisher>();

        Container.Dispose();
        Container = builder.Build();

        var importSvc = Container.Resolve<IImportAppService>();
        var importEventHandler = Container.Resolve<ImportEventHandler>();

        using var _ = new SystemSecurityScope();

        var projectId = await AddProjectAsync(HostOrganizationId);

        var filename = "mishapedpdf.pdf";
        ImportId importId;

        using (var stream = GetResourceStream(filename))
        {
            importId = await importSvc.ImportAsync(projectId, filename, stream);
        }

        var expectedMessage = "The PDF contains a page that is not 11x17.";

        var e = await AssertionUtil.ThrowsExceptionAsync<DomainException>(
            () => importEventHandler.HandleAsync(new ImportAddedEvent(importId))
        );
        Assert.AreEqual(expectedMessage, e.Message);

        var import = (await importSvc.ListAsync(projectId, skip: 0, take: 1)).Single();
        Assert.AreEqual(ImportStatus.Error, import.Status);
        Assert.AreEqual(expectedMessage, import.ErrorMessage);

        Assert.AreEqual(0, InMemoryFileStore.ListFiles(FilePathBuilder.ImportsPath).Count);
    }
}
