namespace PokerCalculator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Calculator.Setup();

            ulong activePool = Generator.FullDeck;

            ulong hand1 = Utility.CreateCard(2, 0) | Utility.CreateCard(3, 0);
            ulong hand2 = Utility.CreateCard(13, 0) | Utility.CreateCard(5, 0);
            ulong community = Utility.CreateCard(2, 2) | Utility.CreateCard(3, 3) | Utility.CreateCard(3, 1) | Utility.CreateCard(6, 1) | Utility.CreateCard(7, 2);

            ulong[] players = [hand1,hand2];
            int winner = Calculator.Calculate(community, players, true);


            Console.WriteLine(Utility.ToNiceBinaryString(hand1) + "\n");
            Console.WriteLine(Utility.ToNiceBinaryString(hand2) + "\n");
            Console.WriteLine(Utility.ToNiceBinaryString(community) + "\n");
            Console.WriteLine(Utility.ToNiceBinaryString(community|hand1|hand2) + "\n");

            Console.WriteLine("Winner: " + winner);
        }
    }
}
