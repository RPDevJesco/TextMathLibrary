using System.Text.RegularExpressions;

namespace TextMathLibrary
{
    /// <summary>
    /// Provides functionality to parse and calculate mathematical expressions written in natural English text.
    /// Supports numbers up to 999 billion and basic arithmetic operations.
    /// </summary>
    /// <remarks>
    /// This class can handle expressions like:
    /// - "three hundred million minus two hundred thousand"
    /// - "one billion plus five hundred million"
    /// - "fifty thousand multiplied by three"
    /// 
    /// Supported operations:
    /// - Addition (plus)
    /// - Subtraction (minus)
    /// - Multiplication (multiplied by, times)
    /// - Division (divided by)
    /// 
    /// Maximum supported number: 999,999,999,999 (999 billion)
    /// </remarks>
    public class TextMath
    {
        /// <summary>
        /// Dictionary mapping word representations of numbers to their numeric values.
        /// Covers numbers from 0-19 and multiples of 10 up to 90.
        /// </summary>
        private static readonly Dictionary<string, long> NumberWords = new()
        {
            // Basic numbers (0-9)
            { "zero", 0 }, { "one", 1 }, { "two", 2 }, { "three", 3 }, { "four", 4 },
            { "five", 5 }, { "six", 6 }, { "seven", 7 }, { "eight", 8 }, { "nine", 9 },
            // Special cases (10-19)
            { "ten", 10 }, { "eleven", 11 }, { "twelve", 12 }, { "thirteen", 13 },
            { "fourteen", 14 }, { "fifteen", 15 }, { "sixteen", 16 }, { "seventeen", 17 },
            { "eighteen", 18 }, { "nineteen", 19 },
            // Tens (20-90)
            { "twenty", 20 }, { "thirty", 30 }, { "forty", 40 }, { "fifty", 50 },
            { "sixty", 60 }, { "seventy", 70 }, { "eighty", 80 }, { "ninety", 90 }
        };

        /// <summary>
        /// Dictionary mapping scale words to their corresponding multipliers.
        /// Example: "million" -> 1,000,000
        /// </summary>
        private static readonly Dictionary<string, long> Scales = new()
        {
            { "hundred", 100 },
            { "thousand", 1000 },
            { "million", 1000000 },
            { "billion", 1000000000 }
        };

        /// <summary>
        /// Dictionary mapping textual operators to their mathematical symbols.
        /// Supports multiple variants for multiplication ("times", "multiplied by").
        /// </summary>
        private static readonly Dictionary<string, string> Operators = new()
        {
            { "plus", "+" },
            { "minus", "-" },
            { "multiplied by", "*" },
            { "times", "*" },
            { "divided by", "/" }
        };

        /// <summary>
        /// Parses a number written in English words into its numeric value.
        /// </summary>
        /// <param name="text">The number written in words (e.g., "three hundred million")</param>
        /// <returns>The numeric value of the written number</returns>
        /// <remarks>
        /// The method handles:
        /// - Basic numbers ("one" through "ninety")
        /// - Scale words ("hundred", "thousand", "million", "billion")
        /// - Composite numbers ("twenty three", "three hundred fifty six")
        /// - The word "and" is ignored ("three hundred and fifty" = "three hundred fifty")
        /// </remarks>
        public static long ParseNumber(string text)
        {
            // Normalize input by converting to lowercase and replacing hyphens with spaces
            text = text.ToLower().Replace("-", " ");
            var words = text.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                          .Where(w => w != "and") // Remove "and" words as they're optional in English numbers
                          .ToList();
            
            // If the entire input is a numeric string, parse it directly
            if (words.Count == 1 && long.TryParse(words[0], out long directNumber))
            {
                return directNumber;
            }

            long total = 0;        // Accumulates the final result
            long currentNumber = 0; // Builds up the current number being processed

            foreach (var word in words)
            {
                // Try to parse the word as a number first
                if (long.TryParse(word, out long parsedNumber))
                {
                    currentNumber += parsedNumber;
                }
                else if (NumberWords.ContainsKey(word))
                {
                    // Add the value of the current word to our running total
                    currentNumber += NumberWords[word];
                }
                else if (Scales.ContainsKey(word))
                {
                    // Handle scale words (hundred, thousand, million, billion)
                    currentNumber = currentNumber == 0 ? 1 : currentNumber; // Default to 1 if no number specified (e.g., "thousand" = "one thousand")
                    
                    if (Scales[word] == 100)
                    {
                        // Special handling for "hundred" as it's part of the current number being built
                        currentNumber *= Scales[word];
                    }
                    else
                    {
                        // For larger scales, multiply current number by scale and add to total
                        currentNumber *= Scales[word];
                        total += currentNumber;
                        currentNumber = 0; // Reset current number for next part
                    }
                }
            }

            return total + currentNumber; // Add any remaining current number to total
        }

        /// <summary>
        /// Calculates the result of a mathematical expression written in English words.
        /// </summary>
        /// <param name="expression">The mathematical expression in words (e.g., "one million plus five hundred thousand")</param>
        /// <returns>The result of the calculation</returns>
        /// <exception cref="ArgumentException">Thrown when the expression is invalid or cannot be evaluated</exception>
        /// <remarks>
        /// Supports:
        /// - Addition: "plus"
        /// - Subtraction: "minus"
        /// - Multiplication: "multiplied by" or "times"
        /// - Division: "divided by"
        /// </remarks>
        public static long Calculate(string expression)
        {
            // Normalize the input expression
            expression = expression.ToLower().Trim();
            var parts = new List<string>();
            var currentPart = "";
            var words = expression.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            // Parse the expression into numbers and operators
            for (int i = 0; i < words.Length; i++)
            {
                bool operatorFound = false;
                foreach (var op in Operators.Keys)
                {
                    // Check if the remaining words start with an operator
                    var remainingWords = string.Join(" ", words.Skip(i));
                    if (remainingWords.StartsWith(op))
                    {
                        // Add the accumulated number part if exists
                        if (!string.IsNullOrWhiteSpace(currentPart))
                        {
                            parts.Add(currentPart.Trim());
                        }
                        parts.Add(op);
                        currentPart = "";
                        i += op.Split(' ').Length - 1; // Skip the operator words
                        operatorFound = true;
                        break;
                    }
                }
                if (!operatorFound)
                {
                    currentPart += words[i] + " ";
                }
            }

            // Add the last number part if exists
            if (!string.IsNullOrWhiteSpace(currentPart))
            {
                parts.Add(currentPart.Trim());
            }

            // Build the computation string
            var computation = "";
            foreach (var part in parts)
            {
                if (Operators.ContainsKey(part))
                {
                    computation += Operators[part];
                }
                else
                {
                    computation += ParseNumber(part);
                }
            }

            return EvaluateExpression(computation);
        }

        /// <summary>
        /// Evaluates a mathematical expression string containing numbers and operators.
        /// </summary>
        /// <param name="expression">The expression to evaluate (e.g., "1000000+500000")</param>
        /// <returns>The result of the evaluation</returns>
        /// <exception cref="ArgumentException">Thrown when the expression is invalid or cannot be evaluated</exception>
        private static long EvaluateExpression(string expression)
        {
            try
            {
                // Split expression into numbers and operators
                var numbers = Regex.Split(expression, @"[+\-*/]")
                                 .Select(n => long.Parse(n.Trim()))
                                 .ToList();
                var operators = Regex.Matches(expression, @"[+\-*/]")
                                   .Select(m => m.Value)
                                   .ToList();

                // Evaluate the expression from left to right
                var result = numbers[0];
                for (int i = 0; i < operators.Count; i++)
                {
                    switch (operators[i])
                    {
                        case "+":
                            result += numbers[i + 1];
                            break;
                        case "-":
                            result -= numbers[i + 1];
                            break;
                        case "*":
                            result *= numbers[i + 1];
                            break;
                        case "/":
                            result /= numbers[i + 1];
                            break;
                    }
                }
                return result;
            }
            catch (Exception)
            {
                throw new ArgumentException("Invalid expression");
            }
        }

        /// <summary>
        /// Formats a number with appropriate thousand separators.
        /// </summary>
        /// <param name="number">The number to format</param>
        /// <returns>A string representation of the number with thousand separators</returns>
        /// <example>
        /// FormatNumber(1000000) returns "1,000,000"
        /// </example>
        public static string FormatNumber(long number)
        {
            return number.ToString("N0");
        }
    }
}