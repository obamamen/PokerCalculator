using System.Reflection;

namespace PokerCalculator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Calculator.Setup();

            Testing.Test5050();
            Console.WriteLine();
            Testing.TestOffsuit27();
            Console.WriteLine();
            Testing.TestPocketAces();
        }
    }
}
