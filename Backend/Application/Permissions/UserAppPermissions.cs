#pragma warning disable S4136 // Method overloads should be grouped together
using DataInterfaces.Queries;
using Enumerations;
using Identities;
using InfraInterfaces;

namespace Permissions;

public class UserAppPermissions : IAppPermissions
{
    private readonly IAppPermissionsQueries _queries;
    private readonly IAppAuthContext _auth;

    public UserAppPermissions(
        IAppPermissionsQueries queries,
        IAppAuthContext auth
    )
    {
        _queries = queries;
        _auth = auth;
    }

    private bool IsSystemAdmin() => _auth.Role == UserRole.SystemAdmin;
    private bool IsOrganizationAdmin() => _auth.Role == UserRole.OrganizationAdmin || IsSystemAdmin();

    public Task<bool> CanViewAsync(OrganizationId organizationId)
    {
        if (IsSystemAdmin())
            return Task.FromResult(true);

        return Task.FromResult(_auth.OrganizationId == organizationId);
    }

    public Task<bool> CanManageOrganizationsAsync() => Task.FromResult(IsSystemAdmin());

    public Task<bool> CanViewUsersAsync(OrganizationId organizationId)
    {
        if (IsSystemAdmin())
            return Task.FromResult(true);

        return Task.FromResult(_auth.OrganizationId == organizationId);
    }

    public async Task<bool> CanManageAsync(UserId userId)
    {
        if (_auth.UserId == userId || IsSystemAdmin())
            return true;

        if (await _queries.UserRoleForAsync(userId) == UserRole.SystemAdmin)
            return false;

        return await CanManageUsersAsync(await _queries.OrganizationOfAsync(userId));
    }

    public Task<bool> CanManageUsersAsync(OrganizationId organizationId)
    {
        if (IsSystemAdmin())
            return Task.FromResult(true);

        if (IsOrganizationAdmin())
            return Task.FromResult(_auth.OrganizationId == organizationId);

        return Task.FromResult(false);
    }

    public async Task<bool> CanViewAsync(ProjectId projectId)
    {
        return await CanViewProjectsAsync(await _queries.OrganizationOfAsync(projectId));
    }

    public Task<bool> CanViewProjectsAsync(OrganizationId organizationId)
    {
        return Task.FromResult(_auth.OrganizationId == organizationId);
    }

    public Task<bool> CanManageProjectsAsync(OrganizationId organizationId)
    {
        return Task.FromResult(_auth.OrganizationId == organizationId);
    }

    public Task<bool> CanViewSymbolsAsync(OrganizationId organizationId)
    {
        if (IsSystemAdmin())
            return Task.FromResult(true);

        return Task.FromResult(_auth.OrganizationId == organizationId);
    }

    public Task<bool> CanManageSymbolsAsync(OrganizationId organizationId)
    {
        return Task.FromResult(IsOrganizationAdmin() && _auth.OrganizationId == organizationId);
    }

    public Task<bool> CanViewProductPhotosAsync(OrganizationId organizationId)
    {
        if (IsSystemAdmin())
            return Task.FromResult(true);

        return Task.FromResult(_auth.OrganizationId == organizationId);
    }

    public Task<bool> CanManageProductPhotosAsync(OrganizationId organizationId)
    {
        return Task.FromResult(IsOrganizationAdmin() && _auth.OrganizationId == organizationId);
    }

    public Task<bool> CanViewCategoriesAsync(OrganizationId organizationId)
    {
        if (IsSystemAdmin())
            return Task.FromResult(true);

        return Task.FromResult(_auth.OrganizationId == organizationId);
    }

    public Task<bool> CanManageCategoriesAsync(OrganizationId organizationId)
    {
        return Task.FromResult(IsOrganizationAdmin() && _auth.OrganizationId == organizationId);
    }

    public Task<bool> CanViewPagesAsync(OrganizationId organizationId)
    {
        return Task.FromResult(_auth.OrganizationId == organizationId);
    }

    public async Task<bool> CanManageAsync(PageId pageId)
    {
        return await CanManagePagesAsync(await _queries.OrganizationOfAsync(pageId));
    }

    public Task<bool> CanManagePagesAsync(OrganizationId organizationId)
    {
        return Task.FromResult(_auth.OrganizationId == organizationId);
    }

    public async Task<bool> CanManageDesignerDataAsync(PageId pageId)
    {
        return _auth.OrganizationId == await _queries.OrganizationOfAsync(pageId);
    }

    public async Task<bool> CanViewAsync(ComponentId componentId)
    {
        return await CanViewComponentsAsync(await _queries.OrganizationOfAsync(componentId));
    }

    public async Task<bool> CanViewAsync(ComponentVersionId componentVersionId)
    {
        return await CanViewComponentsAsync(await _queries.OrganizationOfAsync(componentVersionId));
    }

    public Task<bool> CanViewComponentsAsync(OrganizationId organizationId)
    {
        if (IsSystemAdmin())
            return Task.FromResult(true);

        return Task.FromResult(_auth.OrganizationId == organizationId);
    }

    public Task<bool> CanManageComponentsAsync(OrganizationId organizationId)
    {
        return Task.FromResult(IsOrganizationAdmin() && _auth.OrganizationId == organizationId);
    }

    public async Task<bool> CanViewAsync(ProductKitId productKitId)
    {
        return await CanViewProductKitsAsync(await _queries.OrganizationOfAsync(productKitId));
    }

    public async Task<bool> CanViewProductKitsAsync(ProjectId projectId)
    {
        return await CanViewProductKitsAsync(await _queries.OrganizationOfAsync(projectId));
    }

    public Task<bool> CanViewProductKitsAsync(OrganizationId organizationId)
    {
        if (IsSystemAdmin())
            return Task.FromResult(true);

        return Task.FromResult(_auth.OrganizationId == organizationId);
    }

    public Task<bool> CanManageProductKitsAsync(OrganizationId organizationId)
    {
        return Task.FromResult(IsOrganizationAdmin() && _auth.OrganizationId == organizationId);
    }

    public async Task<bool> CanViewProductKitReferencesAsync(ProjectId projectId)
    {
        return _auth.OrganizationId == await _queries.OrganizationOfAsync(projectId);
    }

    public Task<bool> CanManageProductKitReferencesAsync(OrganizationId organizationId)
    {
        return Task.FromResult(_auth.OrganizationId == organizationId);
    }

    public async Task<bool> CanManageProductKitReferencesAsync(ProjectId projectId)
    {
        return _auth.OrganizationId == await _queries.OrganizationOfAsync(projectId);
    }

    public Task<bool> CanViewProjectPublicationsAsync(OrganizationId organizationId)
    {
        return Task.FromResult(_auth.OrganizationId == organizationId);
    }

    public Task<bool> CanManageProjectPublicationsAsync(OrganizationId organizationId)
    {
        return Task.FromResult(_auth.OrganizationId == organizationId);
    }

    public Task<bool> CanViewReportsAsync(OrganizationId organizationId)
    {
        return Task.FromResult(_auth.OrganizationId == organizationId);
    }

    public Task<bool> CanViewTermsDocumentsAsync(OrganizationId organizationId)
    {
        return Task.FromResult(_auth.OrganizationId == organizationId);
    }

    public Task<bool> CanManageTermsDocumentsAsync(OrganizationId organizationId)
    {
        return Task.FromResult(IsOrganizationAdmin() && _auth.OrganizationId == organizationId);
    }

    public Task<bool> CanViewLogoSetsAsync(OrganizationId organizationId)
    {
        return Task.FromResult(_auth.OrganizationId == organizationId);
    }

    public Task<bool> CanManageLogoSetsAsync(OrganizationId organizationId)
    {
        return Task.FromResult(IsOrganizationAdmin() && _auth.OrganizationId == organizationId);
    }

    public Task<bool> CanViewSheetTypesAsync(OrganizationId organizationId)
    {
        return Task.FromResult(_auth.OrganizationId == organizationId);
    }

    public Task<bool> CanManageSheetTypesAsync(OrganizationId organizationId)
    {
        return Task.FromResult(IsOrganizationAdmin() && _auth.OrganizationId == organizationId);
    }

    public Task<bool> CanViewProductFamiliesAsync(OrganizationId organizationId)
    {
        if (IsSystemAdmin())
            return Task.FromResult(true);

        return Task.FromResult(_auth.OrganizationId == organizationId);
    }

    public Task<bool> CanManageProductFamiliesAsync(OrganizationId organizationId)
    {
        return Task.FromResult(IsOrganizationAdmin() && _auth.OrganizationId == organizationId);
    }

    public Task<bool> CanViewProductRequirementsAsync(OrganizationId organizationId)
    {
        if (IsSystemAdmin())
            return Task.FromResult(true);

        return Task.FromResult(_auth.OrganizationId == organizationId);
    }

    public async Task<bool> CanManageAsync(ProductRequirementId productRequirementId)
    {
        return await CanManageProductRequirementsAsync(await _queries.OrganizationOfAsync(productRequirementId));
    }

    public Task<bool> CanManageProductRequirementsAsync(OrganizationId organizationId)
    {
        return Task.FromResult(IsOrganizationAdmin() && _auth.OrganizationId == organizationId);
    }

    public Task<bool> CanViewGeneralNotesAsync(OrganizationId organizationId)
    {
        return Task.FromResult(_auth.OrganizationId == organizationId);
    }

    public Task<bool> CanManageGeneralNotesAsync(OrganizationId organizationId)
    {
        return Task.FromResult(IsOrganizationAdmin() && _auth.OrganizationId == organizationId);
    }

    public Task<bool> CanViewComponentTypesAsync(OrganizationId organizationId)
    {
        if (IsSystemAdmin())
            return Task.FromResult(true);

        return Task.FromResult(_auth.OrganizationId == organizationId);
    }

    public Task<bool> CanManageComponentTypesAsync(OrganizationId organizationId)
    {
        return Task.FromResult(IsOrganizationAdmin() && _auth.OrganizationId == organizationId);
    }

    public Task<bool> CanViewOrganizationOptionsAsync(OrganizationId organizationId)
    {
        return Task.FromResult(_auth.OrganizationId == organizationId);
    }

    public Task<bool> CanManageOrganizationOptionsAsync(OrganizationId organizationId)
    {
        return Task.FromResult(IsOrganizationAdmin() && _auth.OrganizationId == organizationId);
    }
}
