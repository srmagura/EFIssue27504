namespace AppDTOs;

public class GeneralNoteDto
{
    public GeneralNoteDto(GeneralNoteId id, string text)
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
        Text = text ?? throw new ArgumentNullException(nameof(text));
    }

    public GeneralNoteId Id { get; set; }

    public string Text { get; set; }
}
