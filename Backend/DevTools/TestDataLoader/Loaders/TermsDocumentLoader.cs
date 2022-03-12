using TestDataLoader.Helpers;

namespace TestDataLoader.Loaders;

internal class TermsDocumentLoader
{
    private readonly ITermsDocumentAppService _termsDocAppService;
    private readonly TestDataMonitor _monitor;
    private readonly Func<AppDataContext> _dbFactory;

    private Randomizer _randomizer = new();

    public TermsDocumentLoader(
        ITermsDocumentAppService termsDocAppService,
        TestDataMonitor monitor,
        Func<AppDataContext> dbFactory
    )
    {
        _termsDocAppService = termsDocAppService;
        _monitor = monitor;
        _dbFactory = dbFactory;
    }

    public async Task AddTermsDocumentsAsync()
    {
        _randomizer = new Randomizer(2143);
        List<OrganizationId> organizationIds;

        using (var db = _dbFactory())
        {
            organizationIds = await db.Organizations
                .Select(p => new OrganizationId(p.Id))
                .ToListAsync();
        }

        var filenames = new List<string> { "billing-rates", "construction-drawings", "services-breakdown" };

        foreach (var organizationId in organizationIds)
        {
            for (var i = 0; i < filenames.Count; i++)
            {
                using var stream = ResourceUtil.GetResourceStream($"TermsDocuments.{filenames[i]}.pdf");

                var id = await _termsDocAppService.AddAsync(organizationId, stream);

                if (i == 0)
                    await _termsDocAppService.SetActiveAsync(id, false);
            }
        }

        _monitor.WriteCompletedMessage("Added terms documents.");
    }
}
