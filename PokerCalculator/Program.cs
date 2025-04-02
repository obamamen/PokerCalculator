using System;
using System.Reflection;
using PokerCalculator;

namespace PokerCalculator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Calculator.Setup();

            //ulong hand1 = Utility.CreateCard(Rank.Ace,Suit.Spades) | Utility.CreateCard(Rank.Ace, Suit.Clubs);

            //ulong hand2 = Utility.CreateCard(Rank.Ace, Suit.Diamonds) | Utility.CreateCard(Rank.Ace, Suit.Hearts);

            //var r = Calculator.Calculate(5000000, 0UL, (ulong)hand1, 0UL);

            //r.Display();

            Testing.TestPocketAces(4000000);
            Testing.Test5050(8000000);
            Testing.Test252525(4000000);
            Testing.TestOffsuit27(6000000);

            //Calculator.Calculate(10000000, 0UL, (ulong)hand1, 0UL, 0UL, 0UL).Display();

            //Testing.Test252525(10000);
            //var a = Calculator.SetupCalculator(500, 0UL, (ulong)hand1, 0UL);
            //while (true)
            //{
            //    Calculator.UpdateCalculator(ref a);

            //    Console.SetCursorPosition(0, 0);
            //    a.Results.Display();
            //}


            //List<int> scores = new List<int>();
            //List<ulong> cards = new List<ulong>();


            //ulong hand;
            //int iter = 0;
            //int H = int.MinValue;
            //for (int i = 0; i < Constants.RANKS; i++)
            //{
            //    for (int j = 0; j < Constants.RANKS; j++)
            //    {
            //        iter++;
            //        if (i != j)
            //        {

            //            hand = Utility.CreateCard(i, 0) | Utility.CreateCard(j, 0);
            //            var r1 = Calculator.Calculate(200000, 0UL, hand, 0UL);

            //            scores.Add(r1.Wins[0]);
            //            cards.Add(hand);
            //            if (r1.Wins[0] > H)
            //            {
            //                H = r1.Wins[0];
            //            }
            //        }

            //        hand = Utility.CreateCard(i, 0) | Utility.CreateCard(j, 1);
            //        var r = Calculator.Calculate(100000, 0UL, hand, 0UL);

            //        scores.Add(r.Wins[0]);
            //        cards.Add(hand);
            //        if (r.Wins[0] > H)
            //        {
            //            H = r.Wins[0];
            //        }


            //        if (iter % 10 == 0)
            //        {
            //            Console.SetCursorPosition(0, 0);
            //            Console.WriteLine(((float)iter / (13*12)) * 100);
            //        }
            //    }
            //}

            //Console.Clear();
            //for (int i = 0; i < scores.Count; i++)
            //{
            //    if (scores[i] <= H - 600)
            //    {
            //        continue;
            //    }
            //    Console.WriteLine($"{scores[i]}");
            //    Console.WriteLine(Utility.ToNiceBinaryString(cards[i]));
            //    Console.WriteLine();
            //}
        }
    }
}
