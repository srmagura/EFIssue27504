using FunctionApp.ApiServices;

namespace FunctionApp.Api;

public class PageApi : ApiFunction
{
    private readonly IPageAppService _pageAppService;

    public PageApi(
        IAppAuthContext auth,
        Bugsnag.IClient bugsnag,
        IPageAppService pageAppService
    ) : base(auth, bugsnag)
    {
        _pageAppService = pageAppService;
    }

    public class GetPdfRequestParams
    {
        public Guid? Id { get; set; }
    }

    [FunctionName("api_page_getPdf")]
    public Task<IActionResult> GetPdfAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "page/getPdf")]
        GetPdfRequestParams @params
    )
    {
        return HandleRequestAsync<IActionResult>(async () =>
        {
            RequireParam(@params.Id, nameof(@params.Id));

            using var memoryStream = new MemoryStream();

            var fileType = await _pageAppService.GetPdfAsync(
                new PageId(@params.Id),
                memoryStream
            );

            if (fileType == null)
                return new NotFoundResult();

            return new FileContentResult(memoryStream.ToArray(), fileType);
        });
    }

    public class GetThumbnailRequestParams
    {
        public Guid? Id { get; set; }
    }

    [FunctionName("api_page_getThumbnail")]
    public Task<IActionResult> GetThumbnailAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "page/getThumbnail")]
        GetThumbnailRequestParams @params
    )
    {
        return HandleRequestAsync<IActionResult>(async () =>
        {
            RequireParam(@params.Id, nameof(@params.Id));

            using var memoryStream = new MemoryStream();

            var fileType = await _pageAppService.GetThumbnailAsync(
                new PageId(@params.Id),
                memoryStream
            );

            if (fileType == null)
                return new NotFoundResult();

            return new FileContentResult(memoryStream.ToArray(), fileType);
        });
    }

    public class ListRequestParams
    {
        public Guid? ProjectId { get; set; }
        public int? ActiveFilter { get; set; }
    }

    [FunctionName("api_page_list")]
    public Task<IActionResult> ListAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "page/list")]
        ListRequestParams @params
    )
    {
        return HandleRequestAsync(async () =>
        {
            RequireParam(@params.ProjectId, nameof(@params.ProjectId));
            RequireParam(@params.ActiveFilter, nameof(@params.ActiveFilter));

            return await _pageAppService.ListAsync(
                new ProjectId(@params.ProjectId),
                (ActiveFilter)@params.ActiveFilter.Value
            );
        });
    }

    public record SetIndexRequestBody(
        PageId Id,
        bool DecreaseIndex
    );

    [FunctionName("api_page_setIndex")]
    public Task<IActionResult> SetIndexAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "page/setIndex")]
        SetIndexRequestBody body
    )
    {
        return HandleRequestAsync(async () =>
        {
            await _pageAppService.SetIndexAsync(body.Id, body.DecreaseIndex);
        });
    }

    public record DuplicateRequestBody(
        PageId Id
    );

    [FunctionName("api_page_duplicate")]
    public Task<IActionResult> DuplicateAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "page/duplicate")]
        DuplicateRequestBody body
    )
    {
        return HandleRequestAsync(async () =>
        {
            await _pageAppService.DuplicateAsync(body.Id);
        });
    }

    public record SetActiveRequestBody(
        PageId Id,
        bool IsActive
    );

    [FunctionName("api_page_setActive")]
    public Task<IActionResult> SetActiveAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "page/setActive")]
        SetActiveRequestBody body
    )
    {
        return HandleRequestAsync(async () =>
        {
            await _pageAppService.SetActiveAsync(body.Id, body.IsActive);
        });
    }

    public record SetSheetNumberAndNameRequestBody(
        PageId Id,
        SheetTypeId SheetTypeId,
        string SheetNumberSuffix,
        string? SheetNameSuffix
    );

    [FunctionName("api_page_setSheetNumberAndName")]
    public Task<IActionResult> SetSheetNumberAndNameAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "page/setSheetNumberAndName")]
        SetSheetNumberAndNameRequestBody body
    )
    {
        return HandleRequestAsync(async () =>
        {
            await _pageAppService.SetSheetNumberAndNameAsync(
                body.Id,
                body.SheetTypeId,
                body.SheetNumberSuffix,
                body.SheetNameSuffix
            );
        });
    }
}
