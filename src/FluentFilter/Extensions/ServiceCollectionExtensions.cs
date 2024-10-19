using FluentFilter.Config;
using Microsoft.Extensions.DependencyInjection;

namespace FluentFilter.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFluentFilter(this IServiceCollection services)
    {
        var config = new FluentFilterConfiguration();
        services.AddSingleton(config);
        
        var type = typeof(IFluentFilterConfiguration<>);
        var configTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => p.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == type))
            .ToArray();

        foreach (var configType in configTypes)
        {
            var interfaceType = configType.GetInterfaces().First(i => i.IsGenericType && i.GetGenericTypeDefinition() == type);
            config.AddType(interfaceType.GenericTypeArguments.First());

            var instance = Activator.CreateInstance(configType);

            var builderType = typeof(FluentFilterBuilder<>).MakeGenericType(interfaceType.GenericTypeArguments);
            var builder = Activator.CreateInstance(builderType, config);
            
            var method = configType.GetMethod("Configure");
            method!.Invoke(instance, [builder]);
        }

        return services;
    }
}