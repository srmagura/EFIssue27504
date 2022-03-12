using DataContext;
using InfraInterfaces;
using ITI.DDD.Application.DomainEvents;
using Microsoft.EntityFrameworkCore;
using NSubstitute;

namespace IntegrationTests;

[TestClass]
public class ProjectPublicationTests : IntegrationTest
{
    [TestMethod]
    public async Task Crud()
    {
        var builder = new ContainerBuilder();
        AddRegistrations(builder);

        // Prevent ReportProcessor from running
        builder.RegisterInstance<IDomainEventPublisher>(new NullDomainEventPublisher());

        Container.Dispose();
        Container = builder.Build();

        var auth = Container.Resolve<IAppAuthContext>();
        var projectPublicationSvc = Container.Resolve<IProjectPublicationAppService>();

        using var _ = new TestOrganizationSecurityScope(HostOrganizationId);

        var projectId = await AddProjectAsync(HostOrganizationId);
        await projectPublicationSvc.PublishAsync(projectId);

        var projectPublication = (await projectPublicationSvc.ListAsync(projectId)).Single();
        Assert.AreEqual(auth.UserId, projectPublication.PublishedBy.Id);
        Assert.AreEqual(1, projectPublication.RevisionNumber);
        Assert.IsFalse(projectPublication.ReportsSentToCustomer);

        await projectPublicationSvc.SetReportsSentToCustomerAsync(projectPublication.Id, true);

        projectPublication = (await projectPublicationSvc.ListAsync(projectId)).Single();
        Assert.IsTrue(projectPublication.ReportsSentToCustomer);
    }

    [TestMethod]
    public async Task PublishCausesReportProcessorToRun()
    {
        var builder = new ContainerBuilder();
        AddRegistrations(builder);

        var reportProcessor = Substitute.For<IReportProcessor>();
        builder.RegisterInstance(reportProcessor);

        Container.Dispose();
        Container = builder.Build();

        var projectPublicationSvc = Container.Resolve<IProjectPublicationAppService>();

        using var _ = new TestOrganizationSecurityScope(HostOrganizationId);

        var projectId = await AddProjectAsync(HostOrganizationId);
        await projectPublicationSvc.PublishAsync(projectId);

        using var db = Container.Resolve<AppDataContext>();

        var reports = await db.Reports.ToArrayAsync();
        Assert.AreEqual(2, reports.Length);
        Assert.IsTrue(reports.All(r => r.ProjectPublicationId != null));
        Assert.IsTrue(reports.Any(r => r.Type == ReportType.DrawingSet));
        Assert.IsTrue(reports.Any(r => r.Type == ReportType.Proposal));

        foreach (var report in reports)
        {
            await reportProcessor.Received().ProcessAsync(
                new ReportId(report.Id),
                Arg.Any<CancellationToken>()
            );
        }
    }
}
