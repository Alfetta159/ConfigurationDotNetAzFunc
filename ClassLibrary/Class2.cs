using Microsoft.Extensions.Configuration;

namespace ClassLibrary;

public class Class2
{
    readonly string One;
    readonly string Two;

    public Class2(IConfigurationSection configurationSection)
    {
        One = configurationSection["First"];
        Two = configurationSection["Second"];

        Console.WriteLine("Class2 was instantiated.");
    }

    public void Run() { Console.WriteLine($"Class2 was run: {One}, {Two}"); }
}
