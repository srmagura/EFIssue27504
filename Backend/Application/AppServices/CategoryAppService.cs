using AppDTOs.Enumerations;
using ServiceInterfaces;

namespace AppServices;

public class CategoryAppService : ApplicationService, ICategoryAppService
{
    private readonly IAppPermissions _perms;
    private readonly TreeBuilder _treeBuilder;
    private readonly ICategoryQueries _queries;
    private readonly ICategoryRepository _repo;
    private readonly ICategoryService _categoryService;

    public CategoryAppService(
        IUnitOfWorkProvider uowp,
        ILogger logger,
        IAppAuthContext auth,
        IAppPermissions perms,
        TreeBuilder treeBuilder,
        ICategoryQueries queries,
        ICategoryRepository repo,
        ICategoryService categoryService
    ) : base(uowp, logger, auth)
    {
        _perms = perms;
        _treeBuilder = treeBuilder;
        _queries = queries;
        _repo = repo;
        _categoryService = categoryService;
    }

    public Task<TreeDto> GetCategoryTreeAsync(OrganizationId organizationId, ActiveFilter activeFilter)
    {
        return QueryAsync(
            async () => Authorize.Require(await _perms.CanViewCategoriesAsync(organizationId)),
            async () =>
            {
                var dbCategories = await _queries.ListDbAsync(organizationId, activeFilter);

                return _treeBuilder.BuildTree(dbCategories);
            }
        );
    }

    public Task SetCategoryTreeAsync(OrganizationId organizationId, TreeInputDto tree)
    {
        return CommandAsync(
            async () => Authorize.Require(await _perms.CanManageCategoriesAsync(organizationId)),
            async () =>
            {
                var currentCategories = await _repo.AllForAsync(organizationId);
                var inputCategories = FlattenTreeInputDto(organizationId, tree);
                _categoryService.DiffCategories(currentCategories, inputCategories);
            }
        );
    }

    private List<Category> FlattenTreeInputDto(
        OrganizationId organizationId,
        TreeInputDto tree,
        Category? parent = null,
        int index = 0
    )
    {
        List<Category> categoryEntities = new();

        var categoryDto = tree.Category;
        Category? category = null;

        if (categoryDto != null)
        {
            // If category has a parent, make sure symbol and color are null.
            var symbolId = parent == null ? categoryDto.SymbolId : null;
            var color = parent == null ? categoryDto.Color : null;

            category = new Category(
                organizationId,
                categoryDto.Name,
                parent,
                index,
                symbolId,
                color,
                categoryDto.IsActive
            );
            category.SetId(categoryDto.Id);
            categoryEntities.Add(category);
        }

        for (var i = 0; i < tree.Children.Count; i++)
        {
            var subtree = tree.Children[i];
            categoryEntities.AddRange(FlattenTreeInputDto(organizationId, subtree, category, i));
        }

        return categoryEntities;
    }
}
