using TestDataLoader.Helpers;

namespace TestDataLoader.Loaders
{
    internal class UserLoader
    {
        private readonly IUserAppService _userAppService;
        private readonly TestDataMonitor _monitor;
        private readonly Func<AppDataContext> _dbFactory;
        private Randomizer _randomizer = new();

        public UserLoader(
            IUserAppService userAppService,
            TestDataMonitor monitor,
            Func<AppDataContext> dbFactory
        )
        {
            _userAppService = userAppService;
            _monitor = monitor;
            _dbFactory = dbFactory;
        }

        public async Task AddUsersAsync()
        {
            _randomizer = new Randomizer(2143);
            List<OrganizationId> organizationIds;

            using (var db = _dbFactory())
            {
                organizationIds = db.Organizations
                    .Select(o => new OrganizationId(o.Id))
                    .ToList();
            }

            var emailsAndNames = Generator.EmailsAndNames(92);
            var emailsAndNamesIndex = 0;

            foreach (var organizationId in organizationIds)
            {
                var numberOfUsers = _randomizer.Number(10, 20);

                for (var i = 0; i < numberOfUsers; i++)
                {
                    var emailAndName = emailsAndNames[emailsAndNamesIndex++];
                    if (!await _userAppService.UserEmailIsAvailableAsync(emailAndName.Email))
                        continue;

                    var role = _randomizer.Double() < 0.5 ? UserRole.BasicUser : UserRole.OrganizationAdmin;

                    var id = await _userAppService.AddAsync(
                        organizationId,
                        emailAndName.Email,
                        emailAndName.Name,
                        role,
                        "LetMeIn98"
                    );

                    if (_randomizer.Double() < 0.1)
                    {
                        await _userAppService.SetActiveAsync(id, false);
                    }
                }
            }

            _monitor.WriteCompletedMessage("Added users.");
        }
    }
}
