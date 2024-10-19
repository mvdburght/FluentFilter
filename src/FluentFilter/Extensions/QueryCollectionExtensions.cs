using FluentFilter.Config;
using Microsoft.AspNetCore.Http;

namespace FluentFilter.Extensions;

public static class QueryCollectionExtensions
{
    private static readonly Dictionary<string, Operator> Operators = new()
    {
        ["eq."] = Operator.Equals,
        ["ne."] = Operator.NotEqual,
        ["gt."] = Operator.GreaterThan,
        ["ge."] = Operator.GreaterThanOrEqual,
        ["lt."] = Operator.LessThan,
        ["le."] = Operator.LessThanOrEqual,
        ["bl."] = Operator.Blank,
        ["nb."] = Operator.NotBlank,
        ["co."] = Operator.Contains,
        ["bw."] = Operator.BeginsWith,
        ["ew."] = Operator.EndsWith
    };
    
    public static Query GetQuery<T>(this IQueryCollection collection, FluentFilterConfiguration configProvider)
    {
        var config = configProvider.GetConfig<T>();

        return GetQueryFromQueryString(collection, config);
    }
    
    public static Query GetQueryFromQueryString(this IQueryCollection collection, ICollection<FluentFilterConfig> fields)
    {
        var offset = GetIntValue(collection, "offset") ?? 0;
        var limit = GetIntValue(collection, "limit") ?? 20;

        var pagination = new Pagination(offset, limit);

        var sortBy = collection.TryGetValue("sortby", out var orderBy)
            ? orderBy
                .Select(x => new Sort(x.TrimStart('-'), x.StartsWith('-') ? Direction.Descending : Direction.Ascending))
                .Where(s => fields.Any(f => f.IsSortable && f.Name.Equals(s.Field, StringComparison.InvariantCultureIgnoreCase)))
                .ToArray()
            : [];

        var filters = fields
            .Where(x => collection.ContainsKey(x.Name))
            .SelectMany(f => collection[f.Name].Select(x => ParseFilter(f.Name, x!, f.Type)))
            .ToArray();

        return new Query(pagination, filters, sortBy);
    }

    private static Filter ParseFilter(string field, string value, Type type) =>
        value.Length >= 3 && Operators.TryGetValue(value[..3], out var opp)
            ? new Filter(field, opp, Convert.ChangeType(value[3..], type))
            : new Filter(field, Operator.Equals, Convert.ChangeType(value, type));
    
    private static int? GetIntValue(IQueryCollection collection, string name)
    {
        if (collection.TryGetValue(name, out var stringValue)
            && int.TryParse(stringValue, out var value))
        {
            return value;
        }

        return default;
    }
}