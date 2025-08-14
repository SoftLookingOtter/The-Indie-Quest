using System;
using System.IO;   // Exists, ReadAllText, ReadAllLines, WriteAllText
using System.Linq; // LINQ: Select, Where, Any

namespace Backers
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // === Mission 1: Kom ihåg spelarens namn ===

            // 1) Filnamn där vi sparar spelarens namn
            string playerNamePath = "player-name.txt";

            string playerName;

            // 2) Om filen inte finns: välkomna spelaren, fråga efter namn och spara det
            if (!File.Exists(playerNamePath))
            {
                Console.WriteLine("Welcome to your biggest adventure yet!\n");
                Console.WriteLine("What is your name, traveler?");
                Console.Write("> ");

                playerName = (Console.ReadLine() ?? "").Trim();
                File.WriteAllText(playerNamePath, playerName);

                Console.WriteLine($"\nNice to meet you, {playerName}!");
            }
            // 3) Om filen finns: läs in namnet och hälsa spelaren välkommen tillbaka
            else
            {
                playerName = (File.ReadAllText(playerNamePath) ?? "").Trim();
                Console.WriteLine($"Welcome back, {playerName}, let's continue!\n");
            }

            // === Mission 2: Kolla om spelaren är en Kickstarter-backer ===

            // Fil för backers-listan (läggs till i projektet med Build Action: Content och Copy if newer)
            string backersPath = "backers.txt";

            if (!File.Exists(backersPath))
            {
                Console.WriteLine("Backers file not found. Please add 'backers.txt' to the project (Content, Copy if newer).");
                return;
            }

            // LINQ: läs alla rader, trimma, filtrera bort tomma, och kolla om någon matchar spelarens namn (case-insensitive)
            bool isBacker = File.ReadAllLines(backersPath)
                                .Select(line => (line ?? "").Trim()) // Select (...) - En LINQ-metod som returnerar en trimmad sträng eller " " vid null
                                .Where(s => s.Length > 0) // Where(...) – En LINQ-metod som filtrerar en följd. Den behåller bara de element där villkoret är sant. //Antal tecken i strängen s > 0
                                .Any(s => string.Equals(s, playerName, StringComparison.OrdinalIgnoreCase)); // Any(...) - LINQ-metod som jämför om någon sträng i listan matchar spelarens namn, oavsett versaler eller gemener. 

            //Samma sak

            /* bool isBacker = File.ReadAllLines(backersPath)
            .Select(line => (line ?? "").Trim())
            .Where(backer => backer.Length > 0)
            .Any(backer => string.Equals(backer, playerName, StringComparison.OrdinalIgnoreCase));
            */

            // Skriv ut resultatet
            if (isBacker)
    {
        Console.WriteLine("You successfully enter Dr. Fred's secret laboratory and are greeted with a warm welcome for backing the game's Kickstarter!");
    }
    else
    {
        Console.WriteLine("Unfortunately I cannot let you into Dr. Fred's secret laboratory.");
    }
}
}
}

