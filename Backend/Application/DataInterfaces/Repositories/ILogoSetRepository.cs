namespace DataInterfaces.Repositories
{
    public interface ILogoSetRepository
    {
        public void Add(LogoSet logoSet);

        Task<LogoSet?> GetAsync(LogoSetId id);
    }
}
