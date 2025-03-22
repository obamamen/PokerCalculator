namespace PokerCalculator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ulong hand = 0;
            hand = Utility.CreateCard(Rank.Three, Suit.Hearts);
            Console.WriteLine(Utility.ToBinaryString(hand));
            Console.WriteLine(Utility.HasCard(hand, Rank.Three, Suit.Hearts));
        }
    }
}
