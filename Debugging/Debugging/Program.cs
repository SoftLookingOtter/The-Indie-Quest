using System;

namespace Debugging
{
    internal class Program
    {
        static void Main(string[] args)
        {
            {
                Console.WriteLine("Starting program...");

                GreetUser(); // <-- Sätt breakpoint här

                Console.WriteLine("Back in Main!");
            }

            static void GreetUser()
            {
                Console.WriteLine("Entering GreetUser...");
                SayHello();
                SayGoodbye();
                Console.WriteLine("Leaving GreetUser...");
            }

            static void SayHello()
            {
                Console.WriteLine("Hello there!");
            }

            static void SayGoodbye()
            {
                Console.WriteLine("Goodbye!");
            }
        }
    }
}
