namespace FluentFilter;

public class Query(Pagination pagination, Filter[] filters, Sort[] sortBy)
{
    public Pagination Pagination { get; } = pagination;
    public Filter[] Filters { get; } = filters;
    public Sort[] SortBy { get; } = sortBy;
}