#pragma warning disable S4136 // Method overloads should be grouped together
using Identities;

namespace Permissions;

public interface IAppPermissions
{
    Task<bool> CanViewAsync(OrganizationId organizationId);
    Task<bool> CanManageOrganizationsAsync();

    Task<bool> CanViewUsersAsync(OrganizationId organizationId);
    Task<bool> CanManageAsync(UserId userId);
    Task<bool> CanManageUsersAsync(OrganizationId organizationId);

    Task<bool> CanViewAsync(ProjectId projectId);
    Task<bool> CanViewProjectsAsync(OrganizationId organizationId);
    Task<bool> CanManageProjectsAsync(OrganizationId organizationId);

    Task<bool> CanViewSymbolsAsync(OrganizationId organizationId);
    Task<bool> CanManageSymbolsAsync(OrganizationId organizationId);

    Task<bool> CanViewProductPhotosAsync(OrganizationId organizationId);
    Task<bool> CanManageProductPhotosAsync(OrganizationId organizationId);

    Task<bool> CanViewCategoriesAsync(OrganizationId organizationId);
    Task<bool> CanManageCategoriesAsync(OrganizationId organizationId);

    Task<bool> CanViewPagesAsync(OrganizationId organizationId);
    Task<bool> CanManageAsync(PageId pageId);
    Task<bool> CanManagePagesAsync(OrganizationId organizationId);

    Task<bool> CanManageDesignerDataAsync(PageId pageId);

    Task<bool> CanViewAsync(ComponentId componentId);
    Task<bool> CanViewAsync(ComponentVersionId componentVersionId);
    Task<bool> CanViewComponentsAsync(OrganizationId organizationId);
    Task<bool> CanManageComponentsAsync(OrganizationId organizationId);

    Task<bool> CanViewAsync(ProductKitId productKitId);
    Task<bool> CanViewProductKitsAsync(ProjectId projectId);
    Task<bool> CanViewProductKitsAsync(OrganizationId organizationId);
    Task<bool> CanManageProductKitsAsync(OrganizationId organizationId);

    Task<bool> CanViewProductKitReferencesAsync(ProjectId projectId);
    Task<bool> CanManageProductKitReferencesAsync(OrganizationId organizationId);
    Task<bool> CanManageProductKitReferencesAsync(ProjectId projectId);

    Task<bool> CanViewProjectPublicationsAsync(OrganizationId organizationId);
    Task<bool> CanManageProjectPublicationsAsync(OrganizationId organizationId);

    Task<bool> CanViewReportsAsync(OrganizationId organizationId);

    Task<bool> CanViewTermsDocumentsAsync(OrganizationId organizationId);
    Task<bool> CanManageTermsDocumentsAsync(OrganizationId organizationId);

    Task<bool> CanViewLogoSetsAsync(OrganizationId organizationId);
    Task<bool> CanManageLogoSetsAsync(OrganizationId organizationId);

    Task<bool> CanViewSheetTypesAsync(OrganizationId organizationId);
    Task<bool> CanManageSheetTypesAsync(OrganizationId organizationId);

    Task<bool> CanViewProductFamiliesAsync(OrganizationId organizationId);
    Task<bool> CanManageProductFamiliesAsync(OrganizationId organizationId);

    Task<bool> CanViewProductRequirementsAsync(OrganizationId organizationId);
    Task<bool> CanManageProductRequirementsAsync(OrganizationId organizationId);

    Task<bool> CanViewGeneralNotesAsync(OrganizationId organizationId);
    Task<bool> CanManageGeneralNotesAsync(OrganizationId organizationId);

    Task<bool> CanViewComponentTypesAsync(OrganizationId organizationId);
    Task<bool> CanManageComponentTypesAsync(OrganizationId organizationId);

    Task<bool> CanViewOrganizationOptionsAsync(OrganizationId organizationId);
    Task<bool> CanManageAsync(ProductRequirementId productRequirementId);
    Task<bool> CanManageOrganizationOptionsAsync(OrganizationId organizationId);
}
