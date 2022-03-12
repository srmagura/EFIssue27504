using AppInterfaces.System;
using Enumerations;
using Events;
using ITI.DDD.Application.DomainEvents;

namespace EventHandlers;

public class ProjectEventHandler : IDomainEventHandler<ProjectPublishedEvent>
{
    public static void Register(DomainEventHandlerRegistryBuilder builder)
    {
        builder.Register<ProjectPublishedEvent, ProjectEventHandler>();
    }

    private readonly IReportSystemAppService _reportSystemAppService;

    public ProjectEventHandler(IReportSystemAppService reportSystemAppService)
    {
        _reportSystemAppService = reportSystemAppService;
    }

    public async Task HandleAsync(ProjectPublishedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        await _reportSystemAppService.RequestAsync(
            domainEvent.ProjectId,
            domainEvent.ProjectPublicationId,
            ReportType.DrawingSet
        );
        await _reportSystemAppService.RequestAsync(
            domainEvent.ProjectId,
            domainEvent.ProjectPublicationId,
            ReportType.Proposal
        );
    }
}
