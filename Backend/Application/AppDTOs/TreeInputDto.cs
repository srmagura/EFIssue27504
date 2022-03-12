namespace AppDTOs
{
    public class TreeInputDto
    {
        public CategoryInputDto? Category { get; set; }

        public List<TreeInputDto> Children { get; set; } = new();
    }
}
