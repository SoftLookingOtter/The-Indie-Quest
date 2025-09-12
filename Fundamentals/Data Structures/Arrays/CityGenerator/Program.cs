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
        bool[,] roads = new bool[width, height];

        // 3) Generera några vägar från slumpade startpunkter och riktningar
        Random random = new Random();
        int numberOfRoads = 4;

        for (int i = 0; i < numberOfRoads; i++)
        {
            int startX = random.Next(width);   // 0..width-1
            int startY = random.Next(height);  // 0..height-1
            Direction dir = (Direction)random.Next(4); // 0=Right,1=Down,2=Left,3=Up

            GenerateRoad(roads, startX, startY, dir);
        }

        // 4) Rita kartan i konsolen
        Draw(roads);
    }

    /// <summary>
    /// Lägger ut en rak väg från (startX,startY) i vald riktning tills kartkanten nås.
    /// </summary>
    static void GenerateRoad(bool[,] bananas, int xbanana, int ybanana, Direction potato)
    {
        int mapWidth = bananas.GetLength(0); // första dimensionen = x/kolumn/width // Blir 80
        int mapHeight = bananas.GetLength(1); // andra dimensionen  = y/rad/height // Blir 25

        // Översätt riktning till ett steg (dx, dy)
        (int dx, int dy) = GetStepFromDirection(potato);

        int x = xbanana; // Sätt startposition till xbanana
        int y = ybanana; // Sätt startposition till ybanana

        // Sätt true och stega tills vi lämnar kartan
        while (x >= 0 && x < mapWidth && y >= 0 && y < mapHeight)
        {
            bananas[x, y] = true; // Ändra x och y baserat på värdet av dx och dy som vi får från GetStepFromDirection-metoden
            x += dx;
            y += dy;
        }
    }

    /// <summary>
    /// Returnerar steget (dx,dy) för en given riktning.
    /// </summary>
    static (int dx, int dy) GetStepFromDirection(Direction anotherPotato) // En switch-sats som returnerar olika värden beroende på vad anotherPotato är // Här skickar vi in en enum och returnerar en tuple // Vi skickar i detta fall in Direction dir som vi skapade i Main
    {
        switch (anotherPotato)
        {
            case Direction.Right:
                return (1, 0);
            case Direction.Down:
                return (0, 1);
            case Direction.Left:
                return (-1, 0);
            case Direction.Up:
                return (0, -1);
            default:
                return (0, 0);
        }
    }

    /// <summary>
    /// Skriver ut # för väg, annars mellanslag. Yttre loop = rader (y), inre loop = kolumner (x).
    /// </summary>
    static void Draw(bool[,] roads) // funktion som ritar ut kartan i konsolen // returnerar inget (void) (dvs inget värde) // tar in vår 2D-array av bools som parameter (roads)
    {
        int mapWidth = roads.GetLength(0); // första dimensionen = x/kolumn/width // Blir 80
        int mapHeight = roads.GetLength(1); // andra dimensionen  = y/rad/height // Blir 25

        for (int y = 0; y < mapHeight; y++) // För varje rad (y)
        {
            for (int x = 0; x < mapWidth; x++) // Loopar genom varje kolumn (x)
            {
                Console.Write(roads[x, y] ? '#' : ' '); // Om roads[x,y] är true (dvs det finns en väg) skriv ut '#' annars skriv ut ett mellanslag ' '
            }
            Console.WriteLine(); // Gå till nästa rad efter att ha skrivit ut alla kolumner i den aktuella raden
        }
    }
}
