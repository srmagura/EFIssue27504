using FunctionApp.ApiServices;
using FunctionApp.ApiServices.Exceptions;

namespace FunctionApp.Api;

public class ProductPhotoApi : ApiFunction
{
    private readonly IProductPhotoAppService _productPhotoAppService;

    public ProductPhotoApi(
        IAppAuthContext auth,
        Bugsnag.IClient bugsnag,
        IProductPhotoAppService productPhotoAppService
    ) : base(auth, bugsnag)
    {
        _productPhotoAppService = productPhotoAppService;
    }

    public class GetRequestParams
    {
        public Guid? Id { get; set; }
    }

    [FunctionName("api_productPhoto_get")]
    public Task<IActionResult> GetAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "productPhoto/get")]
        GetRequestParams @params
    )
    {
        return HandleRequestAsync(async () =>
        {
            RequireParam(@params.Id, nameof(@params.Id));

            return await _productPhotoAppService.GetAsync(new ProductPhotoId(@params.Id))
                ?? throw new UserPresentableException("The requested project does not exist.");
        });
    }

    public class GetImageRequestParams
    {
        public Guid? Id { get; set; }
    }

    [FunctionName("api_productPhoto_getImage")]
    public Task<IActionResult> GetImageAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "productPhoto/getImage")]
        GetImageRequestParams @params
    )
    {
        return HandleRequestAsync<IActionResult>(async () =>
        {
            RequireParam(@params.Id, nameof(@params.Id));

            using var memoryStream = new MemoryStream();

            var fileType = await _productPhotoAppService.GetImageAsync(
                new ProductPhotoId(@params.Id),
                memoryStream
            );

            if (fileType == null)
                return new NotFoundResult();

            return new FileContentResult(memoryStream.ToArray(), fileType);
        });
    }

    public class ListRequestParams
    {
        public Guid? OrganizationId { get; set; }
        public int? Skip { get; set; }
        public int? Take { get; set; }
        public int? ActiveFilter { get; set; }
        public string? Search { get; set; }
    }

    [FunctionName("api_productPhoto_list")]
    public Task<IActionResult> ListAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "productPhoto/list")]
        ListRequestParams @params
    )
    {
        return HandleRequestAsync(async () =>
        {
            RequireParam(@params.OrganizationId, nameof(@params.OrganizationId));
            RequireParam(@params.Skip, nameof(@params.Skip));
            RequireParam(@params.Take, nameof(@params.Take));
            RequireParam(@params.ActiveFilter, nameof(@params.ActiveFilter));

            return await _productPhotoAppService.ListAsync(
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

    [FunctionName("api_productPhoto_nameIsAvailable")]
    public Task<IActionResult> NameIsAvailableAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "productPhoto/nameIsAvailable")]
        NameIsAvailableRequestParams @params
    )
    {
        return HandleRequestAsync(async () =>
        {
            RequireParam(@params.OrganizationId, nameof(@params.OrganizationId));
            RequireParam(@params.Name, nameof(@params.Name));

            return await _productPhotoAppService.NameIsAvailableAsync(new OrganizationId(@params.OrganizationId), @params.Name);
        });
    }

    [FunctionName("api_productPhoto_add")]
    public Task<IActionResult> AddAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "productPhoto/add")]
        HttpRequest request
    )
    {
        return HandleRequestAsync(async () =>
        {
            var organizationIdString = request.Form["organizationId"].Single();
            var organizationId = new OrganizationId(Guid.Parse(organizationIdString));

            var name = request.Form["name"].Single();

            var file = request.Form.Files[0];
            using var stream = file.OpenReadStream();

            return await _productPhotoAppService.AddAsync(
                organizationId,
                name,
                stream,
                file.ContentType
            );
        });
    }

    public record SetNameRequestBody(
        ProductPhotoId Id,
        string Name
    );

    [FunctionName("api_productPhoto_setName")]
    public Task<IActionResult> SetNameAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "productPhoto/setName")]
        SetNameRequestBody body
    )
    {
        return HandleRequestAsync(async () =>
        {
            await _productPhotoAppService.SetNameAsync(body.Id, body.Name);
        });
    }

    [FunctionName("api_productPhoto_setPhoto")]
    public Task<IActionResult> SetPhotoAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "productPhoto/setPhoto")]
        HttpRequest request
    )
    {
        return HandleRequestAsync(async () =>
        {
            var idString = request.Form["id"].Single();
            var id = new ProductPhotoId(Guid.Parse(idString));

            var file = request.Form.Files[0];
            using var stream = file.OpenReadStream();

            await _productPhotoAppService.SetPhotoAsync(id, stream, file.ContentType);
        });
    }

    public record SetActiveRequestBody(
        ProductPhotoId Id,
        bool Active
    );

    [FunctionName("api_productPhoto_setActive")]
    public Task<IActionResult> SetActiveAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "productPhoto/setActive")]
        SetActiveRequestBody body
    )
    {
        return HandleRequestAsync(async () =>
        {
            await _productPhotoAppService.SetActiveAsync(body.Id, body.Active);
        });
    }
}
