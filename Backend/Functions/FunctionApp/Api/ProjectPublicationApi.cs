using FunctionApp.ApiServices;

namespace FunctionApp.Api;

public class ProjectPublicationApi : ApiFunction
{
    private readonly IProjectPublicationAppService _projectPublicationAppService;

    public ProjectPublicationApi(
        IAppAuthContext auth,
        Bugsnag.IClient bugsnag,
        IProjectPublicationAppService projectPublicationAppService
    ) : base(auth, bugsnag)
    {
        _projectPublicationAppService = projectPublicationAppService;
    }

    public class ListRequestParams
    {
        public Guid? ProjectId { get; set; }
    }

    [FunctionName("api_projectPublication_list")]
    public Task<IActionResult> ListAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "projectPublication/list")]
        ListRequestParams @params
    )
    {
        return HandleRequestAsync(async () =>
        {
            RequireParam(@params.ProjectId, nameof(@params.ProjectId));

            return await _projectPublicationAppService.ListAsync(
                new ProjectId(@params.ProjectId)
            );
        });
    }

    public record PublishRequestBody(
        ProjectId ProjectId
    );

    [FunctionName("api_projectPublication_publish")]
    public Task<IActionResult> PublishAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "projectPublication/publish")]
        PublishRequestBody body
    )
    {
        return HandleRequestAsync(async () =>
        {
            await _projectPublicationAppService.PublishAsync(
                body.ProjectId
            );
        });
    }

    public record SetReportsSentToCustomerRequestBody(
        ProjectPublicationId Id,
        bool ReportsSentToCustomer
    );

    [FunctionName("api_projectPublication_setReportsSentToCustomer")]
    public Task<IActionResult> SetReportsSentToCustomerAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "projectPublication/setReportsSentToCustomer")]
        SetReportsSentToCustomerRequestBody body
    )
    {
        return HandleRequestAsync(async () =>
        {
            await _projectPublicationAppService.SetReportsSentToCustomerAsync(body.Id, body.ReportsSentToCustomer);
        });
    }
}
