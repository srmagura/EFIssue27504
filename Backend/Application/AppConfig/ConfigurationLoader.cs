using Microsoft.Extensions.Configuration;

namespace AppConfig;

public class ConfigurationLoader : IConfigurationLoader
{
    private readonly IConfiguration _configuration;

    public ConfigurationLoader(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public T GetSettings<T>()
        where T : class, new()
    {
        var name = typeof(T).Name;

        var result = _configuration.GetSection(name).Get<T>();
        return result ?? new();
    }
}
