using FunctionApp.ApiServices;
using FunctionApp.ApiServices.Exceptions;

namespace FunctionApp.Api;

public class ComponentApi : ApiFunction
{
    private readonly IComponentAppService _componentAppService;

    public ComponentApi(
        IAppAuthContext auth,
        Bugsnag.IClient bugsnag,
        IComponentAppService componentAppService
    ) : base(auth, bugsnag)
    {
        _componentAppService = componentAppService;
    }

    public class GetRequestParams
    {
        public Guid? Id { get; set; }
    }

    [FunctionName("api_component_get")]
    public Task<IActionResult> GetAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "component/get")]
        GetRequestParams @params
    )
    {
        return HandleRequestAsync(async () =>
        {
            RequireParam(@params.Id, nameof(@params.Id));

            return await _componentAppService.GetAsync(new ComponentId(@params.Id))
                ?? throw new UserPresentableException("The requested component does not exist.");
        });
    }

    public class ListRequestParams
    {
        public Guid? OrganizationId { get; set; }
    }

    [FunctionName("api_component_list")]
    public Task<IActionResult> ListAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "component/list")]
        ListRequestParams @params
    )
    {
        return HandleRequestAsync(async () =>
        {
            RequireParam(@params.OrganizationId, nameof(@params.OrganizationId));

            return await _componentAppService.ListAsync(new OrganizationId(@params.OrganizationId));
        });
    }

    public class GetNewVersionNameRequestParams
    {
        public Guid? Id { get; set; }
    }

    [FunctionName("api_component_getNewVersionName")]
    public Task<IActionResult> GetNewVersionNameAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "component/getNewVersionName")]
        GetNewVersionNameRequestParams @params
    )
    {
        return HandleRequestAsync(async () =>
        {
            RequireParam(@params.Id, nameof(@params.Id));

            return await _componentAppService.GetNewVersionNameAsync(new ComponentId(@params.Id));
        });
    }

    public class VendorPartNumberIsAvailableRequestParams
    {
        public Guid? OrganizationId { get; set; }
        public Guid? ComponentId { get; set; }
        public string? VendorPartNumber { get; set; }
    }

    [FunctionName("api_component_vendorPartNumberIsAvailable")]
    public Task<IActionResult> VendorPartNumberIsAvailableAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "component/vendorPartNumberIsAvailable")]
        VendorPartNumberIsAvailableRequestParams @params
    )
    {
        return HandleRequestAsync(async () =>
        {
            RequireParam(@params.OrganizationId, nameof(@params.OrganizationId));
            RequireParam(@params.VendorPartNumber, nameof(@params.VendorPartNumber));

            var componentId = @params.ComponentId != null ? new ComponentId(@params.ComponentId) : null;

            return await _componentAppService.VendorPartNumberIsAvailableAsync(new OrganizationId(@params.OrganizationId), componentId, @params.VendorPartNumber);
        });
    }

    public class OrganizationPartNumberIsAvailableRequestParams
    {
        public Guid? OrganizationId { get; set; }
        public Guid? ComponentId { get; set; }
        public string? OrganizationPartNumber { get; set; }
    }

    [FunctionName("api_component_organizationPartNumberIsAvailable")]
    public Task<IActionResult> OrganizationPartNumberIsAvailableAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "component/organizationPartNumberIsAvailable")]
        OrganizationPartNumberIsAvailableRequestParams @params
    )
    {
        return HandleRequestAsync(async () =>
        {
            RequireParam(@params.OrganizationId, nameof(@params.OrganizationId));
            RequireParam(@params.OrganizationPartNumber, nameof(@params.OrganizationPartNumber));

            var componentId = @params.ComponentId != null ? new ComponentId(@params.ComponentId) : null;

            return await _componentAppService.OrganizationPartNumberIsAvailableAsync(new OrganizationId(@params.OrganizationId), componentId, @params.OrganizationPartNumber);
        });
    }

    public record AddRequestBody(
        OrganizationId OrganizationId,
        ComponentTypeId ComponentTypeId,
        MeasurementType MeasurementType,
        bool IsVideoDisplay,
        bool VisibleToCustomer,
        string DisplayName,
        string VersionName,
        decimal SellPrice,
        string? Url,
        string Make,
        string Model,
        string VendorPartNumber,
        string? OrganizationPartNumber,
        string? WhereToBuy,
        string? Style,
        string? Color,
        string? InternalNotes
    );

    [FunctionName("api_component_add")]
    public Task<IActionResult> AddAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "component/add")]
        AddRequestBody body
    )
    {
        return HandleRequestAsync((Func<Task<ComponentId>>)(async () =>
        {
            return await _componentAppService.AddAsync(
                body.OrganizationId,
                body.ComponentTypeId,
                body.MeasurementType,
                body.IsVideoDisplay,
                body.VisibleToCustomer,
                body.DisplayName,
                body.VersionName,
                body.SellPrice,
                body.Url,
                body.Make,
                body.Model,
                body.VendorPartNumber,
                body.OrganizationPartNumber,
                body.WhereToBuy,
                body.Style,
                body.Color,
                body.InternalNotes
            );
        }));
    }

    public record AddVersionRequestBody(
        ComponentId Id,
        string DisplayName,
        string VersionName,
        decimal SellPrice,
        string? Url,
        string Make,
        string Model,
        string VendorPartNumber,
        string? OrganizationPartNumber,
        string? WhereToBuy,
        string? Style,
        string? Color,
        string? InternalNotes
    );

    [FunctionName("api_component_addVersion")]
    public Task<IActionResult> AddVersionAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "component/addVersion")]
        AddVersionRequestBody body
    )
    {
        return HandleRequestAsync((Func<Task<ComponentVersionId>>)(async () =>
        {
            return await _componentAppService.AddVersionAsync(
                body.Id,
                body.DisplayName,
                body.VersionName,
                body.SellPrice,
                body.Url,
                body.Make,
                body.Model,
                body.VendorPartNumber,
                body.OrganizationPartNumber,
                body.WhereToBuy,
                body.Style,
                body.Color,
                body.InternalNotes
            );
        }));
    }

    public record UpdateVersionRequestBody(
        ComponentVersionId VersionId,
        string DisplayName,
        string? Url,
        string Make,
        string Model,
        string VendorPartNumber,
        string? OrganizationPartNumber,
        string? WhereToBuy,
        string? Style,
        string? Color,
        string? InternalNotes
    );

    [FunctionName("api_component_updateVersion")]
    public Task<IActionResult> UpdateVersionAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "component/updateVersion")]
        UpdateVersionRequestBody body
    )
    {
        return HandleRequestAsync(async () =>
        {
            await _componentAppService.UpdateVersionAsync(
                body.VersionId,
                body.DisplayName,
                body.Url,
                body.Make,
                body.Model,
                body.VendorPartNumber,
                body.OrganizationPartNumber,
                body.WhereToBuy,
                body.Style,
                body.Color,
                body.InternalNotes
            );
        });
    }

    public record SetActiveRequestBody(
        ComponentId Id,
        bool Active
    );

    [FunctionName("api_component_setActive")]
    public Task<IActionResult> SetActiveAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "component/setActive")]
        SetActiveRequestBody body
    )
    {
        return HandleRequestAsync(async () =>
        {
            await _componentAppService.SetActiveAsync(body.Id, body.Active);
        });
    }

    public record SetComponentTypeRequestBody(
        ComponentId Id,
        ComponentTypeId ComponentTypeId
    );

    [FunctionName("api_component_setComponentType")]
    public Task<IActionResult> SetComponentTypeAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "component/setComponentType")]
        SetComponentTypeRequestBody body
    )
    {
        return HandleRequestAsync(async () =>
        {
            await _componentAppService.SetComponentTypeAsync(body.Id, body.ComponentTypeId);
        });
    }

    public record SetVisibleToCustomerRequestBody(
        ComponentId Id,
        bool VisibleToCustomer
    );

    [FunctionName("api_component_setVisibleToCustomer")]
    public Task<IActionResult> SetVisibleToCustomerAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "component/setVisibleToCustomer")]
        SetVisibleToCustomerRequestBody body
    )
    {
        return HandleRequestAsync(async () =>
        {
            await _componentAppService.SetVisibleToCustomerAsync(body.Id, body.VisibleToCustomer);
        });
    }
}
