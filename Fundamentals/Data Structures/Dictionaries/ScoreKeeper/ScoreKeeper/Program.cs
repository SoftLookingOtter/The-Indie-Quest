namespace ScoreKeeper;

using System; // Behövs för Console
using System.Collections.Generic; // Behövs för Dictionary
using System.Linq; // Behövs för OrderByDescending

internal static class Program
{
    public static void Main()
    {
        var scores = new Dictionary<string, int>(); // Ny TOM ordbok för att lagra poäng

        while (true)
        {
            Console.Write("Who won this round? ");
            var name = (Console.ReadLine() ?? "").Trim();
            if (name.Length == 0)
                break; // Avsluta om inget namn anges

            if (scores.ContainsKey(name))
                scores[name]++; // Öka poängen om namnet redan finns
            else
                scores[name] = 1; // Annars, lägg till namnet med 1 poäng

            Console.WriteLine("\nRANKINGS"); // Skriv ut nuvarande poängställning med ett mellanrum
            foreach (var p in scores.OrderByDescending(p => p.Value)) // Loopa igenom ordboken sorterad efter poäng i fallande ordning // IntelliSense föreslår att inte använda LINQ
                Console.WriteLine($"{p.Key} {p.Value}"); // Key är namnet, Value är poängen // I en Dictionary<TKey, TValue> är varje post ett KeyValuePair<TKey,TValue>
            Console.WriteLine();
        }
    }
}
