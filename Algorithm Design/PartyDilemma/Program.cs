using System;

class Program
{
    // Recursive factorial method
    static int Factorial(int n)
    {
        // Base case
        if (n == 0)
        {
            return 1;
        }

        // Recursive case
        return n * Factorial(n - 1);
    }

    static void Main()
    {
        for (int i = 0; i <= 10; i++)
        {
            Console.WriteLine($"{i}! = {Factorial(i)}");
        }
    }
}
