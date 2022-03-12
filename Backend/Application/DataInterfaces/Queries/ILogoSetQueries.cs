namespace DataInterfaces.Queries
{
    public interface ILogoSetQueries
    {
        Task<LogoSetDto?> GetAsync(LogoSetId id);

        Task<LogoSetDto[]> ListAsync(OrganizationId organizationId);

        Task<bool> NameIsAvailableAsync(OrganizationId organizationId, string name);
    }
}
