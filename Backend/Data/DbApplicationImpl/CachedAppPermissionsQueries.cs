using ITI.DDD.Domain;
using Microsoft.Extensions.Caching.Memory;

namespace DbApplicationImpl
{
    public class CachedAppPermissionsQueries : IAppPermissionsQueries
    {
        // Cache entries are never invalidated. Only cache things that never change.
        private static readonly IMemoryCache Cache = new MemoryCache(new MemoryCacheOptions
        {
            SizeLimit = 10000, // max number of entries
        });

        private static MemoryCacheEntryOptions GetCacheEntryOptions()
        {
            return new MemoryCacheEntryOptions
            {
                Size = 1,
                AbsoluteExpiration = DateTimeOffset.UtcNow.AddDays(1),
            };
        }

        private readonly EfAppPermissionsQueries _queries;

        public CachedAppPermissionsQueries(EfAppPermissionsQueries queries)
        {
            _queries = queries;
        }

        private static async Task<OrganizationId> OrganizationOfAsync<TId>(TId id, Func<Task<OrganizationId>> queryAsync)
            where TId : Identity
        {
            var key = "Organization|" + id.Guid;

            if (!Cache.TryGetValue(key, out OrganizationId value))
            {
                value = await queryAsync();
                Cache.Set(key, value, GetCacheEntryOptions());
            }

            return value;
        }

        public Task<OrganizationId> OrganizationOfAsync(UserId id)
        {
            return OrganizationOfAsync(id, () => _queries.OrganizationOfAsync(id));
        }

        public Task<OrganizationId> OrganizationOfAsync(ProjectId id)
        {
            return OrganizationOfAsync(id, () => _queries.OrganizationOfAsync(id));
        }

        public Task<OrganizationId> OrganizationOfAsync(ComponentId id)
        {
            return OrganizationOfAsync(id, () => _queries.OrganizationOfAsync(id));
        }

        public Task<OrganizationId> OrganizationOfAsync(ComponentVersionId id)
        {
            return OrganizationOfAsync(id, () => _queries.OrganizationOfAsync(id));
        }

        public Task<OrganizationId> OrganizationOfAsync(ProductKitId id)
        {
            return OrganizationOfAsync(id, () => _queries.OrganizationOfAsync(id));
        }

        public Task<OrganizationId> OrganizationOfAsync(PageId id)
        {
            return OrganizationOfAsync(id, () => _queries.OrganizationOfAsync(id));
        }

        public Task<OrganizationId> OrganizationOfAsync(ProductRequirementId id)
        {
            return OrganizationOfAsync(id, () => _queries.OrganizationOfAsync(id));
        }

        public async Task<UserRole> UserRoleForAsync(UserId id)
        {
            var key = "User|" + id.Guid;

            if (!Cache.TryGetValue(key, out UserRole value))
            {
                value = await _queries.UserRoleForAsync(id);
                Cache.Set(key, value, GetCacheEntryOptions());
            }

            return value;
        }
    }
}
