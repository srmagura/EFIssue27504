namespace AppInterfaces.System;

public interface IImportSystemAppService
{
    Task<ImportDto?> GetAsync(ImportId id);

    Task BeginProcessingAsync(ImportId id);
    Task SetPercentCompleteAsync(ImportId id, decimal percentComplete);

    Task SetCompletedAsync(ImportId id);
    Task SetErrorAsync(ImportId id, string errorMessage);
    Task SetCanceledAsync(ImportId id);
}
