using ClassLibrary;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
        Console.WriteLine("startup!");
        services.AddClassLibraryFromEnvironment();
    })
    .Build();

host.Run();
