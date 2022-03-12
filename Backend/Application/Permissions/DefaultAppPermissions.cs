#pragma warning disable S4136 // Method overloads should be grouped together
using Identities;
using InfraInterfaces;

namespace Permissions;

public class DefaultAppPermissions : IAppPermissions
{
    private readonly IAppAuthContext _auth;
    private readonly UserAppPermissions _perms;

    public DefaultAppPermissions(IAppAuthContext auth, UserAppPermissions perms)
    {
        _auth = auth;
        _perms = perms;
    }

    public Task<bool> CanViewAsync(OrganizationId organizationId)
    {
        if (_auth.IsSystemProcess) return Task.FromResult(true);
        return _perms.CanViewAsync(organizationId);
    }

    public Task<bool> CanManageOrganizationsAsync()
    {
        if (_auth.IsSystemProcess) return Task.FromResult(true);
        return _perms.CanManageOrganizationsAsync();
    }

    public Task<bool> CanViewUsersAsync(OrganizationId organizationId)
    {
        if (_auth.IsSystemProcess) return Task.FromResult(true);
        return _perms.CanViewUsersAsync(organizationId);
    }

    public Task<bool> CanManageAsync(UserId userId)
    {
        if (_auth.IsSystemProcess) return Task.FromResult(true);
        return _perms.CanManageAsync(userId);
    }

    public Task<bool> CanManageUsersAsync(OrganizationId organizationId)
    {
        if (_auth.IsSystemProcess) return Task.FromResult(true);
        return _perms.CanManageUsersAsync(organizationId);
    }

    public Task<bool> CanViewAsync(ProjectId projectId)
    {
        if (_auth.IsSystemProcess) return Task.FromResult(true);
        return _perms.CanViewAsync(projectId);
    }

    public Task<bool> CanViewProjectsAsync(OrganizationId organizationId)
    {
        if (_auth.IsSystemProcess) return Task.FromResult(true);
        return _perms.CanViewProjectsAsync(organizationId);
    }

    public Task<bool> CanManageProjectsAsync(OrganizationId organizationId)
    {
        if (_auth.IsSystemProcess) return Task.FromResult(true);
        return _perms.CanManageProjectsAsync(organizationId);
    }

    public Task<bool> CanViewSymbolsAsync(OrganizationId organizationId)
    {
        if (_auth.IsSystemProcess) return Task.FromResult(true);
        return _perms.CanViewSymbolsAsync(organizationId);
    }

    public Task<bool> CanManageSymbolsAsync(OrganizationId organizationId)
    {
        if (_auth.IsSystemProcess) return Task.FromResult(true);
        return _perms.CanManageSymbolsAsync(organizationId);
    }

    public Task<bool> CanViewProductPhotosAsync(OrganizationId organizationId)
    {
        if (_auth.IsSystemProcess) return Task.FromResult(true);
        return _perms.CanViewProductPhotosAsync(organizationId);
    }

    public Task<bool> CanManageProductPhotosAsync(OrganizationId organizationId)
    {
        if (_auth.IsSystemProcess) return Task.FromResult(true);
        return _perms.CanManageProductPhotosAsync(organizationId);
    }

    public Task<bool> CanViewCategoriesAsync(OrganizationId organizationId)
    {
        if (_auth.IsSystemProcess) return Task.FromResult(true);
        return _perms.CanViewCategoriesAsync(organizationId);
    }

    public Task<bool> CanManageCategoriesAsync(OrganizationId organizationId)
    {
        if (_auth.IsSystemProcess) return Task.FromResult(true);
        return _perms.CanManageCategoriesAsync(organizationId);
    }

    public Task<bool> CanViewPagesAsync(OrganizationId organizationId)
    {
        if (_auth.IsSystemProcess) return Task.FromResult(true);
        return _perms.CanViewPagesAsync(organizationId);
    }

    public Task<bool> CanManageAsync(PageId pageId)
    {
        if (_auth.IsSystemProcess) return Task.FromResult(true);
        return _perms.CanManageAsync(pageId);
    }

    public Task<bool> CanManagePagesAsync(OrganizationId organizationId)
    {
        if (_auth.IsSystemProcess) return Task.FromResult(true);
        return _perms.CanManagePagesAsync(organizationId);
    }

    public Task<bool> CanManageDesignerDataAsync(PageId pageId)
    {
        if (_auth.IsSystemProcess) return Task.FromResult(true);
        return _perms.CanManageDesignerDataAsync(pageId);
    }

    public Task<bool> CanViewAsync(ComponentId componentId)
    {
        if (_auth.IsSystemProcess) return Task.FromResult(true);
        return _perms.CanViewAsync(componentId);
    }

    public Task<bool> CanViewAsync(ComponentVersionId componentVersionId)
    {
        if (_auth.IsSystemProcess) return Task.FromResult(true);
        return _perms.CanViewAsync(componentVersionId);
    }

    public Task<bool> CanViewComponentsAsync(OrganizationId organizationId)
    {
        if (_auth.IsSystemProcess) return Task.FromResult(true);
        return _perms.CanViewComponentsAsync(organizationId);
    }

    public Task<bool> CanManageComponentsAsync(OrganizationId organizationId)
    {
        if (_auth.IsSystemProcess) return Task.FromResult(true);
        return _perms.CanManageComponentsAsync(organizationId);
    }

    public Task<bool> CanViewAsync(ProductKitId productKitId)
    {
        if (_auth.IsSystemProcess) return Task.FromResult(true);
        return _perms.CanViewAsync(productKitId);
    }

    public Task<bool> CanViewProductKitsAsync(ProjectId projectId)
    {
        if (_auth.IsSystemProcess) return Task.FromResult(true);
        return _perms.CanViewProductKitsAsync(projectId);
    }

    public Task<bool> CanViewProductKitsAsync(OrganizationId organizationId)
    {
        if (_auth.IsSystemProcess) return Task.FromResult(true);
        return _perms.CanViewProductKitsAsync(organizationId);
    }

    public Task<bool> CanManageProductKitsAsync(OrganizationId organizationId)
    {
        if (_auth.IsSystemProcess) return Task.FromResult(true);
        return _perms.CanManageProductKitsAsync(organizationId);
    }

    public Task<bool> CanViewProductKitReferencesAsync(ProjectId projectId)
    {
        if (_auth.IsSystemProcess) return Task.FromResult(true);
        return _perms.CanViewProductKitReferencesAsync(projectId);
    }

    public Task<bool> CanManageProductKitReferencesAsync(OrganizationId organizationId)
    {
        if (_auth.IsSystemProcess) return Task.FromResult(true);
        return _perms.CanManageProductKitReferencesAsync(organizationId);
    }

    public Task<bool> CanManageProductKitReferencesAsync(ProjectId projectId)
    {
        if (_auth.IsSystemProcess) return Task.FromResult(true);
        return _perms.CanManageProductKitReferencesAsync(projectId);
    }

    public Task<bool> CanViewProjectPublicationsAsync(OrganizationId organizationId)
    {
        if (_auth.IsSystemProcess) return Task.FromResult(true);
        return _perms.CanViewProjectPublicationsAsync(organizationId);
    }

    public Task<bool> CanManageProjectPublicationsAsync(OrganizationId organizationId)
    {
        if (_auth.IsSystemProcess) return Task.FromResult(true);
        return _perms.CanManageProjectPublicationsAsync(organizationId);
    }

    public Task<bool> CanViewReportsAsync(OrganizationId organizationId)
    {
        if (_auth.IsSystemProcess) return Task.FromResult(true);
        return _perms.CanViewReportsAsync(organizationId);
    }

    public Task<bool> CanViewTermsDocumentsAsync(OrganizationId organizationId)
    {
        if (_auth.IsSystemProcess) return Task.FromResult(true);
        return _perms.CanViewTermsDocumentsAsync(organizationId);
    }

    public Task<bool> CanManageTermsDocumentsAsync(OrganizationId organizationId)
    {
        if (_auth.IsSystemProcess) return Task.FromResult(true);
        return _perms.CanManageTermsDocumentsAsync(organizationId);
    }

    public Task<bool> CanViewLogoSetsAsync(OrganizationId organizationId)
    {
        if (_auth.IsSystemProcess) return Task.FromResult(true);
        return _perms.CanViewLogoSetsAsync(organizationId);
    }

    public Task<bool> CanManageAsync(ProductRequirementId id)
    {
        if (_auth.IsSystemProcess) return Task.FromResult(true);
        return _perms.CanManageAsync(id);
    }

    public Task<bool> CanManageLogoSetsAsync(OrganizationId organizationId)
    {
        if (_auth.IsSystemProcess) return Task.FromResult(true);
        return _perms.CanManageLogoSetsAsync(organizationId);
    }

    public Task<bool> CanViewSheetTypesAsync(OrganizationId organizationId)
    {
        if (_auth.IsSystemProcess) return Task.FromResult(true);
        return _perms.CanViewSheetTypesAsync(organizationId);
    }

    public Task<bool> CanManageSheetTypesAsync(OrganizationId organizationId)
    {
        if (_auth.IsSystemProcess) return Task.FromResult(true);
        return _perms.CanManageSheetTypesAsync(organizationId);
    }

    public Task<bool> CanViewProductFamiliesAsync(OrganizationId organizationId)
    {
        if (_auth.IsSystemProcess) return Task.FromResult(true);
        return _perms.CanViewProductFamiliesAsync(organizationId);
    }

    public Task<bool> CanManageProductFamiliesAsync(OrganizationId organizationId)
    {
        if (_auth.IsSystemProcess) return Task.FromResult(true);
        return _perms.CanManageProductFamiliesAsync(organizationId);
    }

    public Task<bool> CanViewProductRequirementsAsync(OrganizationId organizationId)
    {
        if (_auth.IsSystemProcess) return Task.FromResult(true);
        return _perms.CanViewProductRequirementsAsync(organizationId);
    }

    public Task<bool> CanManageProductRequirementsAsync(OrganizationId organizationId)
    {
        if (_auth.IsSystemProcess) return Task.FromResult(true);
        return _perms.CanManageProductRequirementsAsync(organizationId);
    }

    public Task<bool> CanViewGeneralNotesAsync(OrganizationId organizationId)
    {
        if (_auth.IsSystemProcess) return Task.FromResult(true);
        return _perms.CanViewGeneralNotesAsync(organizationId);
    }

    public Task<bool> CanManageGeneralNotesAsync(OrganizationId organizationId)
    {
        if (_auth.IsSystemProcess) return Task.FromResult(true);
        return _perms.CanManageGeneralNotesAsync(organizationId);
    }

    public Task<bool> CanViewComponentTypesAsync(OrganizationId organizationId)
    {
        if (_auth.IsSystemProcess) return Task.FromResult(true);
        return _perms.CanViewComponentTypesAsync(organizationId);
    }

    public Task<bool> CanManageComponentTypesAsync(OrganizationId organizationId)
    {
        if (_auth.IsSystemProcess) return Task.FromResult(true);
        return _perms.CanManageComponentTypesAsync(organizationId);
    }

    public Task<bool> CanViewOrganizationOptionsAsync(OrganizationId organizationId)
    {
        if (_auth.IsSystemProcess) return Task.FromResult(true);
        return _perms.CanViewOrganizationOptionsAsync(organizationId);
    }

    public Task<bool> CanManageOrganizationOptionsAsync(OrganizationId organizationId)
    {
        if (_auth.IsSystemProcess) return Task.FromResult(true);
        return _perms.CanManageOrganizationOptionsAsync(organizationId);
    }
}
