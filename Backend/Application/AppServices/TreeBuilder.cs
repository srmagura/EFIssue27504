using AutoMapper;
using DbEntities;

namespace AppServices;

public class TreeBuilder
{
    private readonly IMapper _mapper;

    public TreeBuilder(IMapper mapper)
    {
        _mapper = mapper;
    }

    public TreeDto BuildTree(DbCategory[] categories)
    {
        var root = new TreeDto();

        var subtreeDictionary = categories
            .ToDictionary(
                c => c.Id,
                c => new TreeDto
                {
                    Category = _mapper.Map<CategoryDto>(c),
                }
            );

        var parentIdDictionary = categories
            .ToDictionary(
                c => c.Id,
                c => c.ParentId
            );

        // Categories are already ordered by index so they will stay in order as we add them to the tree
        foreach (var subtree in subtreeDictionary.Values)
        {
            var parent = root;
            var parentId = parentIdDictionary[subtree.Category!.Id.Guid];

            if (parentId != null && subtreeDictionary.ContainsKey(parentId.Value))
            {
                parent = subtreeDictionary[parentId.Value];
            }

            parent.Children.Add(subtree);
        }

        return root;
    }
}
