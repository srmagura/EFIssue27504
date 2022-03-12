using AppDTOs.Enumerations;
using ITI.Baseline.Util;
using ValueObjects;

namespace AppServices;

public class ProductPhotoAppService : ApplicationService, IProductPhotoAppService
{
    private readonly IAppPermissions _perms;
    private readonly IProductPhotoQueries _queries;
    private readonly IProductPhotoRepository _repo;
    private readonly IFileStore _fileStore;
    private readonly IFilePathBuilder _path;

    public ProductPhotoAppService(
        IUnitOfWorkProvider uowp,
        ILogger logger,
        IAppAuthContext auth,
        IAppPermissions perms,
        IProductPhotoQueries queries,
        IProductPhotoRepository repo,
        IFileStore fileStore,
        IFilePathBuilder path
    ) : base(uowp, logger, auth)
    {
        _perms = perms;
        _queries = queries;
        _repo = repo;
        _fileStore = fileStore;
        _path = path;
    }

    public Task<ProductPhotoDto?> GetAsync(ProductPhotoId id)
    {
        return QueryAsync(
            Authorize.AuthorizedBelow,
            async () =>
            {
                var productPhoto = await _queries.GetAsync(id);
                if (productPhoto == null) return null;

                Authorize.Require(await _perms.CanViewProductPhotosAsync(productPhoto.OrganizationId));

                return productPhoto;
            }
        );
    }

    public Task<string?> GetImageAsync(ProductPhotoId id, Stream outputStream)
    {
        return QueryAsync(
            Authorize.AuthorizedBelow,
            async () =>
            {
                var productPhoto = await _queries.GetAsync(id);
                if (productPhoto == null)
                    return null;

                Authorize.Require(await _perms.CanViewProductPhotosAsync(productPhoto.OrganizationId));

                await _fileStore.GetAsync(_path.ForProductPhoto(productPhoto.Photo.FileId), outputStream);

                return productPhoto.Photo.FileType;
            }
        );
    }

    public Task<FilteredList<ProductPhotoSummaryDto>> ListAsync(
        OrganizationId organizationId,
        int skip,
        int take,
        ActiveFilter activeFilter,
        string? search
    )
    {
        return QueryAsync(
            async () => Authorize.Require(await _perms.CanViewProductPhotosAsync(organizationId)),
            () => _queries.ListAsync(organizationId, skip, take, activeFilter, search)
        );
    }

    public Task<bool> NameIsAvailableAsync(OrganizationId organizationId, string name)
    {
        return QueryAsync(
            async () => Authorize.Require(await _perms.CanViewProductPhotosAsync(organizationId)),
            () => _queries.NameIsAvailableAsync(organizationId, name)
        );
    }

    //

    public Task<ProductPhotoId> AddAsync(OrganizationId organizationId, string name, Stream stream, string fileType)
    {
        return CommandAsync(
            async () => Authorize.Require(await _perms.CanManageProductPhotosAsync(organizationId)),
            async () =>
            {
                var fileId = new FileId();
                var productPhoto = new ProductPhoto(organizationId, name, new FileRef(fileId, fileType));

                _repo.Add(productPhoto);
                await _fileStore.PutAsync(_path.ForProductPhoto(fileId), stream);

                return productPhoto.Id;
            }
        );
    }

    private async Task<ProductPhoto> GetDomainEntityAsync(ProductPhotoId id)
    {
        var productPhoto = await _repo.GetAsync(id);
        Require.NotNull(productPhoto, "Could not find product photo.");
        Authorize.Require(await _perms.CanManageProductPhotosAsync(productPhoto.OrganizationId));

        return productPhoto;
    }

    public Task SetActiveAsync(ProductPhotoId id, bool active)
    {
        return CommandAsync(
            Authorize.AuthorizedBelow,
            async () => (await GetDomainEntityAsync(id)).SetActive(active)
        );
    }

    public Task SetNameAsync(ProductPhotoId id, string name)
    {
        return CommandAsync(
            Authorize.AuthorizedBelow,
            async () => (await GetDomainEntityAsync(id)).SetName(name)
        );
    }

    public Task SetPhotoAsync(ProductPhotoId id, Stream stream, string fileType)
    {
        return CommandAsync(
            Authorize.AuthorizedBelow,
            async () =>
            {
                var productPhoto = await GetDomainEntityAsync(id);

                productPhoto.SetPhoto(new FileRef(productPhoto.Photo.FileId, fileType));
                await _fileStore.PutAsync(_path.ForProductPhoto(productPhoto.Photo.FileId), stream);
            }
        );
    }
}
