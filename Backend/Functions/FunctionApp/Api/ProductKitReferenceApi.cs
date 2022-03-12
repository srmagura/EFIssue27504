using FunctionApp.ApiServices;

namespace FunctionApp.Api;

public class ProductKitReferenceApi : ApiFunction
{
    private readonly IProductKitReferenceAppService _productKitReferenceAppService;

    public ProductKitReferenceApi(
        IAppAuthContext auth,
        Bugsnag.IClient bugsnag,
        IProductKitReferenceAppService productKitReferenceAppService
    ) : base(auth, bugsnag)
    {
        _productKitReferenceAppService = productKitReferenceAppService;
    }

    public class ListRequestParams
    {
        public Guid? ProjectId { get; set; }
    }

    [FunctionName("api_productKitReference_list")]
    public Task<IActionResult> ListAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "productKitReference/list")]
        ListRequestParams @params
    )
    {
        return HandleRequestAsync(async () =>
        {
            RequireParam(@params.ProjectId, nameof(@params.ProjectId));

            return await _productKitReferenceAppService.ListAsync(new ProjectId(@params.ProjectId));
        });
    }

    public record SetProductKitVersionRequestBody(
        ProductKitReferenceId Id,
        ProductKitVersionId ProductKitVersionId
    );

    [FunctionName("api_productKitReference_setProductKitVersion")]
    public Task<IActionResult> SetProductKitVersionAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "productKitReference/setProductKitVersion")]
        SetProductKitVersionRequestBody body
    )
    {
        return HandleRequestAsync(async () =>
        {
            await _productKitReferenceAppService.SetProductKitVersionAsync(body.Id, body.ProductKitVersionId);
        });
    }

    public record SetTagRequestBody(
        ProductKitReferenceId Id,
        string? Tag
    );

    [FunctionName("api_productKitReference_setTag")]
    public Task<IActionResult> SetTagAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "productKitReference/setTag")]
        SetTagRequestBody body
    )
    {
        return HandleRequestAsync(async () =>
        {
            await _productKitReferenceAppService.SetTagAsync(body.Id, body.Tag);
        });
    }

    public record UpdateAllRequestBody(
        ProjectId ProjectId
    );

    [FunctionName("api_productKitReference_updateAll")]
    public Task<IActionResult> UpdateAllAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "productKitReference/updateAll")]
        UpdateAllRequestBody body
    )
    {
        return HandleRequestAsync(async () =>
        {
            await _productKitReferenceAppService.UpdateAllAsync(body.ProjectId);
        });
    }
}
