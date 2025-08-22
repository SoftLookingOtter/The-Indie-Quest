using System;
using System.Reflection.Metadata;

class Location
{
    public string Name;
    public string Description;
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

        // Välj aktuell plats (ändra denna rad för att testa olika platser)
        Location currentLocation; // Deklaration utan initiering // Klass + variabelnamn, nytt objekt skapas nedanför

        //currentLocation = winterfell;
        //Welcome(currentLocation);
        //Console.WriteLine("—");

        //currentLocation = kingsLanding;
        //Welcome(currentLocation);
        //Console.WriteLine("—");

        currentLocation = riverrun; // Initiering av currentLocation med riverrun-objektet

        Welcome(currentLocation); // Här anropar/”kallar” vi metoden (skickar in ett argument)

        // currentLocation = är ett argument av typen Location som skickas in i metoden Welcome.
        // Väl inne i metoden Welcome tas detta argument emot som parametern loc.
    }

    static void Welcome(Location loc) // Här skapar/definierar vi metoden

    // static = tillhör klassen, inte ett objekt.
    // void = metoden returnerar inget värde.
    // Welcome = metodens namn.
    // Location = typ av parameter, som är ett objekt av klassen Location.
    // loc = namnet på parametern, som används inom metoden.
    {
        Console.WriteLine($"Welcome to {loc.Name}, {loc.Description}.");
    }
}

