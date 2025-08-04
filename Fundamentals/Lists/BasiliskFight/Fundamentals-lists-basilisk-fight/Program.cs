using System;
using System.Collections.Generic;

namespace Fundamentals_lists_basilisk_fight
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Random random = new Random();

            // Skapa fighter-listan
            List<string> fighters = new List<string>() { "Jazlyn", "Theron", "Dayana", "Rolando" };
            Console.WriteLine($"Fighters {String.Join(", ", fighters)} descend into the dungeon.");

            // Skapa basiliskens HP: 8d8 + 16
            int basiliskHP = 0;
            for (int i = 0; i < 8; i++)
            {
                basiliskHP += random.Next(1, 9); // Slå en 8-sidig tärning
            }
            basiliskHP += 16;
            Console.WriteLine($"A basilisk with {basiliskHP} HP appears!");

            // Låt striden fortsätta så länge basilisk lever och hjältar finns kvar
            while (basiliskHP > 0 && fighters.Count > 0)
            {
                // Hjältarnas attacker (med dolkar: 1d4)
                foreach (string fighter in new List<string>(fighters)) // Kopiera lista så att vi kan ta bort under loopen
                {
                    if (basiliskHP <= 0 || fighters.Count == 0) break; // Avbryt om basilisk dör eller inga hjältar kvar

                    int damage = random.Next(1, 5); // 1d4 skada med dolk
                    basiliskHP -= damage;
                    if (basiliskHP < 0) basiliskHP = 0;

                    Console.WriteLine($"{fighter} hits the basilisk for {damage} damage. Basilisk has {basiliskHP} HP left.");
                }

                // Om basilisk fortfarande lever – använd petrifying gaze
                if (basiliskHP > 0 && fighters.Count > 0)
                {
                    // Välj slumpmässig hjälte att attackera
                    int targetIndex = random.Next(fighters.Count);
                    string target = fighters[targetIndex];
                    Console.WriteLine($"The basilisk uses petrifying gaze on {target}!");

                    // Räddningsslag (d20 + 3)
                    int roll = random.Next(1, 21); // 1–20
                    int total = roll + 3;
                    if (total >= 12)
                    {
                        Console.WriteLine($"{target} rolls a {roll} and is saved from the attack.");
                    }
                    else
                    {
                        Console.WriteLine($"{target} rolls a {roll} and fails to be saved. {target} is turned into stone.");
                        fighters.Remove(target); // Ta bort hjälten som förstenats
                    }
                }
            }

            // Avgör slutet
            if (fighters.Count == 0)
            {
                Console.WriteLine("The party has failed and the basilisk continues to turn unsuspecting adventurers to stone.");
            }
            else
            {
                Console.WriteLine("The basilisk collapses and the heroes celebrate their victory!");
            }
        }
    }
}