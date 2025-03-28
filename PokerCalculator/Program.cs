using System.Reflection;

namespace PokerCalculator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Calculator.Setup();

            ulong hand1 = Utility.CreateCard(Rank.Ace,Suit.Hearts) | Utility.CreateCard(Rank.Ace, Suit.Clubs);

            //Calculator.Calculate(10000000, 0UL, (ulong)hand1, 0UL, 0UL, 0UL).Display();

            //Testing.Test252525(10000);
            var a = Calculator.SetupCalculator(500, 0UL, (ulong)hand1, 0UL);
            while (true)
            {
                Calculator.UpdateCalculator(ref a);

                Console.SetCursorPosition(0, 0);
                a.Results.Display();
            }
        }
    }
}
