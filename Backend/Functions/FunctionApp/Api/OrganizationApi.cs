using FunctionApp.ApiServices;
using FunctionApp.ApiServices.Exceptions;
using FunctionApp.ApiServices.OrganizationToken;

namespace FunctionApp.Api;

public class OrganizationApi : ApiFunction
{
    private readonly IOrganizationAppService _organizationAppService;
    private readonly IOrganizationTokenService _organizationTokenService;

    public OrganizationApi(
        IAppAuthContext auth,
        Bugsnag.IClient bugsnag,
        IOrganizationAppService organizationAppService,
        IOrganizationTokenService organizationTokenService
    ) : base(auth, bugsnag)
    {
        _organizationAppService = organizationAppService;
        _organizationTokenService = organizationTokenService;
    }

    public class GetRequestParams
    {
        public Guid? Id { get; set; }
    }

    [FunctionName("api_organization_get")]
    public Task<IActionResult> GetAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "organization/get")]
        GetRequestParams @params
    )
    {
        return HandleRequestAsync(async () =>
        {
            RequireParam(@params.Id, nameof(@params.Id));

            return await _organizationAppService.GetAsync(new OrganizationId(@params.Id))
                ?? throw new UserPresentableException("The requested organization does not exist.");
        });
    }

    public class GetByShortNameRequestParams
    {
        public string? OrganizationShortName { get; set; }
    }

    [FunctionName("api_organization_getByShortName")]
    public Task<IActionResult> GetByShortName(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "organization/getByShortName")]
        GetByShortNameRequestParams @params
    )
    {
        return HandleRequestAsync(async () =>
        {
            RequireParam(@params.OrganizationShortName, nameof(@params.OrganizationShortName));

            return await _organizationAppService.GetByShortNameAsync(@params.OrganizationShortName)
                ?? throw new UserPresentableException("The requested organization does not exist.");
        });
    }

    public class GetOrganizationTokenRequestParams
    {
        public string? OrganizationShortName { get; set; }
    }

    [FunctionName("api_organization_getOrganizationToken")]
    public Task<IActionResult> GetOrganizationToken(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "organization/getOrganizationToken")]
        GetOrganizationTokenRequestParams @params
    )
    {
        return HandleRequestAsync(async () =>
        {
            RequireParam(@params.OrganizationShortName, nameof(@params.OrganizationShortName));

            return await _organizationTokenService.CreateTokenAsync(@params.OrganizationShortName);
        });
    }

    public class NameIsAvailableRequestParams
    {
        public string? Name { get; set; }
    }

    [FunctionName("api_organization_nameIsAvailable")]
    public Task<IActionResult> NameIsAvailableAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "organization/nameIsAvailable")]
        NameIsAvailableRequestParams @params
    )
    {
        return HandleRequestAsync(async () =>
        {
            RequireParam(@params.Name, nameof(@params.Name));

            return await _organizationAppService.NameIsAvailableAsync(@params.Name);
        });
    }

    public class ShortNameIsAvailableRequestParams
    {
        public string? ShortName { get; set; }
    }

    [FunctionName("api_organization_shortNameIsAvailable")]
    public Task<IActionResult> ShortNameIsAvailableAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "organization/shortNameIsAvailable")]
        ShortNameIsAvailableRequestParams @params
    )
    {
        return HandleRequestAsync(async () =>
        {
            RequireParam(@params.ShortName, nameof(@params.ShortName));

            return await _organizationAppService.ShortNameIsAvailableAsync(@params.ShortName);
        });
    }

    public class ListRequestParams
    {
        public int? Skip { get; set; }
        public int? Take { get; set; }
        public int? ActiveFilter { get; set; }
        public string? Search { get; set; }
    }

    [FunctionName("api_organization_list")]
    public Task<IActionResult> ListAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "organization/list")]
        ListRequestParams @params
    )
    {
        return HandleRequestAsync(async () =>
        {
            RequireParam(@params.Skip, nameof(@params.Skip));
            RequireParam(@params.Take, nameof(@params.Take));
            RequireParam(@params.ActiveFilter, nameof(@params.ActiveFilter));

            return await _organizationAppService.ListAsync(
                @params.Skip.Value,
                @params.Take.Value,
                (ActiveFilter)@params.ActiveFilter.Value,
                @params.Search
            );
        });
    }

    public record AddRequestBody(
        string Name,
        string ShortName,
        EmailAddressDto OwnerEmail,
        PersonNameDto OwnerName,
        string OwnerPassword
    );

    [FunctionName("api_organization_add")]
    public Task<IActionResult> AddAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "organization/add")]
        AddRequestBody body
    )
    {
        return HandleRequestAsync(async () =>
        {
            return await _organizationAppService.AddAsync(
                body.Name,
                body.ShortName,
                body.OwnerEmail,
                body.OwnerName,
                body.OwnerPassword
            );
        });
    }

    public record SetActiveRequestBody(
        OrganizationId Id,
        bool Active
    );

    [FunctionName("api_organization_setActive")]
    public Task<IActionResult> SetActiveAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "organization/setActive")]
        SetActiveRequestBody body
    )
    {
        return HandleRequestAsync(async () =>
        {
            await _organizationAppService.SetActiveAsync(body.Id, body.Active);
        });
    }

    public record SetNameRequestBody(
        OrganizationId Id,
        string Name
    );

    [FunctionName("api_organization_setName")]
    public Task<IActionResult> SetNameAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "organization/setName")]
        SetNameRequestBody body
    )
    {
        return HandleRequestAsync(async () =>
        {
            await _organizationAppService.SetNameAsync(body.Id, body.Name);
        });
    }

    public record SetShortNameRequestBody(
        OrganizationId Id,
        string ShortName
    );

    [FunctionName("api_organization_setShortName")]
    public Task<IActionResult> SetShortNameAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "organization/setShortName")]
        SetShortNameRequestBody body
    )
    {
        return HandleRequestAsync(async () =>
        {
            await _organizationAppService.SetShortNameAsync(body.Id, body.ShortName);
        });
    }
}
