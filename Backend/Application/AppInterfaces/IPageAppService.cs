namespace AppInterfaces
{
    public interface IPageAppService
    {
        Task<string?> GetPdfAsync(PageId id, Stream outputStream); // returns "application/pdf" or null
        Task<string?> GetThumbnailAsync(PageId id, Stream outputStream); // returns thumbnail file type

        Task<List<PageSummaryDto>> ListAsync(ProjectId projectId, ActiveFilter activeFilter);

        Task SetIndexAsync(PageId id, bool decreaseIndex);
        Task DuplicateAsync(PageId id);
        Task SetActiveAsync(PageId id, bool isActive);

        Task SetSheetNumberAndNameAsync(
            PageId id,
            SheetTypeId sheetTypeId,
            string sheetNumberSuffix,
            string? sheetNameSuffix
        );
    }
}
