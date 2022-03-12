namespace DbApplicationImpl
{
    public class EfSymbolRepository : Repository<AppDataContext>, ISymbolRepository
    {
        public EfSymbolRepository(IUnitOfWorkProvider uowp, IDbEntityMapper dbMapper)
            : base(uowp, dbMapper)
        {
        }

        public void Add(Symbol symbol)
        {
            var dbe = DbMapper.ToDb<DbSymbol>(symbol);
            Context.Symbols.Add(dbe);
        }

        public async Task<Symbol?> GetAsync(SymbolId id)
        {
            var dbe = await Context.Symbols.FirstOrDefaultAsync(p => p.Id == id.Guid);
            if (dbe == null) return null;

            return DbMapper.ToEntity<Symbol>(dbe);
        }
    }
}
