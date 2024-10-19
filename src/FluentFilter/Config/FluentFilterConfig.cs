namespace FluentFilter.Config;

public class FluentFilterConfig(string name, Type type, bool isFilterable = false, bool isSortable = false)
{
    public string Name { get; } = name;
    public Type Type { get; } = type;
    public bool IsFilterable { get; private set; }
    public bool IsSortable { get; private set; }

    public void SetSortable()
    {
        IsSortable = true;
    }

    public void SetFilterable()
    {
        IsFilterable = true;
    }
}