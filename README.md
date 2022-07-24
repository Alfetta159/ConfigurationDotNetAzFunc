# Configurable for Both Azure Functions and .NET Projects

from [dev.to](https://dev.to/alfetta159/make-your-libraries-configurable-for-both-azure-functions-and-net-projects-3ocn)

Dependency Injection (DI) in .NET is truly powerful when you get accustomed to thinking that way at the beginning of your project. Often, we use one of the `AddScoped` or `AddSingleton` extension methods in order to create objects based on our classes or some third-party library's entities. However, if the third-party libraries aren't allowing us to pass configuration data via objects exposing `IConfiguration` or `IOption`, our `HostBuilder` command chains can get out of hand. And if that is not enough, Azure Functions typically pass configuration data through environment variables.

## What makes our command chains messy?

Often when we have to inject a dependency, we have to pass in  a function to create that object with parameter data that can't be injected earlier up the `IServicesCollection`:

```csharp
// Here we have a constructor with two parameters neither of which is of type IConfiguration
AddScoped((sp) =>
{
   var config = sp.GetService<IConfiguration>();
   return new Client(param1:config["param1"], param2:config["param2"]);
});
```
If we're lucky, there is a constructor or overload that does accept an `IConfiguration` type, and with our configuration having been injected as the `IConfiguration` type we needn't use the parameter to call the other constructor.

```csharp
// Here we have a constructor with one parameters which is of type IConfiguration
AddScoped<IClient, Client>();
```

I often like to pass in an `IConfigurationSection` leaving the fishing out of the specific configuration that is pertinent to the injected object from the larger configuration file to the services building command chain. Doing this will make the injected class more flexible when using them in Web API projects, console applications, and Azure Functions.

## Let's make our libraries work easily in Azure Functions as well as the other .NET projects

Let's say we have a library, and that library has any number of classes. We see this in API client libraries (a.k.a. SDKs). We can allow the user to inject which objects they need, or we can offer an option to inject everything at once in a nice clean extension method. This is often done when many objects are not optional. That could look like this:

```csharp
AddMyClassLibrary();
```
And that would call all of the base injection extension methods:
```csharp
AddScoped<IClient, Client>()
.AddScoped<IClient1, Client1>()
.AddScoped<IClient2, Client2>()
//   ...
.AddScoped<IClientN, ClientN>();
```

Because we injected our configuration, and at least one parameter in all constructors in all of our classes in our library is of `IConfiguration` type, we allow DI to do the work of passing the configuration along.

## But what about Azure Functions?

Azure Functions typically use environment variables to hold their configurations whether that be from the host.json, local.host.json or the configuration in the functions project in the Azure host. In other words, Azure Functions isn't using IConfiguration. This is where we should really consider how we let our consumers create our library objects with dependency injection.

## Extension methods that work together

First, let's offer a basic extension method that injects all of our classes. What if the consumer doesn't need all the classes? Don't worry, they won't be instantiated until it is used somewhere in the aggregating project. It might seem strange that we're digging out the configuration from the calling host builder, but we allowing the consumer not to think about these details:

```csharp
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

```

Second, let's offer a similar overload that allows the user to have a named configuration section in their settings file:

```csharp
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
```

Third, we're offering a easy way to put our classes in an Azure functions project:

```csharp
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
```

## What it looks like for the consumer

In the sample below, it shows the various extensions methods in play. 

> Understand that you wouldn't call all of these together. You would just pick and choose which are appropriate for you application. 

We trade off burying the busy work of our service configuration builder into reusable extension methods to have a nice clean builder here where it counts in our users' application code.

```csharp
        services
            // Uncomment only the following line to add the class library with the 'Default' configuration:
            .AddClassLibrary();

            // Uncomment only the following line to add the class library with the named configuration:
            .AddClassLibrary("SecondSection");

            // Uncomment only the following two lines to add only two classes from the class library:
            .AddScoped<Class1>()
            .AddScoped<Class3>()

            // Uncomment only the following command to add only the second class configured with the named configuration:
            .AddScoped(sp =>
            {
                var configuration = sp.GetService<IConfiguration>();
                return new Class2(configuration!.GetSection("SecondSection"));
            })

            // Uncomment only the following line to configure using the command-line arguments in the launch.json:
            .AddClassLibraryFromCommandline()
            ;
```

In the sample code, I even did a version that would get parameters from the command line arguments, but that's probably not that useful as an application that would import our library would handle parsing command line arguments.

## What to take away:

- These are great patterns for libraries that have lots of classes and/or lots of configuration settings.
- If you're creating reusable libraries especially those that will become NuGet packages, then this is a great way to make it easy for your users to get started, even if those users are just in another department in your company.
- It's not lost on me that everything here is added as scoped. You'll probably find that in your library, some classes are better scoped and others as singleton or even transitional, and you know best how your classes should be used.
- But remember that your consumers can always inject your classes ala carte in whatever way they want, and because you're making sure to use at least one parameter in all of your constructors that is of the `IConfiguration` type, that will be easy for them using the type parameter forms of the `AddScoped/Singleton/Transitional` extensions methods.
- And these are hopefully just some seminal ideas that can be adapted or replaced by the great ideas that you come up with for your library. 

Let me know what you've come up with in the comments, and please clone the repo. I pride myself on never showing code snippets without a working sample.

- Runs on .NET 6
- Created in VSCode on Ubuntu Linux
