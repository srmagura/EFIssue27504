using FunctionApp.ApiServices;
using FunctionApp.ApiServices.Exceptions;

namespace FunctionApp.Api;

public class ProductKitApi : ApiFunction
{
    private readonly IProductKitAppService _productKitAppService;

    public ProductKitApi(
        IAppAuthContext auth,
        Bugsnag.IClient bugsnag,
        IProductKitAppService productKitAppService
    ) : base(auth, bugsnag)
    {
        _productKitAppService = productKitAppService;
    }

    public class GetRequestParams
    {
        public Guid? Id { get; set; }
    }

    [FunctionName("api_productKit_get")]
    public Task<IActionResult> GetAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "productKit/get")]
        GetRequestParams @params
    )
    {
        return HandleRequestAsync(async () =>
        {
            RequireParam(@params.Id, nameof(@params.Id));

            return await _productKitAppService.GetAsync(new ProductKitId(@params.Id))
                ?? throw new UserPresentableException("The requested product kit does not exist.");
        });
    }

    public class GetVersionRequestParams
    {
        public Guid? Id { get; set; }
    }

    [FunctionName("api_productKit_getVersion")]
    public Task<IActionResult> GetVersionAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "productKit/getVersion")]
        GetVersionRequestParams @params
    )
    {
        return HandleRequestAsync(async () =>
        {
            RequireParam(@params.Id, nameof(@params.Id));

            return await _productKitAppService.GetVersionAsync(new ProductKitVersionId(@params.Id));
        });
    }

    public class ListRequestParams
    {
        public Guid? OrganizationId { get; set; }
    }

    [FunctionName("api_productKit_list")]
    public Task<IActionResult> ListAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "productKit/list")]
        ListRequestParams @params
    )
    {
        return HandleRequestAsync(async () =>
        {
            RequireParam(@params.OrganizationId, nameof(@params.OrganizationId));

            return await _productKitAppService.ListAsync(new OrganizationId(@params.OrganizationId));
        });
    }

    public class ListForDesignerRequestParams
    {
        public Guid? ProjectId { get; set; }
    }

    [FunctionName("api_productKit_listForDesigner")]
    public Task<IActionResult> ListForDesignerAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "productKit/listForDesigner")]
        ListForDesignerRequestParams @params
    )
    {
        return HandleRequestAsync(async () =>
        {
            RequireParam(@params.ProjectId, nameof(@params.ProjectId));

            return await _productKitAppService.ListForDesignerAsync(new ProjectId(@params.ProjectId));
        });
    }

    public class ListForComponentRequestParams
    {
        public Guid? ComponentId { get; set; }
    }

    [FunctionName("api_productKit_listForComponent")]
    public Task<IActionResult> ListForComponentAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "productKit/listForComponent")]
        ListForComponentRequestParams @params
    )
    {
        return HandleRequestAsync(async () =>
        {
            RequireParam(@params.ComponentId, nameof(@params.ComponentId));

            return await _productKitAppService.ListForComponentAsync(new ComponentId(@params.ComponentId));
        });
    }

    public class ListForComponentVersionRequestParams
    {
        public Guid? ComponentVersionId { get; set; }
    }

    [FunctionName("api_productKit_listForComponentVersion")]
    public Task<IActionResult> ListForComponentVersionAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "productKit/listForComponentVersion")]
        ListForComponentVersionRequestParams @params
    )
    {
        return HandleRequestAsync(async () =>
        {
            RequireParam(@params.ComponentVersionId, nameof(@params.ComponentVersionId));

            return await _productKitAppService.ListForComponentVersionAsync(new ComponentVersionId(@params.ComponentVersionId));
        });
    }

    public class GetNewVersionNameRequestParams
    {
        public Guid? Id { get; set; }
    }

    [FunctionName("api_productKit_getNewVersionName")]
    public Task<IActionResult> GetNewVersionNameAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "productKit/getNewVersionName")]
        GetNewVersionNameRequestParams @params
    )
    {
        return HandleRequestAsync(async () =>
        {
            RequireParam(@params.Id, nameof(@params.Id));

            return await _productKitAppService.GetNewVersionNameAsync(new ProductKitId(@params.Id));
        });
    }

    public record AddRequestBody(
        OrganizationId OrganizationId,
        CategoryId CategoryId,
        string Name,
        string Description,
        string VersionName,
        SymbolId SymbolId,
        ProductPhotoId? ProductPhotoId,
        ComponentVersionId MainComponentVersionId,
        List<ProductKitComponentMapInputDto> ComponentMapDtos,
        ProductFamilyId? ProductFamilyId
    );

    [FunctionName("api_productKit_add")]
    public Task<IActionResult> AddAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "productKit/add")]
        AddRequestBody body
    )
    {
        return HandleRequestAsync(async () =>
        {
            return await _productKitAppService.AddAsync(body.OrganizationId, body.CategoryId, body.Name, body.Description, body.VersionName, body.SymbolId, body.ProductPhotoId, body.MainComponentVersionId, body.ComponentMapDtos, body.ProductFamilyId);
        });
    }

    public record AddVersionRequestBody(
        OrganizationId OrganizationId,
        ProductKitId ProductKitId,
        string Name,
        string Description,
        string VersionName,
        ComponentVersionId MainComponentVersionId,
        List<ProductKitComponentMapInputDto> ComponentMapDtos
    );

    [FunctionName("api_productKit_addVersion")]
    public Task<IActionResult> AddVersionAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "productKit/addVersion")]
        AddVersionRequestBody body
    )
    {
        return HandleRequestAsync(async () =>
        {
            return await _productKitAppService.AddVersionAsync(
                body.OrganizationId,
                body.ProductKitId,
                body.Name,
                body.Description,
                body.VersionName,
                body.MainComponentVersionId,
                body.ComponentMapDtos
            );
        });
    }

    public record SetCategoryRequestBody(
        ProductKitId Id,
        CategoryId CategoryId
    );

    [FunctionName("api_productKit_setCategory")]
    public Task<IActionResult> SetCategoryAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "productKit/setCategory")]
        SetCategoryRequestBody body
    )
    {
        return HandleRequestAsync(async () =>
        {
            await _productKitAppService.SetCategoryAsync(body.Id, body.CategoryId);
        });
    }

    public record SetNameRequestBody(
        ProductKitVersionId VersionId,
        string Name
    );

    [FunctionName("api_productKit_setName")]
    public Task<IActionResult> SetNameAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "productKit/setName")]
        SetNameRequestBody body
    )
    {
        return HandleRequestAsync(async () =>
        {
            await _productKitAppService.SetNameAsync(body.VersionId, body.Name);
        });
    }

    public record SetDescriptionRequestBody(
        ProductKitVersionId VersionId,
        string Description
    );

    [FunctionName("api_productKit_setDescription")]
    public Task<IActionResult> SetDescriptionAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "productKit/setDescription")]
        SetDescriptionRequestBody body
    )
    {
        return HandleRequestAsync(async () =>
        {
            await _productKitAppService.SetDescriptionAsync(body.VersionId, body.Description);
        });
    }

    public record SetSymbolRequestBody(
        ProductKitVersionId VersionId,
        SymbolId SymbolId
    );

    [FunctionName("api_productKit_setSymbol")]
    public Task<IActionResult> SetSymbolAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "productKit/setSymbol")]
        SetSymbolRequestBody body
    )
    {
        return HandleRequestAsync(async () =>
        {
            await _productKitAppService.SetSymbolAsync(body.VersionId, body.SymbolId);
        });
    }

    public record SetProductPhotoRequestBody(
        ProductKitVersionId VersionId,
        ProductPhotoId? ProductPhotoId
    );

    [FunctionName("api_productKit_setProductPhoto")]
    public Task<IActionResult> SetProductPhotoAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "productKit/setProductPhoto")]
        SetProductPhotoRequestBody body
    )
    {
        return HandleRequestAsync(async () =>
        {
            await _productKitAppService.SetProductPhotoAsync(body.VersionId, body.ProductPhotoId);
        });
    }

    public record SetActiveRequestBody(
        ProductKitId Id,
        bool Active
    );

    [FunctionName("api_productKit_setActive")]
    public Task<IActionResult> SetActiveAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "productKit/setActive")]
        SetActiveRequestBody body
    )
    {
        return HandleRequestAsync(async () =>
        {
            await _productKitAppService.SetActiveAsync(body.Id, body.Active);
        });
    }

    public record SetProductFamilyRequestBody(
        ProductKitId Id,
        ProductFamilyId? ProductFamilyId
    );

    [FunctionName("api_productKit_setProductFamily")]
    public Task<IActionResult> SetProductFamilyAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "productKit/setProductFamily")]
        SetProductFamilyRequestBody body
    )
    {
        return HandleRequestAsync(async () =>
        {
            await _productKitAppService.SetProductFamilyAsync(body.Id, body.ProductFamilyId);
        });
    }
}
