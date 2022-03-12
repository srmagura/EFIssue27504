using Autofac;
using InfraInterfaces;
using ITI.DDD.Application.DomainEvents.Direct;
using ITI.DDD.Auth;

namespace AppConfig;

public class DirectDomainEventPublisherLifetimeScopeProvider : IDirectDomainEventPublisherLifetimeScopeProvider
{
    private static ILifetimeScope? _rootLifetimeScope;

    public static void OnContainerBuilt(ILifetimeScope rootLifetimeScope)
    {
        _rootLifetimeScope = rootLifetimeScope;
    }

    public ILifetimeScope BeginLifetimeScope()
    {
        if (_rootLifetimeScope == null)
        {
            throw new InvalidOperationException(
                "OnContainerBuilt must be calld before BeginLifetimeScope."
            );
        }

        return _rootLifetimeScope.BeginLifetimeScope(c =>
        {
            c.RegisterType<SystemAuthContext>()
                .As<IAppAuthContext>()
                .As<IAuthContext>();
        });
    }
}
