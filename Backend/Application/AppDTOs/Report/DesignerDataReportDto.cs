using AppDTOs.Designer;

namespace AppDTOs.Report
{
    public class DesignerDataReportDto
    {
        public DesignerDataReportDto(
            PageId pageId,
            FileId pdfFileId,
            PageOptionsDto? pageOptions,
            PlacedProductKitDto[]? placedProductKits,
            NoteDto[]? notes,
            NoteBlockDto? noteBlock
        )
        {
            PageId = pageId;
            PdfFileId = pdfFileId;
            PageOptions = pageOptions;
            PlacedProductKits = placedProductKits ?? new PlacedProductKitDto[0];
            Notes = notes ?? new NoteDto[0];
            NoteBlock = noteBlock;
        }

        public PageId PageId { get; set; }
        public FileId PdfFileId { get; set; }

        public PageOptionsDto? PageOptions { get; set; }
        public PlacedProductKitDto[] PlacedProductKits { get; set; }
        public NoteDto[] Notes { get; set; }
        public NoteBlockDto? NoteBlock { get; set; }
    }
}
