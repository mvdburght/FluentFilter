namespace FluentFilter;

public class Pagination(int offset, int limit)
{
    public int Offset { get; } = offset;
    public int Limit { get; } = limit;
}