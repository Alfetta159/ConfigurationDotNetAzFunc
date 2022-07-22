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
            .AddScoped((sp) =>
            {
                var configuration = sp.GetService<IConfiguration>();
                return new Class1(configuration);
            })
            .AddClassLibrary(sp =>
            {
                var configuration = sp.GetService<IConfiguration>();
                return new Class2(configuration!.GetSection("SecondSection"));
            })
            .AddClassLibrary(sp =>
            {
                var configuration = sp.GetService<IConfiguration>();
                return new Class3(configuration!.GetSection("SecondSection"));
            });
    })
    .Build();

var one = host.Services.GetService<Class1>();
// var test = host.Services.GetService<Class2>();
var three = host.Services.GetService<Class3>();

one!.Run();
// two!.Run();
three!.Run();