using System;
using System.IO; // För filhantering

namespace RememberPlayerName
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 1) Filnamn där vi sparar spelarens namn // Sparar i en variabel för att enkelt kunna ändra filenamn vid behov
            string playerNamePath = "player-name.txt";

            // 2) Om filen inte finns: välkomna spelaren, fråga efter namn och spara det
            if (!File.Exists(playerNamePath))
            {
                Console.WriteLine("Welcome to your biggest adventure yet!\n");
                Console.WriteLine("What is your name, traveler?");
                Console.Write("> ");
                string name = (Console.ReadLine() ?? "").Trim();

                File.WriteAllText(playerNamePath, name);
                Console.WriteLine($"\nNice to meet you, {name}!");
            }
            // 3) Om filen finns: läs in namnet och hälsa spelaren välkommen tillbaka
            else
            {
                string name = File.ReadAllText(playerNamePath).Trim();

                Console.WriteLine($"Welcome back, {name}, let's continue!");
            }
        }
    }
}


