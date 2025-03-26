namespace PokerCalculator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Calculator.Setup();

            ulong activePool = Generator.FullDeck;

            ulong hand1 = Utility.CreateCard(2, 0) | Utility.CreateCard(5, 0);
            ulong hand2 = Utility.CreateCard(12, 0) | Utility.CreateCard(12, 1);
            ulong community = Utility.CreateCard(12, 2) | Utility.CreateCard(12, 3) | Utility.CreateCard(3, 1) | Utility.CreateCard(6, 1) | Utility.CreateCard(7, 2);

            ulong[] players = [hand1,hand2];
            Action<string> action = Console.WriteLine;
            int winner = Calculator.Calculate(community, players, action);


            Console.WriteLine(Utility.ToNiceBinaryString(hand1) + "\n");
            Console.WriteLine(Utility.ToNiceBinaryString(hand2) + "\n");
            Console.WriteLine(Utility.ToNiceBinaryString(community) + "\n");
            Console.WriteLine(Utility.ToNiceBinaryString(community|hand1|hand2) + "\n");

            Console.WriteLine("p1 highc " + Calculator.HighCard(hand1));
            Console.WriteLine("p2 highc " + Calculator.HighCard(hand2));

            Console.WriteLine("Winner: " + winner);
        }
    }
}
