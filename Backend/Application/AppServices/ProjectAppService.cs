using AppDTOs.Enumerations;
using ITI.Baseline.Util;
using Settings;
using ValueObjects;

namespace AppServices;

public class ProjectAppService : ApplicationService, IProjectAppService
{
    private readonly IAppAuthContext _auth;
    private readonly IAppPermissions _perms;
    private readonly IProjectQueries _queries;
    private readonly IProjectRepository _repo;
    private readonly IFileStore _fileStore;
    private readonly IFilePathBuilder _path;
    private readonly ProjectSettings _projectSettings;

    public ProjectAppService(
        IUnitOfWorkProvider uowp,
        ILogger logger,
        IAppAuthContext auth,
        IAppPermissions perms,
        IProjectQueries queries,
        IProjectRepository repo,
        IFileStore fileStore,
        IFilePathBuilder path,
        ProjectSettings projectSettings
    ) : base(uowp, logger, auth)
    {
        _auth = auth;
        _perms = perms;
        _queries = queries;
        _repo = repo;
        _fileStore = fileStore;
        _path = path;
        _projectSettings = projectSettings;
    }

    public Task<ProjectDto?> GetAsync(ProjectId id)
    {
        return QueryAsync(
            async () => Authorize.Require(await _perms.CanViewAsync(id)),
            () => _queries.GetAsync(id)
        );
    }

    public Task<string?> GetPhotoAsync(ProjectId id, Stream outputStream)
    {
        return QueryAsync(
            async () => Authorize.Require(await _perms.CanViewAsync(id)),
            async () =>
            {
                var photo = (await _queries.GetAsync(id))?.Photo;
                if (photo == null)
                    return null;

                await _fileStore.GetAsync(_path.ForProjectPhoto(photo.FileId), outputStream);

                return photo.FileType;
            }
        );
    }

    public Task<FilteredList<ProjectSummaryDto>> ListAsync(
        OrganizationId organizationId,
        int skip,
        int take,
        ActiveFilter activeFilter,
        string? search
    )
    {
        return QueryAsync(
            async () => Authorize.Require(await _perms.CanViewProjectsAsync(organizationId)),
            () => _queries.ListAsync(organizationId, skip, take, activeFilter, search)
        );
    }

    public Task<bool> NameIsAvailableAsync(OrganizationId organizationId, string name)
    {
        return QueryAsync(
            async () => Authorize.Require(await _perms.CanViewProjectsAsync(organizationId)),
            () => _queries.NameIsAvailableAsync(organizationId, name)
        );
    }

    //

    public Task<ProjectId> AddAsync(
        OrganizationId organizationId,
        string name,
        string shortName,
        string description,
        PartialAddressDto address,
        string customerName,
        string signeeName,
        LogoSetId logoSetId,
        TermsDocumentId termsDocumentId,
        int estimatedSquareFeet
    )
    {
        return CommandAsync(
           async () => Authorize.Require(await _perms.CanManageProjectsAsync(organizationId)),
           () =>
           {
               Require.NotNull(_auth.UserName, "User name is unexpectedly null.");

               var project = new Project(
                   organizationId: organizationId,
                   name: name,
                   shortName: shortName,
                   description: description,
                   address: address.ToValueObject(),
                   customerName: customerName,
                   signeeName: signeeName,
                   preparerName: _auth.UserName,
                   logoSetId: logoSetId,
                   termsDocumentId: termsDocumentId,
                   estimatedSquareFeet: estimatedSquareFeet,
                   projectSettings: _projectSettings
               );

               _repo.Add(project);

               return Task.FromResult(project.Id);
           }
        );
    }

    private async Task<Project> GetDomainEntityAsync(ProjectId id)
    {
        var project = await _repo.GetAsync(id);
        Require.NotNull(project, "Could not find project.");
        Authorize.Require(await _perms.CanManageProjectsAsync(project.OrganizationId));

        return project;
    }

    public Task SetActiveAsync(ProjectId id, bool active)
    {
        return CommandAsync(
            Authorize.AuthorizedBelow,
            async () => (await GetDomainEntityAsync(id)).SetActive(active)
        );
    }

    public Task SetBudgetOptionsAsync(ProjectId id, ProjectBudgetOptionsDto budgetOptions)
    {
        return CommandAsync(
            Authorize.AuthorizedBelow,
            async () => (await GetDomainEntityAsync(id)).SetBudgetOptions(budgetOptions.ToValueObject())
        );
    }

    public Task SetReportOptionsAsync(ProjectId id, ProjectReportOptionsDto reportOptions)
    {
        return CommandAsync(
            Authorize.AuthorizedBelow,
            async () => (await GetDomainEntityAsync(id)).SetReportOptions(reportOptions.ToValueObject())
        );
    }

    public Task SetCustomerNameAsync(ProjectId id, string customerName)
    {
        return CommandAsync(
            Authorize.AuthorizedBelow,
            async () => (await GetDomainEntityAsync(id)).SetCustomerName(customerName)
        );
    }

    public Task SetDescriptionAsync(ProjectId id, string description)
    {
        return CommandAsync(
            Authorize.AuthorizedBelow,
            async () => (await GetDomainEntityAsync(id)).SetDescription(description)
        );
    }

    public Task SetEstimatedSquareFeetAsync(ProjectId id, int estimatedSquareFeet)
    {
        return CommandAsync(
            Authorize.AuthorizedBelow,
            async () => (await GetDomainEntityAsync(id)).SetEstimatedSquareFeet(estimatedSquareFeet)
        );
    }

    public Task SetNameAsync(ProjectId id, string name, string shortName)
    {
        return CommandAsync(
            Authorize.AuthorizedBelow,
            async () =>
            {
                var project = await GetDomainEntityAsync(id);
                project.SetName(name);
                project.SetShortName(shortName);
            }
        );
    }

    public Task SetPhotoAsync(ProjectId id, Stream? stream, string? fileType)
    {
        return CommandAsync(
            Authorize.AuthorizedBelow,
            async () =>
            {
                var project = await GetDomainEntityAsync(id);

                if (stream != null && fileType != null)
                {
                    var fileId = project.Photo?.FileId ?? new FileId();
                    project.SetPhoto(new FileRef(fileId, fileType));
                    await _fileStore.PutAsync(_path.ForProjectPhoto(fileId), stream);
                }
                else
                {
                    var fileId = project.Photo?.FileId;
                    if (fileId != null) await _fileStore.RemoveAsync(_path.ForProjectPhoto(fileId));
                    project.SetPhoto(null);
                }
            }
        );
    }

    public Task SetAddressAsync(ProjectId id, PartialAddressDto address)
    {
        return CommandAsync(
            Authorize.AuthorizedBelow,
            async () => (await GetDomainEntityAsync(id)).SetAddress(address.ToValueObject())
        );
    }

    public Task AcquireDesignerLockAsync(ProjectId id)
    {
        return CommandAsync(
            Authorize.AuthorizedBelow,
            async () =>
            {
                Require.NotNull(_auth.UserId, "Only users can acquire the designer lock.");

                (await GetDomainEntityAsync(id)).AcquireDesignerLock(_auth.UserId);
            }
        );
    }

    public Task ReleaseDesignerLockAsync(ProjectId id)
    {
        return CommandAsync(
            Authorize.AuthorizedBelow,
            async () => (await GetDomainEntityAsync(id)).ReleaseDesignerLock()
        );
    }
}
