using Autofac;
using Microsoft.Extensions.Logging;

namespace FunctionUtil.DI;

public class MicrosoftLoggerModule : Module
{
    public const string LoggerFactoryParam = "loggerFactory";

    protected override void Load(ContainerBuilder builder)
    {
        builder.Register((ctx, p) => p.Named<ILoggerFactory>(LoggerFactoryParam))
            .SingleInstance();
    }
}
