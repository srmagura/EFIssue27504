using AppDTOs.Enumerations;
using ITI.Baseline.Util;

namespace DbApplicationImpl
{
    public class EfOrganizationQueries : Queries<AppDataContext>, IOrganizationQueries
    {
        private readonly IMapper _mapper;

        public EfOrganizationQueries(IUnitOfWorkProvider uowp, IMapper mapper) : base(uowp)
        {
            _mapper = mapper;
        }

        public Task<OrganizationDto?> GetAsync(OrganizationId id)
        {
            var q = Context.Organizations
                .Where(p => p.Id == id.Guid);

            return _mapper.ProjectToDtoAsync<DbOrganization, OrganizationDto>(q);
        }

        public Task<OrganizationDto?> GetByShortNameAsync(string shortName)
        {
            var q = Context.Organizations
                .Where(p => p.ShortName.Value == shortName);

            return _mapper.ProjectToDtoAsync<DbOrganization, OrganizationDto>(q);
        }

        public Task<bool> IsActiveAsync(OrganizationId organizationId)
        {
            return Context.Organizations
                .Where(p => p.Id == organizationId.Guid)
                .Select(p => p.IsActive)
                .FirstOrDefaultAsync();
        }

        public async Task<FilteredList<OrganizationSummaryDto>> ListAsync(
            int skip,
            int take,
            ActiveFilter activeFilter,
            string? search
        )
        {
            var q = Context.Organizations.AsQueryable();

            switch (activeFilter)
            {
                case ActiveFilter.ActiveOnly:
                    q = q.Where(p => p.IsActive);
                    break;
                case ActiveFilter.InactiveOnly:
                    q = q.Where(p => !p.IsActive);
                    break;
                case ActiveFilter.All:
                    break;
            }

            if (search.HasValue())
            {
                q = q.Where(p => p.Name.Contains(search) || p.ShortName.Value.Contains(search));
            }

            var count = await q.CountAsync();

            q = q.OrderBy(p => p.Name)
                .Skip(skip)
                .Take(take);

            var items = await _mapper.ProjectToDtoArrayAsync<DbOrganization, OrganizationSummaryDto>(q);

            return new FilteredList<OrganizationSummaryDto>(items, count);
        }

        public Task<bool> NameIsAvailableAsync(string name)
        {
            return Context.Organizations.AllAsync(p => p.Name != name);
        }

        public Task<bool> ShortNameIsAvailableAsync(string shortName)
        {
            return Context.Organizations.AllAsync(p => p.ShortName.Value != shortName);
        }
    }
}
