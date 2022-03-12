namespace AppInterfaces;

public interface IBudgetAppService
{
    // TODO:AQ:SAM this should load the necessary data and call the domain service
    // Task<ProjectBudgetDto> GetBudgetAsync(ProjectId projectId);

    Task<TypicalBudgetRangeDto?> GetTypicalBudgetRange(ProjectId projectId);
}
