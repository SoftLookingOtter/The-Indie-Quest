const int Levels = 100;
const int MinMonsters = 1;
const int MaxMonsters = 50;

int[] monstersPerLevel = new int[Levels];

var rng = Random.Shared;

// Fyll array med nummer mellan 1-50
for (int i = 0; i < monstersPerLevel.Length; i++)
{
    monstersPerLevel[i] = rng.Next(MinMonsters, MaxMonsters + 1);
}

// Sortera nummer 
Array.Sort(monstersPerLevel); // Sorterar i stigande ordning

// Skriv ut 
Console.WriteLine("Number of monsters in levels: " + string.Join(", ", monstersPerLevel));

