using System.Reflection;
using PokerCalculator;

namespace PokerCalculator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Calculator.Setup();

            ulong hand1 = Utility.CreateCard(Rank.Two,Suit.Hearts) | Utility.CreateCard(Rank.Seven, Suit.Clubs);

            //Calculator.Calculate(10000000, 0UL, (ulong)hand1, 0UL, 0UL, 0UL).Display();

            //Testing.Test252525(10000);
            //var a = Calculator.SetupCalculator(500, 0UL, (ulong)hand1, 0UL);
            //while (true)
            //{
            //    Calculator.UpdateCalculator(ref a);

            //    Console.SetCursorPosition(0, 0);
            //    a.Results.Display();
            //}

            List<int> scores = new List<int>();
            List<ulong> cards = new List<ulong>();

            ulong hand;
            int iter = 0;
            int lowestScore = int.MaxValue;
            for (int i = 0; i < Constants.RANKS; i++)
            {
                for (int j = 0; j < Constants.RANKS; j++)
                {
                    if (i == j)
                    {
                        continue;
                    }
                    iter++;

                    hand = Utility.CreateCard(i, 0) | Utility.CreateCard(j, 1);
                    var r = Calculator.Calculate(200000, 0UL, hand, 0UL, 0UL, 0UL,0UL,0UL);

                    scores.Add(r.Wins[0]);
                    cards.Add(hand);
                    if (r.Wins[0] < lowestScore)
                    {
                        lowestScore = r.Wins[0];
                    }
                    if (iter % 10 == 0)
                    {
                        Console.SetCursorPosition(0, 0);
                        Console.WriteLine(((float)iter / (13*12)) * 100);
                    }
                }
            }

            Console.Clear();
            for (int i = 0; i < scores.Count; i++)
            {
                if (scores[i] >= lowestScore + 150)
                {
                    continue;
                }
                Console.WriteLine($"{scores[i]}");
                Console.WriteLine(Utility.ToNiceBinaryString(cards[i]));
                Console.WriteLine();
            }
        }
    }
}
