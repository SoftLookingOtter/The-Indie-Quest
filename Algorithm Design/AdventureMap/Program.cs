class Program
{
    static int width;
    static int height;
    static char[,] map = null!;
    static int playerX;
    static int playerY;

    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        // 1. Sätt storlek
        width = 5;
        height = 4;

        // 2. Skapa map
        map = new char[width, height];

        // 3. Fyll map (enkel bana)
        string[] level =
        {
            "#####",
            "#S  #",
            "#   #",
            "#####"
        };

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                char c = level[y][x];
                map[x, y] = c;

                // hitta start
                if (c == 'S')
                {
                    playerX = x;
                    playerY = y;
                }
            }
        }

        // 4. Rita kartan
        DrawMap();
    }

    static void DrawMap()
{
    for (int y = 0; y < height; y++)
    {
        for (int x = 0; x < width; x++)
        {
            // spelare
            if (x == playerX && y == playerY)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("☺");
            }
            else
            {
                char c = map[x, y];

                // vägg
                if (c == '#')
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
                else
                {
                    Console.ResetColor();
                }

                Console.Write(c);
            }
        }

        Console.ResetColor();
        Console.WriteLine();
    }
}
}