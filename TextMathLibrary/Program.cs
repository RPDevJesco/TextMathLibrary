using TextMathLibrary;

public class Program
{
    public static void Main()
    {
        // Test the original example
        var expression = "three hundred million minus two hundred thousand";
        var result = TextMath.Calculate(expression);
        Console.WriteLine($"{expression} = {TextMath.FormatNumber(result)}");

        // Test maximum number
        var maxTest = "nine hundred ninety nine billion nine hundred ninety nine million nine hundred ninety nine thousand nine hundred ninety nine";
        var maxResult = TextMath.Calculate(maxTest);
        Console.WriteLine($"\nMaximum number test:\n{maxTest} = {TextMath.FormatNumber(maxResult)}");

        // Test all operations
        var operations = new[]
        {
            "one million plus five hundred thousand",
            "two million minus one hundred thousand",
            "three million multiplied by two",
            "ten million divided by two",
            "two plus five",
            "ten divided by ten",
            "10 divided by 10",
            "three divided by two plus five",
        };

        Console.WriteLine("\nTesting all operations:");
        foreach (var op in operations)
        {
            Console.WriteLine($"{op} = {TextMath.FormatNumber(TextMath.Calculate(op))}");
        }
    }
}