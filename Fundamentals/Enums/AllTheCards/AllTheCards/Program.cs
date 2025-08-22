using System;

namespace AllTheCards
{
    enum Suit { Heart, Spade, Diamond, Club }

    class Program
    {
        static void Main()
        {
            // (Windows) Se till att konsolen visar ♥♠♦♣ och box-ramen korrekt
            Console.OutputEncoding = System.Text.Encoding.UTF8;
          
            // Loopar valörerna 1..13 och ANROPAR METODEN DrawCard(...) varje varv.
            // I varje anrop skickar vi in 2 argument:
            //   1) suit-argumentet: Suit.Spade
            //   2) rank-argumentet: rank (1..13)
            for (int rank = 1; rank <= 13; rank++)
            {
                DrawCard(Suit.Spade, rank); // ← FUNKTIONSANROP med argument
                Console.WriteLine("—");
            }
        }

        // --- HÄR SKAPAR VI SJÄLVA METODEN (FUNKTIONEN) ---
        // Metoden heter DrawCard och TAR EMOT 2 PARAMETRAR:
        //   1) Suit suit  → en parameter av typen Suit (svit)
        //   2) int rank   → en parameter av typen int  (valör 1..13)
        // När vi ANROPAR metoden (se ovan) matchas ARGUMENTEN till dessa PARAMETRAR.
        static void DrawCard(Suit suit, int rank)
        {
            // Enkel validering av rank-argumentet
            if (rank < 1 || rank > 13) return;

            // Väljer vilken symbol som ska ritas baserat på parametern "suit"
            char sym = suit switch
            {
                Suit.Heart => '♥',
                Suit.Spade => '♠',
                Suit.Diamond => '♦',
                Suit.Club => '♣',
                _ => '?' // Om något annat värde, använd frågetecken som fallback
            };

            // Väljer vilken hörntext kortet ska ha baserat på rank-parametern
            string label = rank switch
            {
                1 => "A",
                11 => "J",
                12 => "Q",
                13 => "K",
                _ => rank.ToString() // Om inte 1, 11, 12 eller 13, använd rank som text
            };

            // --- KORTLAYOUT MALL (kallas mask) ---
            // GetMask returnerar 7 rader x 9 kolumner med 'x' där valörer ska vara och '.' där det ska vara tomt.
            string[] mask = GetMask(rank); // GetMask(rank) = Ett funktionsanrop. Du kallar metoden GetMask och skickar in argumentet rank (t.ex. 1 för A, 11 för J).

            // --- HÄR SKAPAR VI KORTET ---
            // 1) översta ramen, 2) övre hörnrad (vänsterjusterad label),
            // 3) maskens rader (x → svitsymbol, . → mellanslag),
            // 4) nedre hörnrad (högerjusterad label), 5) nedersta ramen.
            Console.WriteLine("╭─────────╮");
            Console.WriteLine($"│{label.PadRight(9)}│");

            foreach (var line in mask)
            {
                // Byt ut symbolmarkörer i masken till synliga tecken
                string row = line
                    .Replace('x', sym)  // x blir svitsymbolen (♥♠♦♣)
                    .Replace('.', ' '); // . blir mellanslag

                Console.WriteLine($"│{row}│");
            }

            Console.WriteLine($"│{label.PadLeft(9)}│");
            Console.WriteLine("╰─────────╯");
        }

        static string[] GetMask(int rank) // metoden returnerar en array av strängar (varje sträng är en rad i kortet)
        {
            return rank switch
            {
                1 => new[] { // A 
                    ".........",
                    ".x.......",
                    ".........",
                    "....x....",
                    ".........",
                    ".......x.",
                    ".........",
                },
                2 => new[] {
                    ".........",
                    ".x.......",
                    ".........",
                    ".........",
                    ".........",
                    ".......x.",
                    ".........",
                },
                3 => new[] {
                    ".........",
                    ".x.......",
                    ".........",
                    "....x....",
                    ".........",
                    ".......x.",
                    ".........",
                },
                4 => new[] {
                    ".........",
                    ".x.....x.",
                    ".........",
                    ".........",
                    ".........",
                    ".x.....x.",
                    ".........",
                },
                5 => new[] {
                    ".........",
                    ".x.....x.",
                    ".........",
                    "....x....",
                    ".........",
                    ".x.....x.",
                    ".........",
                },
                6 => new[] {
                    ".........",
                    ".x.....x.",
                    ".........",
                    ".x.....x.",
                    ".........",
                    ".x.....x.",
                    ".........",
                },
                7 => new[] {
                    ".........",
                    ".x.....x.",
                    "....x....",
                    ".x.....x.",
                    ".........",
                    ".x.....x.",
                    ".........",
                },
                8 => new[] {
                    ".........",
                    ".x.....x.",
                    "....x....",
                    ".x.....x.",
                    "....x....",
                    ".x.....x.",
                    ".........",
                },
                9 => new[] {
                    ".........",
                    ".x.....x.",
                    ".x.....x.",
                    "....x....",
                    ".x.....x.",
                    ".x.....x.",
                    ".........",
                },
                10 => new[] {
                    ".........",
                    ".x.....x.",
                    ".x.....x.",
                    ".x.....x.",
                    ".x.....x.",
                    ".x.....x.",
                    ".........",
                },
                // J, Q, K 
                11 or 12 or 13 => new[] {
                    ".........",
                    ".........",
                    ".........",
                    "....x....",
                    ".........",
                    ".........",
                    ".........",
                },
                _ => Array.Empty<string>() //Om rank inte är 1–13, returnera en tom mask.
            };
        }
    }
}
