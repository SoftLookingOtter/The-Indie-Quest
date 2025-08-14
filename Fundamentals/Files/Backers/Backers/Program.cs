using System;
using System.IO; // För filhantering (Exists, ReadAllText, ReadAllLines, WriteAllText)

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

            // Ladda alla backers (en per rad)
            string[] backers = File.ReadAllLines(backersPath);

            // Jämför spelarens namn mot varje rad (case-insensitive, ignorerar tomma rader och extra mellanslag)
            bool isBacker = false;
            foreach (var line in backers)
            {
                string candidate = (line ?? "").Trim();

                if (candidate.Length == 0) continue;

                if (string.Equals(candidate, playerName, StringComparison.OrdinalIgnoreCase))
                {
                    isBacker = true;
                    break;
                }
            }

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

