using System;
using System.Collections.Generic; // Behövs för List<T> / Dictionary<,>
using System.Linq; // Behövs för OrderBy, First, Find, etc.

namespace ConnectingLocations
{
    // ========== DATASTRUKTURER ==========

    class Location
    {
        public string Name;
        public string Description;
        // Skapar en tom lista för varje Location-instans
        public List<Neighbor> Neighbors = new();

        // Kortaste vägar från denna plats (fylls av Dijkstra)
        public List<Path> ShortestPaths = new();
    }

    /*
    Neighbor = alla DIREKTA grannar.

    Path = en post per DESTINATION (alla noder), där varje post är den KORTASTE vägen från aktuell källa till just den destinationen. Inte bara “närmsta grannen”,  kan gå via flera noder. */

    // Granne + avstånd
    class Neighbor
    {
        public Location Destination; // Platsen på andra sidan kopplingen (grannen som denna kant leder till).
        public int Distance;         // kantens längd (vikt) – “kostnaden” för att gå denna kant.
    }

    // Kortaste väg-info
    class Path
    {
        public Location Destination;                 // destination (slutplatsen för vägen)
        public int Distance;                         // totaldistans från källan till Destination
        public List<string> StopNames = new(); // lista som innehåller alla mellan-stopp (utan start & mål)
    }

    class Program
    {
        static void Main()
        {
            // ---------- Skapa platser ----------
            var winterfell = new Location { Name = "Winterfell", Description = "the capital of the Kingdom of the North" };
            var pyke = new Location { Name = "Pyke", Description = "the stronghold and seat of House Greyjoy" };
            var riverrun = new Location { Name = "Riverrun", Description = "a large castle located in the central-western part of the Riverlands" };
            var theTrident = new Location { Name = "The Trident", Description = "one of the largest and most well-known rivers on the continent of Westeros" };
            var kingsLanding = new Location { Name = "King's Landing", Description = "the capital, and largest city, of the Seven Kingdoms" };
            var highgarden = new Location { Name = "Highgarden", Description = "the seat of House Tyrell and the regional capital of the Reach" };

            // Samla grafen (behövs för Dijkstra)
            var graph = new List<Location> { winterfell, pyke, riverrun, theTrident, kingsLanding, highgarden }; // Utan en komplett lista över noderna kan algoritmen varken initiera dist/prev korrekt eller besöka alla noder.

            // ---------- Koppla med avstånd (dubbelriktat: a↔b) ----------

            // Varje rad anropar: ConnectLocations(a, b, distance)
            //   a = 1:a argumentet, b = 2:a argumentet, distance = 3:e argumentet.
            // ConnectLocations gör sedan a→b och b→a (dubbelriktad kant).

            ConnectLocations(pyke, winterfell, 18); // a=pyke, b=winterfell, distance=18
            ConnectLocations(winterfell, theTrident, 10); // Hade kunnat skrivits med samma betydelse som -> 
                                                          // ConnectLocations(a: winterfell, b: theTrident, distance: 10);

            ConnectLocations(pyke, riverrun, 3);
            ConnectLocations(riverrun, theTrident, 2);
            ConnectLocations(riverrun, kingsLanding, 7);
            ConnectLocations(riverrun, highgarden, 10);
            ConnectLocations(kingsLanding, highgarden, 8);
            ConnectLocations(theTrident, kingsLanding, 5);


            // ---------- Start ----------
            var currentLocation = winterfell;
            Welcome(currentLocation); // Välkomstmeddelande-metoden anropas

            // Beräkna kortaste vägar för alla källor (fyller ShortestPaths-listor)
            foreach (var src in graph)
                Dijkstra(graph: graph, source: src); // Kör Dijkstrametoden nedan för varje källa i grafen

            // Kör metoden för att visa kortaste vägar från startplatsen anropas
            ShowAllDestinations(currentLocation);

            // ================== Rese-loop (via kortaste vägar) ==================
            while (true)
            {
                Console.WriteLine();
                Console.Write($"Where do you want to travel? (1..{currentLocation.ShortestPaths.Count}, or q to quit)\n> "); // currentLocation.ShortestPaths.Count ger antalet destinationer från nuvarande plats = alltid lika med graph.Count - 1 pga Dijkstra-metodens uppbyggnad av ShortestPaths
                var input = Console.ReadLine();

                // 'q' = avsluta
                if (string.Equals(input, "q", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Goodbye!");
                    break; // Avsluta programmet
                }

                // Försök tolka sifferval (1-baserat)
                if (!int.TryParse(input, out int choice))
                {
                    Console.WriteLine("Please enter a number (or 'q' to quit).");
                    continue; // hoppa till nästa varv
                }

                // Validera att 'choice' ligger mellan [1, currentLocation.ShortestPaths.Count] (1-baserad meny)
                if (choice < 1 || choice > currentLocation.ShortestPaths.Count)
                {
                    Console.WriteLine("That destination does not exist here.");
                    continue; // hoppa till nästa varv
                }

                // Byt plats till vald Destination (index i listan är 0-baserat → därför choice - 1)
                currentLocation = currentLocation.ShortestPaths[choice - 1].Destination;

                Console.WriteLine(); // Ny tom rad för bättre läsbarhet
                Welcome(currentLocation); // Välkomstmeddelande för den nya platsen
                ShowAllDestinations(currentLocation); // Visa alla destinationer från den nya platsen
            }
        }

        // ========== UTSKRIFT ==========

        static void Welcome(Location loc) // Metod för att skriva ut välkomstmeddelande
        {
            Console.WriteLine($"Welcome to {loc.Name}, {loc.Description}.");
            Console.WriteLine();
        }

        static void ShowAllDestinations(Location from) // Metod för att visa alla destinationer från en given plats + via mellanstopp
        {
            Console.WriteLine("Possible destinations are:");
            int i = 1; // Räknare för att numrera destinationerna
            foreach (var p in from.ShortestPaths) // Iterera över alla kortaste vägar från nuvarande plats
            {
                if (p.StopNames.Count == 0) // Om inga mellanstopp finns
                    Console.WriteLine($"{i++}.  {p.Destination.Name} ({p.Distance})"); // Skriv ut destinationens nummer, namn och avstånd
                else
                    Console.WriteLine($"{i++}. {p.Destination.Name} ({p.Distance} via {string.Join(", ", p.StopNames)})"); // Skriv ut destinationens nummer, namn, avstånd och mellanstopp
            }
            Console.WriteLine(); // Ny tom rad för bättre läsbarhet
        }

        // ========== GRAF-KOPPLING ==========

        // Skapar en dubbelriktad koppling mellan a och b genom att lägga in två direkta kanter: a→b och b→a.
        // Parametrarna är: plats a, plats b, avståndet mellan dem.
        static void ConnectLocations(Location a, Location b, int distance)
        {
            AddOrUpdateNeighbor(a, b, distance); // här blir: from = a, to = b, distance = distance 
                                                 // kan också med samma betydelse  skrivas som:
                                                 // AddOrUpdateNeighbor(from: winterfell, to: theTrident, distance: 10);
            AddOrUpdateNeighbor(b, a, distance); // här blir: from = b, to = a distance = distance

            // ...om kanten redan finns uppdateras vikten
        }

        // Säkerställ att det finns en (och bara en) kant from→to med vikten 'distance'.
        // Finns kanten redan uppdateras vikten; annars skapas den.
        static void AddOrUpdateNeighbor(Location from, Location to, int distance)
        {
            // Parametrar:
            // from = startnoden där kanten utgår.
            // to = noden som är målet för kanten.
            // distance = vikten (längden) på kanten.


            // Hitta befintlig kant from→to (matchar på EXAKT samma Location-instans, dvs Location-objektet måste vara samma referens)
            var existing = from.Neighbors.Find(n => ReferenceEquals(n.Destination, to));

            // from.Neighbors är en List<Neighbor>: alla utgående kanter från from.

            if (existing is null)
            {
                // Ingen sådan kant: lägg till ny
                from.Neighbors.Add(new Neighbor { Destination = to, Distance = distance });
            }
            else
            {
                // Kanten fanns redan: uppdatera vikten
                existing.Distance = distance;
            }
        }



        // ========== DIJKSTRA ==========
        // Fyller 'source.ShortestPaths' med kortaste vägar till alla andra noder i grafen.
        static void Dijkstra(List<Location> graph, Location source) // List<Location> graph = alla noder i grafen, Location source = startplatsen

        // graph är samma lista varje gång.
        //source(= src) är just den platsen för varvet.
        //Dijkstra fyller då source.ShortestPaths för den platsen.

        {
            var dist = new Dictionary<Location, int>();        // min-känd distans från source till varje nod // Dictionary<,> är en generisk samling som lagrar nyckel-värde-par. 
            var prev = new Dictionary<Location, Location>();   // Sparar vilken nod vi kom ifrån längs den kortaste vägen till varje nod, baklänges spårning för att kunna bygga upp vägen senare.
            var Q = new List<Location>(graph);                 // noder kvar att besöka

            // Init
            foreach (var v in graph)
            {
                dist[v] = int.MaxValue; // "oändligt"
                prev[v] = null;
            }
            dist[source] = 0;

            // Huvudloop
            while (Q.Count > 0)
            {
                // plocka nod med minsta dist
                var u = Q.OrderBy(v => dist[v]).First();
                Q.Remove(u);

                if (dist[u] == int.MaxValue)
                    break; // resten oåtkomligt

                // relaxera kanter u -> v
                foreach (var edge in u.Neighbors)
                {
                    var v = edge.Destination;           // ← konsekvent: Neighbor.Destination
                    var alt = dist[u] + edge.Distance;  // kandidatdistans via u
                    if (alt < dist[v])
                    {
                        dist[v] = alt;
                        prev[v] = u;
                    }
                }
            }

            // Bygg Path-objekt för source → alla andra
            source.ShortestPaths.Clear();
            foreach (var other in graph)
            {
                if (other == source)
                    continue;

                var path = new Path
                {
                    Destination = other,
                    Distance = dist[other]
                };

                // Backtracka prev-kedjan för att få mellanstopp (utan source & dest)
                var stops = new List<string>();
                var stop = prev[other];
                while (stop != null && stop != source)
                {
                    stops.Add(stop.Name);
                    stop = prev[stop];
                }
                stops.Reverse();          // rätt ordning (source → destination)
                path.StopNames = stops;

                source.ShortestPaths.Add(path);
            }

            // Sortera efter totaldistans (snyggare meny)
            source.ShortestPaths.Sort((a, b) => a.Distance.CompareTo(b.Distance));
        }
    }
}

