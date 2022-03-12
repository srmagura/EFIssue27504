namespace AppDTOs.Designer;

// Be very careful when changing since the JSON in the database uses this format
public class NoteBlockDto
{
    public NoteBlockDto(double[] position, double[] dimensions, int columns)
    {
        Position = position ?? throw new ArgumentNullException(nameof(position));
        Dimensions = dimensions ?? throw new ArgumentNullException(nameof(dimensions));
        Columns = columns;
    }

    public double[] Position { get; set; }
    public double[] Dimensions { get; set; }
    public int Columns { get; set; }
}
