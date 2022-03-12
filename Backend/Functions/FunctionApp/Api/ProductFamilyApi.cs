using FunctionApp.ApiServices;

namespace FunctionApp.Api;

public class ProductFamilyApi : ApiFunction
{
    private readonly IProductFamilyAppService _productFamilyAppService;

    public ProductFamilyApi(
        IAppAuthContext auth,
        Bugsnag.IClient bugsnag,
        IProductFamilyAppService productFamilyAppService
    ) : base(auth, bugsnag)
    {
        _productFamilyAppService = productFamilyAppService;
    }

    public class ListRequestParams
    {
        public Guid? OrganizationId { get; set; }
        public int? Skip { get; set; }
        public int? Take { get; set; }
        public int? ActiveFilter { get; set; }
        public string? Search { get; set; }
    }

    [FunctionName("api_productFamily_list")]
    public Task<IActionResult> ListAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "productFamily/list")]
        ListRequestParams @params
    )
    {
        return HandleRequestAsync(async () =>
        {
            RequireParam(@params.OrganizationId, nameof(@params.OrganizationId));
            RequireParam(@params.Skip, nameof(@params.Skip));
            RequireParam(@params.Take, nameof(@params.Take));
            RequireParam(@params.ActiveFilter, nameof(@params.ActiveFilter));

            return await _productFamilyAppService.ListAsync(
                new OrganizationId(@params.OrganizationId),
                @params.Skip.Value,
                @params.Take.Value,
                (ActiveFilter)@params.ActiveFilter.Value,
                @params.Search
            );
        });
    }

    public class NameIsAvailableRequestParams
    {
        public Guid? OrganizationId { get; set; }
        public string? Name { get; set; }
    }

    [FunctionName("api_productFamily_nameIsAvailable")]
    public Task<IActionResult> NameIsAvailableAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "productFamily/nameIsAvailable")]
        NameIsAvailableRequestParams @params
    )
    {
        return HandleRequestAsync(async () =>
        {
            RequireParam(@params.OrganizationId, nameof(@params.OrganizationId));
            RequireParam(@params.Name, nameof(@params.Name));

            return await _productFamilyAppService.NameIsAvailableAsync(new OrganizationId(@params.OrganizationId), @params.Name);
        });
    }

    public record AddRequestBody(
        OrganizationId OrganizationId,
        string Name
    );

    [FunctionName("api_productFamily_add")]
    public Task<IActionResult> AddAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "productFamily/add")]
        AddRequestBody body
    )
    {
        return HandleRequestAsync(async () =>
        {
            return await _productFamilyAppService.AddAsync(body.OrganizationId, body.Name);
        });
    }

    public record SetNameRequestBody(
        ProductFamilyId Id,
        string Name
    );

    [FunctionName("api_productFamily_setName")]
    public Task<IActionResult> SetNameAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "productFamily/setName")]
        SetNameRequestBody body
    )
    {
        return HandleRequestAsync(async () =>
        {
            await _productFamilyAppService.SetNameAsync(body.Id, body.Name);
        });
    }

    public record SetActiveRequestBody(
        ProductFamilyId Id,
        bool Active
    );

    [FunctionName("api_productFamily_setActive")]
    public Task<IActionResult> SetActiveAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "productFamily/setActive")]
        SetActiveRequestBody body
    )
    {
        return HandleRequestAsync(async () =>
        {
            await _productFamilyAppService.SetActiveAsync(body.Id, body.Active);
        });
    }
}
