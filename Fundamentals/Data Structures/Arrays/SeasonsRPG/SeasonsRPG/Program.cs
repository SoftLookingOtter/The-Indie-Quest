namespace SeasonsRPG;

using System;

internal enum Season
{
    Spring = 0, Summer = 1, Fall = 2, Winter = 3
}

internal static class Program
{
    // Måste ligga i samma ordning som enumen ovan
    private static readonly string[] Seasons = ["Spring", "Summer", "Fall", "Winter"]; // Readonly för att undvika oavsiktliga ändringar i arrayen

    public static void Main()
    {
        Console.WriteLine(CreateDayDescription(7, Season.Summer, 134));
        Console.WriteLine(CreateDayDescription(41, Season.Winter, 22));
        Console.WriteLine(CreateDayDescription(3, Season.Spring, 1601)); // Testar med olika värden
    }

    // Skapa funktionen som returnerar en sträng med formatet "X day of Y in the year Z"
    public static string CreateDayDescription(int day, Season season, int year)
    {
        return $"{day} day of {Seasons[(int)season]} in the year {year}"; // Enum (int)season = summer = 1, Array seasons[1] = "Summer" // Ordningen i enumen måste matcha ordningen i arrayen
    }
}
