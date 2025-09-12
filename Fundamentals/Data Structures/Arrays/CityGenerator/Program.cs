enum Direction // Skapar en egen byggsten ( till skillnad från int, string, bool som är inbyggda )
{
    Right = 0,
    Down = 1,
    Left = 2,
    Up = 3
}

class Program
{
    static void Main()
    {
        // 1) Kartans storlek
        int width = 80;
        int height = 25;

        // 2) Kartan: false = ingen väg, true = väg (standardvärde är false) // 2-dimensionell array
        bool[,] potatoes = new bool[width, height];

        // 3) Generera några KORSNINGAR från slumpade startpunkter (Part 2)
        Random potatoRng = new();
        int numberOfIntersections = 3;

        for (int i = 0; i < numberOfIntersections; i++)
        {
            int xPotato = potatoRng.Next(width);   // 0..width-1 // Slumpa en x-position
            int yPotato = potatoRng.Next(height);  // 0..height-1 // Slumpa en y-position
            GenerateIntersection(potatoes, xPotato, yPotato, potatoRng); // Skapa en korsning vid (xPotato, yPotato)
        }

        // 4) Rita kartan i konsolen
        Draw(potatoes);
    }

    /// <summary>
    /// Skapar en korsning vid (xPotato, yPotato). Varje riktning (höger/ner/vänster/upp)
    /// har 70% chans att genereras. Använder GenerateRoad för själva vägen.
    /// Garanterar minst en väg om alla fyra misslyckas.
    /// </summary>
    static void GenerateIntersection(bool[,] potatoes, int xPotato, int yPotato, Random potatoRng)
    {
        potatoes[xPotato, yPotato] = true; // markera korsningsrutan

        const double chancePerDirection = 0.70;
        int generatedCount = 0;

        if (potatoRng.NextDouble() < chancePerDirection)
        {
            GenerateRoad(potatoes, xPotato, yPotato, Direction.Right);
            generatedCount++;
        }
        if (potatoRng.NextDouble() < chancePerDirection)
        {
            GenerateRoad(potatoes, xPotato, yPotato, Direction.Down);
            generatedCount++;
        }
        if (potatoRng.NextDouble() < chancePerDirection)
        {
            GenerateRoad(potatoes, xPotato, yPotato, Direction.Left);
            generatedCount++;
        }
        if (potatoRng.NextDouble() < chancePerDirection)
        {
            GenerateRoad(potatoes, xPotato, yPotato, Direction.Up);
            generatedCount++;
        }

        // Säkra minst en väg från korsningen
        if (generatedCount == 0)
        {
            Direction fallback = (Direction)potatoRng.Next(4); // 0=Right,1=Down,2=Left,3=Up
            GenerateRoad(potatoes, xPotato, yPotato, fallback);
        }
    }

    /// <summary>
    /// Lägger ut en rak väg från (xPotato,yPotato) i vald riktning tills kartkanten nås.
    /// </summary>
    static void GenerateRoad(bool[,] potatoes, int xPotato, int yPotato, Direction potato)
    {
        int mapWidth = potatoes.GetLength(0); // första dimensionen = x/kolumn/width // Blir 80
        int mapHeight = potatoes.GetLength(1); // andra dimensionen  = y/rad/height   // Blir 25

        // Översätt riktning till ett steg (dx, dy)
        (int dx, int dy) = GetStepFromDirection(potato);

        int x = xPotato; // Sätt startposition till xPotato
        int y = yPotato; // Sätt startposition till yPotato

        // Sätt true och stega tills vi lämnar kartan
        while (x >= 0 && x < mapWidth && y >= 0 && y < mapHeight)
        {
            potatoes[x, y] = true; // Ändra x och y baserat på värdet av dx och dy som vi får från GetStepFromDirection-metoden
            x += dx;
            y += dy;
        }
    }

    /// <summary>
    /// Returnerar steget (dx,dy) för en given riktning.
    /// </summary>
    static (int dx, int dy) GetStepFromDirection(Direction anotherPotato) // En switch-sats som returnerar olika värden beroende på vad anotherPotato är // Här skickar vi in en enum och returnerar en tuple
        => anotherPotato switch
        {
            Direction.Right => (1, 0),
            Direction.Down => (0, 1),
            Direction.Left => (-1, 0),
            Direction.Up => (0, -1),
            _ => (0, 0),
        };

    /// <summary>
    /// Skriver ut # för väg, annars mellanslag. Yttre loop = rader (y), inre loop = kolumner (x).
    /// </summary>
    static void Draw(bool[,] potatoes) // funktion som ritar ut kartan i konsolen // returnerar inget (void) (dvs inget värde) // tar in vår 2D-array av bools som parameter (potatoes)
    {
        int mapWidth = potatoes.GetLength(0); // första dimensionen = x/kolumn/width // Blir 80
        int mapHeight = potatoes.GetLength(1); // andra dimensionen  = y/rad/height   // Blir 25

        for (int y = 0; y < mapHeight; y++) // För varje rad (y)
        {
            for (int x = 0; x < mapWidth; x++) // Loopar genom varje kolumn (x)
            {
                Console.Write(potatoes[x, y] ? '#' : ' '); // Om potatoes[x,y] är true (dvs det finns en väg) skriv ut '#' annars skriv ut ett mellanslag ' '
            }
            Console.WriteLine(); // Gå till nästa rad efter att ha skrivit ut alla kolumner i den aktuella raden
        }
    }
}
