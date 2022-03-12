namespace Identities
{
    public record SymbolId : Identity
    {
        public SymbolId() { }
        public SymbolId(Guid guid) : base(guid) { }
        public SymbolId(Guid? guid) : base(guid ?? Guid.Empty) { }
    }
}
