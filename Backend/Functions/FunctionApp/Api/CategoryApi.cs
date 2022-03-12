using FunctionApp.ApiServices;

namespace FunctionApp.Api;

public class CategoryApi : ApiFunction
{
    private readonly ICategoryAppService _categoryAppService;

    public CategoryApi(
        IAppAuthContext auth,
        Bugsnag.IClient bugsnag,
        ICategoryAppService categoryAppService
    ) : base(auth, bugsnag)
    {
        _categoryAppService = categoryAppService;
    }

    public class GetCategoryTreeRequestParams
    {
        public Guid? OrganizationId { get; set; }
        public int? ActiveFilter { get; set; }
    }

    [FunctionName("api_category_getCategoryTree")]
    public Task<IActionResult> GetCategoryTreeAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "category/getCategoryTree")]
        GetCategoryTreeRequestParams @params
    )
    {
        return HandleRequestAsync(async () =>
        {
            RequireParam(@params.OrganizationId, nameof(@params.OrganizationId));
            RequireParam(@params.ActiveFilter, nameof(@params.ActiveFilter));

            return await _categoryAppService.GetCategoryTreeAsync(
                new OrganizationId(@params.OrganizationId),
                (ActiveFilter)@params.ActiveFilter.Value
            );
        });
    }

    public record SetCategoryTreeRequestBody(
        OrganizationId OrganizationId,
        TreeInputDto Tree
    );

    [FunctionName("api_category_setCategoryTree")]
    public Task<IActionResult> SetCategoryTreeAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "category/setCategoryTree")]
        SetCategoryTreeRequestBody body
    )
    {
        return HandleRequestAsync(async () =>
        {
            await _categoryAppService.SetCategoryTreeAsync(body.OrganizationId, body.Tree);
        });
    }
}
