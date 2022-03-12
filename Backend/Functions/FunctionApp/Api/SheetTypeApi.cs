using FunctionApp.ApiServices;

namespace FunctionApp.Api;

public class SheetTypeApi : ApiFunction
{
    private readonly ISheetTypeAppService _sheetTypeAppService;

    public SheetTypeApi(
        IAppAuthContext auth,
        Bugsnag.IClient bugsnag,
        ISheetTypeAppService sheetTypeAppService
    ) : base(auth, bugsnag)
    {
        _sheetTypeAppService = sheetTypeAppService;
    }

    public class ListRequestParams
    {
        public Guid? OrganizationId { get; set; }
    }

    [FunctionName("api_sheetType_list")]
    public Task<IActionResult> ListAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "sheetType/list")]
        ListRequestParams @params
    )
    {
        return HandleRequestAsync(async () =>
        {
            RequireParam(@params.OrganizationId, nameof(@params.OrganizationId));

            return await _sheetTypeAppService.ListAsync(
                new OrganizationId(@params.OrganizationId.Value)
            );
        });
    }

    public record AddRequestBody(
        OrganizationId OrganizationId,
        string SheetNumberPrefix,
        string SheetNamePrefix
    );

    [FunctionName("api_sheetType_add")]
    public Task<IActionResult> AddAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "sheetType/add")]
        AddRequestBody body
    )
    {
        return HandleRequestAsync(async () =>
        {
            return await _sheetTypeAppService.AddAsync(
                body.OrganizationId,
                body.SheetNumberPrefix,
                body.SheetNamePrefix
            );
        });
    }

    public record SetPrefixesRequestBody(
        SheetTypeId Id,
        string SheetNumberPrefix,
        string SheetNamePrefix
    );

    [FunctionName("api_sheetType_setPrefixes")]
    public Task<IActionResult> SetPrefixesAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "sheetType/setPrefixes")]
        SetPrefixesRequestBody body
    )
    {
        return HandleRequestAsync(async () =>
        {
            await _sheetTypeAppService.SetPrefixesAsync(body.Id, body.SheetNumberPrefix, body.SheetNamePrefix);
        });
    }

    public record SetActiveRequestBody(
        SheetTypeId Id,
        bool Active
    );

    [FunctionName("api_sheetType_setActive")]
    public Task<IActionResult> SetActiveAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "sheetType/setActive")]
        SetActiveRequestBody body
    )
    {
        return HandleRequestAsync(async () =>
        {
            await _sheetTypeAppService.SetActiveAsync(body.Id, body.Active);
        });
    }
}
