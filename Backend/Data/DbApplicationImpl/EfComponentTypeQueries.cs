using AppDTOs.Enumerations;

namespace DbApplicationImpl;

public class EfComponentTypeQueries : Queries<AppDataContext>, IComponentTypeQueries
{
    private readonly IMapper _mapper;

    public EfComponentTypeQueries(IUnitOfWorkProvider uowp, IMapper mapper) : base(uowp)
    {
        _mapper = mapper;
    }

    public Task<ComponentTypeDto?> GetAsync(ComponentTypeId id)
    {
        var q = Context.ComponentTypes
            .Where(p => p.Id == id.Guid);

        return _mapper.ProjectToDtoAsync<DbComponentType, ComponentTypeDto>(q);
    }

    public Task<ComponentTypeDto[]> ListAsync(OrganizationId organizationId, ActiveFilter activeFilter)
    {
        var q = Context.ComponentTypes
            .Where(p => p.OrganizationId == organizationId.Guid);

        switch (activeFilter)
        {
            case ActiveFilter.ActiveOnly:
                q = q.Where(p => p.IsActive);
                break;
            case ActiveFilter.InactiveOnly:
                q = q.Where(p => !p.IsActive);
                break;
            case ActiveFilter.All:
                break;
        }

        q = q.OrderBy(p => p.Name);

        return _mapper.ProjectToDtoArrayAsync<DbComponentType, ComponentTypeDto>(q);
    }

    public Task<bool> NameIsAvailableAsync(OrganizationId organizationId, string name)
    {
        return Context.ComponentTypes
            .Where(p => p.OrganizationId == organizationId.Guid)
            .AllAsync(p => p.Name != name);
    }
}
