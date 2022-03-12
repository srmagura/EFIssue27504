namespace AppServices.System;

public abstract class SystemApplicationService : ApplicationService
{
    private readonly IAppAuthContext _auth;

    protected SystemApplicationService(
        IUnitOfWorkProvider uowp,
        ILogger logger,
        IAppAuthContext auth
    ) : base(uowp, logger, auth)
    {
        _auth = auth;
    }

    protected Task<T> QueryAsync<T>(Func<Task<T>> exec)
    {
        return QueryAsync(
            () =>
            {
                Authorize.Require(_auth.IsSystemProcess);
                return Task.CompletedTask;
            },
            exec
        );
    }

    protected Task CommandAsync(Func<Task> exec)
    {
        return CommandAsync(
            () =>
            {
                Authorize.Require(_auth.IsSystemProcess);
                return Task.CompletedTask;
            },
            exec
        );
    }

    protected Task<T> CommandAsync<T>(Func<Task<T>> exec)
    {
        return CommandAsync(
            () =>
            {
                Authorize.Require(_auth.IsSystemProcess);
                return Task.CompletedTask;
            },
            exec
        );
    }
}
