using System.Text.Json;
using AppConfig;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using DataContext;
using FunctionApp.Api;
using FunctionApp.ApiServices.AuthContext;
using FunctionApp.ApiServices.OrganizationToken;
using FunctionUtil.DI;
using ITI.Baseline.Util;
using ITI.DDD.Auth;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Settings;

[assembly: FunctionsStartup(typeof(FunctionApp.Startup))]

namespace FunctionApp;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
#if DEBUG
        AppDataContext.MigrateForDevelopment(new ConnectionStrings().AppDataContext);
#endif

        // See https://itidev.atlassian.net/browse/SLP-285
        builder.Services.Configure<JsonSerializerOptions>(o =>
        {
            o.Converters.Add(new TrimStringsJsonConverter());
        });

        builder.Services.AddDataProtection()
            .PersistKeysToDbContext<AppDataContext>();

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

        RegisterAuthServices(builder);

        builder.RegisterModule<MicrosoftLoggerModule>();
        builder.RegisterAssemblyTypes(typeof(Startup).Assembly)
            .InNamespaceOf<UserApi>();

        return builder.Build();
    }

    private static void RegisterAuthServices(ContainerBuilder builder)
    {
        builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>();

        // InstancePerLifetimeScope so we only parse the JWT once per request
        builder.RegisterType<ClaimsAccessor>().InstancePerLifetimeScope();

        builder.RegisterType<ApiAuthContext>()
           .As<IAuthContext>()
           .As<IAppAuthContext>();

        builder.RegisterType<OrganizationTokenService>().As<IOrganizationTokenService>();
        builder.RegisterType<ApiOrganizationContext>().As<IOrganizationContext>();
    }
}
