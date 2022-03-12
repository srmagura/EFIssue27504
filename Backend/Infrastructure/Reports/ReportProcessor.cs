using AppInterfaces.System;
using Enumerations;
using Identities;
using InfraInterfaces;
using ITI.Baseline.Util;
using ITI.DDD.Core;
using ValueObjects;

namespace Reports;

public class ReportProcessor : IReportProcessor
{
    private readonly DrawingSetReportBuilder _drawingSetReportBuilder;
    private readonly ProposalReportBuilder _proposalReportBuilder;
    private readonly IReportSystemAppService _reportSystemAppService;
    private readonly IFilePathBuilder _path;

    public ReportProcessor(
        DrawingSetReportBuilder reportBuilder,
        ProposalReportBuilder proposalReportBuilder,
        IReportSystemAppService reportSystemAppService,
        IFilePathBuilder path
    )
    {
        _drawingSetReportBuilder = reportBuilder;
        _proposalReportBuilder = proposalReportBuilder;
        _reportSystemAppService = reportSystemAppService;
        _path = path;
    }

    public async Task ProcessAsync(ReportId reportId, CancellationToken cancellationToken)
    {
        var report = await _reportSystemAppService.GetAsync(reportId);
        Require.NotNull(report, "Report could not be found.");

        // Event handlers must be idempotent
        if (report.Status != ReportStatus.Pending) return;

        try
        {
            if (cancellationToken.IsCancellationRequested)
                throw new TaskCanceledException();

            await _reportSystemAppService.BeginProcessingAsync(reportId);

            FileId fileId = new FileId();
            var filePath = _path.ForReport(fileId);

            Task OnProgressAsync(Percentage percentComplete)
            {
                return _reportSystemAppService.SetPercentCompleteAsync(reportId, percentComplete);
            }

            var isDraft = report.ProjectPublicationId == null;

            switch (report.Type)
            {
                case ReportType.DrawingSet:
                    await _drawingSetReportBuilder.BuildAsync(
                        report.ProjectId,
                        filePath,
                        OnProgressAsync,
                        isDraft,
                        devGenerateHtml: false,
                        cancellationToken
                    );
                    break;
                case ReportType.Proposal:
                    await _proposalReportBuilder.BuildAsync(
                        report.ProjectId,
                        filePath,
                        OnProgressAsync,
                        isDraft,
                        devGenerateHtml: false,
                        cancellationToken
                    );
                    break;
                default:
                    throw new Exception($"Unexpected report type: {report.Type}.");
            }

            await _reportSystemAppService.SetCompletedAsync(reportId, fileId, "application/pdf");
        }
        catch (TaskCanceledException)
        {
            await _reportSystemAppService.SetCanceledAsync(reportId);
        }
        catch (DomainException e)
        {
            await _reportSystemAppService.SetErrorAsync(reportId, e.Message);
            throw;
        }
        catch (Exception e)
        {
            await _reportSystemAppService.SetErrorAsync(reportId, e.ToString());
            throw;
        }
    }
}
