using Microsoft.Extensions.Configuration;

namespace ClassLibrary;

public class Class1
{
    readonly string One;

    public Class1(IConfigurationSection configurationSection)
    {
        One = configurationSection["First"];

        Console.WriteLine("Class1 was instantiated.");
    }

    public void Run() { Console.WriteLine($"Class1 was run: {One}"); }
}
