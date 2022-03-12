namespace AppConfig;

// Returns the default settings for any settings class
public class DefaultConfigurationLoader : IConfigurationLoader
{
    public T GetSettings<T>()
        where T : class, new()
    {
        return new();
    }
}
