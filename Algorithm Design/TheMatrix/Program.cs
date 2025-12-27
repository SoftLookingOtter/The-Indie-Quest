using System;
using System.Collections.Generic;
using System.Threading;

class Program
{
    static void Main()
    {
        Random random = new Random();

        // List of active stream positions (columns)
        List<int> streams = new List<int>();

        // Symbols used in the Matrix effect
        string symbols = @"!@#$%^&*()_+-=[];',.\/~{}:|<>?";

        // Initialize with 10 random streams
        for (int i = 0; i < 10; i++)
        {
            streams.Add(random.Next(0, 80));
        }

        Console.ForegroundColor = ConsoleColor.DarkGreen;
        Console.CursorVisible = false;

        while (true)
        {
            // Draw one row
            for (int x = 0; x < 80; x++)
            {
                Console.Write(
                    streams.Contains(x)
                        ? symbols[random.Next(symbols.Length)]
                        : ' '
                );
            }

            Console.WriteLine();
            Thread.Sleep(100);

            // Occasionally remove a stream
            if (streams.Count > 0 && random.Next(3) == 0)
            {
                streams.RemoveAt(random.Next(streams.Count));
            }

            // Occasionally add a new stream
            if (random.Next(3) == 0)
            {
                streams.Add(random.Next(0, 80));
            }
        }
    }
}
