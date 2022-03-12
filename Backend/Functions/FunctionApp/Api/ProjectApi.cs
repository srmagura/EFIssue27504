using FunctionApp.ApiServices;
using FunctionApp.ApiServices.Exceptions;

namespace FunctionApp.Api;

public class ProjectApi : ApiFunction
{
    private readonly IProjectAppService _projectAppService;

    public ProjectApi(
        IAppAuthContext auth,
        Bugsnag.IClient bugsnag,
        IProjectAppService projectAppService
    ) : base(auth, bugsnag)
    {
        _projectAppService = projectAppService;
    }

    public class GetRequestParams
    {
        public Guid? Id { get; set; }
    }

    [FunctionName("api_project_get")]
    public Task<IActionResult> GetAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "project/get")] GetRequestParams @params
    )
    {
        return HandleRequestAsync(async () =>
        {
            RequireParam(@params.Id, nameof(@params.Id));

            return await _projectAppService.GetAsync(new ProjectId(@params.Id))
                ?? throw new UserPresentableException("The requested project does not exist.");
        });
    }

    public class GetPhotoRequestParams
    {
        public Guid? Id { get; set; }
    }

    [FunctionName("api_project_getPhoto")]
    public Task<IActionResult> GetPhotoAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "project/getPhoto")]
        GetPhotoRequestParams @params
    )
    {
        return HandleRequestAsync<IActionResult>(async () =>
        {
            RequireParam(@params.Id, nameof(@params.Id));

            using var memoryStream = new MemoryStream();

            var fileType = await _projectAppService.GetPhotoAsync(
                new ProjectId(@params.Id),
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
        public string? Search { get; set; }
    }

    [FunctionName("api_project_list")]
    public Task<IActionResult> ListAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "project/list")]
        ListRequestParams @params
    )
    {
        return HandleRequestAsync(async () =>
        {
            RequireParam(@params.OrganizationId, nameof(@params.OrganizationId));
            RequireParam(@params.Skip, nameof(@params.Skip));
            RequireParam(@params.Take, nameof(@params.Take));
            RequireParam(@params.ActiveFilter, nameof(@params.ActiveFilter));

            return await _projectAppService.ListAsync(
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

    [FunctionName("api_project_nameIsAvailable")]
    public Task<IActionResult> NameIsAvailableAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "project/nameIsAvailable")]
        NameIsAvailableRequestParams @params
    )
    {
        return HandleRequestAsync(async () =>
        {
            RequireParam(@params.OrganizationId, nameof(@params.OrganizationId));
            RequireParam(@params.Name, nameof(@params.Name));

            return await _projectAppService.NameIsAvailableAsync(new OrganizationId(@params.OrganizationId), @params.Name);
        });
    }

    public record AddRequestBody(
        OrganizationId OrganizationId,
        string Name,
        string ShortName,
        string Description,
        PartialAddressDto Address,
        string CustomerName,
        string SigneeName,
        LogoSetId LogoSetId,
        TermsDocumentId TermsDocumentId,
        int EstimatedSquareFeet
    );

    [FunctionName("api_project_add")]
    public Task<IActionResult> AddAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "project/add")]
        AddRequestBody body
    )
    {
        return HandleRequestAsync(async () =>
        {
            return await _projectAppService.AddAsync(
                body.OrganizationId,
                body.Name,
                body.ShortName,
                body.Description,
                body.Address,
                body.CustomerName,
                body.SigneeName,
                body.LogoSetId,
                body.TermsDocumentId,
                body.EstimatedSquareFeet
            );
        });
    }

    public record SetNameRequestBody(
        ProjectId Id,
        string Name,
        string ShortName
    );

    [FunctionName("api_project_setName")]
    public Task<IActionResult> SetNameAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "project/setName")]
        SetNameRequestBody body
    )
    {
        return HandleRequestAsync(async () =>
        {
            await _projectAppService.SetNameAsync(body.Id, body.Name, body.ShortName);
        });
    }

    public record SetDescriptionRequestBody(
        ProjectId Id,
        string Description
    );

    [FunctionName("api_project_setDescription")]
    public Task<IActionResult> SetDescriptionAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "project/setDescription")]
        SetDescriptionRequestBody body
    )
    {
        return HandleRequestAsync(async () =>
        {
            await _projectAppService.SetDescriptionAsync(body.Id, body.Description);
        });
    }

    public record SetCustomerNameRequestBody(
        ProjectId Id,
        string CustomerName
    );

    [FunctionName("api_project_setCustomerName")]
    public Task<IActionResult> SetCustomerNameAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "project/setCustomerName")]
        SetCustomerNameRequestBody body
    )
    {
        return HandleRequestAsync(async () =>
        {
            await _projectAppService.SetCustomerNameAsync(body.Id, body.CustomerName);
        });
    }

    public record SetEstimatedSquareFeetRequestBody(
        ProjectId Id,
        int EstimatedSquareFeet
    );

    [FunctionName("api_project_setEstimatedSquareFeet")]
    public Task<IActionResult> SetEstimatedSquareFeetAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "project/setEstimatedSquareFeet")]
        SetEstimatedSquareFeetRequestBody body
    )
    {
        return HandleRequestAsync(async () =>
        {
            await _projectAppService.SetEstimatedSquareFeetAsync(body.Id, body.EstimatedSquareFeet);
        });
    }

    [FunctionName("api_project_setPhoto")]
    public Task<IActionResult> SetPhotoAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "project/setPhoto")]
        HttpRequest request
    )
    {
        return HandleRequestAsync(async () =>
        {
            var idString = request.Form["id"].Single();
            var id = new ProjectId(Guid.Parse(idString));

            var file = request.Form.Files[0];
            using var stream = file.OpenReadStream();

            await _projectAppService.SetPhotoAsync(id, stream, file.ContentType);
        });
    }

    public record SetActiveRequestBody(
        ProjectId Id,
        bool Active
    );

    [FunctionName("api_project_setActive")]
    public Task<IActionResult> SetActiveAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "project/setActive")]
        SetActiveRequestBody body
    )
    {
        return HandleRequestAsync(async () =>
        {
            await _projectAppService.SetActiveAsync(body.Id, body.Active);
        });
    }

    public record SetAddressRequestBody(
        ProjectId Id,
        PartialAddressDto Address
    );

    [FunctionName("api_project_setAddress")]
    public Task<IActionResult> SetAddressAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "project/setAddress")]
        SetAddressRequestBody body
    )
    {
        return HandleRequestAsync(async () =>
        {
            await _projectAppService.SetAddressAsync(body.Id, body.Address);
        });
    }

    public record SetBudgetOptionsRequestBody(
        ProjectId Id,
        ProjectBudgetOptionsDto BudgetOptions
    );

    [FunctionName("api_project_setBudgetOptions")]
    public Task<IActionResult> SetBudgetOptionsAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "project/setBudgetOptions")]
        SetBudgetOptionsRequestBody body
    )
    {
        return HandleRequestAsync(async () =>
        {
            await _projectAppService.SetBudgetOptionsAsync(body.Id, body.BudgetOptions);
        });
    }

    public record SetReportOptionsRequestBody(
        ProjectId Id,
        ProjectReportOptionsDto ReportOptions
    );

    [FunctionName("api_project_setReportOptions")]
    public Task<IActionResult> SetReportOptionsAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "project/setReportOptions")]
        SetReportOptionsRequestBody body
    )
    {
        return HandleRequestAsync(async () =>
        {
            await _projectAppService.SetReportOptionsAsync(body.Id, body.ReportOptions);
        });
    }

    public record AcquireDesignerLockRequestBody(
        ProjectId Id
    );

    [FunctionName("api_project_acquireDesignerLock")]
    public Task<IActionResult> AcquireDesignerLockAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "project/acquireDesignerLock")]
        AcquireDesignerLockRequestBody body
    )
    {
        return HandleRequestAsync(async () =>
        {
            await _projectAppService.AcquireDesignerLockAsync(body.Id);
        });
    }

    public record ReleaseDesignerLockRequestBody(
        ProjectId Id
    );

    [FunctionName("api_project_releaseDesignerLock")]
    public Task<IActionResult> ReleaseDesignerLockAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "project/releaseDesignerLock")]
        ReleaseDesignerLockRequestBody body
    )
    {
        return HandleRequestAsync(async () =>
        {
            await _projectAppService.ReleaseDesignerLockAsync(body.Id);
        });
    }
}
