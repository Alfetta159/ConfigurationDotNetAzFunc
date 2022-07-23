using Microsoft.Extensions.Configuration;

namespace ClassLibrary;

public class Class1
{
    readonly string One;

    public Class1(IConfiguration configuration)
    {
        One = configuration["First"];

        // Always make sure that you let your consumer know exactly what they need for configuration!
        if (String.IsNullOrWhiteSpace(One))
            throw new ArgumentException($"The value for 'One' is not configured. Did you forget to add a value for 'First' in you settings?");

        Console.WriteLine("Class1 was instantiated.");
    }

    public void Run() { Console.WriteLine($"Class1 was run: {One}"); }
}
