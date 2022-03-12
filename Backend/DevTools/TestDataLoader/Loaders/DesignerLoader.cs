using System.Text.Json;
using AppDTOs.Designer;
using TestDataLoader.Helpers;

namespace TestDataLoader.Loaders;

internal class DesignerLoader
{
    private readonly IProjectAppService _projectAppService;
    private readonly IDesignerAppService _designerAppService;
    private readonly IProductKitReferenceAppService _productKitReferenceAppService;
    private readonly TestDataMonitor _monitor;
    private readonly Func<AppDataContext> _dbFactory;

    private readonly Bogus.DataSets.Lorem _lorem = new();
    private Randomizer _randomizer = new();

    public DesignerLoader(
        IProjectAppService projectAppService,
        IDesignerAppService designerAppService,
        IProductKitReferenceAppService productKitReferenceAppService,
        TestDataMonitor monitor,
        Func<AppDataContext> dbFactory
    )
    {
        _projectAppService = projectAppService;
        _designerAppService = designerAppService;
        _productKitReferenceAppService = productKitReferenceAppService;
        _monitor = monitor;
        _dbFactory = dbFactory;
    }

    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    private async Task AddPageOptionsAsync(PageId pageId)
    {
        if (_randomizer.Double() < 0.5)
            return;

        var pageOptions = new PageOptionsDto
        {
            SymbolScale = _randomizer.Number(13, 17)
        };

        var json = JsonSerializer.Serialize(pageOptions, JsonSerializerOptions);
        await _designerAppService.SetAsync(pageId, DesignerDataType.PageOptions, json);
    }

    private double[] GetPosition()
    {
        return new double[]
        {
            _randomizer.Double(0.5, 16.5),
            _randomizer.Double(0.5, 10.5)
        };
    }

    private double GetRotation()
    {
        if (_randomizer.Double() < 0.5)
            return 0;

        var angleIncrement = 45;
        var t = 360 / angleIncrement;

        return _randomizer.Number(0, t) * angleIncrement;
    }

    private string GetNoteText()
    {
        var count = _randomizer.Number(1, 2);
        return _lorem.Sentences(count);
    }

    private async Task<Guid[]> AddPlacedProductKitsAsync(PageId pageId, ProductKitId[] productKitIds)
    {
        var count = _randomizer.Number(1, 5);

        var placedProductKits = Enumerable.Range(0, count)
            .Select(p =>
                new PlacedProductKitDto(
                    id: Guid.NewGuid(),
                    productKitId: _randomizer.ArrayElement(productKitIds),
                    position: GetPosition(),
                    rotation: GetRotation(),
                    lengthInches: null
                )
            )
            .ToArray();

        var json = JsonSerializer.Serialize(placedProductKits, JsonSerializerOptions);
        await _designerAppService.SetAsync(pageId, DesignerDataType.PlacedProductKits, json);

        return placedProductKits.Select(p => p.Id).ToArray();
    }

    private async Task AddNotesAsync(PageId pageId, Guid[] placedProductKitIds)
    {
        var count = _randomizer.Number(1, 5);

        var standaloneNotes = Enumerable.Range(0, count)
            .Select(p =>
                new NoteDto(
                    id: Guid.NewGuid(),
                    placedProductKitId: null,
                    position: GetPosition(),
                    text: GetNoteText()
                )
            );

        var productKitNotes = _randomizer.ArrayElements(placedProductKitIds, placedProductKitIds.Length / 2)
            .Select(id =>
                new NoteDto(
                    id: Guid.NewGuid(),
                    placedProductKitId: id,
                    position: null,
                    text: GetNoteText()
                )
            );

        var notes = standaloneNotes.Concat(productKitNotes).ToArray();

        var json = JsonSerializer.Serialize(notes, JsonSerializerOptions);
        await _designerAppService.SetAsync(pageId, DesignerDataType.Notes, json);
    }

    private async Task AddNoteBlockAsync(PageId pageId)
    {
        var noteBlock = new NoteBlockDto(
            position: new double[] { 0.25, 6.25 },
            dimensions: new double[] { 3.5, 4.5 },
            columns: _randomizer.Number(1, 2)
        );

        var json = JsonSerializer.Serialize(noteBlock, JsonSerializerOptions);
        await _designerAppService.SetAsync(pageId, DesignerDataType.NoteBlock, json);
    }

    private async Task AddTagsAsync(ProjectId projectId)
    {
        ProductKitReferenceId[] referenceIds;

        using (var db = _dbFactory())
        {
            referenceIds = await db.ProductKitReferences
                .Where(r => r.ProjectId == projectId.Guid)
                .Select(r => new ProductKitReferenceId(r.Id))
                .ToArrayAsync();
        }

        for (var i = 0; i < referenceIds.Length; i += 2)
        {
            await _productKitReferenceAppService.SetTagAsync(referenceIds[i], $"R{(i + 1) / 2}");
        }
    }

    public async Task AddDesignerDataAsync()
    {
        _randomizer = new Randomizer(87945);

        ProjectId projectId;
        PageId[] pageIds;
        ProductKitId[] productKitIds;

        using (var db = _dbFactory())
        {
            projectId = await LoaderQueries.GetFirstHostOrganizationProjectAsync(db);

            // Include inactive pages to test that they are ignored by the designer
            pageIds = await db.Pages
                .Where(p => p.ProjectId == projectId.Guid)
                .Select(p => new PageId(p.Id))
                .ToArrayAsync();

            // Include inactive product kits to test that they are displayed in the designer
            productKitIds = await db.ProductKits
                .Where(p => p.Organization!.IsHost && p.MeasurementType == MeasurementType.Normal)
                .Select(p => new ProductKitId(p.Id))
                .ToArrayAsync();
        }

        await _projectAppService.AcquireDesignerLockAsync(projectId);

        foreach (var pageId in pageIds)
        {
            await AddPageOptionsAsync(pageId);
            var placedProductKitIds = await AddPlacedProductKitsAsync(pageId, productKitIds);
            await AddNotesAsync(pageId, placedProductKitIds);
            await AddNoteBlockAsync(pageId);
            await AddTagsAsync(projectId);
        }

        _monitor.WriteCompletedMessage("Added designer data.");
    }
}
