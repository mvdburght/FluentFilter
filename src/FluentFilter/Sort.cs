namespace FluentFilter;

public class Sort(string field, Direction direction)
{
    public string Field { get; } = field;
    public Direction Direction { get; } = direction;
}