using System;

class Program
{
    // Mission 3: Ordinal numbers
    static string OrdinalNumber(int number)
    {
        // Get last digit
        int lastDigit = number % 10;

        // Get second to last digit
        int secondLastDigit = (number / 10) % 10;

        // Special case: numbers ending in 11, 12, 13
        if (secondLastDigit == 1)
        {
            return number + "th";
        }

        // Otherwise, decide based on last digit
        if (lastDigit == 1)
        {
            return number + "st";
        }

        if (lastDigit == 2)
        {
            return number + "nd";
        }

        if (lastDigit == 3)
        {
            return number + "rd";
        }

        return number + "th";
    }

    static void Main()
    {
        int[] testNumbers =
        {
            1, 2, 3, 4,
            10, 11, 12, 13,
            21, 101, 111, 121
        };

        foreach (int number in testNumbers)
        {
            Console.WriteLine(OrdinalNumber(number));
        }
    }
}
