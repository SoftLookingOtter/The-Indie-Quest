using System;

enum Suit { Heart, Spade, Diamond, Club }

class Program
{
    static void Main()
    {
        // Windows: visa ♥♠♦♣ och ramen korrekt
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        // Ritar bara ett ess (ändra svit här om jag vill)
        DrawAce(Suit.Heart); // Anrop av metoden. // Här skickar jag in argumentet Suit.Heart // (ett enum-värde av typen Suit).
    }

    static void DrawAce(Suit suit) // Suit = typ enum, suit = parameterns namn // Deklaration av metoden. Den säger: ”Den här metoden tar emot en parameter som heter suit och som är av typen Suit.”
    {
        char sym = suit switch
        {
            Suit.Heart => '♥',
            Suit.Spade => '♠',
            Suit.Diamond => '♦',
            Suit.Club => '♣',
            _ => '?'
        };

        var orig = Console.ForegroundColor;
        if (suit is Suit.Heart or Suit.Diamond) Console.ForegroundColor = ConsoleColor.Red;

        string[] lines =
        {
            "╭───────╮",
            "│A      │",
            $"│{sym}      │",
            $"│   {sym}   │",
            $"│      {sym}│",
            "│      A│",
            "╰───────╯"
        };

        foreach (var line in lines) Console.WriteLine(line);
        Console.ForegroundColor = orig;
    }
}
