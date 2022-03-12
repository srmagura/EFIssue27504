using AppDTOs.Enumerations;
using ITI.Baseline.Util;

namespace AppServices
{
    public class SymbolAppService : ApplicationService, ISymbolAppService
    {
        private readonly IAppAuthContext _auth;
        private readonly IAppPermissions _perms;
        private readonly ISymbolQueries _queries;
        private readonly ISymbolRepository _repo;

        public SymbolAppService(
            IUnitOfWorkProvider uowp,
            ILogger logger,
            IAppAuthContext auth,
            IAppPermissions perms,
            ISymbolQueries queries,
            ISymbolRepository repo
        ) : base(uowp, logger, auth)
        {
            _auth = auth;
            _perms = perms;
            _queries = queries;
            _repo = repo;
        }

        public Task<SymbolDto?> GetAsync(SymbolId id)
        {
            return QueryAsync(
                Authorize.AuthorizedBelow,
                async () =>
                {
                    var symbol = await _queries.GetAsync(id);
                    if (symbol == null) return null;

                    Authorize.Require(await _perms.CanViewSymbolsAsync(symbol.OrganizationId));

                    return symbol;
                }
            );
        }

        public Task<FilteredList<SymbolSummaryDto>> ListAsync(
            OrganizationId organizationId,
            int skip,
            int take,
            ActiveFilter activeFilter,
            string? search
        )
        {
            return QueryAsync(
                async () => Authorize.Require(await _perms.CanViewSymbolsAsync(organizationId)),
                () => _queries.ListAsync(organizationId, skip, take, activeFilter, search)
            );
        }

        public Task<bool> NameIsAvailableAsync(OrganizationId organizationId, string name)
        {
            return QueryAsync(
                async () => Authorize.Require(await _perms.CanViewAsync(organizationId)),
                () => _queries.NameIsAvailableAsync(organizationId, name)
            );
        }

        //

        public Task<SymbolId> AddAsync(OrganizationId organizationId, string name, string svgText)
        {
            return CommandAsync(
                async () => Authorize.Require(await _perms.CanManageSymbolsAsync(organizationId)),
                () =>
                {
                    var symbol = new Symbol(organizationId, name, svgText);
                    _repo.Add(symbol);
                    return Task.FromResult(symbol.Id);
                }
            );
        }

        private async Task<Symbol> GetDomainEntityAsync(SymbolId id)
        {
            var symbol = await _repo.GetAsync(id);
            Require.NotNull(symbol, "Could not find symbol.");
            Authorize.Require(await _perms.CanViewSymbolsAsync(symbol.OrganizationId));

            return symbol;
        }

        public Task SetActiveAsync(SymbolId id, bool active)
        {
            return CommandAsync(
                Authorize.AuthorizedBelow,
                async () => (await GetDomainEntityAsync(id)).SetActive(active)
            );
        }

        public Task SetNameAsync(SymbolId id, string name)
        {
            return CommandAsync(
                Authorize.AuthorizedBelow,
                async () => (await GetDomainEntityAsync(id)).SetName(name)
            );
        }

        public Task SetSvgTextAsync(SymbolId id, string svgText)
        {
            return CommandAsync(
                Authorize.AuthorizedBelow,
                async () => (await GetDomainEntityAsync(id)).SetSvgText(svgText)
            );
        }
    }
}
