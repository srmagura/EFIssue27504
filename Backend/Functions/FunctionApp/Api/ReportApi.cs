using FunctionApp.ApiServices;
using FunctionApp.ApiServices.Exceptions;

namespace FunctionApp.Api;

public class ReportApi : ApiFunction
{
    private readonly IReportAppService _reportAppService;

    public ReportApi(
        IAppAuthContext auth,
        Bugsnag.IClient bugsnag,
        IReportAppService reportAppService
    ) : base(auth, bugsnag)
    {
        _reportAppService = reportAppService;
    }

    public class GetRequestParams
    {
        public Guid? Id { get; set; }
    }

    [FunctionName("api_report_get")]
    public Task<IActionResult> GetAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "report/get")]
        GetRequestParams @params
    )
    {
        return HandleRequestAsync(async () =>
        {
            RequireParam(@params.Id, nameof(@params.Id));

            return await _reportAppService.GetAsync(new ReportId(@params.Id))
                ?? throw new UserPresentableException("The requested report does not exist.");
        });
    }

    public class GetPdfRequestParams
    {
        public Guid? Id { get; set; }
    }

    [FunctionName("api_report_getPdf")]
    public Task<IActionResult> GetImageAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "report/getPdf")]
        GetPdfRequestParams @params
    )
    {
        return HandleRequestAsync<IActionResult>(async () =>
        {
            RequireParam(@params.Id, nameof(@params.Id));

            using var memoryStream = new MemoryStream();

            var fileType = await _reportAppService.GetPdfAsync(
                new ReportId(@params.Id),
                memoryStream
            );

            if (fileType == null)
                return new NotFoundResult();

            return new FileContentResult(memoryStream.ToArray(), fileType);
        });
    }

    public record RequestDraftRequestBody(
        ProjectId ProjectId,
        ReportType ReportType
    );

    [FunctionName("api_report_requestDraft")]
    public Task<IActionResult> RequestDraftAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "report/requestDraft")]
        RequestDraftRequestBody body
    )
    {
        return HandleRequestAsync(async () =>
        {
            return await _reportAppService.RequestDraftAsync(body.ProjectId, body.ReportType);
        });
    }
}
