namespace OlympicGames;

using System;
using System.Collections.Generic;

internal static class Program
{
    public static void Main() // Startpunkt
    {
        // Mappar år → land (vinter-OS – 2000-talet)
        var hosts = new Dictionary<int, string>
        {
            [2002] = "United States",
            [2006] = "Italy",
            [2010] = "Canada",
            [2014] = "Russia",
            [2018] = "South Korea",
            [2022] = "China",
        };

        // Slumpa ett år från nycklarna (åren)
        var years = new int[hosts.Count];
        hosts.Keys.CopyTo(years, 0); // Kopiera nycklarna till en array med index 0 som start
        int year = years[Random.Shared.Next(years.Length)]; // Slumpa ett index

        Console.Write($"Which country hosted the Winter Olympics in {year}? ");
        var guess = (Console.ReadLine() ?? "").Trim(); // Läs in och trimma bort blanksteg // Vid null använd tom sträng

        // Enkel skiftlägesokänslig jämförelse
        if (string.Equals(guess, hosts[year], StringComparison.OrdinalIgnoreCase)) // Jämför gissning med rätt svar
            Console.WriteLine("Correct!");
        else
            Console.WriteLine($"Incorrect. It was {hosts[year]}.");
    }
}

