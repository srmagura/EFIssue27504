using AppDTOs.Enumerations;
using ITI.Baseline.Util;
using ITI.Baseline.ValueObjects;

namespace DbApplicationImpl
{
    public class EfUserQueries : Queries<AppDataContext>, IUserQueries
    {
        private readonly IMapper _mapper;

        public EfUserQueries(IUnitOfWorkProvider uowp, IMapper mapper) : base(uowp)
        {
            _mapper = mapper;
        }

        public Task<UserDto?> GetAsync(EmailAddress email)
        {
            var q = Context.Users
                .Where(p => p.Email.Value == email.Value);

            return _mapper.ProjectToDtoAsync<DbUser, UserDto>(q);
        }

        public Task<UserDto?> GetAsync(UserId id)
        {
            var q = Context.Users
                .Where(p => p.Id == id.Guid);

            return _mapper.ProjectToDtoAsync<DbUser, UserDto>(q);
        }

        public async Task<FilteredList<UserSummaryDto>> ListAsync(
            OrganizationId organizationId,
            int skip,
            int take,
            ActiveFilter activeFilter,
            string? search
        )
        {
            var q = Context.Users.AsQueryable();

            switch (activeFilter)
            {
                case ActiveFilter.ActiveOnly:
                    q = q.Where(p => p.IsActive && p.OrganizationId == organizationId.Guid);
                    break;
                case ActiveFilter.InactiveOnly:
                    q = q.Where(p => !p.IsActive && p.OrganizationId == organizationId.Guid);
                    break;
                case ActiveFilter.All:
                    q = q.Where(p => p.OrganizationId == organizationId.Guid);
                    break;
            }

            if (search.HasValue())
            {
                q = q.Where(p => p.Email.Value.Contains(search)
                    || p.Name.First.Contains(search)
                    || p.Name.Last.Contains(search)
                );
            }

            var count = await q.CountAsync();

            q = q.OrderBy(p => p.Name.Last).ThenBy(p => p.Name.First);
            q = q.Skip(skip).Take(take);

            var items = await _mapper.ProjectToDtoArrayAsync<DbUser, UserSummaryDto>(q);

            return new FilteredList<UserSummaryDto>(items, count);
        }

        public Task<bool> EmailIsAvailableAsync(EmailAddress email)
        {
            return Context.Users.AllAsync(p => p.Email.Value != email.Value);
        }
    }
}
