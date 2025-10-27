class Program
{
    static void Main()
    {
        // Rita "fraktalen" enligt pseudokoden
        for (int y = -10; y <= 10; y++)
        {
            for (int x = 1; x <= 80; x++)
            {
                double r = 0.0;   // realdel
                double i = 0.0;   // imaginärdel
                int k = -1;       // iterationer

                // WHILE r² + i² < 4 AND k < 112
                while ((r * r + i * i) < 4 && k < 112)
                {
                    double t = r;
                    r = (t * t) - (i * i) - 2.3 + (x / 24.5);
                    i = (2 * t * i) + (y / 8.5);
                    k++; // INCREMENT k
                }

                int c = k % 16; // MOD i C# är %

                Console.BackgroundColor = (ConsoleColor)c; // typ-cast till enum
                Console.Write(' '); // "SEND ' ' TO DISPLAY"
            }

            Console.ResetColor();
            Console.WriteLine(); // ny rad
        }

        Console.ResetColor();
    }
}
