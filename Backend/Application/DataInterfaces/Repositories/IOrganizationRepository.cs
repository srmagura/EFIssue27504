namespace DataInterfaces.Repositories
{
    public interface IOrganizationRepository
    {
        void Add(Organization organization);
        Task<Organization?> GetAsync(OrganizationId id);
    }
}
