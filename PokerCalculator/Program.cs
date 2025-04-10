using System;
using System.Diagnostics;
using System.Reflection;
using PokerCalculator;

namespace PokerCalculator
{
    public class Program
    {
        static void Main(string[] args)
        {
            Calculator.Setup(true);

            ulong hand1 = Utility.CreateCard(Rank.Queen, Suit.Clubs) | Utility.CreateCard(Rank.Ace, Suit.Clubs);

            var a = Calculator.SetupCalculator(250_000, 0UL, 0UL, 0UL);
            Stopwatch s = new Stopwatch();
            while (true)
            {
                s.Restart();
                Calculator.UpdateCalculator(ref a);

                Console.SetCursorPosition(0, 0);
                for (int i = 0; i < 10; i++)
                {
                    Console.WriteLine("           ");
                }
                Console.SetCursorPosition(0, 0);
                a.Results.Display();
                s.Stop();
                Console.WriteLine($"Time: {s.ElapsedMilliseconds}ms");
            }
        }
    }
}
