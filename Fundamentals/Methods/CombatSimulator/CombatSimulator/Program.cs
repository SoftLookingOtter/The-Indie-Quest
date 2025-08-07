// Main() 
//│
//├─▶ SimulateCombat(fighters, "orc", ...)
//│     └─ Striden börjar
//│     └─ En hjälte attackerar -> monstret attackerar tillbaka
//│     └─ Loopen fortsätter tills någon vinner
//│
//├─▶ SimulateCombat(fighters, "azer", ...)   ← om någon lever
//│
//└─▶ SimulateCombat(fighters, "troll", ...)  ← om någon fortfarande lever

// Main()
//│
//├─▶ SimulateCombat()
//      │
//      ├─▶ AttackMonster()   ← körs för varje hjälte
//      │
//      └─▶ MonsterAttacks()  ← körs en gång per runda

using System;
using System.Collections.Generic;

namespace CombatSimulator
{
    internal class Program
    {
        static Random random = new Random();

        // Hjältes tur – attackerar monstret
        static void AttackMonster(string fighter, string monsterName, ref int monsterHP)
        {
            int damage = random.Next(1, 7) + random.Next(1, 7); // 2d6
            monsterHP -= damage;
            if (monsterHP < 0) monsterHP = 0;

            Console.WriteLine($"{fighter} hits the {monsterName} for {damage} damage. The {monsterName} has {monsterHP} HP left.");
        }

        // Monstrets tur – attackerar en hjälte
        static void MonsterAttacks(string monsterName, int savingThrowDC, List<string> fighters)
        {
            int targetIndex = random.Next(fighters.Count);
            string target = fighters[targetIndex];
            Console.WriteLine($"The {monsterName} attacks {target}!");

            int roll = random.Next(1, 21); // d20
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

        // Striden – En strid mot ett monster
        static void SimulateCombat(List<string> fighters, string monsterName, int monsterHP, int savingThrowDC)
        {
            if (fighters.Count == 0)
            {
                Console.WriteLine($"All heroes are dead. The {monsterName} is not fought.\n");
                return;
            }

            Console.WriteLine($"Watch out, {monsterName} with {monsterHP} HP appears!");

            while (monsterHP > 0 && fighters.Count > 0)
            {
                foreach (string fighter in new List<string>(fighters))
                {
                    if (monsterHP <= 0 || fighters.Count == 0) break;
                    AttackMonster(fighter, monsterName, ref monsterHP);
                }

                if (monsterHP > 0 && fighters.Count > 0)
                {
                    MonsterAttacks(monsterName, savingThrowDC, fighters);
                }
            }

            if (fighters.Count == 0)
            {
                Console.WriteLine($"The party has failed and the {monsterName} roars in triumph.\n");
            }
            else
            {
                Console.WriteLine($"The {monsterName} collapses and the heroes celebrate their victory!\n");
            }
        }

        // Main – Här körs hela simuleringen och metoderna anropas
        static void Main(string[] args)
        {
            List<string> fighters = new List<string> { "Jazlyn", "Theron", "Dayana", "Rolando" };
            Console.WriteLine($"Fighters {string.Join(", ", fighters)} descend into the dungeon.\n");

            SimulateCombat(fighters, "orc", 15, 10);
            if (fighters.Count > 0) SimulateCombat(fighters, "azer", 39, 18);
            if (fighters.Count > 0) SimulateCombat(fighters, "troll", 84, 16);

            if (fighters.Count > 0)
            {
                Console.WriteLine($"After three grueling battles, the heroes {string.Join(", ", fighters)} return from the dungeons to live another day.");
            }
            else
            {
                Console.WriteLine("All heroes have perished in the dungeons.");
            }
        }
    }
}
