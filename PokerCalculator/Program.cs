using System.Reflection;

namespace PokerCalculator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Calculator.Setup();
            Action<string> action = Console.WriteLine;
            ulong handp1 = Utility.CreateCard(7, 0) | Utility.CreateCard(2,1);

            //Calculator.Results results = Calculator.Calculate(5000000, 0UL,handp1,0UL);

            int total = 0;
            int wins = 0;
            int losses = 0;
            int ties = 0;

            //for (int i = 0; i < 2000000; i++)
            //{
            //    total++;
            //    ulong activePool = Generator.FullDeck;

            //    ulong hand1 = Generator.GenerateRandomHand(2, ref activePool);
            //    ulong hand2 = Generator.GenerateRandomHand(2, ref activePool);
            //    ulong community = Generator.GenerateRandomHand(5, ref activePool);

            //    //Console.WriteLine($"{Utility.ToBinaryString(hand1)} {Utility.ToBinaryString(hand2)}");

            //    ulong[] players = [hand1, hand2];
            //    int winner = Calculator.Winner(community, players);
            //    if (winner == -1)
            //    {
            //        ties++;
            //    }
            //    if (winner == 0)
            //    {
            //        wins++;
            //    }
            //    if (winner == 1)
            //    {
            //        losses++;
            //    }
            //}

            //Console.WriteLine(wins);
            //Console.WriteLine(ties);
            //Console.WriteLine(losses);

            Testing.Test5050();
            Console.WriteLine();
            Testing.TestOffsuit27();
            Console.WriteLine();
            Testing.TestPocketAces();
        }
    }
}
