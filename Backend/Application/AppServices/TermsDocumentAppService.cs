using AppDTOs.Enumerations;
using ITI.Baseline.Util;
using ValueObjects;

namespace AppServices;

public class TermsDocumentAppService : ApplicationService, ITermsDocumentAppService
{
    private readonly IAppPermissions _perms;
    private readonly ITermsDocumentRepository _repo;
    private readonly ITermsDocumentQueries _queries;
    private readonly IFileStore _fileStore;
    private readonly IFilePathBuilder _path;

    public TermsDocumentAppService(
        IUnitOfWorkProvider uowp,
        ILogger logger,
        IAppAuthContext auth,
        IAppPermissions perms,
        ITermsDocumentRepository repo,
        ITermsDocumentQueries queries,
        IFileStore fileStore,
        IFilePathBuilder path
    ) : base(uowp, logger, auth)
    {
        _perms = perms;
        _repo = repo;
        _queries = queries;
        _fileStore = fileStore;
        _path = path;
    }

    public Task<string?> GetPdfAsync(TermsDocumentId id, Stream outputStream)
    {
        return QueryAsync(
            Authorize.AuthorizedBelow,
            async () =>
            {
                var termsDocumentDto = await _queries.GetAsync(id);
                if (termsDocumentDto == null)
                    return null;

                Authorize.Require(await _perms.CanViewTermsDocumentsAsync(termsDocumentDto.OrganizationId));

                await _fileStore.GetAsync(_path.ForTermsDocument(termsDocumentDto.File.FileId), outputStream);

                return termsDocumentDto.File.FileType;
            }
        );
    }

    public Task<FilteredList<TermsDocumentSummaryDto>> ListAsync(
        OrganizationId organizationId,
        int skip,
        int take,
        ActiveFilter activeFilter
    )
    {
        return QueryAsync(
            async () => Authorize.Require(await _perms.CanViewAsync(organizationId)),
            () => _queries.ListAsync(organizationId, skip, take, activeFilter)
        );
    }

    public async Task<TermsDocumentId> AddAsync(OrganizationId organizationId, Stream stream)
    {
        var file = new FileRef(new FileId(), "application/pdf");
        var filePath = _path.ForTermsDocument(file.FileId);

        try
        {
            return await CommandAsync(
                async () => Authorize.Require(await _perms.CanManageTermsDocumentsAsync(organizationId)),
                async () =>
                {
                    await _fileStore.PutAsync(filePath, stream);

                    var lastNumber = await _queries.LastNumberAsync(organizationId);
                    var termsDocument = new TermsDocument(organizationId, lastNumber + 1, file);
                    _repo.Add(termsDocument);

                    return termsDocument.Id;
                }
            );
        }
        catch
        {
            if (await _fileStore.ExistsAsync(filePath))
                await _fileStore.RemoveAsync(filePath);

            throw;
        }
    }

    private async Task<TermsDocument> GetDomainEntityAsync(TermsDocumentId id)
    {
        var termsDocument = await _repo.GetAsync(id);
        Require.NotNull(termsDocument, "Could not find terms document.");
        Authorize.Require(await _perms.CanManageTermsDocumentsAsync(termsDocument.OrganizationId));

        return termsDocument;
    }

    public Task SetActiveAsync(TermsDocumentId id, bool active)
    {
        return CommandAsync(
            Authorize.AuthorizedBelow,
            async () => (await GetDomainEntityAsync(id)).SetActive(active)
        );
    }
}
