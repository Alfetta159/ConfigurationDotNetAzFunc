using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ClassLibrary;

public static class Extensions
{
    public static IServiceCollection AddClassLibrary<TService>(this IServiceCollection services)
    where TService : class
    {
        return services;
    }

    public static IServiceCollection AddClassLibrary(this IServiceCollection services, Type serviceType)
    {
        return services;
    }

    public static IServiceCollection AddClassLibrary(this IServiceCollection services, Func<object> implementationFactory)
    // where TService : class
    // where TImplementation : class, TService
    {
        return services;
    }

    public static IServiceCollection AddClassLibrary<TService>(this IServiceCollection services, Func<IServiceProvider, TService> implementationFactory)
    where TService : class
    {
        return services;
    }

    // public static IServiceCollection AddClassLibrary(this IServiceCollection services, IConfigurationSection configurationSection)
    // {
    //     services
    //         .AddClassLibrary<Class1>()
    //         .AddClassLibrary<Class2>()
    //         .AddClassLibrary<Class3>();

    //     return services;
    // }

    // public static IServiceCollection AddClassLibrary(this IServiceCollection services, string sectionName = "ConfigurationSection")
    // {
    //     services
    //     .AddClassLibrary<Class1>()
    //         .AddClassLibrary(services =>
    //         {
    //             var c = sp.GetService<IConfiguration>();
    //             return new ErpApi.Client(c);
    //         })
    //         .AddClassLibrary(configuration.GetSection(sectionName));

    //     return services;
    // }

    // public static IServiceCollection AddClassLibrary(this IServiceCollection services)
    // {
    //     // services
    //     //     .AddClassLibrary(configuration.GetSection(sectionName));

    //     return services;
    // }

    // public static IServiceCollection AddClassLibrary<TService>(this IServiceCollection services, Func<IServiceProvider, TService> implementationFactory)
    // where TService : class
    // {
    //     return services;
    // }

    // public static IServiceCollection AddClassLibrary<TService, TImplementation>(this IServiceCollection services, Func<IServiceProvider, TImplementation> implementationFactory)
    //     where TService : class
    //     where TImplementation : class, TService
    // {
    //     return services;
    // }

}