class Program
{
    static int width;
    static int height;
    static char[,] map = null!;
    static int playerX;
    static int playerY;
    static Random random = new Random();

    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.WriteLine("Get ready for: Minotaur's Lair");
        Console.WriteLine();
        Console.WriteLine("Press any key to start ...");

        Console.ReadKey(true);
        Console.Clear();

        width = 39;
        height = 23;

        map = new char[width, height];

        string[] level =
        {
            "#######################################",
            "#                                     #",
            "#                                     #",
            "#                  S                  #",
            "#                  #                  #",
            "#   #########      #      #########   #",
            "#   #       #      #      #       #   #",
            "#   # ###   #   #######   #   ### #   #",
            "#   #   #   #             #   #   #   #",
            "#   ### #   #####     #####   # ###   #",
            "#       #       #     #       #       #",
            "#   #####   ### #     # ###   #####   #",
            "#   #       #   #######   #       #   #",
            "#   #   ### #      M      # ###   #   #",
            "#   #   #   ###############   #   #   #",
            "#   #   #                     #   #   #",
            "#   #   #####   #######   #####   #   #",
            "#   #       #   #     #   #       #   #",
            "#   ####### #   #     #   # #######   #",
            "#         # #   #######   # #         #",
            "#   ##### # #             # # #####   #",
            "#         #                 #         #",
            "#######################################"
        };

        // Fyll kartan först
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                char c = level[y][x];

                if (c == 'S')
                {
                    playerX = x;
                    playerY = y;
                    map[x, y] = ' ';
                }
                else
                {
                    map[x, y] = c;
                }
            }
        }

        // Lägg till träd EFTER att kartan är fylld
        for (int y = 0; y < 3; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (map[x, y] == ' ' && random.Next(4) == 0)
                {
                    map[x, y] = '♠';
                }
            }
        }

        while (true)
        {
            DrawMap();

            var key = Console.ReadKey(true);

            if (key.Key == ConsoleKey.Escape)
                return;

            int newX = playerX;
            int newY = playerY;

            if (key.Key == ConsoleKey.UpArrow)
                newY--;
            else if (key.Key == ConsoleKey.DownArrow)
                newY++;
            else if (key.Key == ConsoleKey.LeftArrow)
                newX--;
            else if (key.Key == ConsoleKey.RightArrow)
                newX++;

            // WIN CONDITION
            if (map[newX, newY] == 'M')
            {
                Console.Clear();
                Console.WriteLine("🏆 You defeated the Minotaur!");
                Console.ReadKey();
                return;
            }

            // vanlig movement
            if (newX >= 0 && newX < width &&
                newY >= 0 && newY < height &&
                map[newX, newY] != '#')
            {
                playerX = newX;
                playerY = newY;
            }
        }
    }

    static void DrawMap()
    {
        Console.SetCursorPosition(0, 0);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (x == playerX && y == playerY)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("☺");
                }
                else
                {
                    char c = map[x, y];

                    if (c == '♠')
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(c);
                    }
                    else if (c == '#')
                    {
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write(c);
                    }
                    else if (c == 'M')
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(c);
                    }
                    else
                    {
                        Console.ResetColor();
                        Console.Write(c);
                    }
                }
            }

            Console.ResetColor();
            Console.WriteLine();
        }
    }
}