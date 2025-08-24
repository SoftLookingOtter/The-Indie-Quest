using System;
using System.Collections.Generic; // Behövs för List<T> / Dictionary<,>

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
                    Console.WriteLine($"{i++}. {p.Destination.Name} ({p.Distance})"); // Skriv ut destinationens nummer, namn och avstånd
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
            var dist = new Dictionary<Location, int>();        // kortaste kända avstånd från 'source' till varje nod // Dictionary<,> är en generisk samling som lagrar nyckel-värde-par.                
            var prev = new Dictionary<Location, Location>();   // föregångare på bästa vägen (för backtracking)

            var Q = new List<Location>(graph);                 // kandidater att finalisera (börjar med ALLA noder, inkl. 'source')

            // Loopa igenom alla noder
            foreach (var v in graph)
            {
                dist[v] = int.MaxValue; // och sätt distansen som "oändlig"
                prev[v] = null; // föregångare ännu okänd
            }
            dist[source] = 0; // distansen till källan är 0

            // HUVUDLOOP // algoritmens kärna // VÄLJ ALLTID NODEN MED LÄGSTA DIST (SOURCE FÖRST), FINALISERA DEN, RELAXERA DESS GRANNAR, UPPREPA.

            /*

            1. VÄLJ U = NODEN MED LÄGSTA DIST I Q

            2. OM U ÄR NULL ELLER MINSTA DIST = ∞ → BRYT (RESTEN OÅTKOMLIGT)

            3. TA BORT U UR Q (FINALISERA U)

            4. RELAXERA U:S DIREKTA KANTER: för varje kant U→V

                - ALT = DIST[U] + EDGE.DISTANCE

                - OM ALT < DIST[V] → UPPDATERA DIST[V] OCH PREV[V] = U

            5. UPPREPA TILLS Q ÄR TOMT ELLER VI BRYTER

            */

            while (Q.Count > 0) // Första varvet väljer den startnoden (source) // så länge det finns kandidater kvar
            {
                Location u = null; // kandidatnod vi ska välja (ingen vald ännu)
                int best = int.MaxValue; // "bästa distansen" = oändligt som start
                foreach (var v in Q) // loopa igenom alla ofinaliserade noder FÖR ATT HITTA NODEN MED MINSTA DISTANS
                {
                    int d = dist[v];  // nuvarande kända avstånd från 'source' till v
                    if (d < best) // om vi hittat en kortare distans
                    {
                        best = d; // uppdatera "bästa distansen"
                        u = v; // och välj denna nod som kandidat

                        // NU ÄR U = NODEN MED LÄGSTA DIST I Q

                    }
                }

                // om minsta dist fortfarande är "oändlig" är resten oåtkomligt
                if (u == null || best == int.MaxValue)
                    break;

                Q.Remove(u); // // HÄR FINALISERAS 'u' (ta bort från kandidater) för den har nu kortaste distansen från source

                // RELAXERA U:S DIREKTA KANTER (GÅ IGENOM ALLA GRANNAR V TILL U)
                foreach (var edge in u.Neighbors)
                {
                    var v = edge.Destination;   // granne till u
                    var alt = dist[u] + edge.Distance;  // kandidatdistans via u

                    if (alt < dist[v]) // bättre än nuvarande känd distans?
                    {
                        dist[v] = alt; // uppdatera kortaste kända distans
                        prev[v] = u; // uppdatera föregångare (för backtracking)
                    }
                }
            }

            // Bygg Path-objekt för source → alla andra
            source.ShortestPaths.Clear(); // töm tidigare resultat (om det finns)

            foreach (var other in graph) // gå igenom alla noder i grafen
            {
                if (other == source)
                    continue; // hoppa över source (vi vill inte ha en väg till sig själv)

                var path = new Path // skapa ett nytt Path-objekt
                {
                    Destination = other, // destinationen är den andra noden
                    Distance = dist[other] // totaldistansen från source till other
                };

                // Backtracka prev-kedjan för att få mellanstopp (utan source & dest)
                var stops = new List<string>(); // tom lista för mellan-stopp
                var stop = prev[other]; // börja backtracka från föregångaren till 'other'
                while (stop != null && stop != source) // så länge vi inte nått källan
                {
                    stops.Add(stop.Name); // lägg till nodens namn i listan
                    stop = prev[stop]; // gå vidare till föregångaren
                }
                stops.Reverse();          // rätt ordning (source → destination)
                path.StopNames = stops; // kopiera in i Path-objektet

                source.ShortestPaths.Add(path); // lägg till Path-objektet i source.ShortestPaths
            }

            // Sortera efter totaldistans
            source.ShortestPaths.Sort((a, b) => a.Distance.CompareTo(b.Distance));
        }
    }
}

