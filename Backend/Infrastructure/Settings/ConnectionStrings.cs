using System.Runtime.InteropServices;
using ITI.DDD.Logging;

namespace Settings;

public class ConnectionStrings : IDbLoggerSettings
{
    private const string AppDataContextLocalBase = "Server=localhost;Database=Slateplan;TrustServerCertificate=True;MultipleActiveResultSets=True;";

    public string AppDataContext { get; set; } =
        RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
            ? AppDataContextLocalBase + "Trusted_Connection=True;"
            : AppDataContextLocalBase + "User=sa;Password=LetMeIn98;";

    public string AzureBlobStorage { get; set; } = ""; // not used in local development
    public string AzureWebJobsServiceBus { get; set; } = ""; // not used in local development

    public string LogConnectionString => AppDataContext;
    public string LogTableName => "LogEntries";
}
