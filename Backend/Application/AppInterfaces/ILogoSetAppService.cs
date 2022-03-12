namespace AppInterfaces;

public interface ILogoSetAppService
{
    Task<string?> GetDarkLogoAsync(LogoSetId id, Stream outputStream); // returns file type
    Task<string?> GetLightLogoAsync(LogoSetId id, Stream outputStream); // returns file type

    Task<LogoSetDto[]> ListAsync(OrganizationId organizationId);

    Task<bool> NameIsAvailableAsync(OrganizationId organizationId, string name);

    Task<LogoSetId> AddAsync(
        OrganizationId organizationId,
        string name,
        Stream darkLogoStream,
        string darkLogoFileType,
        Stream lightLogoStream,
        string lightLogoFileType
    );

    Task SetNameAsync(LogoSetId id, string name);
    Task SetActiveAsync(LogoSetId id, bool active);

    Task SetDarkLogoAsync(LogoSetId id, Stream stream, string fileType);
    Task SetLightLogoAsync(LogoSetId id, Stream stream, string fileType);
}
