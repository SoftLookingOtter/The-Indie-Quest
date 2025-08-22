using System;
using System.Collections.Generic;
// using System.Reflection.Metadata; // onödig här, kan tas bort

class Location
{
    public string Name;
    public string Description;

    // Nytt fält för Part 2: grannar / närliggande destinationer
    public List<Location> Neighbors = new List<Location>();
}

class Program
{
    static void Main()
    {
        // Skapa plats-objekt
        var winterfell = new Location { Name = "Winterfell", Description = "the capital of the Kingdom of the North" };
        var pyke = new Location { Name = "Pyke", Description = "the stronghold and seat of House Greyjoy" };
        var riverrun = new Location { Name = "Riverrun", Description = "a large castle located in the central-western part of the Riverlands" };
        var theTrident = new Location { Name = "The Trident", Description = "one of the largest and most well-known rivers on the continent of Westeros" };
        var kingsLanding = new Location { Name = "King's Landing", Description = "the capital, and largest city, of the Seven Kingdoms" };
        var highgarden = new Location { Name = "Highgarden", Description = "the seat of House Tyrell and the regional capital of the Reach" };

        // Koppla platser enligt kartan (dubbelriktat)
        ConnectLocations(pyke, winterfell);
        ConnectLocations(winterfell, theTrident);
        ConnectLocations(pyke, riverrun);
        ConnectLocations(riverrun, theTrident);
        ConnectLocations(riverrun, kingsLanding);
        ConnectLocations(riverrun, highgarden);
        ConnectLocations(kingsLanding, highgarden);
        ConnectLocations(theTrident, kingsLanding);

        // Välj aktuell plats (ändra denna rad för att testa olika platser)
        Location currentLocation; // Deklaration utan initiering // Klass + variabelnamn, nytt "värde" sätts nedanför

        // currentLocation = winterfell;
        // Welcome(currentLocation);
        // Console.WriteLine("—");

        // currentLocation = kingsLanding;
        // Welcome(currentLocation);
        // Console.WriteLine("—");

        currentLocation = riverrun; // Initiering av currentLocation med riverrun-objektet

        Welcome(currentLocation);        // Här anropar/”kallar” vi metoden (skickar in ett argument)
        ShowDestinations(currentLocation); // Lista möjliga destinationer från nuvarande plats

        // currentLocation är ARGUMENT av typen Location som skickas in i Welcome/ShowDestinations.
        // Inne i respektive metod tas argumentet emot som PARAMETERN 'loc' / 'from'.
    }

    static void Welcome(Location loc) // Här skapar/definierar vi metoden
    // static = tillhör klassen, inte ett objekt.
    // void = metoden returnerar inget värde.
    // Welcome = metodens namn.
    // Location = typ av parameter, som är ett objekt av klassen Location.
    // loc = namnet på parametern, som används inom metoden.
    {
        Console.WriteLine($"Welcome to {loc.Name}, {loc.Description}.");
        Console.WriteLine();
    }

    // Ny metod: kopplar a och b som grannar åt båda håll (a↔b)
    static void ConnectLocations(Location a, Location b)
    {
        if (!a.Neighbors.Contains(b)) a.Neighbors.Add(b);
        if (!b.Neighbors.Contains(a)) b.Neighbors.Add(a);
    }

    // Ny metod: skriver möjliga destinationer från en plats
    static void ShowDestinations(Location from)
    {
        Console.WriteLine("Possible destinations are:");
        int i = 1;
        foreach (var n in from.Neighbors)
            Console.WriteLine($"{i++}. {n.Name}");
    }
}
