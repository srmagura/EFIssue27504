using Bogus.DataSets;
using TestDataLoader.Helpers;

namespace TestDataLoader.Loaders
{
    internal class ProjectLoader
    {
        private readonly IProjectAppService _projectAppService;
        private readonly ILogoSetAppService _logoSetAppService;
        private readonly ITermsDocumentAppService _termsDocumentAppService;
        private readonly TestDataMonitor _monitor;
        private readonly Func<AppDataContext> _dbFactory;
        private readonly Lorem _lorem = new();
        private readonly Name _name = new();

        private Randomizer _randomizer = new();

        public ProjectLoader(
            IProjectAppService projectAppService,
            ILogoSetAppService logoSetAppService,
            ITermsDocumentAppService termsDocumentAppService,
            TestDataMonitor monitor,
            Func<AppDataContext> dbFactory
        )
        {
            _projectAppService = projectAppService;
            _logoSetAppService = logoSetAppService;
            _termsDocumentAppService = termsDocumentAppService;
            _monitor = monitor;
            _dbFactory = dbFactory;
        }

        public async Task AddProjectsAsync()
        {
            _randomizer = new Randomizer(4444);
            var addresses = Generator.Addresses(4445);

            List<OrganizationId> organizationIds;
            using (var db = _dbFactory())
            {
                organizationIds = db.Organizations
                    .Select(o => new OrganizationId(o.Id))
                    .ToList();
            }

            var addressIndex = 0;
            foreach (var organizationId in organizationIds)
            {
                var numberOfProjects = _randomizer.Number(15, 50);

                var lastNameSet = new HashSet<string>();
                while (lastNameSet.Count < numberOfProjects)
                    lastNameSet.Add(_name.LastName());

                var lastNames = lastNameSet.ToArray();

                var logoSets = await _logoSetAppService.ListAsync(organizationId);
                var termsDocuments = (await _termsDocumentAppService.ListAsync(organizationId, 0, 1000, ActiveFilter.All)).Items;

                for (var i = 0; i < numberOfProjects; i++)
                {
                    var firstName = _name.FirstName();
                    var lastName = lastNames[i];

                    var fullName = $"{firstName} {lastName}";

                    var id = await _projectAppService.AddAsync(
                        organizationId,
                        name: $"The {lastName} Project",
                        shortName: $"{lastName} Project",
                        description: _lorem.Sentences(sentenceCount: _randomizer.Number(3, 6), separator: " "),
                        address: addresses[addressIndex++],
                        customerName: _randomizer.Double() < 0.5 ? fullName : $"The {lastName} Group",
                        signeeName: fullName,
                        logoSetId: _randomizer.ArrayElement(logoSets).Id,
                        termsDocumentId: _randomizer.ArrayElement(termsDocuments).Id,
                        // Round to 20 sq. ft
                        estimatedSquareFeet: _randomizer.Number(1500, 5000) / 20 * 20
                    );

                    if (i % 10 == 9)
                        await _projectAppService.SetActiveAsync(id, false);
                }
            }

            _monitor.WriteCompletedMessage("Added projects.");
        }
    }
}
