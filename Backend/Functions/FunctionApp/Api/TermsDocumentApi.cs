using FunctionApp.ApiServices;
using ITI.Baseline.Util;

namespace FunctionApp.Api;

public class TermsDocumentApi : ApiFunction
{
    private readonly ITermsDocumentAppService _termsDocumentAppService;

    public TermsDocumentApi(
        IAppAuthContext auth,
        Bugsnag.IClient bugsnag,
        ITermsDocumentAppService termsDocumentAppService
    ) : base(auth, bugsnag)
    {
        _termsDocumentAppService = termsDocumentAppService;
    }

    public class GetPdfRequestParams
    {
        public Guid? Id { get; set; }
    }

    [FunctionName("api_termsDocument_getPdf")]
    public Task<IActionResult> GetPdfAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "termsDocument/getPdf")]
        GetPdfRequestParams @params
    )
    {
        return HandleRequestAsync<IActionResult>(async () =>
        {
            RequireParam(@params.Id, nameof(@params.Id));

            using var memoryStream = new MemoryStream();

            var fileType = await _termsDocumentAppService.GetPdfAsync(
                new TermsDocumentId(@params.Id),
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
    }

    [FunctionName("api_termsDocument_list")]
    public Task<IActionResult> ListAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "termsDocument/list")]
        ListRequestParams @params
    )
    {
        return HandleRequestAsync(async () =>
        {
            RequireParam(@params.OrganizationId, nameof(@params.OrganizationId));
            RequireParam(@params.Skip, nameof(@params.Skip));
            RequireParam(@params.Take, nameof(@params.Take));
            RequireParam(@params.ActiveFilter, nameof(@params.ActiveFilter));

            return await _termsDocumentAppService.ListAsync(
                new OrganizationId(@params.OrganizationId),
                @params.Skip.Value,
                @params.Take.Value,
                (ActiveFilter)@params.ActiveFilter.Value
            );
        });
    }

    [FunctionName("api_termsDocument_add")]
    public Task<IActionResult> AddAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "termsDocument/add")]
        HttpRequest request
    )
    {
        return HandleRequestAsync(async () =>
        {
            var organizationIdString = request.Form["organizationId"].Single();
            var organizationId = new OrganizationId(Guid.Parse(organizationIdString));

            var file = request.Form.Files[0];
            Require.IsTrue(
                file.ContentType.Equals("application/pdf", StringComparison.OrdinalIgnoreCase),
                "The file must be a PDF."
            );

            using var stream = file.OpenReadStream();

            return await _termsDocumentAppService.AddAsync(organizationId, stream);
        });
    }

    public record SetActiveRequestBody(
        TermsDocumentId Id,
        bool Active
    );

    [FunctionName("api_termsDocument_setActive")]
    public Task<IActionResult> SetActiveAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "termsDocument/setActive")]
        SetActiveRequestBody body
    )
    {
        return HandleRequestAsync(async () =>
        {
            await _termsDocumentAppService.SetActiveAsync(body.Id, body.Active);
        });
    }
}
