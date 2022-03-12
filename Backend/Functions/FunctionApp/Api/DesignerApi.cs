using FunctionApp.ApiServices;

namespace FunctionApp.Api;

public class DesignerApi : ApiFunction
{
    private readonly IDesignerAppService _designerAppService;

    public DesignerApi(
        IAppAuthContext auth,
        Bugsnag.IClient bugsnag,
        IDesignerAppService designerAppService
    ) : base(auth, bugsnag)
    {
        _designerAppService = designerAppService;
    }

    public class ListRequestParams
    {
        public Guid? ProjectId { get; set; }
    }

    [FunctionName("api_designer_list")]
    public Task<IActionResult> ListAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "designer/list")]
        ListRequestParams @params
    )
    {
        return HandleRequestAsync(async () =>
        {
            RequireParam(@params.ProjectId, nameof(@params.ProjectId));

            return await _designerAppService.ListAsync(new ProjectId(@params.ProjectId));
        });
    }

    public record SetRequestBody(
        PageId PageId,
        DesignerDataType Type,
        string Json
    );

    [FunctionName("api_designer_set")]
    public Task<IActionResult> SetAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "designer/set")]
        SetRequestBody body
    )
    {
        return HandleRequestAsync(async () =>
        {
            await _designerAppService.SetAsync(body.PageId, body.Type, body.Json);
        });
    }
}
