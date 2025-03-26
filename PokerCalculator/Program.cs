namespace PokerCalculator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Calculator.Setup();

            ulong hand = 0;
            hand |= Utility.CreateCard(Rank.Ace, (Suit)1);
            hand |= Utility.CreateCard(Rank.King, (Suit)1);
            hand |= Utility.CreateCard(Rank.Queen, (Suit)2);
            hand |= Utility.CreateCard(Rank.Jack, (Suit)1);
            hand |= Utility.CreateCard(Rank.Ten, (Suit)1);
            hand |= Utility.CreateCard(Rank.Ace, (Suit)2);
            hand |= Utility.CreateCard(Rank.Ace, (Suit)1);
            Console.WriteLine(Utility.ToNiceBinaryString((hand)));

            Console.WriteLine((Rank)Calculator.RoyalFlush(hand));



            //foreach (ulong u in Calculator.Highcards)
            //{
            //    Console.WriteLine(Utility.ToBinaryString(u));
            //}
        }
    }
}
