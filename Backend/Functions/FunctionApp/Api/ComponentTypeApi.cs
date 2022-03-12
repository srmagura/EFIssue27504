using FunctionApp.ApiServices;
using FunctionApp.ApiServices.Exceptions;

namespace FunctionApp.Api;

public class ComponentTypeApi : ApiFunction
{
    private readonly IComponentTypeAppService _componentTypeAppService;

    public ComponentTypeApi(
        IAppAuthContext auth,
        Bugsnag.IClient bugsnag,
        IComponentTypeAppService componentTypeAppService
    ) : base(auth, bugsnag)
    {
        _componentTypeAppService = componentTypeAppService;
    }

    public class GetRequestParams
    {
        public Guid? Id { get; set; }
    }

    [FunctionName("api_componentType_get")]
    public Task<IActionResult> GetAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "componentType/get")]
        GetRequestParams @params
    )
    {
        return HandleRequestAsync(async () =>
        {
            RequireParam(@params.Id, nameof(@params.Id));

            return await _componentTypeAppService.GetAsync(new ComponentTypeId(@params.Id))
                ?? throw new UserPresentableException("The requested component type does not exist.");
        });
    }

    public class ListRequestParams
    {
        public Guid? OrganizationId { get; set; }
        public int? ActiveFilter { get; set; }
    }

    [FunctionName("api_componentType_list")]
    public Task<IActionResult> ListAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "componentType/list")]
        ListRequestParams @params
    )
    {
        return HandleRequestAsync(async () =>
        {
            RequireParam(@params.OrganizationId, nameof(@params.OrganizationId));
            RequireParam(@params.ActiveFilter, nameof(@params.ActiveFilter));

            return await _componentTypeAppService.ListAsync(
                new OrganizationId(@params.OrganizationId),
                (ActiveFilter)@params.ActiveFilter.Value
            );
        });
    }

    public class NameIsAvailableRequestParams
    {
        public Guid? OrganizationId { get; set; }
        public string? Name { get; set; }
    }

    [FunctionName("api_componentType_nameIsAvailable")]
    public Task<IActionResult> NameIsAvailableAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "componentType/nameIsAvailable")]
        NameIsAvailableRequestParams @params
    )
    {
        return HandleRequestAsync(async () =>
        {
            RequireParam(@params.OrganizationId, nameof(@params.OrganizationId));
            RequireParam(@params.Name, nameof(@params.Name));

            return await _componentTypeAppService.NameIsAvailableAsync(new OrganizationId(@params.OrganizationId), @params.Name);
        });
    }

    public record AddRequestBody(
        OrganizationId OrganizationId,
        string Name
    );

    [FunctionName("api_componentType_add")]
    public Task<IActionResult> AddAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "componentType/add")]
        AddRequestBody body
    )
    {
        return HandleRequestAsync(async () =>
        {
            return await _componentTypeAppService.AddAsync(body.OrganizationId, body.Name);
        });
    }

    public record SetNameRequestBody(
        ComponentTypeId Id,
        string Name
    );

    [FunctionName("api_componentType_setName")]
    public Task<IActionResult> SetNameAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "componentType/setName")]
        SetNameRequestBody body
    )
    {
        return HandleRequestAsync(async () =>
        {
            await _componentTypeAppService.SetNameAsync(body.Id, body.Name);
        });
    }

    public record SetActiveRequestBody(
        ComponentTypeId Id,
        bool Active
    );

    [FunctionName("api_componentType_setActive")]
    public Task<IActionResult> SetActiveAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "componentType/setActive")]
        SetActiveRequestBody body
    )
    {
        return HandleRequestAsync(async () =>
        {
            await _componentTypeAppService.SetActiveAsync(body.Id, body.Active);
        });
    }
}
