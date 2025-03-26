namespace PokerCalculator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Calculator.Setup();

            ulong hand = 0;
            hand |= Utility.CreateCard(Rank.Two, (Suit)0);
            hand |= Utility.CreateCard(Rank.Two, (Suit)1);
            hand |= Utility.CreateCard(Rank.Five, (Suit)2);
            hand |= Utility.CreateCard(Rank.Five, (Suit)1);
            hand |= Utility.CreateCard(Rank.Five, (Suit)0);
            hand |= Utility.CreateCard(Rank.Ace, (Suit)2);
            hand |= Utility.CreateCard(Rank.Ace, (Suit)1);
            Console.WriteLine(Utility.ToBinaryString(Utility.HandToNormalized(hand)));
            Console.WriteLine((Rank)Calculator.FullHouse(hand));



            //foreach (ulong u in Calculator.Highcards)
            //{
            //    Console.WriteLine(Utility.ToBinaryString(u));
            //}
        }
    }
}
