using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ClassLibrary;

public static class Extensions
{
    public static IServiceCollection AddClassLibrary(this IServiceCollection services, IConfigurationSection configurationSection)
    {
        services
            .AddScoped<Class1>()
            .AddScoped<Class2>()
            .AddScoped<Class3>();

        return services;
    }

    public static IServiceCollection AddClassLibrary(this IServiceCollection services, IConfiguration configuration, string sectionName = "ConfigurationSection")
    {
        services
            .AddClassLibrary(configuration.GetSection(sectionName));

        return services;
    }
}