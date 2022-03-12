namespace DbEntities
{
    public class DbCategory : DbEntity
    {
        public DbCategory(Guid organizationId, string name, int index)
        {
            OrganizationId = organizationId;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Index = index;
        }

        public Guid OrganizationId { get; set; }
        public DbOrganization? Organization { get; set; }

        [MaxLength(FieldLengths.Category.Name)]
        public string Name { get; set; }

        public Guid? ParentId { get; set; }
        public int Index { get; set; }

        public Guid? SymbolId { get; set; }
        public DbSymbol? Symbol { get; set; }

        [MaxLength(FieldLengths.Misc.Color)]
        public string? Color { get; set; }

        public bool IsActive { get; set; } = true;

        public static void OnModelCreating(ModelBuilder mb)
        {
            // placeholder method
        }
    }
}
