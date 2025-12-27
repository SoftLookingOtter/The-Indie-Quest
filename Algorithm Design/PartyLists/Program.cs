using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        // Test 1: 2 participants
        var list2 = new List<string> { "Allie", "Ben" };
        WriteAllPermutations(list2);

        Console.WriteLine("—");

        // Test 2: 3 participants
        var list3 = new List<string> { "Allie", "Ben", "Claire" };
        WriteAllPermutations(list3);
    }

    // Entry method required by the assignment
    static void WriteAllPermutations(List<string> items)
    {
        Console.WriteLine("Signed-up participants: " + string.Join(", ", items));
        Console.WriteLine();
        Console.WriteLine("Starting orders:");

        int counter = 1;
        GeneratePermutations(items, 0, ref counter);

        Console.WriteLine();
    }

    // Recursive permutation generator
    static void GeneratePermutations(List<string> items, int startIndex, ref int counter)
    {
        // Base case: reached end of list
        if (startIndex == items.Count - 1)
        {
            Console.WriteLine($"{counter}. {string.Join(", ", items)}");
            counter++;
            return;
        }

        // Recursive case
        for (int i = startIndex; i < items.Count; i++)
        {
            Swap(items, startIndex, i);
            GeneratePermutations(items, startIndex + 1, ref counter);
            Swap(items, startIndex, i); // backtrack
        }
    }

    // Helper method to swap two elements
    static void Swap(List<string> items, int i, int j)
    {
        string temp = items[i];
        items[i] = items[j];
        items[j] = temp;
    }
}
