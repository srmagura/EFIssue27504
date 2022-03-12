using FunctionApp.ApiServices;
using FunctionApp.ApiServices.Exceptions;

namespace FunctionApp.Api;

public class SymbolApi : ApiFunction
{
    private readonly ISymbolAppService _symbolAppService;

    public SymbolApi(
        IAppAuthContext auth,
        Bugsnag.IClient bugsnag,
        ISymbolAppService symbolAppService
    ) : base(auth, bugsnag)
    {
        _symbolAppService = symbolAppService;
    }

    public class GetRequestParams
    {
        public Guid? Id { get; set; }
    }

    [FunctionName("api_symbol_get")]
    public Task<IActionResult> GetAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "symbol/get")]
        GetRequestParams @params
    )
    {
        return HandleRequestAsync(async () =>
        {
            RequireParam(@params.Id, nameof(@params.Id));

            return await _symbolAppService.GetAsync(new SymbolId(@params.Id))
                ?? throw new UserPresentableException("The requested symbol does not exist.");
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

    [FunctionName("api_symbol_list")]
    public Task<IActionResult> ListAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "symbol/list")]
        ListRequestParams @params
    )
    {
        return HandleRequestAsync(async () =>
        {
            RequireParam(@params.OrganizationId, nameof(@params.OrganizationId));
            RequireParam(@params.Skip, nameof(@params.Skip));
            RequireParam(@params.Take, nameof(@params.Take));
            RequireParam(@params.ActiveFilter, nameof(@params.ActiveFilter));

            return await _symbolAppService.ListAsync(
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

    [FunctionName("api_symbol_nameIsAvailable")]
    public Task<IActionResult> NameIsAvailableAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "symbol/nameIsAvailable")]
        NameIsAvailableRequestParams @params
    )
    {
        return HandleRequestAsync(async () =>
        {
            RequireParam(@params.OrganizationId, nameof(@params.OrganizationId));
            RequireParam(@params.Name, nameof(@params.Name));

            return await _symbolAppService.NameIsAvailableAsync(new OrganizationId(@params.OrganizationId), @params.Name);
        });
    }

    public record AddRequestBody(
        OrganizationId OrganizationId,
        string Name,
        string SvgText
    );

    [FunctionName("api_symbol_add")]
    public Task<IActionResult> AddAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "symbol/add")]
        AddRequestBody body
    )
    {
        return HandleRequestAsync(async () =>
        {
            return await _symbolAppService.AddAsync(body.OrganizationId, body.Name, body.SvgText);
        });
    }

    public record SetNameRequestBody(
        SymbolId Id,
        string Name
    );

    [FunctionName("api_symbol_setName")]
    public Task<IActionResult> SetNameAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "symbol/setName")]
        SetNameRequestBody body
    )
    {
        return HandleRequestAsync(async () =>
        {
            await _symbolAppService.SetNameAsync(body.Id, body.Name);
        });
    }

    public record SetSvgTextRequestBody(
        SymbolId Id,
        string SvgText
    );

    [FunctionName("api_symbol_setSvgText")]
    public Task<IActionResult> SetSvgTextAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "symbol/setSvgText")]
        SetSvgTextRequestBody body
    )
    {
        return HandleRequestAsync(async () =>
        {
            await _symbolAppService.SetSvgTextAsync(body.Id, body.SvgText);
        });
    }

    public record SetActiveRequestBody(
        SymbolId Id,
        bool Active
    );

    [FunctionName("api_symbol_setActive")]
    public Task<IActionResult> SetActiveAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "symbol/setActive")]
        SetActiveRequestBody body
    )
    {
        return HandleRequestAsync(async () =>
        {
            await _symbolAppService.SetActiveAsync(body.Id, body.Active);
        });
    }
}
