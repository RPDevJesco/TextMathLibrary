# TextMath Library

A C# library that converts English text descriptions of mathematical expressions into numerical calculations. This library allows users to perform mathematical operations using natural language input.
A perfect solution for apps that target children who are learning numbers in numerical format from written word and vice-versa.

## Features

- Convert written numbers to numeric values (e.g., "three hundred fifty-two" → 352)
- Support for large numbers up to 999,999,999,999 (999 billion)
- Basic arithmetic operations:
  - Addition ("plus")
  - Subtraction ("minus")
  - Multiplication ("multiplied by" or "times")
  - Division ("divided by")
- Handles both text and numeric input formats
- Built-in number formatting with thousand separators

## Installation

1. Add the TextMathLibrary project to your solution
2. Add a reference to TextMathLibrary in your project
3. Add the following using statement:
```csharp
using TextMathLibrary;
```

## Usage

### Basic Number Parsing

```csharp
// Parse numbers written in words
long result1 = TextMath.ParseNumber("three hundred fifty-two");  // Returns: 352
long result2 = TextMath.ParseNumber("one million");             // Returns: 1000000

// Format numbers with thousand separators
string formatted = TextMath.FormatNumber(result2);              // Returns: "1,000,000"
```

### Mathematical Operations

```csharp
// Basic arithmetic
long sum = TextMath.Calculate("one million plus five hundred thousand");  // Returns: 1500000
long diff = TextMath.Calculate("one million minus five hundred thousand"); // Returns: 500000
long product = TextMath.Calculate("three million multiplied by two");     // Returns: 6000000
long quotient = TextMath.Calculate("ten million divided by two");         // Returns: 5000000

// Mixed numeric and word formats
long result = TextMath.Calculate("10 divided by ten");                    // Returns: 1
```

### Supported Number Formats

- Single digits: "one" through "nine"
- Teens: "eleven" through "nineteen"
- Tens: "twenty" through "ninety"
- Scales: "hundred", "thousand", "million", "billion"
- Numeric values: "0" through "999999999999"
- Hyphenated numbers: "twenty-five"
- Mixed formats: "10 million", "fifty 2"

## Examples

```csharp
// Large numbers
var bigNumber = TextMath.Calculate("nine hundred ninety nine billion nine hundred ninety nine million nine hundred ninety nine thousand nine hundred ninety nine");
Console.WriteLine(TextMath.FormatNumber(bigNumber));  // Output: 999,999,999,999

// Complex calculations
var result = TextMath.Calculate("three hundred million minus two hundred thousand");
Console.WriteLine(TextMath.FormatNumber(result));     // Output: 299,800,000

// Mixed format calculations
var mixed = TextMath.Calculate("50 million plus twenty five thousand");
Console.WriteLine(TextMath.FormatNumber(mixed));      // Output: 50,025,000
```

## Error Handling

The library throws `ArgumentException` for:
- Invalid expressions
- Numbers outside the supported range
- Unrecognized words or operators
- Invalid mathematical operations (e.g., division by zero)

```csharp
try
{
    var result = TextMath.Calculate("one million divided by zero");
}
catch (ArgumentException ex)
{
    Console.WriteLine("Invalid expression");
}
```

## Limitations

- Maximum supported number is 999,999,999,999 (999 billion)
- Only supports basic arithmetic operations (+, -, *, /)
- Does not support decimal numbers
- No support for mathematical precedence (operations are performed left to right)
- No support for parentheses or grouping
- Only supports English number words

## Best Practices

1. Always use proper spacing between words
2. The word "and" is optional and will be ignored
3. Hyphens in numbers (e.g., "twenty-five") will be treated the same as spaces
4. Use "multiplied by" instead of "times" for clearer expressions
5. Input is case-insensitive

## Contributing

Contributions are welcome! Here are some ways you can contribute to this project:
1. Report bugs
2. Suggest new features
3. Add support for additional languages
4. Improve documentation
5. Submit pull requests

## License

This library is available under the MIT License. See the LICENSE file for more details.
