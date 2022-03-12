using System.Net;
using FunctionApp.ApiServices.Exceptions;
using ITI.DDD.Auth;
using ITI.DDD.Core;

namespace FunctionApp.ApiServices;

internal record ExceptionHandlerResult(
    ErrorDtoType ErrorType,
    string Message,
    HttpStatusCode StatusCode,
    bool ReportToBugsnag
);

internal static class ExceptionHandler
{
    internal static ExceptionHandlerResult Handle(Exception e)
    {
        return e switch
        {
            NotAuthorizedException _ =>
                new ExceptionHandlerResult(
                    ErrorDtoType.NotAuthorized,
                    "You are not authorized to perform this action.",
                    HttpStatusCode.Forbidden,
                    ReportToBugsnag: false
                ),
            LoginInvalidCredentialsException _ =>
                new ExceptionHandlerResult(
                    ErrorDtoType.LoginInvalidCredentials,
                    "Login credentials were invalid.",
                    HttpStatusCode.BadRequest,
                    ReportToBugsnag: false
                ),
            LoginPasswordNotSetException _ =>
                new ExceptionHandlerResult(
                    ErrorDtoType.LoginPasswordNotSet,
                    "Your password has not been set. Please check your email.",
                    HttpStatusCode.Forbidden,
                    ReportToBugsnag: false
                ),
            LoginUserDisabledException _ =>
                new ExceptionHandlerResult(
                    ErrorDtoType.LoginUserDisabled,
                    "Your user account has been disabled.",
                    HttpStatusCode.Forbidden,
                    ReportToBugsnag: false
                ),
            UserDoesNotExistException _ =>
                new ExceptionHandlerResult(
                    ErrorDtoType.UserDoesNotExist,
                    "The requested user does not exist.",
                    HttpStatusCode.InternalServerError,
                    ReportToBugsnag: false
                ),
            MissingRequestParameterException _ =>
                new ExceptionHandlerResult(
                    ErrorDtoType.InternalServerError,
                    "The request to the server was invalid.",
                    HttpStatusCode.BadRequest,
                    ReportToBugsnag: true
                ),
            UserPresentableException _ =>
                new ExceptionHandlerResult(
                    ErrorDtoType.InternalServerError,
                    e.Message,
                    HttpStatusCode.InternalServerError,
                    ReportToBugsnag: false
                ),
            DomainException _ =>
                new ExceptionHandlerResult(
                    ErrorDtoType.InternalServerError,
                    e.Message,
                    HttpStatusCode.InternalServerError,

                    // app service will have already logged if exception should be logged
                    ReportToBugsnag: false
                ),
            _ =>
                new ExceptionHandlerResult(
                    ErrorDtoType.InternalServerError,
                    "There was an unexpected error.",
                    HttpStatusCode.InternalServerError,
                    ReportToBugsnag: true
                ),
        };
    }
}
