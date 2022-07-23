using Microsoft.Extensions.Configuration;

namespace ClassLibrary;

public class Class2
{
    readonly string One;
    readonly string Two;

    public Class2(IConfiguration configuration)
    {
        One = configuration["First"];
        Two = configuration["Second"];

        Console.WriteLine("Class2 was instantiated.");
    }

    public void Run() { Console.WriteLine($"Class2 was run: {One}, {Two}"); }
}
