using Microsoft.Extensions.Configuration;

namespace ClassLibrary;

public class Class3
{
    readonly string One;
    readonly string Two;
    readonly string Three;

    public Class3(IConfigurationSection configurationSection)
    {
        One = configurationSection["First"];
        Two = configurationSection["Second"];
        Three = configurationSection["Third"];

        Console.WriteLine("Class2 was instantiated.");
    }

    public void Run() { Console.WriteLine($"Class2 was run: {One}, {Two}, {Three}"); }
}
