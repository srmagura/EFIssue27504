using AppDTOs.Enumerations;
using DbEntities;

namespace DataInterfaces.Queries
{
    public interface ICategoryQueries
    {
        Task<DbCategory[]> ListDbAsync(OrganizationId organizationId, ActiveFilter activeFilter);
    }
}
