using Bugsnag;
using Settings;

namespace AppConfig;

internal static class BugsnagConfigurationFactory
{
    public static Configuration Create(
        BugsnagSettings bugsnagSettings,
        AppVersionSettings appVersionSettings
    )
    {
        return new Configuration
        {
            ApiKey = bugsnagSettings.ApiKey,
            ReleaseStage = bugsnagSettings.ReleaseStage,
            NotifyReleaseStages = new[] { "dev", "test", "prod" },
            AppVersion = appVersionSettings.Version
        };
    }
}
