namespace DataInterfaces.Repositories;

public interface ISheetTypeRepository
{
    Task<SheetType?> GetAsync(SheetTypeId id);

    void Add(SheetType sheetType);
}
