using AppDTOs.Enumerations;
using ITI.Baseline.Util;

namespace DbApplicationImpl
{
    public class EfProductPhotoQueries : Queries<AppDataContext>, IProductPhotoQueries
    {
        private readonly IMapper _mapper;

        public EfProductPhotoQueries(IUnitOfWorkProvider uowp, IMapper mapper) : base(uowp)
        {
            _mapper = mapper;
        }

        public Task<ProductPhotoDto?> GetAsync(ProductPhotoId id)
        {
            var q = Context.ProductPhotos
                .Where(p => p.Id == id.Guid);

            return _mapper.ProjectToDtoAsync<DbProductPhoto, ProductPhotoDto>(q);
        }

        public async Task<FilteredList<ProductPhotoSummaryDto>> ListAsync(OrganizationId organizationId, int skip, int take, ActiveFilter activeFilter, string? search)
        {
            var q = Context.ProductPhotos.Where(p => p.OrganizationId == organizationId.Guid);

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
                q = q.Where(p => p.Name.Contains(search));
            }

            var count = await q.CountAsync();

            q = q.OrderBy(p => p.Name)
                .Skip(skip)
                .Take(take);

            var items = await _mapper.ProjectToDtoArrayAsync<DbProductPhoto, ProductPhotoSummaryDto>(q);

            return new FilteredList<ProductPhotoSummaryDto>(items, count);
        }

        public Task<bool> NameIsAvailableAsync(OrganizationId organizationId, string name)
        {
            var q = Context.ProductPhotos.Where(p => p.OrganizationId == organizationId.Guid);

            return q.AllAsync(p => p.Name != name);
        }
    }
}
