using FunctionApp.ApiServices;
using FunctionApp.ApiServices.AuthContext;
using FunctionApp.ApiServices.Exceptions;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Settings;

namespace FunctionApp.Api;

public class UserApi : ApiFunction
{
    private readonly IUserAppService _userAppService;
    private readonly IAppAuthContext _auth;
    private readonly ApiAuthenticationSettings _apiAuthenticationSettings;

    public UserApi(
        IUserAppService userAppService,
        IAppAuthContext auth,
        ApiAuthenticationSettings websiteSettings,
        Bugsnag.IClient bugsnag
    ) : base(auth, bugsnag)
    {
        _userAppService = userAppService;
        _auth = auth;
        _apiAuthenticationSettings = websiteSettings;
    }

    public class GetRequestParams
    {
        public Guid? Id { get; set; }
    }

    [FunctionName("api_user_get")]
    public Task<IActionResult> GetAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "user/get")]
        GetRequestParams @params
    )
    {
        return HandleRequestAsync(async () =>
        {
            RequireParam(@params.Id, nameof(@params.Id));

            return await _userAppService.GetAsync(new UserId(@params.Id))
                ?? throw new UserPresentableException("The requested user does not exist.");
        });
    }

    [FunctionName("api_user_me")]
    public Task<IActionResult> Me(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "user/me")]
        HttpRequest request
    )
    {
        return HandleRequestAsync(async () =>
        {
            if (_auth.UserId == null)
                throw new Exception("User ID is unexpectedly null.");

            return await _userAppService.GetAsync(_auth.UserId)
                ?? throw new UserDoesNotExistException();
        });
    }

    public class ListRequestParams
    {
        public Guid? OrganizationId { get; set; }
        public int? Skip { get; set; }
        public int? Take { get; set; }
        public int? ActiveFilter { get; set; }
        public string? Search { get; set; }
    }

    [FunctionName("api_user_list")]
    public Task<IActionResult> ListAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "user/list")]
        ListRequestParams @params
    )
    {
        return HandleRequestAsync(async () =>
        {
            RequireParam(@params.OrganizationId, nameof(@params.OrganizationId));
            RequireParam(@params.Skip, nameof(@params.Skip));
            RequireParam(@params.Take, nameof(@params.Take));
            RequireParam(@params.ActiveFilter, nameof(@params.ActiveFilter));

            return await _userAppService.ListAsync(
                new OrganizationId(@params.OrganizationId),
                @params.Skip.Value,
                @params.Take.Value,
                (ActiveFilter)@params.ActiveFilter.Value,
                @params.Search
            );
        });
    }

    public class UserEmailIsAvailableRequestParams
    {
        public string? Email { get; set; }
    }

    [FunctionName("api_user_userEmailIsAvailable")]
    public Task<IActionResult> UserEmailIsAvailableAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "user/userEmailIsAvailable")]
        UserEmailIsAvailableRequestParams @params
    )
    {
        return HandleRequestAsync(async () =>
        {
            RequireParam(@params.Email, nameof(@params.Email));

            return await _userAppService.UserEmailIsAvailableAsync(new EmailAddressDto(@params.Email));
        });
    }

    public record LogInRequestBody(string Email, string Password);

    [FunctionName("api_user_logIn")]
    public Task<IActionResult> LogIn(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "user/logIn")]
        LogInRequestBody body
    )
    {
        return HandleRequestAsync(
            allowAnonymous: true,
            func: async () =>
            {
                var loginResult = await _userAppService.LogInAsync(new EmailAddressDto(body.Email), body.Password);

                UserDto user = loginResult.Result switch
                {
                    LogInResult.Success => loginResult.User!,
                    LogInResult.InvalidCredentials => throw new LoginInvalidCredentialsException(),
                    LogInResult.PasswordNotSet => throw new LoginPasswordNotSetException(),
                    LogInResult.InactiveUser or LogInResult.InactiveOrganization => throw new LoginUserDisabledException(),
                    _ => throw new UserPresentableException("Log in failed for an unknown reason."),
                };

                // Add the jti (random nonce), iat (issued timestamp), and sub (subject/user) claims.
                var claims = new Dictionary<string, object>
                {
                    [JwtRegisteredClaimNames.Sub] = user.Id.Guid.ToString(),
                    [JwtRegisteredClaimNames.Jti] = Guid.NewGuid().ToString(),
                    [JwtRegisteredClaimNames.Iat] = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),
                    [SlateplanClaimTypes.UserName] = user.Name.ToString(),
                    [SlateplanClaimTypes.OrganizationId] = user.Organization.Id.Guid.ToString(),
                    [SlateplanClaimTypes.Role] = ((int)user.Role).ToString(),
                };

                var expires = DateTime.UtcNow.AddDays(_apiAuthenticationSettings.AccessTokenExpiresAfterDays);

                var accessToken = new JsonWebTokenHandler().CreateToken(new SecurityTokenDescriptor
                {
                    Claims = claims,
                    NotBefore = DateTime.UtcNow,
                    Expires = expires,
                    SigningCredentials = new SigningCredentials(
                        _apiAuthenticationSettings.TokenSecurityKey,
                        _apiAuthenticationSettings.TokenAlgorithm
                    )
                });

                var expiresUtc = new DateTimeOffset(expires, TimeSpan.Zero);
                var softExpiresUtc = DateTimeOffset.UtcNow.AddDays(_apiAuthenticationSettings.AccessTokenSoftExpiresAfterDays);

                return new UserLogInDto(
                    accessToken,
                    expiresUtc,
                    softExpiresUtc
                );
            }
        );
    }

    public record AddRequestBody(
        OrganizationId OrganizationId,
        EmailAddressDto Email,
        PersonNameDto Name,
        UserRole Role,
        string Password
    );

    [FunctionName("api_user_add")]
    public Task<IActionResult> AddAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "user/add")]
        AddRequestBody body
    )
    {
        return HandleRequestAsync(async () =>
        {
            return await _userAppService.AddAsync(
                body.OrganizationId,
                body.Email,
                body.Name,
                body.Role,
                body.Password
            );
        });
    }

    public record SetNameRequestBody(
        UserId Id,
        PersonNameDto Name
    );

    [FunctionName("api_user_setName")]
    public Task<IActionResult> SetNameAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "user/setName")]
        SetNameRequestBody body
    )
    {
        return HandleRequestAsync(async () =>
        {
            await _userAppService.SetNameAsync(body.Id, body.Name);
        });
    }

    public record SetActiveRequestBody(
        UserId Id,
        bool Active
    );

    [FunctionName("api_user_setActive")]
    public Task<IActionResult> SetActiveAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "user/setActive")]
        SetActiveRequestBody body
    )
    {
        return HandleRequestAsync(async () =>
        {
            await _userAppService.SetActiveAsync(body.Id, body.Active);
        });
    }

    public record SetRoleRequestBody(
        UserId Id,
        UserRole Role
    );

    [FunctionName("api_user_setRole")]
    public Task<IActionResult> SetRoleAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "user/setRole")]
        SetRoleRequestBody body
    )
    {
        return HandleRequestAsync(async () =>
        {
            await _userAppService.SetRoleAsync(body.Id, body.Role);
        });
    }

    public record ChangePasswordRequestBody(
        UserId Id,
        string CurrentPassword,
        string NewPassword
    );

    [FunctionName("api_user_changePassword")]
    public Task<IActionResult> ChangePasswordAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "user/changePassword")]
        ChangePasswordRequestBody body
    )
    {
        return HandleRequestAsync(async () =>
        {
            await _userAppService.ChangePasswordAsync(
                body.Id,
                body.CurrentPassword,
                body.NewPassword
            );
        });
    }

    public record SetPasswordRequestBody(
        UserId Id,
        string NewPassword
    );

    [FunctionName("api_user_setPassword")]
    public Task<IActionResult> SetPasswordAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "user/setPassword")]
        SetPasswordRequestBody body
    )
    {
        return HandleRequestAsync(async () =>
        {
            await _userAppService.SetPasswordAsync(body.Id, body.NewPassword);
        });
    }
}
