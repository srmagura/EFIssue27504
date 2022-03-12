using System.Diagnostics.CodeAnalysis;
using System.Net;
using FunctionApp.ApiServices.Exceptions;

namespace FunctionApp.ApiServices;

public abstract class ApiFunction
{
    private readonly IAppAuthContext _auth;
    private readonly Bugsnag.IClient _bugsnag;

    protected ApiFunction(IAppAuthContext auth, Bugsnag.IClient bugsnag)
    {
        _auth = auth;
        _bugsnag = bugsnag;
    }

    protected static void RequireParam<T>([NotNull] T param, string name)
    {
        if (param == null)
            throw new MissingRequestParameterException(name);
    }

    private async Task<IActionResult> HandleRequestCoreAsync(Func<Task<IActionResult>> func, bool allowAnonymous)
    {
        try
        {
            if (!allowAnonymous && !_auth.IsAuthenticated)
                return new StatusCodeResult((int)HttpStatusCode.Unauthorized);

            return await func();
        }
        catch (Exception e)
        {
            var result = ExceptionHandler.Handle(e);

            if (result.ReportToBugsnag)
                _bugsnag.Notify(e);

            var error = new ErrorDto(
                result.ErrorType,
                result.Message,
                diagnosticInfo: e.ToString()
            );

            return new JsonResult(error)
            {
                StatusCode = (int)result.StatusCode
            };
        }
    }

    // HandleRequest MUST BE CALLED IN THE FIRST LINE OF EVERY API FUNCTION.
    //
    // It validates that the user is authenticated and handles exceptions.

    protected Task<IActionResult> HandleRequestAsync(Func<Task> func, bool allowAnonymous = false)
    {
        return HandleRequestCoreAsync(
            async () =>
            {
                await func();
                return new NoContentResult();
            },
            allowAnonymous
        );
    }

    protected Task<IActionResult> HandleRequestAsync<T>(Func<Task<T>> func, bool allowAnonymous = false)
    {
        return HandleRequestCoreAsync(
            async () =>
            {
                var result = await func();

                if (result is IActionResult actionResult)
                    return actionResult;

                return new JsonResult(result);
            },
            allowAnonymous
        );
    }
}
