using ITI.Baseline.ValueObjects;

namespace DbApplicationImpl
{
    public class EfUserRepository : Repository<AppDataContext>, IUserRepository
    {
        public EfUserRepository(IUnitOfWorkProvider uowp, IDbEntityMapper dbMapper)
            : base(uowp, dbMapper)
        {
        }

        public void Add(User user)
        {
            var dbe = DbMapper.ToDb<DbUser>(user);
            Context.Users.Add(dbe);
        }

        public async Task<User?> GetAsync(UserId id)
        {
            var dbe = await Context.Users.FirstOrDefaultAsync(p => p.Id == id.Guid);
            if (dbe == null) return null;

            return DbMapper.ToEntity<User>(dbe);
        }

        public async Task<User?> GetAsync(EmailAddress email)
        {
            var dbe = await Context.Users.FirstOrDefaultAsync(p => p.Email.Value == email.Value);
            if (dbe == null) return null;

            return DbMapper.ToEntity<User>(dbe);
        }
    }
}
