namespace WebDTOs
{
    public class PermissionDto
    {
        public string Name { get; set; }
        public List<string> Args { get; set; }
        public bool IsPermitted { get; set; }
    }
}
