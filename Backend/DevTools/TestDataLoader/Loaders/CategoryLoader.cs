using AppDTOs;

namespace TestDataLoader.Loaders;

internal class CategoryLoader
{
    private readonly ICategoryAppService _categoryAppService;
    private readonly ISymbolAppService _symbolAppService;
    private readonly TestDataMonitor _monitor;
    private readonly Func<AppDataContext> _dbFactory;

    public CategoryLoader(
        ICategoryAppService categoryAppService,
        ISymbolAppService symbolAppService,
        TestDataMonitor monitor,
        Func<AppDataContext> dbFactory
    )
    {
        _categoryAppService = categoryAppService;
        _symbolAppService = symbolAppService;
        _monitor = monitor;
        _dbFactory = dbFactory;
    }

    public async Task AddCategoriesAsync()
    {
        List<OrganizationId> organizationIds;
        using (var db = _dbFactory())
        {
            organizationIds = db.Organizations
                .Where(o => o.IsActive)
                .Select(o => new OrganizationId(o.Id))
                .ToList();
        }

        foreach (var organizationId in organizationIds)
        {
            var symbols = await _symbolAppService.ListAsync(organizationId, 0, 10000, ActiveFilter.All, search: null);

            var tree = MakeTree(symbols.Items);

            await _categoryAppService.SetCategoryTreeAsync(organizationId, tree);
        }

        _monitor.WriteCompletedMessage("Added categories.");
    }

    private static TreeInputDto MakeTree(SymbolSummaryDto[] symbols)
    {
        return new TreeInputDto
        {
            Children = new()
            {
                new TreeInputDto
                {
                    Category = MakeTopLevelCategory(
                        "Audio & Video",
                        "#4d4dff",
                        symbols.First(p => p.Name.Contains("65 Inch Television")).Id
                    ),
                    Children = new()
                    {
                        new TreeInputDto
                        {
                            Category = MakeCategory("Sony"),
                            Children = new()
                            {
                                new TreeInputDto { Category = MakeCategory("Crystal") }
                            }
                        },
                        MakeLeafCategory("Bose"),
                        MakeLeafCategory("Bowers & Wilkins"),
                        MakeLeafCategory("Samsung"),
                        MakeLeafCategory("LG")
                    }
                },
                new TreeInputDto
                {
                    Category = MakeTopLevelCategory(
                        "Voice, Data & Infastructure",
                        "#008000",
                        symbols.First(p => p.Name.Contains("Indoor Wireless Access Point")).Id
                    ),
                    Children = new()
                    {
                        new TreeInputDto
                        {
                            Category = MakeCategory("Google"),
                            Children = new()
                            {
                                new TreeInputDto { Category = MakeCategory("Nest") }
                            }
                        },
                        MakeLeafCategory("Netgear"),
                        MakeLeafCategory("Linksys"),
                    }
                },
                new TreeInputDto
                {
                    Category = MakeTopLevelCategory(
                        "Lighting Control",
                        "#FFD700",
                        symbols.First(p => p.Name.Contains("Lighting Control Headend Equipment")).Id
                    ),
                    Children = new()
                    {
                        new TreeInputDto
                        {
                            Category = MakeCategory("Philips"),
                            Children = new()
                            {
                                new TreeInputDto { Category = MakeCategory("Hue") }
                            }
                        },
                        MakeLeafCategory("Lumens"),
                        MakeLeafCategory("Lifx"),
                    }
                },
                new TreeInputDto
                {
                    Category = MakeTopLevelCategory(
                        "Motorized Shades",
                        "#ff6680",
                        symbols.First(p => p.Name.Contains("Stewart Motorized Film Screen")).Id
                    ),
                    Children = new()
                    {
                        MakeLeafCategory("Lutron"),
                        MakeLeafCategory("Graber"),
                    }
                },
                new TreeInputDto
                {
                    Category = MakeTopLevelCategory(
                        "Safety & Security",
                        "#cc0000",
                        symbols.First(p => p.Name.Contains("Access Control Door Station")).Id
                    ),
                    Children = new()
                    {
                        MakeLeafCategory("Ring"),
                        MakeLeafCategory("SimpliSafe"),
                        MakeLeafCategory("ADT"),
                        MakeLeafCategory("Skylink"),
                    }
                },
                new TreeInputDto
                {
                    Category = new CategoryInputDto(new CategoryId(), "Energy")
                    {
                        IsActive = false,
                        Color = "#808080",
                        SymbolId = symbols.First(p => p.Name.Contains("2 Port Data Outlet")).Id
                    },
                    Children = new()
                    {
                        new TreeInputDto() { Category = new CategoryInputDto(new CategoryId(), "Solar") },
                        new TreeInputDto() { Category = new CategoryInputDto(new CategoryId(), "Natural Gas") }
                    }
                },
            }
        };
    }

    private static CategoryInputDto MakeTopLevelCategory(string name, string color, SymbolId symbolId)
    {
        return new CategoryInputDto(new CategoryId(), name)
        {
            IsActive = true,
            Color = color,
            SymbolId = symbolId
        };
    }

    private static CategoryInputDto MakeCategory(string name)
    {
        return new CategoryInputDto(new CategoryId(), name)
        {
            IsActive = true
        };
    }

    private static TreeInputDto MakeLeafCategory(string name)
    {
        return new TreeInputDto
        {
            Category = MakeCategory(name)
        };
    }
}
