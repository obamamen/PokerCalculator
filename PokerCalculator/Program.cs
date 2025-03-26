namespace PokerCalculator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Calculator.Setup();

            ulong hand = 0;
            hand |= Utility.CreateCard(Rank.Two, (Suit)1);
            hand |= Utility.CreateCard(Rank.Three, (Suit)0);
            hand |= Utility.CreateCard(Rank.Four, (Suit)3);
            hand |= Utility.CreateCard(Rank.Five, (Suit)1);
            hand |= Utility.CreateCard(Rank.Ace, (Suit)2);
            Console.WriteLine(Utility.ToBinaryString(Utility.HandToNormalized(hand)));
            Console.WriteLine((Rank)Calculator.Flush(hand));



            //foreach (ulong u in Calculator.Highcards)
            //{
            //    Console.WriteLine(Utility.ToBinaryString(u));
            //}
        }
    }
}
