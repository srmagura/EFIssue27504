namespace AppDTOs;

public class GeneralNoteInputDto
{
    public GeneralNoteInputDto(GeneralNoteId? id, string text)
    {
        Id = id;
        Text = text ?? throw new ArgumentNullException(nameof(text));
    }

    public GeneralNoteId? Id { get; set; }

    public string Text { get; set; }
}
