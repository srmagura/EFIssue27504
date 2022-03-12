using Autofac;

namespace FunctionUtil.DI;

public sealed class LifetimeScopeWrapper : IDisposable
{
    public ILifetimeScope Scope { get; }

    public LifetimeScopeWrapper(IContainer container)
    {
        Scope = container.BeginLifetimeScope();
    }

    public void Dispose()
    {
        Scope.Dispose();
    }
}
