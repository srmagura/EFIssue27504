using DataInterfaces.Repositories;
using Entities;
using ITI.Baseline.Passwords;
using ITI.Baseline.ValueObjects;
using ITI.DDD.Core;
using ValueObjects;

namespace TestDataLoader.Loaders
{
    internal class HostOrganizationLoader
    {
        private readonly IUnitOfWorkProvider _uowp;
        private readonly IUserRepository _userRepo;
        private readonly IOrganizationRepository _organizationRepo;
        private readonly IPasswordEncoder _passwordEncoder;
        private readonly TestDataMonitor _monitor;

        public static readonly string HostAdminEmail = "System7Admin@example2.com";
        public static OrganizationId HostOrganizationId { get; private set; } = new();

        public HostOrganizationLoader(
            IUnitOfWorkProvider uowp,
            IUserRepository userRepo,
            IOrganizationRepository organizationRepo,
            IPasswordEncoder passwordEncoder,
            TestDataMonitor monitor
        )
        {
            _uowp = uowp;
            _userRepo = userRepo;
            _organizationRepo = organizationRepo;
            _passwordEncoder = passwordEncoder;
            _monitor = monitor;
        }

        // Can't go through the app service since no users exist at this point
        public async Task AddHostOrganizationAsync()
        {
            using (var uow = _uowp.Begin())
            {
                var organization = new Organization(
                    "System 7",
                    new OrganizationShortName("system7")
                );
                _organizationRepo.Add(organization);
                HostOrganizationId = organization.Id;

                var owner = new User(
                    organization.Id,
                    new EmailAddress(HostAdminEmail),
                    new PersonName("System7", "Admin"),
                    UserRole.SystemAdmin,
                    _passwordEncoder.Encode("LetMeIn98")
                );
                _userRepo.Add(owner);

                await uow.CommitAsync();
            }

            using (var uow = _uowp.Begin())
            {
                var organization = uow.GetDataContext<AppDataContext>().Organizations.Single();
                organization.IsHost = true;

                await uow.CommitAsync();
            }

            _monitor.WriteCompletedMessage("Added host organization and users.");
        }
    }
}
