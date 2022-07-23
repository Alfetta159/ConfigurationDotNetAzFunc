using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ClassLibrary;

public static class Extensions
{
    public static IServiceCollection AddClassLibrary(this IServiceCollection services)
    {
        services
            .AddScoped((sp) =>
            {
                var configuration = sp.GetService<IConfiguration>();
                return new Class1(configuration!.GetSection("DefaultSection"));
            })
            .AddScoped((sp) =>
            {
                var configuration = sp.GetService<IConfiguration>();
                return new Class2(configuration!.GetSection("DefaultSection"));
            })
            .AddScoped((sp) =>
            {
                var configuration = sp.GetService<IConfiguration>();
                return new Class3(configuration!.GetSection("DefaultSection"));
            });

        return services;
    }

    public static IServiceCollection AddClassLibrary(this IServiceCollection services, string configurationSectionName)
    {
        services
            .AddScoped((sp) =>
            {
                var configuration = sp.GetService<IConfiguration>();
                return new Class1(configuration!.GetSection(configurationSectionName));
            })
            .AddScoped((sp) =>
            {
                var configuration = sp.GetService<IConfiguration>();
                return new Class2(configuration!.GetSection("DefaultSection"));
            })
            .AddScoped((sp) =>
            {
                var configuration = sp.GetService<IConfiguration>();
                return new Class3(configuration!.GetSection("DefaultSection"));
            });

        return services;
    }

    public static IServiceCollection AddClassLibraryFromEnvironment(this IServiceCollection services)
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                ["First"] = Environment.GetEnvironmentVariable("First")!,
                ["Second"] = Environment.GetEnvironmentVariable("Second")!,
                ["Third"] = Environment.GetEnvironmentVariable("Third")!
            })
            .Build();

        services
            .AddScoped(s => new Class1(configuration))
            .AddScoped(s => new Class2(configuration))
            .AddScoped(s => new Class3(configuration));

        return services;
    }

    public static IServiceCollection AddClassLibraryFromCommandline(this IServiceCollection services)
    {
        var args = Environment.GetCommandLineArgs();

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                ["First"] = args[1],
                ["Second"] = args[2],
                ["Third"] = args[3]
            })
            .Build();

        services
            .AddScoped(s => new Class1(configuration))
            .AddScoped(s => new Class2(configuration))
            .AddScoped(s => new Class3(configuration));

        return services;
    }
}