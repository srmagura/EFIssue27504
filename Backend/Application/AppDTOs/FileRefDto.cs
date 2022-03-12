namespace AppDTOs
{
    public class FileRefDto
    {
        public FileRefDto(FileId fileId, string fileType)
        {
            FileId = fileId ?? throw new ArgumentNullException(nameof(fileId));
            FileType = fileType ?? throw new ArgumentNullException(nameof(fileType));
        }

        public FileId FileId { get; set; }
        public string FileType { get; set; }
    }
}
