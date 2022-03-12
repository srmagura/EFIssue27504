using AppConfig;
using AppDTOs;
using Autofac;
using AutoMapper;
using DataContext;
using DbEntities;
using Identities;
using InfraInterfaces;
using ITI.DDD.Application.DomainEvents.Direct;
using ITI.DDD.Auth;
using ITI.DDD.Infrastructure.DataMapping;
using ITI.DDD.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Reports;
using TestUtilities;

namespace DevTests;

[Ignore] // These tests run against the real database
[TestClass]
public class ReportBuilderTests
{
    private const string OrganizationShortName = "system7";
    private const string SystemAdminEmail = "System7Admin@example2.com";
    private static readonly ProjectId ProjectId = new(Guid.Parse("0bf42d22-5e77-4a68-bcf1-ae3c010522de"));

    private static IContainer GetContainer()
    {
        DirectDomainEventPublisher.ShouldWaitForHandlersToComplete(true);

        var builder = new ContainerBuilder();
        builder.RegisterModule(new AppModule(null));

        builder.RegisterType<TestAuthContext>().As<IAppAuthContext>().As<IAuthContext>();
        builder.RegisterType<TestScopedOrganizationContext>().As<IOrganizationContext>();

        builder.RegisterType<ConsoleLogWriter>().As<ILogWriter>();

        builder.RegisterInstance(Substitute.For<IReportProcessor>());

        return builder.Build();
    }

    private static Stream GetOutputStream(string reportFilename)
    {
        var outputDirectory = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            nameof(ReportBuilderTests)
        );
        Directory.CreateDirectory(outputDirectory);

        var path = Path.Combine(outputDirectory, reportFilename);

        if (File.Exists(path)) File.Delete(path);
        return File.OpenWrite(path);
    }

    private static async Task<OrganizationId> AuthenticateOrganizationAndUser(IContainer container)
    {
        var mapper = container.Resolve<IMapper>();

        OrganizationId organizationId;
        using var db = container.Resolve<AppDataContext>();

        organizationId = db.Organizations
            .Where(o => o.ShortName.Value == OrganizationShortName)
            .Select(o => new OrganizationId(o.Id))
            .First();

        var user = await db.Users
            .Where(u => u.Email.Value == SystemAdminEmail)
            .ProjectToDtoAsync<DbUser, UserDto>(mapper);

        if (user == null)
            throw new Exception("User is null.");

        TestAuthContext.SetUser(user);

        return organizationId;
    }

    [TestMethod]
    public async Task DrawingSet()
    {
        var container = GetContainer();
        var fileStore = container.Resolve<IFileStore>();
        var path = container.Resolve<IFilePathBuilder>();
        var drawingSetReportBuilder = container.Resolve<DrawingSetReportBuilder>();

        var organizationId = await AuthenticateOrganizationAndUser(container);

        using var _ = new TestOrganizationSecurityScope(organizationId);

        var drawingSetFilePath = path.ForReport(new FileId());
        using var outputStream = GetOutputStream("DrawingSet.pdf");

        await drawingSetReportBuilder.BuildAsync(
            ProjectId,
            drawingSetFilePath,
            percentComplete => Task.CompletedTask,
            true,
            devGenerateHtml: true,
            CancellationToken.None
        );
        await fileStore.GetAsync(drawingSetFilePath, outputStream);
    }

    [TestMethod]
    public async Task Proposal()
    {
        var container = GetContainer();
        var fileStore = container.Resolve<IFileStore>();
        var path = container.Resolve<IFilePathBuilder>();
        var proposalReportBuilder = container.Resolve<ProposalReportBuilder>();

        var organizationId = await AuthenticateOrganizationAndUser(container);

        using var _ = new TestOrganizationSecurityScope(organizationId);

        var proposalFilePath = path.ForReport(new FileId());
        using var outputStream = GetOutputStream("Proposal.pdf");

        await proposalReportBuilder.BuildAsync(
            ProjectId,
            proposalFilePath,
            percentComplete => Task.CompletedTask,
            true,
            devGenerateHtml: true,
            CancellationToken.None
        );
        await fileStore.GetAsync(proposalFilePath, outputStream);
    }
}
