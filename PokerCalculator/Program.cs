using System.Reflection;

namespace PokerCalculator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Calculator.Setup();
            Action<string> action = Console.WriteLine;
            ulong handp1 = Utility.CreateCard(12, 0) | Utility.CreateCard(12,3);
            ulong handp2 = Utility.CreateCard(12, 1) | Utility.CreateCard(12, 2);
            uint total = 0;
            uint wins = 0;
            uint ties = 0;
            uint losses = 0;

            int score1 = Calculator.GetHandScore(handp1);
            int score2 = Calculator.GetHandScore(handp2);

            Console.WriteLine($"Player 1 score: {score1}");
            Console.WriteLine($"Player 2 score: {score2}");
            Console.WriteLine();

            for (int i = 0; i < 2000000; i++)
            {
                total++;
                ulong activePool = Generator.FullDeck ^ (handp1 | handp2);

                ulong hand1 = handp1;
                ulong hand2 = handp2;
                ulong community = Generator.GenerateRandomHand(5, ref activePool);

                ulong[] players = [hand1, hand2];
                int winner = Calculator.Calculate(community, players);
                if (winner == -1)
                {
                    ties++;
                }
                if (winner == 0)
                {
                    wins++;
                }
                if (winner == 1)
                {
                    losses++;
                }
            }
            Console.WriteLine($" wins %{((double)wins / (double)total) * 100}");
            Console.WriteLine($" ties %{((double)ties / (double)total) * 100}");
            Console.WriteLine($" losses %{((double)losses / (double)total) * 100}");
        }
    }
}
