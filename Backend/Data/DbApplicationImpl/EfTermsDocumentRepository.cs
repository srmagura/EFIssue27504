namespace DbApplicationImpl;

public class EfTermsDocumentRepository : Repository<AppDataContext>, ITermsDocumentRepository
{
    public EfTermsDocumentRepository(IUnitOfWorkProvider uowp, IDbEntityMapper dbMapper)
        : base(uowp, dbMapper)
    {
    }

    public void Add(TermsDocument termsDocument)
    {
        var dbe = DbMapper.ToDb<DbTermsDocument>(termsDocument);
        Context.TermsDocuments.Add(dbe);
    }

    public async Task<TermsDocument?> GetAsync(TermsDocumentId id)
    {
        var dbe = await Context.TermsDocuments.FirstOrDefaultAsync(p => p.Id == id.Guid);
        if (dbe == null) return null;

        return DbMapper.ToEntity<TermsDocument>(dbe);
    }
}
