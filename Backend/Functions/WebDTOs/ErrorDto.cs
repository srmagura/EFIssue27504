namespace WebDTOs
{
    public class ErrorDto
    {
        public ErrorDto(ErrorDtoType type, string message, string? diagnosticInfo = null)
        {
            Type = type;
            Message = message;
            DiagnosticInfo = diagnosticInfo;
        }

        public ErrorDtoType Type { get; protected set; }
        public string Message { get; protected set; }
        public string? DiagnosticInfo { get; set; }
    }
}
