namespace DbApplicationImpl;

public class EfSheetTypeQueries : Queries<AppDataContext>, ISheetTypeQueries
{
    private readonly IMapper _mapper;

    public EfSheetTypeQueries(IUnitOfWorkProvider uowp, IMapper mapper) : base(uowp)
    {
        _mapper = mapper;
    }

    public Task<SheetTypeSummaryDto[]> ListAsync(OrganizationId organizationId)
    {
        return Context.SheetTypes
            .Where(t => t.OrganizationId == organizationId.Guid)
            .OrderBy(t => t.SheetNumberPrefix)
            .ProjectToDtoArrayAsync<DbSheetType, SheetTypeSummaryDto>(_mapper);
    }
}
