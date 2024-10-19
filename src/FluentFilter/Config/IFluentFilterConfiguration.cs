namespace FluentFilter.Config;

public interface IFluentFilterConfiguration<T>
{
    void Configure(FluentFilterBuilder<T> builder);
}