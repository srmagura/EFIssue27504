using AppDTOs.Enumerations;
using ITI.Baseline.Passwords;
using ITI.Baseline.Util;
using ITI.Baseline.ValueObjects;
using ValueObjects;

namespace AppServices;

public class OrganizationAppService : ApplicationService, IOrganizationAppService
{
    private readonly IAppPermissions _perms;
    private readonly IOrganizationQueries _queries;
    private readonly IOrganizationRepository _repo;
    private readonly IUserRepository _userRepo;
    private readonly IPasswordEncoder _passwordEncoder;

    public OrganizationAppService(
        IUnitOfWorkProvider uowp,
        ILogger logger,
        IAppAuthContext auth,
        IAppPermissions perms,
        IOrganizationQueries queries,
        IOrganizationRepository repo,
        IUserRepository userRepo,
        IPasswordEncoder passwordEncoder
    ) : base(uowp, logger, auth)
    {
        _perms = perms;
        _queries = queries;
        _repo = repo;
        _userRepo = userRepo;
        _passwordEncoder = passwordEncoder;
    }

    public Task<OrganizationDto?> GetAsync(OrganizationId id)
    {
        return QueryAsync(
            async () => Authorize.Require(await _perms.CanViewAsync(id)),
            () => _queries.GetAsync(id)
        );
    }

    public Task<OrganizationDto?> GetByShortNameAsync(string shortName)
    {
        return QueryAsync(
            Authorize.AuthorizedBelow,
            async () =>
            {
                var organization = await _queries.GetByShortNameAsync(shortName);
                if (organization == null)
                    return null;

                Authorize.Require(await _perms.CanViewAsync(organization.Id));

                return organization;
            }
        );
    }

    public Task<FilteredList<OrganizationSummaryDto>> ListAsync(
        int skip,
        int take,
        ActiveFilter activeFilter,
        string? search
    )
    {
        return QueryAsync(
            async () => Authorize.Require(await _perms.CanManageOrganizationsAsync()),
            () => _queries.ListAsync(skip, take, activeFilter, search)
        );
    }

    public Task<bool> NameIsAvailableAsync(string name)
    {
        return QueryAsync(
            async () => Authorize.Require(await _perms.CanManageOrganizationsAsync()),
            () => _queries.NameIsAvailableAsync(name)
        );
    }

    public Task<bool> ShortNameIsAvailableAsync(string shortName)
    {
        return QueryAsync(
            async () => Authorize.Require(await _perms.CanManageOrganizationsAsync()),
            () => _queries.ShortNameIsAvailableAsync(shortName)
        );
    }

    public Task<OrganizationId> AddAsync(
        string name,
        string shortName,
        EmailAddressDto ownerEmail,
        PersonNameDto ownerName,
        string ownerPassword
    )
    {
        return CommandAsync(
           async () => Authorize.Require(await _perms.CanManageOrganizationsAsync()),
           () =>
           {
               var organization = new Organization(
                   name,
                   new OrganizationShortName(shortName)
               );
               _repo.Add(organization);

               var eEmail = new EmailAddress(ownerEmail.Value);
               var eName = new PersonName(ownerName.First, ownerName.Last);
               var owner = new User(organization.Id, eEmail, eName, UserRole.OrganizationAdmin, _passwordEncoder.Encode(ownerPassword));
               _userRepo.Add(owner);

               return Task.FromResult(organization.Id);
           }
        );
    }

    private async Task<Organization> GetDomainEntityAsync(OrganizationId id)
    {
        var organization = await _repo.GetAsync(id);
        Require.NotNull(organization, "Could not find organization.");

        return organization;
    }

    public Task SetActiveAsync(OrganizationId id, bool active)
    {
        return CommandAsync(
            async () => Authorize.Require(await _perms.CanManageOrganizationsAsync()),
            async () => (await GetDomainEntityAsync(id)).SetActive(active)
        );
    }

    public Task SetNameAsync(OrganizationId id, string name)
    {
        return CommandAsync(
            async () => Authorize.Require(await _perms.CanManageOrganizationsAsync()),
            async () => (await GetDomainEntityAsync(id)).SetName(name)
        );
    }

    public Task SetShortNameAsync(OrganizationId id, string shortName)
    {
        return CommandAsync(
            async () => Authorize.Require(await _perms.CanManageOrganizationsAsync()),
            async () => (await GetDomainEntityAsync(id)).SetShortName(new OrganizationShortName(shortName))
        );
    }
}
