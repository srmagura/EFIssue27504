using TestDataLoader.Helpers;

namespace TestDataLoader.Loaders
{
    internal class OrganizationLoader
    {
        private readonly IOrganizationAppService _organizationAppService;
        private readonly TestDataMonitor _monitor;

        public OrganizationLoader(
            IOrganizationAppService organizationAppService,
            TestDataMonitor monitor
        )
        {
            _organizationAppService = organizationAppService;
            _monitor = monitor;
        }

        public async Task AddOrganizationsAsync()
        {
            var companyNames = Generator.CompanyNames(19992);

            var numberOfOrganizations = 5;
            var emailsAndNames = Generator.EmailsAndNames(2349, count: numberOfOrganizations);

            for (var i = 0; i < numberOfOrganizations; i++)
            {
                var name = companyNames[i];
                var nameParts = name.Split();
                var shortName = nameParts[0].Replace(",", "").ToLower();

                var id = await _organizationAppService.AddAsync(
                    name: name,
                    shortName: shortName,
                    ownerEmail: emailsAndNames[i].Email,
                    ownerName: emailsAndNames[i].Name,
                    ownerPassword: "LetMeIn98"
                );

                if (i == numberOfOrganizations - 1)
                {
                    await _organizationAppService.SetActiveAsync(id, false);
                }
            }

            _monitor.WriteCompletedMessage("Added organizations.");
        }
    }
}
