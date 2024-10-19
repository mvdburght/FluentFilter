namespace FluentFilter;

public class Filter(string field, Operator @operator, object? value)
{
    public string Field { get; } = field;
    public Operator Operator { get; } = @operator;
    public object? Value { get; } = value;
}