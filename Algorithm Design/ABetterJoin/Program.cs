// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Generic;

class Program
{
    static string JoinWithAnd(List<string> items, bool useSerialComma = true)
    {
        int count = items.Count;

        // count = 0
        if (count == 0)
        {
            return string.Empty;
        }

        // count = 1
        if (count == 1)
        {
            return items[0];
        }

        // count = 2
        if (count == 2)
        {
            return items[0] + " and " + items[1];
        }

        // Create copy of items
        List<string> copy = new List<string>(items);

        // Use serial comma?
        if (useSerialComma)
        {
            // prepend "and " to last item
            int lastIndex = copy.Count - 1;
            copy[lastIndex] = "and " + copy[lastIndex];

            // join with ", "
            return string.Join(", ", copy);
        }
        else
        {
            // Join last two items with " and "
            int lastIndex = copy.Count - 1;
            string combined = copy[lastIndex - 1] + " and " + copy[lastIndex];

            // set this as second to last item
            copy[lastIndex - 1] = combined;

            // remove last item
            copy.RemoveAt(lastIndex);

            // return joined with ", "
            return string.Join(", ", copy);
        }
    }

    static void Main()
    {
        var heroes = new List<string> { "Jazlyn", "Theron", "Dayana", "Rolando" };

        Console.WriteLine(JoinWithAnd(heroes, useSerialComma: true));
        Console.WriteLine(JoinWithAnd(heroes, useSerialComma: false));
    }
}
