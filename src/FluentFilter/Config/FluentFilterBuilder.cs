using System.Linq.Expressions;
using System.Reflection;

namespace FluentFilter.Config;

public class FluentFilterBuilder<T>(FluentFilterConfiguration configuration)
{
    public FluentFilterBuilder<T> AddFilter<TProperty>(Expression<Func<T, TProperty>> expression)
    {
        var propInfo = GetPropertyInfo(expression);

        configuration.SetFilterable<T>(propInfo.Name);

        return this;
    }
    
    public FluentFilterBuilder<T> AddSort<TProperty>(Expression<Func<T, TProperty>> expression)
    {
        var propInfo = GetPropertyInfo(expression);
        
        configuration.SetSortable<T>(propInfo.Name);
        
        return this;
    }
    
    private static PropertyInfo GetPropertyInfo<TProperty>(Expression<Func<T, TProperty>> propertyLambda)
    {
        if (propertyLambda.Body is not MemberExpression member)
        {
            throw new ArgumentException($"Expression '{propertyLambda}' refers to a method, not a property.");
        }

        if (member.Member is not PropertyInfo propInfo)
        {
            throw new ArgumentException($"Expression '{propertyLambda}' refers to a field, not a property.");
        }

        Type type = typeof(T);
        if (propInfo.ReflectedType != null && type != propInfo.ReflectedType && !type.IsSubclassOf(propInfo.ReflectedType))
        {
            throw new ArgumentException(
                $"Expression '{propertyLambda}' refers to a property that is not from type {type}.");
        }

        return propInfo;
    }
}