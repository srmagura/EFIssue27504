using AppDTOs.Report;
namespace DataInterfaces.Queries;

public interface IDesignerQueries
{
    Task<DesignerDataDto[]> ListAsync(ProjectId projectId);
    Task<DesignerDataReportDto[]> ListForReportAsync(ProjectId projectId);
}
