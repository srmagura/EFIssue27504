using AppConfig;
using Autofac;
using AutoMapper;

namespace UnitTests.Helpers;

public static class AutoMapperUtil
{
    public static IMapper GetMapper()
    {
        var builder = new ContainerBuilder();
        builder.RegisterModule<MapperModule>();

        return builder.Build().Resolve<IMapper>();
    }
}
