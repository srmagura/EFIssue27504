using FunctionApp.ApiServices;

namespace FunctionApp.Api;

public class OrganizationOptionsApi : ApiFunction
{
    private readonly IOrganizationOptionsAppService _organizationOptionsAppService;

    public OrganizationOptionsApi(
        IAppAuthContext auth,
        Bugsnag.IClient bugsnag,
        IOrganizationOptionsAppService organizationOptionsAppService
    ) : base(auth, bugsnag)
    {
        _organizationOptionsAppService = organizationOptionsAppService;
    }

    public class GetDefaultProjectDescriptionRequestParams
    {
        public Guid? OrganizationId { get; set; }
    }

    [FunctionName("api_organizationOptions_getDefaultProjectDescription")]
    public Task<IActionResult> GetDefaultProjectDescriptionAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "organizationOptions/getDefaultProjectDescription")]
        GetDefaultProjectDescriptionRequestParams @params
    )
    {
        return HandleRequestAsync(async () =>
        {
            RequireParam(@params.OrganizationId, nameof(@params.OrganizationId));

            return await _organizationOptionsAppService.GetDefaultProjectDescriptionAsync(new OrganizationId(@params.OrganizationId));
        });
    }

    public record SetDefaultProjectDescriptionRequestBody(
        OrganizationId OrganizationId,
        string Description
    );

    [FunctionName("api_organizationOptions_setDefaultProjectDescription")]
    public Task<IActionResult> SetDefaultProjectDescriptionAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "organizationOptions/setDefaultProjectDescription")]
        SetDefaultProjectDescriptionRequestBody body
    )
    {
        return HandleRequestAsync(async () =>
        {
            await _organizationOptionsAppService.SetDefaultProjectDescriptionAsync(body.OrganizationId, body.Description);
        });
    }

    public class GetNotForConstructionDisclaimerTextRequestParams
    {
        public Guid? OrganizationId { get; set; }
    }

    [FunctionName("api_organizationOptions_getNotForConstructionDisclaimerText")]
    public Task<IActionResult> GetNotForConstructionDisclaimerTextAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "organizationOptions/getNotForConstructionDisclaimerText")]
        GetNotForConstructionDisclaimerTextRequestParams @params
    )
    {
        return HandleRequestAsync(async () =>
        {
            RequireParam(@params.OrganizationId, nameof(@params.OrganizationId));

            return await _organizationOptionsAppService.GetNotForConstructionDisclaimerTextAsync(new OrganizationId(@params.OrganizationId));
        });
    }

    public record SetNotForConstructionDisclaimerTextRequestBody(
        OrganizationId OrganizationId,
        string Disclaimer
    );

    [FunctionName("api_organizationOptions_setNotForConstructionDisclaimerText")]
    public Task<IActionResult> SetNotForConstructionDisclaimerTextAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "organizationOptions/setNotForConstructionDisclaimerText")]
        SetNotForConstructionDisclaimerTextRequestBody body
    )
    {
        return HandleRequestAsync(async () =>
        {
            await _organizationOptionsAppService.SetNotForConstructionDisclaimerTextAsync(body.OrganizationId, body.Disclaimer);
        });
    }
}
