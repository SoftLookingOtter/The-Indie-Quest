using System;

namespace Tank_project
{
    internal class Program
    {
        static void Main(string[] args)
        {

#if DEBUG
            Console.WriteLine("Debug Mode");
#else
            Console.WriteLine("Release Mode");
#endif

            Random random = new Random();

            Console.WriteLine("What is your name, soldier?");

            string playerName = Console.ReadLine();
            int tankPosition = random.Next(40, 71); // Tanken börjar en bit bort

            while (true)
            {
                Console.Clear();
                Console.WriteLine("DANGER! A tank is approaching our position. Your artillery unit is our only hope!");
                Console.WriteLine();
                Console.WriteLine("Here is the map of the battlefield:");

                // Rita slagfältet
                string battlefield = "_/";

                for (int i = 2; i < 80; i++)
                {
                    if (i == tankPosition)
                        battlefield += "T";
                    else
                        battlefield += "_";
                }

                Console.WriteLine(battlefield);
                Console.WriteLine();

                // Fråga spelaren om avstånd
                Console.WriteLine($"{playerName}, choose a distance to fire at (e.g. 60):");
                string input = Console.ReadLine();

                if (!int.TryParse(input, out int shotDistance) || shotDistance < 0 || shotDistance >= 80)
                {
                    Console.WriteLine("Invalid input. Please enter a number between 0 and 79.");
                    Console.ReadLine();
                    continue;
                }

                // Rita explosionen
                string explosionLine = "";

                for (int i = 0; i < 80; i++)
                {
                    explosionLine += (i == shotDistance) ? "*" : " ";
                }

                Console.WriteLine();
                Console.WriteLine(explosionLine);
                Console.WriteLine();

                // Träff?
                if (shotDistance == tankPosition)
                {
                    Console.WriteLine("Direct hit! The tank is destroyed!");
                    break;
                }
                else if (shotDistance < tankPosition)
                {
                    Console.WriteLine("Too short! The shell exploded before reaching the tank.");
                }
                else
                {
                    Console.WriteLine("Too far! The shell flew past the tank.");
                }
                Console.WriteLine();

                // Flytta tanken framåt
                int advance = random.Next(1, 16); // 1 till 15 steg
                tankPosition -= advance;

                if (tankPosition <= 1)
                {
                    Console.WriteLine("💥 The tank has reached your position. You're out of time!");
                    break;
                }
                Console.WriteLine("Press Enter to prepare the next shot...");
                Console.ReadLine(); // Väntar på spelaren innan nästa runda
            }
            Console.WriteLine();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
