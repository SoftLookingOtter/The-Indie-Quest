using System;
using System.Collections.Generic;

namespace DiceRollSimulator
{
    internal class Program
    {
        // Slumpgenerator // Skapas en gång – readonly för att förhindra att Random byts ut och skapar upprepade resultat

        private static readonly Random random = new();

        #region Dice Utilities

        /// <summary>
        /// Slår ett angivet antal tärningar och returnerar summan, inklusive bonus.
        /// </summary>
        /// <param name="numberOfRolls">Antal tärningar att slå.</param>
        /// <param name="diceSides">Antal sidor per tärning.</param>
        /// <param name="fixedBonus">Fast bonus som läggs till slutresultatet.</param>
        /// <returns>Summan av slagen och bonus.</returns>
        private static int DiceRoll(int numberOfRolls, int diceSides, int fixedBonus = 0)
        {
            int total = 0;

            for (int i = 0; i < numberOfRolls; i++)
            {
                // random.Next(1, diceSides + 1) → 1 till diceSides (inkl. övre gräns)
                total += random.Next(1, diceSides + 1);
            }

            return total + fixedBonus;
        }

        #endregion

        #region Combat Logic

        /// <summary>
        /// Simulerar en strid mellan hjältar och ett monster.
        /// </summary>
        /// <param name="fighters">Listan med levande hjältar.</param>
        /// <param name="monsterName">Namnet på monstret.</param>
        /// <param name="monsterHP">Monstrets start-HP.</param>
        /// <param name="savingThrowDC">Räddningsslagsvärde för att överleva attack.</param>
        private static void SimulateCombat(
            List<string> fighters,
            string monsterName,
            int monsterHP,
            int savingThrowDC)
        {
            if (fighters.Count == 0)
            {
                Console.WriteLine($"All heroes are dead. The {monsterName} is not fought.\n");
                return;
            }

            Console.WriteLine($"Watch out, {monsterName} with {monsterHP} HP appears!");

            while (monsterHP > 0 && fighters.Count > 0)
            {
                // Hjältarna attackerar
                foreach (string fighter in new List<string>(fighters))
                {
                    int damage = DiceRoll(2, 6); // Greatsword: 2d6 // Anropar DiceRoll-metoden som slår 2 tärningar med 6 sidor och returnerar summan (2d6) // Returnerat värde sparas i variabeln damage

                    monsterHP -= damage;
                    monsterHP = Math.Max(monsterHP, 0); // Hindra negativa HP // Math.Max väljer högsta värdet av monsterHP eller 0

                    Console.WriteLine(
                        $"{fighter} hits the {monsterName} for {damage} damage. " +
                        $"The {monsterName} has {monsterHP} HP left.");

                    if (monsterHP <= 0)
                        break;
                }

                // Monstret attackerar om det fortfarande lever
                if (monsterHP > 0 && fighters.Count > 0)
                {
                    int targetIndex = random.Next(fighters.Count);
                    string target = fighters[targetIndex];

                    Console.WriteLine($"The {monsterName} attacks {target}!");

                    int roll = DiceRoll(1, 20); // Räddningsslag
                    if (roll >= savingThrowDC)
                    {
                        Console.WriteLine($"{target} rolls a {roll} and is saved from the attack.");
                    }
                    else
                    {
                        Console.WriteLine($"{target} rolls a {roll} and fails to be saved. {target} is killed.");
                        fighters.RemoveAt(targetIndex);
                    }
                }
            }

            // Slut på strid – visa resultat
            Console.WriteLine(monsterHP <= 0
                ? $"The {monsterName} collapses and the heroes celebrate their victory!\n"
                : $"The party has failed and the {monsterName} continues to attack unsuspecting adventurers.\n");
        }

        #endregion

        #region Program Start

        private static void Main()
        {
            // Skapa hjältarna
            List<string> fighters = new()
            {
                "Jazlyn", "Theron", "Dayana", "Rolando"
            };

            Console.WriteLine($"Fighters {string.Join(", ", fighters)} descend into the dungeon.\n"); // Join slår ihop alla namn i listan med ", " emellan → "Jazlyn, Theron, ..."

            // Orc: 2d8 + 6 HP, DC 10
            SimulateCombat(fighters, "orc", DiceRoll(2, 8, 6), 10);

            // Azer: 6d8 + 12 HP, DC 18
            if (fighters.Count > 0)
                SimulateCombat(fighters, "azer", DiceRoll(6, 8, 12), 18);

            // Troll: 8d10 + 40 HP, DC 16
            if (fighters.Count > 0)
                SimulateCombat(fighters, "troll", DiceRoll(8, 10, 40), 16);

            // Efter alla strider – visa vilka som överlevde
            Console.WriteLine(fighters.Count > 0
                ? $"After three grueling battles, the heroes {string.Join(", ", fighters)} return from the dungeons to live another day."
                : "All heroes have perished in the dungeons.");
        }

        #endregion
    }
}