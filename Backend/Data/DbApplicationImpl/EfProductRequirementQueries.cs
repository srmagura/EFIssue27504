namespace DbApplicationImpl;

public class EfProductRequirementQueries : Queries<AppDataContext>, IProductRequirementQueries
{
    private readonly IMapper _mapper;

    public EfProductRequirementQueries(IUnitOfWorkProvider uowp, IMapper mapper) : base(uowp)
    {
        _mapper = mapper;
    }

    public Task<ProductRequirementDto?> GetAsync(ProductRequirementId id)
    {
        var q = Context.ProductRequirements
            .Where(p => p.Id == id.Guid);

        return _mapper.ProjectToDtoAsync<DbProductRequirement, ProductRequirementDto>(q);
    }

    public Task<ProductRequirementDto[]> ListAsync(OrganizationId organizationId)
    {
        var q = Context.ProductRequirements
            .Where(p => p.OrganizationId == organizationId.Guid);

        q = q.OrderBy(p => p.Index);

        return _mapper.ProjectToDtoArrayAsync<DbProductRequirement, ProductRequirementDto>(q);
    }

    public Task<int> GetNextIndexAsync(OrganizationId organizationId)
    {
        var q = Context.ProductRequirements
            .Where(p => p.OrganizationId == organizationId.Guid);

        return q.CountAsync();
    }
}
