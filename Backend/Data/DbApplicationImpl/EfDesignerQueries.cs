using System.Text.Json;
using AppDTOs.Designer;
using AppDTOs.Report;

namespace DbApplicationImpl;

public class EfDesignerQueries : Queries<AppDataContext>, IDesignerQueries
{
    private readonly IMapper _mapper;

    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public EfDesignerQueries(IUnitOfWorkProvider uowp, IMapper mapper) : base(uowp)
    {
        _mapper = mapper;
    }

    public Task<DesignerDataDto[]> ListAsync(ProjectId projectId)
    {
        return Context.DesignerData
            .Where(d => d.Page!.ProjectId == projectId.Guid && d.Page.IsActive)
            .ProjectToDtoArrayAsync<DbDesignerData, DesignerDataDto>(_mapper);
    }

    public async Task<DesignerDataReportDto[]> ListForReportAsync(ProjectId projectId)
    {
        var allDesignerData = await Context.DesignerData
            .AsNoTracking()
            .Include(p => p.Page)
            .Where(d => d.Page!.ProjectId == projectId.Guid && d.Page.IsActive)
            .ToArrayAsync();

        var designerDataGroups = allDesignerData.GroupBy(d => d.PageId);
        var designerDataReportDtos = new List<DesignerDataReportDto>();

        foreach (var designerData in designerDataGroups)
        {
            var pdfFileId = designerData.Select(p => p.Page!.Pdf.FileId).First();

            var pageOptionsJson = designerData.FirstOrDefault(p => p.Type == DesignerDataType.PageOptions)?.Json;
            var pageOptions = pageOptionsJson != null
                ? JsonSerializer.Deserialize<PageOptionsDto>(pageOptionsJson, JsonSerializerOptions)
                : null;

            var placedProductKitsJson = designerData.FirstOrDefault(p => p.Type == DesignerDataType.PlacedProductKits)?.Json;
            var placedProductKits = placedProductKitsJson != null
                ? JsonSerializer.Deserialize<PlacedProductKitDto[]>(placedProductKitsJson, JsonSerializerOptions)
                : null;

            var notesJson = designerData.FirstOrDefault(p => p.Type == DesignerDataType.Notes)?.Json;
            var notes = notesJson != null
                ? JsonSerializer.Deserialize<NoteDto[]>(notesJson, JsonSerializerOptions)
                : null;

            var noteBlockJson = designerData.FirstOrDefault(p => p.Type == DesignerDataType.NoteBlock)?.Json;
            var noteBlock = noteBlockJson != null
                ? JsonSerializer.Deserialize<NoteBlockDto>(noteBlockJson, JsonSerializerOptions)
                : null;

            designerDataReportDtos.Add(
                new DesignerDataReportDto(
                    new PageId(designerData.Key),
                    new FileId(pdfFileId),
                    pageOptions,
                    placedProductKits,
                    notes,
                    noteBlock
                )
            );
        }

        return designerDataReportDtos.ToArray();
    }
}
