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
            Calculator.Setup();

            ulong hand1 = Utility.CreateCard(Rank.Ace, Suit.Hearts) | Utility.CreateCard(Rank.Ace, Suit.Clubs);

            ulong hand2 = Utility.CreateCard(Rank.Two, Suit.Hearts) | Utility.CreateCard(Rank.Three, Suit.Clubs);


            var a = Calculator.SetupCalculator(2_000, 0UL, hand1, hand2);
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
