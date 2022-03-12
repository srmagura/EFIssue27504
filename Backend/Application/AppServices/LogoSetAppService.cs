using ITI.Baseline.Util;
using ValueObjects;

namespace AppServices;

public class LogoSetAppService : ApplicationService, ILogoSetAppService
{
    private readonly IAppPermissions _perms;
    private readonly ILogoSetQueries _queries;
    private readonly ILogoSetRepository _repo;
    private readonly IFileStore _fileStore;
    private readonly IFilePathBuilder _path;

    public LogoSetAppService(
        IUnitOfWorkProvider uowp,
        ILogger logger,
        IAppAuthContext auth,
        IAppPermissions perms,
        ILogoSetQueries queries,
        ILogoSetRepository repo,
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

    public Task<string?> GetDarkLogoAsync(LogoSetId id, Stream outputStream)
    {
        return QueryAsync(
            Authorize.AuthorizedBelow,
            async () =>
            {
                var logoSet = await _queries.GetAsync(id);
                var darkLogo = logoSet?.DarkLogo;

                if (logoSet == null || darkLogo == null)
                    return null;

                Authorize.Require(await _perms.CanViewLogoSetsAsync(logoSet.OrganizationId));

                await _fileStore.GetAsync(_path.ForLogo(darkLogo.FileId), outputStream);

                return darkLogo.FileType;
            }
        );
    }

    public Task<string?> GetLightLogoAsync(LogoSetId id, Stream outputStream)
    {
        return QueryAsync(
            Authorize.AuthorizedBelow,
            async () =>
            {
                var logoSet = await _queries.GetAsync(id);
                var lightLogo = logoSet?.LightLogo;

                if (logoSet == null || lightLogo == null)
                    return null;

                Authorize.Require(await _perms.CanViewLogoSetsAsync(logoSet.OrganizationId));

                await _fileStore.GetAsync(_path.ForLogo(lightLogo.FileId), outputStream);

                return lightLogo.FileType;
            }
        );
    }

    public Task<LogoSetDto[]> ListAsync(OrganizationId organizationId)
    {
        return QueryAsync(
            async () => Authorize.Require(await _perms.CanViewLogoSetsAsync(organizationId)),
            () => _queries.ListAsync(organizationId)
        );
    }

    public Task<bool> NameIsAvailableAsync(OrganizationId organizationId, string name)
    {
        return QueryAsync(
            async () => Authorize.Require(await _perms.CanViewLogoSetsAsync(organizationId)),
            () => _queries.NameIsAvailableAsync(organizationId, name)
        );
    }

    public async Task<LogoSetId> AddAsync(
        OrganizationId organizationId,
        string name,
        Stream darkLogoStream,
        string darkLogoFileType,
        Stream lightLogoStream,
        string lightLogoFileType
    )
    {
        var darkLogo = new FileRef(new FileId(), darkLogoFileType);
        var darkLogoPath = _path.ForLogo(darkLogo.FileId);

        var lightLogo = new FileRef(new FileId(), lightLogoFileType);
        var lightLogoPath = _path.ForLogo(lightLogo.FileId);

        try
        {
            return await CommandAsync(
                async () => Authorize.Require(await _perms.CanManageLogoSetsAsync(organizationId)),
                async () =>
                {
                    await _fileStore.PutAsync(darkLogoPath, darkLogoStream);
                    await _fileStore.PutAsync(lightLogoPath, lightLogoStream);

                    var logoSet = new LogoSet(organizationId, name, darkLogo, lightLogo);
                    _repo.Add(logoSet);

                    return logoSet.Id;
                }
            );
        }
        catch
        {
            if (await _fileStore.ExistsAsync(darkLogoPath))
                await _fileStore.RemoveAsync(darkLogoPath);

            if (await _fileStore.ExistsAsync(lightLogoPath))
                await _fileStore.RemoveAsync(lightLogoPath);

            throw;
        }
    }

    private async Task<LogoSet> GetDomainEntityAsync(LogoSetId id)
    {
        var logoSet = await _repo.GetAsync(id);
        Require.NotNull(logoSet, "Could not find logo set.");
        Authorize.Require(await _perms.CanManageLogoSetsAsync(logoSet.OrganizationId));

        return logoSet;
    }

    public Task SetNameAsync(LogoSetId id, string name)
    {
        return CommandAsync(
            Authorize.AuthorizedBelow,
            async () => (await GetDomainEntityAsync(id)).SetName(name)
        );
    }

    public Task SetActiveAsync(LogoSetId id, bool active)
    {
        return CommandAsync(
            Authorize.AuthorizedBelow,
            async () => (await GetDomainEntityAsync(id)).SetActive(active)
        );
    }

    public Task SetDarkLogoAsync(LogoSetId id, Stream stream, string fileType)
    {
        return CommandAsync(
            Authorize.AuthorizedBelow,
            async () =>
            {
                var logoSet = await GetDomainEntityAsync(id);

                logoSet.SetDarkLogo(new FileRef(logoSet.DarkLogo.FileId, fileType));
                await _fileStore.PutAsync(_path.ForLogo(logoSet.DarkLogo.FileId), stream);
            }
        );
    }

    public Task SetLightLogoAsync(LogoSetId id, Stream stream, string fileType)
    {
        return CommandAsync(
            Authorize.AuthorizedBelow,
            async () =>
            {
                var logoSet = await GetDomainEntityAsync(id);

                logoSet.SetLightLogo(new FileRef(logoSet.LightLogo.FileId, fileType));
                await _fileStore.PutAsync(_path.ForLogo(logoSet.LightLogo.FileId), stream);
            }
        );
    }
}
