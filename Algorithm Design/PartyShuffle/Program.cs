using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        // Create list of participants
        List<string> participants = new List<string>
        {
            "Allie",
            "Ben",
            "Claire",
            "Dan",
            "Eleanor"
        };

        // Output original order
        Console.WriteLine("Signed-up participants:");
        Console.WriteLine(string.Join(", ", participants));

        Console.WriteLine();
        Console.WriteLine("Generating starting order ...");
        Console.WriteLine();

        // Shuffle the list
        ShuffleList(participants);

        // Output shuffled order
        Console.WriteLine("Starting order:");
        Console.WriteLine(string.Join(", ", participants));
    }

    // Fisher–Yates (modern) shuffle
    static void ShuffleList(List<string> items)
    {
        Random random = new Random();

        for (int i = items.Count - 1; i > 0; i--)
        {
            int j = random.Next(i + 1);

            // Swap items[i] and items[j]
            string temp = items[i];
            items[i] = items[j];
            items[j] = temp;
        }
    }
}
