using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AppDTOs;
using AppDTOs.Enumerations;
using AppInterfaces;
using Enumerations;
using Identities;
using InfraInterfaces;
using ITI.Baseline.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Settings;
using WebDTOs;
using Website.Code.AuthContext;
using Website.Code.Exceptions;

namespace Website.Controllers
{
    [Route("api/[Controller]/[Action]")]
    public class UserController : ControllerBase
    {
        private readonly IUserAppService _userAppService;
        private readonly IAppAuthContext _auth;
        private readonly ApiAuthenticationSettings _websiteSettings;

        public UserController(
            IUserAppService userAppService,
            IAppAuthContext auth,
            ApiAuthenticationSettings websiteSettings
        )
        {
            _userAppService = userAppService;
            _auth = auth;
            _websiteSettings = websiteSettings;
        }

        public class LogInRequestBody
        {
            public EmailAddressDto Email { get; set; }
            public string Password { get; set; }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<UserLogInDto> LogIn([FromBody] LogInRequestBody body)
        {
            var loginResult = await _userAppService.LogInAsync(body.Email, body.Password);

            UserDto user = loginResult.Result switch
            {
                LogInResult.Success => loginResult.User,
                LogInResult.InvalidCredentials => throw new LoginInvalidCredentialsException(),
                LogInResult.PasswordNotSet => throw new LoginPasswordNotSetException(),
                LogInResult.InactiveUser or LogInResult.InactiveOrganization => throw new LoginUserDisabledException(),
                _ => throw new UserPresentableException("Log In Failed (Unknown Reason)."),
            };

            // Add the jti (random nonce), iat (issued timestamp), and sub (subject/user) claims.
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.Guid.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
                new Claim(SlateplanClaimTypes.UserName, user.Name.ToString()),
                new Claim(SlateplanClaimTypes.OrganizationId, user.Organization.Id.Guid.ToString()),
                new Claim(SlateplanClaimTypes.Role, ((int)user.Role).ToString()),
            };

            var signingCredentials = new SigningCredentials(_websiteSettings.TokenSecurityKey, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.Add(TimeSpan.FromDays(_websiteSettings.AccessTokenExpiresAfterDays));

            // Create the JWT and write it to a string
            var jwt = new JwtSecurityToken(
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: expires,
                signingCredentials: signingCredentials);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return new UserLogInDto(encodedJwt, new DateTimeOffset(expires, TimeSpan.Zero));
        }

        [HttpGet]
        public async Task<UserDto> Get(Guid id)
        {
            return await _userAppService.GetAsync(new UserId(id))
                ?? throw new UserDoesNotExistException();
        }

        [HttpGet]
        public Task<UserDto> Me()
        {
            if (_auth.UserId == null)
                throw new Exception("User ID is unexpectedly null.");

            return Get(_auth.UserId.Guid);
        }

        [HttpGet]
        public Task<FilteredList<UserSummaryDto>> List(
            Guid organizationId,
            int skip,
            int take,
            ActiveFilter activeFilter,
            string? search
        )
        {
            return _userAppService.ListAsync(
                new OrganizationId(organizationId),
                skip,
                take,
                activeFilter,
                search
            );
        }

        public class AddRequestBody
        {
            public OrganizationId OrganizationId { get; set; }
            public EmailAddressDto Email { get; set; }
            public PersonNameDto Name { get; set; }
            public UserRole UserRole { get; set; }
            public string Password { get; set; }
        }

        [HttpPost]
        public Task<UserId> Add([FromBody] AddRequestBody body)
        {
            return _userAppService.AddAsync(body.OrganizationId, body.Email, body.Name, body.UserRole, body.Password);
        }

        public class SetNameRequestBody
        {
            public UserId Id { get; set; }
            public PersonNameDto Name { get; set; }
        }

        [HttpPost]
        public Task SetName([FromBody] SetNameRequestBody body)
        {
            return _userAppService.SetNameAsync(body.Id, body.Name);
        }

        public class SetRoleRequestBody
        {
            public UserId Id { get; set; }
            public UserRole Role { get; set; }
        }

        [HttpPost]
        public Task SetRole([FromBody] SetRoleRequestBody body)
        {
            return _userAppService.SetRoleAsync(body.Id, body.Role);
        }

        public class SetPasswordRequestBody
        {
            public UserId Id { get; set; }
            public string NewPassword { get; set; }
        }

        [HttpPost]
        public Task SetPassword([FromBody] SetPasswordRequestBody body)
        {
            return _userAppService.SetPasswordAsync(body.Id, body.NewPassword);
        }

        public class SetActiveRequestBody
        {
            public UserId Id { get; set; }
            public bool Active { get; set; }
        }

        public class ChangePasswordRequestBody
        {
            public UserId Id { get; set; }
            public string CurrentPassword { get; set; }
            public string NewPassword { get; set; }
        }

        [HttpPost]
        public Task ChangePassword([FromBody] ChangePasswordRequestBody body)
        {
            return _userAppService.ChangePasswordAsync(body.Id, body.CurrentPassword, body.NewPassword);
        }

        [HttpPost]
        public Task SetActive([FromBody] SetActiveRequestBody body)
        {
            return _userAppService.SetActiveAsync(body.Id, body.Active);
        }

        [HttpGet]
        public Task<bool> UserEmailIsAvailable(string email)
        {
            return _userAppService.UserEmailIsAvailableAsync(new EmailAddressDto(email));
        }
    }
}
