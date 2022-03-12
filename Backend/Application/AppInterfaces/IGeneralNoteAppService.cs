namespace AppInterfaces;

public interface IGeneralNoteAppService
{
    Task<GeneralNoteDto[]> ListAsync(OrganizationId organizationId);

    Task SetAsync(OrganizationId organizationId, GeneralNoteInputDto[] generalNotes);
}
