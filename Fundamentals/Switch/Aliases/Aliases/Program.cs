using System;
using System.Drawing;
using System.Globalization; // För att läsa tal med punkt istället för komma

namespace SimpleCalculator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 1. Be spelaren sätta pris
            Console.Write("Set the price: ");
            string input = Console.ReadLine()?.Trim() ?? ""; // ?. - kraschar inte om ReadLine() skulle ge null. // ?? "" – om allt ändå blev null, ersättning med tom sträng så resten av koden kan fortsätta.

            // 2. Dela upp texten vid mellanslag // 
            string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries); // .Split(' ') betyder: – Dela upp texten där det finns mellanslag -> t.ex. "5 + 3" blir ["5", "+", "3"] // StringSplitOptions.RemoveEmptyEntries tar bort om det skulle finnas flera mellanslag.

            // 3. Om vi bara fick ett tal
            if (parts.Length == 1)
            {
                if (TryReadNumber(parts[0], out double number)) // TryReadNumber är en hjälpfunktion som returnerar true om den lyckas läsa ett tal från texten. - konverterar string till double.
                {
                    Console.WriteLine($"The price was set to {number}.");
                }
                else
                {
                    Console.WriteLine("Invalid number.");
                }
            }
            // 4. Om vi fick (minst) tre delar: tal, operator (ev. flera ord), tal
            else if (parts.Length >= 3)
            {
                string firstText = parts[0];
                string secondText = parts[parts.Length - 1];

                if (TryReadNumber(firstText, out double num1) &&
                    TryReadNumber(secondText, out double num2))
                {
                    double result;

                    // Operatorn är allt mellan första och sista ordet (t.ex. "divided by")
                    string op = string.Join(' ', parts, 1, parts.Length - 2).ToLowerInvariant();

                    /* 
                    
                    [  0   |   1    2   ...  n-2 |  n-1 ] 
                    [first |   ----- operator ---- | last]
                    
                    startIndex = 1 count = Length - 2 
                    
                    */

                    // 5. Använd switch för att välja räknesätt
                    switch (op)
                    {
                        // Addition
                        case "+":
                        case "plus":
                        case "add":
                            result = num1 + num2;
                            break;

                        // Subtraktion
                        case "-":
                        case "minus":
                        case "subtract":
                            result = num1 - num2;
                            break;

                        // Multiplikation
                        case "*":
                        case "x":
                        case "times":
                        case "multiply":
                        case "multiplied by":
                            result = num1 * num2;
                            break;

                        // Division
                        case "/":
                        case "div":
                        case "divide":
                        case "divided by":
                            if (num2 != 0)
                                result = num1 / num2;
                            else
                            {
                                Console.WriteLine("Cannot divide by zero.");
                                return;
                            }
                            break;

                        default:
                            Console.WriteLine("Unsupported operator.");
                            return;
                    }

                    Console.WriteLine($"The price was set to {result}.");
                }
                else
                {
                    Console.WriteLine("Invalid numbers in equation.");
                }
            }
            // 6. Annars är formatet fel
            else
            {
                Console.WriteLine("Invalid input format. Use 'number' or 'number operator number' with spaces.");
            }
        }

        // Hjälpfunktion för att läsa tal (med punkt som decimaltecken)
        static bool TryReadNumber(string text, out double number)
        {
            return double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out number);
        }
    }
}


