namespace CountryCapitalsQuiz;

using System;
using System.Collections.Generic;

internal static class Program
{
    public static void Main()
    {
        // Land → huvudstad
        var capitals = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["Sweden"] = "Stockholm",
            ["Norway"] = "Oslo",
            ["Denmark"] = "Copenhagen",
            ["Finland"] = "Helsinki",
            ["Iceland"] = "Reykjavik",
            ["Germany"] = "Berlin",
            ["France"] = "Paris",
            ["Spain"] = "Madrid",
            ["Italy"] = "Rome",
            ["Portugal"] = "Lisbon",
            ["Poland"] = "Warsaw",
            ["Czech Republic"] = "Prague",
            ["Slovakia"] = "Bratislava",
            ["Slovenia"] = "Ljubljana",
            ["Austria"] = "Vienna",
            ["Hungary"] = "Budapest",
            ["Greece"] = "Athens",
            ["Croatia"] = "Zagreb",
            ["Switzerland"] = "Bern",
            ["Belgium"] = "Brussels",
            ["Ireland"] = "Dublin",
            ["United Kingdom"] = "London",
            ["United States"] = "Washington",
            ["Canada"] = "Ottawa",
            ["Mexico"] = "Mexico City",
            ["Brazil"] = "Brasilia",
            ["Argentina"] = "Buenos Aires",
            ["China"] = "Beijing",
            ["Japan"] = "Tokyo",
            ["South Korea"] = "Seoul",
            ["Turkey"] = "Ankara",
            ["India"] = "New Delhi",
            ["Australia"] = "Canberra",
            ["New Zealand"] = "Wellington",
            ["Russia"] = "Moscow",
            ["Egypt"] = "Cairo",
            ["Iran"] = "Tehran",
            ["Iraq"] = "Baghdad",
            ["Netherlands"] = "Amsterdam"
        };

        // Skapa en indexerbar lista av länder (enligt hint)
        var countries = new List<string>(capitals.Keys);

        // Slumpa ett land
        string country = countries[Random.Shared.Next(countries.Count)];

        // Fråga spelaren (all interaktion på engelska enligt uppgiften)
        Console.Write($"What is the capital of {country}? ");
        string guess = (Console.ReadLine() ?? "").Trim();

        // Jämför skiftlägesokänsligt (räcker för uppgiften)
        string expected = capitals[country]; // Hämta rätt svar
        if (string.Equals(guess, expected, StringComparison.OrdinalIgnoreCase)) // Jämför expected med guess
            Console.WriteLine("Correct!");
        else
            Console.WriteLine($"Incorrect. It is {expected}.");
    }
}