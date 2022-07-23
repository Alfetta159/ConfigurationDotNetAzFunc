// See https://aka.ms/new-console-template for more information
using ClassLibrary;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Console.WriteLine("Hello, World!");

using IHost host = Host
    .CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services
            // Uncomment only the following line to add the class library with the 'Default' configuration:
            .AddClassLibrary();

            // Uncomment only the following line to add the class library with the named configuration:
            //.AddClassLibrary("SecondSection");

            // Uncomment only the following two lines to add only two classes from the class library:
            // .AddScoped<Class1>()
            // .AddScoped<Class3>()

            // Uncomment only the following command to add only the second class configured with the named configuration:
            // .AddScoped(sp =>
            // {
            //     var configuration = sp.GetService<IConfiguration>();
            //     return new Class2(configuration!.GetSection("SecondSection"));
            // })

            // Uncomment only the following line to configure using the command-line arguments in the launch.json:
            //.AddClassLibraryFromCommandline()
            ;
    })
    .Build();

var one = host.Services.GetService<Class1>();
// var test = host.Services.GetService<Class2>();
var three = host.Services.GetService<Class3>();

one!.Run();
// two!.Run();
three!.Run();