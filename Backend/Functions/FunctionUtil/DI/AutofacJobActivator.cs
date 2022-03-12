using Autofac;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.Host.Executors;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FunctionUtil.DI;

public class AutofacJobActivator : IJobActivatorEx
{
    public T CreateInstance<T>()
    {
        throw new NotSupportedException();
    }

    public T CreateInstance<T>(IFunctionInstanceEx functionInstance)
        where T : notnull
    {
        var lifetimeScope = functionInstance.InstanceServices.GetRequiredService<LifetimeScopeWrapper>().Scope;

        // Some dependencies of ILoggerFactory are registered after FunctionsStartup,
        // thus not allowing us to get the ILoggerFactory from Autofac container.
        var loggerFactory = functionInstance.InstanceServices.GetRequiredService<ILoggerFactory>();
        lifetimeScope.Resolve<ILoggerFactory>(
            new NamedParameter(MicrosoftLoggerModule.LoggerFactoryParam, loggerFactory)
        );

        return lifetimeScope.Resolve<T>();
    }
}
