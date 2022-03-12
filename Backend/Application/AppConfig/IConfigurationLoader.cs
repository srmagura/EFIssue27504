namespace AppConfig;

public interface IConfigurationLoader
{
    T GetSettings<T>()
        where T : class, new();
}
