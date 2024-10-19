namespace FluentFilter.Config;

public class FluentFilterConfiguration
{
    private readonly Dictionary<Type, Dictionary<string, FluentFilterConfig>> _configuration = new();

    public ICollection<FluentFilterConfig> GetConfig<T>()
    {
        return GetConfig(typeof(T));
    }
    
    public ICollection<FluentFilterConfig> GetConfig(Type type)
    {
        return _configuration.TryGetValue(type, out var dictionary)
            ? dictionary.Values
            : type.GetProperties().Select(x => new FluentFilterConfig(x.Name, x.PropertyType, true, true)).ToList();
    }
    
    internal void AddType(Type type)
    {
        _configuration.Add(type, type.GetProperties().ToDictionary(x => x.Name, x => new FluentFilterConfig(x.Name, x.PropertyType)));
    }

    internal void SetSortable<T>(string name)
    {
        _configuration[typeof(T)][name].SetSortable();
    }

    internal void SetFilterable<T>(string name)
    {
        _configuration[typeof(T)][name].SetFilterable();
    }
}