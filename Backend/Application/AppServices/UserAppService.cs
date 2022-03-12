using AppDTOs.Enumerations;
using DomainExceptions;
using ITI.Baseline.Passwords;
using ITI.Baseline.Util;

namespace AppServices;

public class UserAppService : ApplicationService, IUserAppService
{
    private readonly IAppAuthContext _auth;
    private readonly IAppPermissions _perms;
    private readonly IUserQueries _queries;
    private readonly IUserRepository _repo;
    private readonly IUserRoleService _userRoleSvc;
    private readonly IOrganizationQueries _organizationQueries;
    private readonly IPasswordEncoder _passwordEncoder;
    private readonly IPasswordValidator _passwordValidator;

    public UserAppService(
        IUnitOfWorkProvider uowp,
        ILogger logger,
        IAppAuthContext auth,
        IAppPermissions perms,
        IUserQueries queries,
        IUserRoleService userRoleSvc,
        IOrganizationQueries organizationQueries,
        IUserRepository repo,
        IPasswordEncoder passwordEncoder,
        IPasswordValidator passwordValidator
    ) : base(uowp, logger, auth)
    {
        _auth = auth;
        _perms = perms;
        _queries = queries;
        _userRoleSvc = userRoleSvc;
        _organizationQueries = organizationQueries;
        _repo = repo;
        _passwordEncoder = passwordEncoder;
        _passwordValidator = passwordValidator;
    }

    public Task<UserDto?> GetAsync(UserId id)
    {
        return QueryAsync(
            Authorize.AuthorizedBelow,
            async () =>
            {
                var user = await _queries.GetAsync(id);
                if (user == null) return null;

                Authorize.Require(await _perms.CanViewUsersAsync(user.Organization.Id));

                return user;
            }
        );
    }

    public Task<FilteredList<UserSummaryDto>> ListAsync(
        OrganizationId organizationId,
        int skip,
        int take,
        ActiveFilter activeFilter,
        string? search
    )
    {
        return QueryAsync(
            async () => Authorize.Require(await _perms.CanViewUsersAsync(organizationId)),
            () => _queries.ListAsync(organizationId, skip, take, activeFilter, search)
        );
    }

    public Task<bool> UserEmailIsAvailableAsync(EmailAddressDto email)
    {
        return QueryAsync(
            Authorize.AnyUser,
            () => _queries.EmailIsAvailableAsync(email.ToValueObject())
        );
    }

    public Task<UserId> AddAsync(
        OrganizationId organizationId,
        EmailAddressDto email,
        PersonNameDto name,
        UserRole role,
        string password
    )
    {
        return CommandAsync(
            // Additional authorization below
            async () => Authorize.Require(await _perms.CanManageUsersAsync(organizationId)),
            async () =>
            {
                var targetOrganization = await _organizationQueries.GetAsync(organizationId);
                Require.NotNull(targetOrganization, "Organization not found.");

                if (!_userRoleSvc.GetGrantableRoles(targetOrganization.IsHost).Contains(role))
                    throw new NotAuthorizedException();

                if (!_passwordValidator.IsValid(password))
                    throw new InvalidPasswordException();

                var user = new User(
                    organizationId,
                    email.ToValueObject(),
                    name.ToValueObject(),
                    role,
                    _passwordEncoder.Encode(password)
                );
                _repo.Add(user);

                return user.Id;
            }
        );
    }

    private async Task<User> GetDomainEntityAsync(UserId id)
    {
        var user = await _repo.GetAsync(id);
        Require.NotNull(user, "User not found.");

        return user;
    }

    public Task SetActiveAsync(UserId id, bool active)
    {
        return CommandAsync(
            async () => Authorize.Require(await _perms.CanManageAsync(id)),
            async () => (await GetDomainEntityAsync(id)).SetActive(active)
        );
    }

    public Task SetNameAsync(UserId id, PersonNameDto name)
    {
        return CommandAsync(
            async () => Authorize.Require(await _perms.CanManageAsync(id)),
            async () => (await GetDomainEntityAsync(id)).SetName(name.ToValueObject())
        );
    }

    public Task SetRoleAsync(UserId id, UserRole role)
    {
        return CommandAsync(
            // Additional authorization below
            async () => Authorize.Require(await _perms.CanManageAsync(id)),
            async () =>
            {
                var user = await GetDomainEntityAsync(id);

                var targetOrganization = await _organizationQueries.GetAsync(user.OrganizationId);
                Require.NotNull(targetOrganization, "Organization not found.");

                if (!_userRoleSvc.GetGrantableRoles(targetOrganization.IsHost).Contains(role))
                    throw new NotAuthorizedException();

                user.SetRole(role);
            }
        );
    }

    public Task<LogInResultDto> LogInAsync(EmailAddressDto email, string password)
    {
        return CommandAsync(
            Authorize.Unauthenticated,
            async () =>
            {
                var user = await _repo.GetAsync(email.ToValueObject());

                if (user == null)
                    return new LogInResultDto(LogInResult.InvalidCredentials);

                if (!user.IsActive)
                    return new LogInResultDto(LogInResult.InactiveUser);

                if (!await _organizationQueries.IsActiveAsync(user.OrganizationId))
                    return new LogInResultDto(LogInResult.InactiveOrganization);

                if (user.EncodedPassword == null)
                    return new LogInResultDto(LogInResult.PasswordNotSet);

                if (!_passwordEncoder.IsCorrect(password, user.EncodedPassword))
                    return new LogInResultDto(LogInResult.InvalidCredentials);

                // Successful login
                var userDto = await _queries.GetAsync(user.Id);
                Require.NotNull(userDto, "User not found.");

                return new LogInResultDto(userDto);
            }
        );
    }

    public Task ChangePasswordAsync(UserId id, string currentPassword, string newPassword)
    {
        return CommandAsync(
            () =>
            {
                if (id != _auth.UserId)
                    throw new NotAuthorizedException("You may not change the password for other users via this method.");

                return Task.CompletedTask;
            },
            async () =>
            {
                var user = await GetDomainEntityAsync(id);

                if (user.EncodedPassword == null)
                {
                    throw new DomainException(
                        "This method cannot be used to set your initial password.",
                        DomainException.AppServiceLogAs.Error
                    );
                }

                if (!_passwordEncoder.IsCorrect(currentPassword, user.EncodedPassword))
                    throw new IncorrectPasswordException();

                if (!_passwordValidator.IsValid(newPassword))
                    throw new InvalidPasswordException();

                user.SetPassword(_passwordEncoder.Encode(newPassword));
            }
        );
    }

    public Task SetPasswordAsync(UserId id, string newPassword)
    {
        return CommandAsync(
            async () => Authorize.Require(await _perms.CanManageAsync(id)),
            async () =>
            {
                var user = await GetDomainEntityAsync(id);

                var encPassword = _passwordEncoder.Encode(newPassword);
                user.SetPassword(encPassword);
            }
        );
    }
}
