namespace TestDataLoader.Loaders
{
    internal class SheetTypeLoader
    {
        private readonly ISheetTypeAppService _sheetTypeAppService;
        private readonly TestDataMonitor _monitor;
        private readonly Func<AppDataContext> _dbFactory;

        private Randomizer _randomizer = new();

        public SheetTypeLoader(
            ISheetTypeAppService logoSetAppService,
            TestDataMonitor monitor,
            Func<AppDataContext> dbFactory
        )
        {
            _sheetTypeAppService = logoSetAppService;
            _monitor = monitor;
            _dbFactory = dbFactory;
        }

        public async Task AddSheetTypesAsync()
        {
            _randomizer = new Randomizer(2143);
            OrganizationId[] organizationIds;

            using (var db = _dbFactory())
            {
                organizationIds = await db.Organizations
                    .Select(o => new OrganizationId(o.Id))
                    .ToArrayAsync();
            }

            foreach (var organizationId in organizationIds)
            {
                await _sheetTypeAppService.AddAsync(organizationId, "T1.2", "Technology Design — AV");
                await _sheetTypeAppService.AddAsync(organizationId, "T2.2", "Technology Design — LC & SH");

                var inactiveSheetTypeId = await _sheetTypeAppService.AddAsync(
                    organizationId,
                    "T9.9",
                    "Inactive Sheet Type"
                );
                await _sheetTypeAppService.SetActiveAsync(inactiveSheetTypeId, false);
            }

            _monitor.WriteCompletedMessage("Added sheet types.");
        }
    }
}
