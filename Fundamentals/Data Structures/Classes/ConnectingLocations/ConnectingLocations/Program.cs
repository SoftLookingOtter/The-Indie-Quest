using System;
using System.Collections.Generic; // Behövs för listor

namespace ConnectingLocations
{
    class Location
    {
        public string Name;
        public string Description;
        public List<Location> Neighbors = new List<Location>(); // List<T> är en generisk lista; här är T = Location, alltså en lista som innehåller Location-objekt. // Neighbors – fältets namn. Varje Location-instans får sin egen lista med grannar.

        //VÄNSTER SIDA (DEKLARATION)

        //public – åtkomst(synlig utanför klassen).

        //List<Location> – typen: en generisk lista som kan innehålla Location-objekt.

        //Neighbors – fältets namn.

        //→ Du talar om att det finns ett fält som ska hålla en referens till en lista.

        //HÖGER SIDA (INITIERING)

        //new List<Location>() – skapar en ny TOM lista i minnet(anropar listans parameterlösa konstruktor).
        //→ Utvärderas till en referens till den nya listan.

        // new List<Location>() → ny tom lista (ingen kopia).
        // new List<Location>(någonLista) → kopia av elementen (ytlig kopia).
    }

    class Program
    {
        static void Main()
        {
            var winterfell = new Location { Name = "Winterfell", Description = "the capital of the Kingdom of the North" };

            var pyke = new Location { Name = "Pyke", Description = "the stronghold and seat of House Greyjoy" };

            var riverrun = new Location { Name = "Riverrun", Description = "a large castle located in the central-western part of the Riverlands" };

            var theTrident = new Location { Name = "The Trident", Description = "one of the largest and most well-known rivers on the continent of Westeros" };

            var kingsLanding = new Location { Name = "King's Landing", Description = "the capital, and largest city, of the Seven Kingdoms" };

            var highgarden = new Location { Name = "Highgarden", Description = "the seat of House Tyrell and the regional capital of the Reach" };

            ConnectLocations(pyke, winterfell);
            ConnectLocations(winterfell, theTrident);
            ConnectLocations(pyke, riverrun);
            ConnectLocations(riverrun, theTrident);
            ConnectLocations(riverrun, kingsLanding);
            ConnectLocations(riverrun, highgarden);
            ConnectLocations(kingsLanding, highgarden);
            ConnectLocations(theTrident, kingsLanding);

            var currentLocation = riverrun;

            Welcome(currentLocation);
            ShowDestinations(currentLocation);
        }

        static void Welcome(Location loc)
        {
            Console.WriteLine($"Welcome to {loc.Name}, {loc.Description}.");
            Console.WriteLine();
        }

        static void ConnectLocations(Location a, Location b)
        {
            if (!a.Neighbors.Contains(b))
                a.Neighbors.Add(b);
            if (!b.Neighbors.Contains(a))
                b.Neighbors.Add(a);
        }

        static void ShowDestinations(Location from)
        {
            Console.WriteLine("Possible destinations are:");
            int i = 1;
            foreach (var n in from.Neighbors)
                Console.WriteLine($"{i++}. {n.Name}");
        }
    }
}
