using FunctionApp.ApiServices;

namespace FunctionApp.Api;

public class LogoSetApi : ApiFunction
{
    private readonly ILogoSetAppService _logoSetAppService;

    public LogoSetApi(
        IAppAuthContext auth,
        Bugsnag.IClient bugsnag,
        ILogoSetAppService logoSetAppService
    ) : base(auth, bugsnag)
    {
        _logoSetAppService = logoSetAppService;
    }

    public class GetDarkLogoRequestParams
    {
        public Guid? Id { get; set; }
    }

    [FunctionName("api_logoSet_getDarkLogo")]
    public Task<IActionResult> GetDarkLogoAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "logoSet/getDarkLogo")]
        GetDarkLogoRequestParams @params
    )
    {
        return HandleRequestAsync<IActionResult>(async () =>
        {
            RequireParam(@params.Id, nameof(@params.Id));

            using var memoryStream = new MemoryStream();

            var fileType = await _logoSetAppService.GetDarkLogoAsync(new LogoSetId(@params.Id), memoryStream);

            if (fileType == null)
                return new NotFoundResult();

            return new FileContentResult(memoryStream.ToArray(), fileType);
        });
    }

    public class GetLightLogoRequestParams
    {
        public Guid? Id { get; set; }
    }

    [FunctionName("api_logoSet_getLightLogo")]
    public Task<IActionResult> GetLightLogoAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "logoSet/getLightLogo")]
        GetLightLogoRequestParams @params
    )
    {
        return HandleRequestAsync<IActionResult>(async () =>
        {
            RequireParam(@params.Id, nameof(@params.Id));

            using var memoryStream = new MemoryStream();

            var fileType = await _logoSetAppService.GetLightLogoAsync(new LogoSetId(@params.Id), memoryStream);

            if (fileType == null)
                return new NotFoundResult();

            return new FileContentResult(memoryStream.ToArray(), fileType);
        });
    }

    public class ListRequestParams
    {
        public Guid? OrganizationId { get; set; }
    }

    [FunctionName("api_logoSet_list")]
    public Task<IActionResult> ListAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "logoSet/list")]
        ListRequestParams @params
    )
    {
        return HandleRequestAsync(async () =>
        {
            RequireParam(@params.OrganizationId, nameof(@params.OrganizationId));

            return await _logoSetAppService.ListAsync(new OrganizationId(@params.OrganizationId));
        });
    }

    public class NameIsAvailableRequestParams
    {
        public Guid? OrganizationId { get; set; }
        public string? Name { get; set; }
    }

    [FunctionName("api_logoSet_nameIsAvailable")]
    public Task<IActionResult> NameIsAvailableAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "logoSet/nameIsAvailable")]
        NameIsAvailableRequestParams @params
    )
    {
        return HandleRequestAsync(async () =>
        {
            RequireParam(@params.OrganizationId, nameof(@params.OrganizationId));
            RequireParam(@params.Name, nameof(@params.Name));

            return await _logoSetAppService.NameIsAvailableAsync(new OrganizationId(@params.OrganizationId), @params.Name);
        });
    }

    [FunctionName("api_logoSet_add")]
    public Task<IActionResult> AddAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "logoSet/add")]
        HttpRequest request
    )
    {
        return HandleRequestAsync(async () =>
        {
            var organizationIdString = request.Form["organizationId"].Single();
            var organizationId = new OrganizationId(Guid.Parse(organizationIdString));

            var name = request.Form["name"].Single();
            var darkFile = request.Form.Files[0];
            var lightFile = request.Form.Files[1];

            using var darkStream = new MemoryStream();
            darkFile.CopyTo(darkStream);
            darkStream.Position = 0;

            using var lightStream = new MemoryStream();
            lightFile.CopyTo(lightStream);
            lightStream.Position = 0;

            return await _logoSetAppService.AddAsync(
                organizationId,
                name,
                darkStream,
                darkFile.ContentType,
                lightStream,
                lightFile.ContentType
            );
        });
    }

    public record SetNameRequestBody(
        LogoSetId Id,
        string Name
    );

    [FunctionName("api_logoSet_setName")]
    public Task<IActionResult> SetNameAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "logoSet/setName")]
        SetNameRequestBody body
    )
    {
        return HandleRequestAsync(async () =>
        {
            await _logoSetAppService.SetNameAsync(body.Id, body.Name);
        });
    }

    public record SetActiveRequestBody(
        LogoSetId Id,
        bool Active
    );

    [FunctionName("api_logoSet_setActive")]
    public Task<IActionResult> SetActiveAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "logoSet/setActive")]
        SetActiveRequestBody body
    )
    {
        return HandleRequestAsync(async () =>
        {
            await _logoSetAppService.SetActiveAsync(body.Id, body.Active);
        });
    }

    [FunctionName("api_logoSet_setDarkLogo")]
    public Task<IActionResult> SetDarkLogoAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "logoSet/setDarkLogo")]
        HttpRequest request
    )
    {
        return HandleRequestAsync(async () =>
        {
            var idString = request.Form["id"].Single();
            var id = new LogoSetId(Guid.Parse(idString));

            var file = request.Form.Files[0];
            using var stream = file.OpenReadStream();

            await _logoSetAppService.SetDarkLogoAsync(id, stream, file.ContentType);
        });
    }

    [FunctionName("api_logoSet_setLightLogo")]
    public Task<IActionResult> SetLightLogoAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "logoSet/setLightLogo")]
        HttpRequest request
    )
    {
        return HandleRequestAsync(async () =>
        {
            var idString = request.Form["id"].Single();
            var id = new LogoSetId(Guid.Parse(idString));

            var file = request.Form.Files[0];
            using var stream = file.OpenReadStream();

            await _logoSetAppService.SetLightLogoAsync(id, stream, file.ContentType);
        });
    }
}
