namespace DbApplicationImpl
{
    public class EfOrganizationRepository : Repository<AppDataContext>, IOrganizationRepository
    {
        public EfOrganizationRepository(IUnitOfWorkProvider uowp, IDbEntityMapper dbMapper)
            : base(uowp, dbMapper)
        {
        }

        public void Add(Organization organization)
        {
            var dbe = DbMapper.ToDb<DbOrganization>(organization);
            Context.Organizations.Add(dbe);
        }

        public async Task<Organization?> GetAsync(OrganizationId id)
        {
            var dbe = await Context.Organizations.FirstOrDefaultAsync(p => p.Id == id.Guid);
            if (dbe == null) return null;

            return DbMapper.ToEntity<Organization>(dbe);
        }
    }
}
