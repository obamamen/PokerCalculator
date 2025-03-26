using System.Reflection;

namespace PokerCalculator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Calculator.Setup();
            Action<string> action = Console.WriteLine;
            ulong hand = Utility.CreateCard(12, 0) | Utility.CreateCard(12,3);
            uint total = 0;
            uint wins = 0;

            for (int i = 0; i < 1000000; i++)
            {
                ulong activePool = Generator.FullDeck ^ hand;

                ulong hand1 = hand;
                ulong hand2 = Generator.GenerateRandomHand(2, ref activePool);
                ulong community = Generator.GenerateRandomHand(5, ref activePool);

                ulong[] players = [hand1, hand2];
                int winner = Calculator.Calculate(community, players);
                if (winner != -1)
                {
                    total++;
                }
                if (winner == 0)
                {
                    wins++;
                }
            }
            Console.WriteLine($"%{((double)wins/(double)total)*100}");
        }
    }
}
