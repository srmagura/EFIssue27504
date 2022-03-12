namespace DataInterfaces.Repositories
{
    public interface ITermsDocumentRepository
    {
        public void Add(TermsDocument termsDocument);

        Task<TermsDocument?> GetAsync(TermsDocumentId id);
    }
}
