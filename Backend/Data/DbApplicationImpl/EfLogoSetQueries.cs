namespace DbApplicationImpl
{
    public class EfLogoSetQueries : Queries<AppDataContext>, ILogoSetQueries
    {
        private readonly IMapper _mapper;

        public EfLogoSetQueries(IUnitOfWorkProvider uowp, IMapper mapper) : base(uowp)
        {
            _mapper = mapper;
        }

        public Task<LogoSetDto?> GetAsync(LogoSetId id)
        {
            var q = Context.LogoSets
                .Where(p => p.Id == id.Guid);

            return _mapper.ProjectToDtoAsync<DbLogoSet, LogoSetDto>(q);
        }

        public async Task<LogoSetDto[]> ListAsync(OrganizationId organizationId)
        {
            var q = Context.LogoSets
                .Where(p => p.OrganizationId == organizationId.Guid)
                .OrderBy(p => p.Name);

            return await _mapper.ProjectToDtoArrayAsync<DbLogoSet, LogoSetDto>(q);
        }

        public Task<bool> NameIsAvailableAsync(OrganizationId organizationId, string name)
        {
            var q = Context.LogoSets.Where(p => p.OrganizationId == organizationId.Guid);

            return q.AllAsync(p => p.Name != name);
        }
    }
}
