using System.Data;
using System.Data.Common;
using InfraInterfaces;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DataContext;

internal class RowLevelSecurityInterceptor : DbCommandInterceptor
{
    private sealed class OmitInterceptionScope : IDisposable
    {
        private static readonly AsyncLocal<bool> _omitInterception = new() { Value = false };

        public static bool OmitInterception { get { return _omitInterception.Value; } }

        public OmitInterceptionScope()
        {
            _omitInterception.Value = true;
        }

        public void Dispose()
        {
            _omitInterception.Value = false;
            GC.SuppressFinalize(this);
        }
    }

    // to disable RLS when running queries manually:
    // EXEC sp_set_session_context 'OrganizationLowId', '00000000-0000-0000-0000-000000000000'; EXEC sp_set_session_context 'OrganizationHighId', 'FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF';
    private const string SessionStateModifier = "EXEC sp_set_session_context 'OrganizationLowId', @organizationLowId; EXEC sp_set_session_context 'OrganizationHighId', @organizationHighId;";
    private static readonly Guid MinGuid = Guid.Empty;
    private static readonly Guid MaxGuid = new("FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF");

    private readonly IAppAuthContext _appAuthContext;
    private readonly IOrganizationContext _organizationContext;
    private readonly AppDataContext _context;

    public RowLevelSecurityInterceptor(
        AppDataContext context,
        IAppAuthContext appAuthContext,
        IOrganizationContext organizationContext
    )
    {
        _context = context;
        _appAuthContext = appAuthContext;
        _organizationContext = organizationContext;
    }

    private void SetSessionVariables(DbCommand command)
    {
        if (!OmitInterceptionScope.OmitInterception)
        {
            using (new OmitInterceptionScope())
            {
                if (_context.Database.CanConnect())
                {
                    if (_appAuthContext.IsSystemProcess)
                    {
                        // System can view everything
                        SetOrganizationRange(command, MinGuid, MaxGuid);
                    }
                    else if (_organizationContext.OrganizationId != null)
                    {
                        // Filter to the organization only
                        SetOrganizationRange(command, _organizationContext.OrganizationId.Guid, _organizationContext.OrganizationId.Guid);
                    }
                }
            }
        }
    }

    private static void SetOrganizationRange(DbCommand command, Guid organizationLowId, Guid organizationHighId)
    {
        // System can view everything
        command.CommandText = SessionStateModifier + command.CommandText;

        var parameterLow = command.CreateParameter();
        parameterLow.ParameterName = "@organizationLowId";
        parameterLow.DbType = DbType.Guid;
        parameterLow.Value = organizationLowId;
        command.Parameters.Add(parameterLow);

        var parameterHigh = command.CreateParameter();
        parameterHigh.ParameterName = "@organizationHighId";
        parameterHigh.DbType = DbType.Guid;
        parameterHigh.Value = organizationHighId;
        command.Parameters.Add(parameterHigh);
    }

    public override InterceptionResult<int> NonQueryExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<int> result)
    {
        SetSessionVariables(command);
        return base.NonQueryExecuting(command, eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> NonQueryExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        SetSessionVariables(command);
        return base.NonQueryExecutingAsync(command, eventData, result, cancellationToken);
    }

    public override InterceptionResult<DbDataReader> ReaderExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result)
    {
        SetSessionVariables(command);
        return base.ReaderExecuting(command, eventData, result);
    }

    public override ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<DbDataReader> result, CancellationToken cancellationToken = default)
    {
        SetSessionVariables(command);
        return base.ReaderExecutingAsync(command, eventData, result, cancellationToken);
    }

    public override InterceptionResult<object> ScalarExecuting(DbCommand command, CommandEventData eventData, InterceptionResult<object> result)
    {
        SetSessionVariables(command);
        return base.ScalarExecuting(command, eventData, result);
    }

    public override ValueTask<InterceptionResult<object>> ScalarExecutingAsync(DbCommand command, CommandEventData eventData, InterceptionResult<object> result, CancellationToken cancellationToken = default)
    {
        SetSessionVariables(command);
        return base.ScalarExecutingAsync(command, eventData, result, cancellationToken);
    }
}
