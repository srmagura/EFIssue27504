using FunctionApp.ApiServices;
using ITI.DDD.Core;
using Permissions;

namespace FunctionApp.Api;

public class AppPermissionsApi : ApiFunction
{
    private readonly IUnitOfWorkProvider _uowp;
    private readonly IAppPermissions _appPermissions;
    private readonly IUserRoleService _userRoleService;
    private readonly IOrganizationAppService _organizationAppService;

    public AppPermissionsApi(
        IAppAuthContext auth,
        Bugsnag.IClient bugsnag,
        IUnitOfWorkProvider uowp,
        IAppPermissions appPermissions,
        IUserRoleService userRoleService,
        IOrganizationAppService organizationAppService
    ) : base(auth, bugsnag)
    {
        _uowp = uowp;
        _appPermissions = appPermissions;
        _userRoleService = userRoleService;
        _organizationAppService = organizationAppService;
    }

    private async Task<bool> EvaluatePermissionCoreAsync(string permissionName, List<string> args)
    {
        Guid G0()
        {
            return Guid.Parse(args[0]);
        }

        using var _ = _uowp.Begin();

        return permissionName switch
        {
            PermissionNames.CanViewOrganization => await _appPermissions.CanViewAsync(new OrganizationId(G0())),
            PermissionNames.CanManageOrganizations => await _appPermissions.CanManageOrganizationsAsync(),

            PermissionNames.CanViewUsers => await _appPermissions.CanViewUsersAsync(new OrganizationId(G0())),
            PermissionNames.CanManageUsers => await _appPermissions.CanManageUsersAsync(new OrganizationId(G0())),
            PermissionNames.CanManageUser => await _appPermissions.CanManageAsync(new UserId(G0())),

            PermissionNames.CanViewProjects => await _appPermissions.CanViewProjectsAsync(new OrganizationId(G0())),
            PermissionNames.CanManageProjects => await _appPermissions.CanManageProjectsAsync(new OrganizationId(G0())),

            PermissionNames.CanViewSymbols => await _appPermissions.CanViewSymbolsAsync(new OrganizationId(G0())),
            PermissionNames.CanManageSymbols => await _appPermissions.CanManageSymbolsAsync(new OrganizationId(G0())),

            PermissionNames.CanViewProductPhotos => await _appPermissions.CanViewProductPhotosAsync(new OrganizationId(G0())),
            PermissionNames.CanManageProductPhotos => await _appPermissions.CanManageProductPhotosAsync(new OrganizationId(G0())),

            PermissionNames.CanViewCategories => await _appPermissions.CanViewCategoriesAsync(new OrganizationId(G0())),
            PermissionNames.CanManageCategories => await _appPermissions.CanManageCategoriesAsync(new OrganizationId(G0())),

            PermissionNames.CanManagePages => await _appPermissions.CanManagePagesAsync(new OrganizationId(G0())),

            PermissionNames.CanViewComponents => await _appPermissions.CanViewComponentsAsync(new OrganizationId(G0())),
            PermissionNames.CanManageComponents => await _appPermissions.CanManageComponentsAsync(new OrganizationId(G0())),

            PermissionNames.CanViewProductKits => await _appPermissions.CanViewProductKitsAsync(new OrganizationId(G0())),
            PermissionNames.CanManageProductKits => await _appPermissions.CanManageProductKitsAsync(new OrganizationId(G0())),

            PermissionNames.CanViewProjectPublications => await _appPermissions.CanViewProjectPublicationsAsync(new OrganizationId(G0())),
            PermissionNames.CanManageProjectPublications => await _appPermissions.CanManageProjectPublicationsAsync(new OrganizationId(G0())),

            PermissionNames.CanViewTermsDocuments => await _appPermissions.CanViewTermsDocumentsAsync(new OrganizationId(G0())),
            PermissionNames.CanManageTermsDocuments => await _appPermissions.CanManageTermsDocumentsAsync(new OrganizationId(G0())),

            PermissionNames.CanViewLogoSets => await _appPermissions.CanViewLogoSetsAsync(new OrganizationId(G0())),
            PermissionNames.CanManageLogoSets => await _appPermissions.CanManageLogoSetsAsync(new OrganizationId(G0())),

            PermissionNames.CanViewSheetTypes => await _appPermissions.CanViewSheetTypesAsync(new OrganizationId(G0())),
            PermissionNames.CanManageSheetTypes => await _appPermissions.CanManageSheetTypesAsync(new OrganizationId(G0())),

            PermissionNames.CanViewProductFamilies => await _appPermissions.CanViewProductFamiliesAsync(new OrganizationId(G0())),
            PermissionNames.CanManageProductFamilies => await _appPermissions.CanManageProductFamiliesAsync(new OrganizationId(G0())),

            PermissionNames.CanViewProductRequirements => await _appPermissions.CanViewProductRequirementsAsync(new OrganizationId(G0())),
            PermissionNames.CanManageProductRequirements => await _appPermissions.CanManageProductRequirementsAsync(new OrganizationId(G0())),

            PermissionNames.CanViewGeneralNotes => await _appPermissions.CanViewGeneralNotesAsync(new OrganizationId(G0())),
            PermissionNames.CanManageGeneralNotes => await _appPermissions.CanManageGeneralNotesAsync(new OrganizationId(G0())),

            PermissionNames.CanViewComponentTypes => await _appPermissions.CanViewComponentTypesAsync(new OrganizationId(G0())),
            PermissionNames.CanManageComponentTypes => await _appPermissions.CanManageComponentTypesAsync(new OrganizationId(G0())),

            PermissionNames.CanViewOrganizationOptions => await _appPermissions.CanViewOrganizationOptionsAsync(new OrganizationId(G0())),
            PermissionNames.CanManageOrganizationOptions => await _appPermissions.CanManageOrganizationOptionsAsync(new OrganizationId(G0())),

            _ => false, // Unrecognized permission
        };
    }

    private async Task<PermissionDto> EvaluatePermissionAsync(string permissionName, List<string> args)
    {
        try
        {
            var isPermitted = await EvaluatePermissionCoreAsync(permissionName, args);

            return new PermissionDto
            {
                Name = permissionName,
                Args = args,
                IsPermitted = isPermitted,
            };
        }
        catch (IndexOutOfRangeException e)
        {
            throw new Exception($"Did not receive the expected number of arguments for {permissionName}.", e);
        }
        catch (FormatException e)
        {
            throw new Exception($"Failed parsing an argument for {permissionName}.", e);
        }
    }

    [FunctionName("api_appPermissions_get")]
    public Task<IActionResult> GetAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "appPermissions/get")]
        PermissionRequestDto[] permissionRequests
    )
    {
        return HandleRequestAsync(async () =>
        {
            var permissionsList = new List<PermissionDto>();

            foreach (var permissionRequest in permissionRequests)
            {
                permissionsList.Add(await EvaluatePermissionAsync(permissionRequest.Name, permissionRequest.Args));
            }

            return permissionsList;
        });
    }

    public class GetGrantableUserRolesRequestParams
    {
        public Guid? TargetUserOrganizationId { get; set; }
    }

    [FunctionName("api_appPermissions_getGrantableUserRoles")]
    public Task<IActionResult> GetGrantableUserRolesAsync(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "appPermissions/getGrantableUserRoles")]
        GetGrantableUserRolesRequestParams @params
    )
    {
        return HandleRequestAsync(async () =>
        {
            RequireParam(@params.TargetUserOrganizationId, nameof(@params.TargetUserOrganizationId));

            var targetOrganization = await _organizationAppService.GetAsync(
                new OrganizationId(@params.TargetUserOrganizationId)
            );

            if (targetOrganization == null)
                throw new Exception("Organization does not exist.");

            return _userRoleService.GetGrantableRoles(targetOrganization.IsHost);
        });
    }
}
