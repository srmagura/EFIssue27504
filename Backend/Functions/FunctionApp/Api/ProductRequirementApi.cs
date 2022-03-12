using FunctionApp.ApiServices;

namespace FunctionApp.Api;

public class ProductRequirementApi : ApiFunction
{
    private readonly IProductRequirementAppService _productRequirementAppService;

    public ProductRequirementApi(
        IAppAuthContext auth,
        Bugsnag.IClient bugsnag,
        IProductRequirementAppService productRequirementAppService
    ) : base(auth, bugsnag)
    {
        _productRequirementAppService = productRequirementAppService;
    }

    public class GetRequestParams
    {
        public Guid? Id { get; set; }
    }

    [FunctionName("api_productRequirement_get")]
    public Task<IActionResult> GetAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "productRequirement/get")]
        GetRequestParams @params
    )
    {
        return HandleRequestAsync(async () =>
        {
            RequireParam(@params.Id, nameof(@params.Id));

            return await _productRequirementAppService.GetAsync(new ProductRequirementId(@params.Id));
        });
    }

    public class ListRequestParams
    {
        public Guid? OrganizationId { get; set; }
    }

    [FunctionName("api_productRequirement_list")]
    public Task<IActionResult> ListAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "productRequirement/list")]
        ListRequestParams @params
    )
    {
        return HandleRequestAsync(async () =>
        {
            RequireParam(@params.OrganizationId, nameof(@params.OrganizationId));

            return await _productRequirementAppService.ListAsync(new OrganizationId(@params.OrganizationId));
        });
    }

    public record AddRequestBody(
        OrganizationId OrganizationId,
        string Label,
        string SvgText
    );

    [FunctionName("api_productRequirement_add")]
    public Task<IActionResult> AddAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "productRequirement/add")]
        AddRequestBody body
    )
    {
        return HandleRequestAsync(async () =>
        {
            return await _productRequirementAppService.AddAsync(body.OrganizationId, body.Label, body.SvgText);
        });
    }

    public record SetLabelRequestBody(
        ProductRequirementId Id,
        string Label
    );

    [FunctionName("api_productRequirement_setLabel")]
    public Task<IActionResult> SetLabelAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "productRequirement/setLabel")]
        SetLabelRequestBody body
    )
    {
        return HandleRequestAsync(async () =>
        {
            await _productRequirementAppService.SetLabelAsync(body.Id, body.Label);
        });
    }

    public record SetSvgTextRequestBody(
        ProductRequirementId Id,
        string SvgText
    );

    [FunctionName("api_productRequirement_setSvgText")]
    public Task<IActionResult> SetSvgTextAsync(
       [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "productRequirement/setSvgText")]
       SetSvgTextRequestBody body
    )
    {
        return HandleRequestAsync(async () =>
        {
            await _productRequirementAppService.SetSvgTextAsync(body.Id, body.SvgText);
        });
    }

    public record SetIndexRequestBody(
        ProductRequirementId Id,
        int Index
    );

    [FunctionName("api_productRequirement_setIndex")]
    public Task<IActionResult> SetIndexAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "productRequirement/setIndex")]
        SetIndexRequestBody body
    )
    {
        return HandleRequestAsync(async () =>
        {
            await _productRequirementAppService.SetIndexAsync(body.Id, body.Index);
        });
    }

    public record RemoveRequestBody(
        ProductRequirementId Id
    );

    [FunctionName("api_productRequirement_remove")]
    public Task<IActionResult> RemoveAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "productRequirement/remove")]
        RemoveRequestBody body
    )
    {
        return HandleRequestAsync(async () =>
        {
            await _productRequirementAppService.RemoveAsync(body.Id);
        });
    }
}
