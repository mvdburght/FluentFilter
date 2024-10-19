using FluentFilter.Config;
using FluentFilter.Extensions;
using Microsoft.AspNetCore.Http;

namespace FluentFilter;

public class QueryProvider(IHttpContextAccessor httpContextAccessor, FluentFilterConfiguration fluentFilterConfiguration)
{
    public Query GetQueryFromQueryString<T>()
    {
        var config = fluentFilterConfiguration.GetConfig<T>();

        return httpContextAccessor.HttpContext.Request.Query.GetQueryFromQueryString(config);
    }
}