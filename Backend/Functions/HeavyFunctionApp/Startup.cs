using AppConfig;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using DataContext;
using FunctionUtil.DI;
using HeavyFunctionApp.Functions;
using InfraInterfaces;
using ITI.DDD.Auth;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Settings;

[assembly: FunctionsStartup(typeof(HeavyFunctionApp.Startup))]

namespace HeavyFunctionApp;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
#if DEBUG
        AppDataContext.MigrateForDevelopment(new ConnectionStrings().AppDataContext);
#endif

        builder.Services.AddSingleton(GetContainer(builder.Services));
        builder.Services.AddScoped<LifetimeScopeWrapper>();

        builder.Services.Replace(ServiceDescriptor.Singleton(typeof(IJobActivator), typeof(AutofacJobActivator)));
        builder.Services.Replace(ServiceDescriptor.Singleton(typeof(IJobActivatorEx), typeof(AutofacJobActivator)));
    }

    private static IContainer GetContainer(IServiceCollection serviceCollection)
    {
        var builder = new ContainerBuilder();
        builder.Populate(serviceCollection);

        var configuration = new ConfigurationBuilder().AddEnvironmentVariables().Build();

        var configurationLoader = new ConfigurationLoader(configuration);
        builder.RegisterModule(new AppModule(configurationLoader));

        builder.RegisterType<SystemAuthContext>()
            .As<IAppAuthContext>()
            .As<IAuthContext>();

        builder.RegisterType<SystemOrganizationContext>().As<IOrganizationContext>();

        builder.RegisterAssemblyTypes(typeof(Startup).Assembly)
            .InNamespaceOf<ImportEventHandlerFunctions>();

        builder.RegisterModule<MicrosoftLoggerModule>();

        return builder.Build();
    }
}
