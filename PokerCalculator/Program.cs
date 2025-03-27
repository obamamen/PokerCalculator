using System.Reflection;

namespace PokerCalculator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Calculator.Setup();

            ulong hand1 = Utility.CreateCard(Rank.Queen,Suit.Hearts) | Utility.CreateCard(Rank.Ace, Suit.Clubs);

            Calculator.Calculate(5000000, 0UL, (ulong)hand1, 0UL, 0UL).Display();
        }
    }
}
