using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ClassLibrary;

/// <summary>
/// A collection of easy to use extension methods allowing consumers to use our library w/o out too much start up pain.
/// </summary>
/// <remarks>
/// When using a settings.json file, configure settings like:
/// <code>
/// "DefaultSection":  {
///     "First": "xx",
///     "Second": "xx",
///     "Third": "xx"
/// },
/// </code>
/// </remarks>
public static class Extensions
{
    public const string DefaultSectionName = "DefaultSection";

    /// <summary>Add all classes with the IConfiguration object</summary>
    /// <remarks>Ironically, we dig out the configuration to find the default section in the settings file.
    /// It might seem a bit clunky, but it's very reuseable and makes for an easy reference in your consumers' start up files.
    /// </remarks>
    public static IServiceCollection AddClassLibrary(this IServiceCollection services)
    {
        services
            .AddScoped((sp) =>
            {
                var configuration = sp.GetService<IConfiguration>();
                return new Class1(configuration!.GetSection(DefaultSectionName));
            })
            .AddScoped((sp) =>
            {
                var configuration = sp.GetService<IConfiguration>();
                return new Class2(configuration!.GetSection(DefaultSectionName));
            })
            .AddScoped((sp) =>
            {
                var configuration = sp.GetService<IConfiguration>();
                return new Class3(configuration!.GetSection(DefaultSectionName));
            });

        return services;
    }

    /// <summary>Add all classes with the IConfiguration object but with a named configuration section.</summary>
    /// <remarks>Ironically, we dig out the configuration to find the default section in the settings file.
    /// It might seem a bit clunky, but it's very reuseable and makes for an easy reference in your consumers' start up files.
    /// </remarks>
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
                return new Class2(configuration!.GetSection(configurationSectionName));
            })
            .AddScoped((sp) =>
            {
                var configuration = sp.GetService<IConfiguration>();
                return new Class3(configuration!.GetSection(configurationSectionName));
            });

        return services;
    }

    /// <summary>Add all classes with the IConfiguration object but with configuration that you find in Azure functions.</summary>
    /// <remarks>We create a configuration as an in-memory collection, which is completely separate from any configuration already in the service collection),
    /// This is really handy for Azure Functions that rely more on the environment variables collection and not an IConfiguration object.
    /// </remarks>
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

    /// <summary>Add all classes and configure with parameters from the command line.</summary>
    /// <remarks>It's just like the previous, but were getting command line args from teh Environment.
    /// </remarks>
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