namespace DataInterfaces.Repositories
{
    public interface ISymbolRepository
    {
        void Add(Symbol symbol);
        Task<Symbol?> GetAsync(SymbolId id);
    }
}
