using FunctionApp.ApiServices;
using ITI.Baseline.Util;

namespace FunctionApp.Api;

public class ImportApi : ApiFunction
{
    private readonly IImportAppService _importAppService;

    public ImportApi(
        IAppAuthContext auth,
        Bugsnag.IClient bugsnag,
        IImportAppService importAppService
    ) : base(auth, bugsnag)
    {
        _importAppService = importAppService;
    }

    public class ListRequestParams
    {
        public Guid? ProjectId { get; set; }
        public int? Skip { get; set; }
        public int? Take { get; set; }
    }

    [FunctionName("api_import_list")]
    public Task<IActionResult> ListAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "import/list")]
        ListRequestParams @params
    )
    {
        return HandleRequestAsync(async () =>
        {
            RequireParam(@params.ProjectId, nameof(@params.ProjectId));
            RequireParam(@params.Skip, nameof(@params.Skip));
            RequireParam(@params.Take, nameof(@params.Take));

            return await _importAppService.ListAsync(
                new ProjectId(@params.ProjectId.Value),
                @params.Skip.Value,
                @params.Take.Value
            );
        });
    }

    [FunctionName("api_import_import")]
    public Task<IActionResult> ImportAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "import/import")]
        HttpRequest request
    )
    {
        return HandleRequestAsync(async () =>
        {
            var projectIdString = request.Form["projectId"].Single();
            var projectId = new ProjectId(Guid.Parse(projectIdString));

            var file = request.Form.Files[0];
            Require.IsTrue(
                file.ContentType.Equals("application/pdf", StringComparison.OrdinalIgnoreCase),
                "The file must be a PDF."
            );

            using var stream = file.OpenReadStream();

            return await _importAppService.ImportAsync(projectId, file.FileName, stream);
        });
    }
}
