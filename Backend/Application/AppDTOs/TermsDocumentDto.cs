namespace AppDTOs
{
    public class TermsDocumentDto
    {
        public TermsDocumentDto(TermsDocumentId id, OrganizationId organizationId, FileRefDto file)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            OrganizationId = organizationId ?? throw new ArgumentNullException(nameof(organizationId));
            File = file ?? throw new ArgumentNullException(nameof(file));
        }

        public TermsDocumentId Id { get; set; }
        public OrganizationId OrganizationId { get; set; }

        public DateTimeOffset DateCreatedUtc { get; set; }
        public bool IsActive { get; set; }

        public int Number { get; set; }

        public FileRefDto File { get; set; }
    }
}
