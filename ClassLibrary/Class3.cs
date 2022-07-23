using Microsoft.Extensions.Configuration;

namespace ClassLibrary;

public class Class3
{
    readonly string One;
    readonly string Two;
    readonly string Three;

    public Class3(IConfiguration configuration)
    {
        One = configuration["First"];
        Two = configuration["Second"];
        Three = configuration["Third"];

        Console.WriteLine("Class3 was instantiated.");
    }

    public void Run() { Console.WriteLine($"Class3 was run: {One}, {Two}, {Three}"); }
}
