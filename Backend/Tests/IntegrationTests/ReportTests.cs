using System.Text.Json;
using AppDTOs.Designer;
using AppInterfaces.System;
using DataContext;
using DataInterfaces.Repositories;
using Entities;
using InfraInterfaces;
using ITI.DDD.Application.DomainEvents;
using ITI.DDD.Core;
using NSubstitute;
using ValueObjects;

namespace IntegrationTests;

[TestClass]
public class ReportTests : IntegrationTest
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    private async Task SetupValidReportAsync(ProjectId projectId)
    {
        var componentSvc = Container.Resolve<IComponentAppService>();
        var productKitSvc = Container.Resolve<IProductKitAppService>();
        var pageSvc = Container.Resolve<IPageAppService>();
        var designerSvc = Container.Resolve<IDesignerAppService>();

        await ImportAsync(projectId, "floorplan.pdf");
        SymbolId symbolId = await AddSymbolAsync(HostOrganizationId);
        CategoryId categoryId = await AddCategoryAsync(HostOrganizationId, symbolId);
        ComponentId componentId = await AddComponentAsync(HostOrganizationId);
        var component = await componentSvc.GetAsync(componentId);
        Assert.IsNotNull(component);

        ComponentVersionId componentVersionId = component.Versions.First().Id;
        ProductKitId productKitId = await AddProductKitAsync(HostOrganizationId, categoryId, symbolId, componentVersionId);
        var productKit = await productKitSvc.GetAsync(productKitId);
        Assert.IsNotNull(productKit);

        var placedProductKits = new PlacedProductKitDto[]
        {
            new PlacedProductKitDto(
                id: Guid.NewGuid(),
                productKitId: productKit.Id,
                position: new double[] { 0.5, 0.5 },
                rotation: 0,
                lengthInches: null
            )
        };
        var notes = new NoteDto[]
        {
            new NoteDto(
                id: Guid.NewGuid(),
                placedProductKitId: placedProductKits[0].Id,
                position: new double[] { 0.5, 0.5 },
                text: "note"
            )
        };

        var pages = await pageSvc.ListAsync(projectId, ActiveFilter.ActiveOnly);
        await AcquireDesignerLockAsync(projectId);

        await designerSvc.SetAsync(
            pages[0].Id,
            DesignerDataType.PlacedProductKits,
            JsonSerializer.Serialize(placedProductKits, JsonSerializerOptions)
        );
        await designerSvc.SetAsync(
            pages[0].Id,
            DesignerDataType.Notes,
            JsonSerializer.Serialize(notes, JsonSerializerOptions)
        );
    }

    [TestMethod]
    public async Task CanRequestDraftReport()
    {
        var builder = new ContainerBuilder();
        AddRegistrations(builder);
        builder.RegisterInstance(Substitute.For<IReportProcessor>());

        Container.Dispose();
        Container = builder.Build();

        var reportSvc = Container.Resolve<IReportAppService>();

        using var _ = new TestOrganizationSecurityScope(HostOrganizationId);

        var projectId = await AddProjectAsync(HostOrganizationId);

        var reportId = await reportSvc.RequestDraftAsync(projectId, ReportType.DrawingSet);
        var report = await reportSvc.GetAsync(reportId);
        Assert.IsNotNull(report);
    }

    [TestMethod]
    public async Task CanProcessDrawingSet()
    {
        ProjectId projectId;

        // Must be a user to acquire designer lock
        using (new TestOrganizationSecurityScope(HostOrganizationId))
        {
            projectId = await AddProjectAsync(HostOrganizationId);
            await SetupValidReportAsync(projectId);
        }

        // Wait until import is complete before registering NullDomainEventPublisher
        var builder = new ContainerBuilder();
        AddRegistrations(builder);
        builder.RegisterType<NullDomainEventPublisher>().As<IDomainEventPublisher>();

        Container.Dispose();
        Container = builder.Build();

        var projectRepository = Container.Resolve<IProjectRepository>();
        var reportRepository = Container.Resolve<IReportRepository>();
        var reportSvc = Container.Resolve<IReportAppService>();
        var reportSystemSvc = Container.Resolve<IReportSystemAppService>();
        var reportProcessor = Container.Resolve<IReportProcessor>();
        var fileStore = Container.Resolve<IFileStore>();
        var path = Container.Resolve<IFilePathBuilder>();
        var unitOfWorkProvider = Container.Resolve<IUnitOfWorkProvider>();

        using var _ = new SystemSecurityScope();

        Project? project;
        ReportId reportId;
        using (var uow = unitOfWorkProvider.Begin())
        {
            project = await projectRepository.GetAsync(projectId);
            Assert.IsNotNull(project);

            var drawingSetReport = new Report(project, projectPublication: null, ReportType.DrawingSet);
            reportId = drawingSetReport.Id;

            reportRepository.Add(drawingSetReport);
            await uow.CommitAsync();
        }

        var report = await reportSvc.GetAsync(reportId);
        Assert.IsNotNull(report);
        Assert.AreEqual(ReportStatus.Pending, report.Status);
        Assert.AreEqual(ReportType.DrawingSet, report.Type);
        Assert.AreEqual(0m, report.PercentComplete);

        await reportProcessor.ProcessAsync(report.Id, CancellationToken.None);

        var processedReport = await reportSystemSvc.GetAsync(report.Id);
        Assert.IsNotNull(processedReport);
        Assert.AreEqual(ReportStatus.Completed, processedReport.Status);
        Assert.IsNotNull(processedReport.File);

        using var stream = new MemoryStream();
        await fileStore.GetAsync(path.ForReport(processedReport.File.FileId), stream);
        AssertionUtil.StreamContainsData(stream);
    }

    [TestMethod]
    public async Task CanProcessProposal()
    {
        ProjectId projectId;

        // Must be a user to acquire designer lock
        using (new TestOrganizationSecurityScope(HostOrganizationId))
        {
            projectId = await AddProjectAsync(HostOrganizationId);
            await SetupValidReportAsync(projectId);
        }

        var builder = new ContainerBuilder();
        AddRegistrations(builder);
        builder.RegisterType<NullDomainEventPublisher>().As<IDomainEventPublisher>();

        Container.Dispose();
        Container = builder.Build();

        var projectRepository = Container.Resolve<IProjectRepository>();
        var reportRepository = Container.Resolve<IReportRepository>();
        var reportSvc = Container.Resolve<IReportAppService>();
        var reportSystemSvc = Container.Resolve<IReportSystemAppService>();
        var reportProcessor = Container.Resolve<IReportProcessor>();
        var fileStore = Container.Resolve<IFileStore>();
        var path = Container.Resolve<IFilePathBuilder>();
        var unitOfWorkProvider = Container.Resolve<IUnitOfWorkProvider>();

        using var _ = new SystemSecurityScope();

        Project? project;
        ReportId reportId;
        using (var uow = unitOfWorkProvider.Begin())
        {
            project = await projectRepository.GetAsync(projectId);
            Assert.IsNotNull(project);

            var proposalReport = new Report(project, projectPublication: null, ReportType.Proposal);
            reportId = proposalReport.Id;

            reportRepository.Add(proposalReport);
            await uow.CommitAsync();
        }

        var report = await reportSvc.GetAsync(reportId);
        Assert.IsNotNull(report);
        Assert.AreEqual(ReportStatus.Pending, report.Status);
        Assert.AreEqual(ReportType.Proposal, report.Type);
        Assert.AreEqual(0m, report.PercentComplete);

        await reportProcessor.ProcessAsync(report.Id, CancellationToken.None);

        var processedReport = await reportSystemSvc.GetAsync(report.Id);
        Assert.IsNotNull(processedReport);
        Assert.AreEqual(ReportStatus.Completed, processedReport.Status);
        Assert.IsNotNull(processedReport.File);

        using var stream = new MemoryStream();
        await fileStore.GetAsync(path.ForReport(processedReport.File.FileId), stream);
        AssertionUtil.StreamContainsData(stream);
    }

    [TestMethod]
    public async Task ItCanBeCanceledViaToken()
    {
        var builder = new ContainerBuilder();
        AddRegistrations(builder);
        builder.RegisterType<NullDomainEventPublisher>().As<IDomainEventPublisher>();

        Container.Dispose();
        Container = builder.Build();

        var projectRepository = Container.Resolve<IProjectRepository>();
        var reportRepository = Container.Resolve<IReportRepository>();
        var reportSvc = Container.Resolve<IReportAppService>();
        var reportProcessor = Container.Resolve<IReportProcessor>();
        var unitOfWorkProvider = Container.Resolve<IUnitOfWorkProvider>();

        using var _ = new SystemSecurityScope();

        Project? project;
        ProjectId projectId = await AddProjectAsync(HostOrganizationId);
        ReportId reportId;
        using (var uow = unitOfWorkProvider.Begin())
        {
            project = await projectRepository.GetAsync(projectId);
            Assert.IsNotNull(project);

            var proposalReport = new Report(project, projectPublication: null, ReportType.Proposal);
            reportId = proposalReport.Id;

            reportRepository.Add(proposalReport);
            await uow.CommitAsync();
        }

        using var cts = new CancellationTokenSource();
        cts.Cancel();

        await reportProcessor.ProcessAsync(reportId, cts.Token);

        var report = await reportSvc.GetAsync(reportId);
        Assert.IsNotNull(report);
        Assert.AreEqual(ReportStatus.Canceled, report.Status);
        Assert.IsNull(report.ErrorMessage);
        Assert.IsTrue(report.PercentComplete < new Percentage(1));
    }
}
