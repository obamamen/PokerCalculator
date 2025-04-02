using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Numerics;
using static PokerCalculator.Calculator;
using System.Diagnostics;

namespace PokerCalculator
{
    public static class Testing
    {
        private static Stopwatch stopwatch = new Stopwatch();
        private static void DisplayStopWatch()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("Elapsed Milliseconds: " + stopwatch.ElapsedMilliseconds);
            Console.ResetColor();
        }
        public static void TestPocketAces(int iter = 2000000)
        {
            Console.WriteLine("Testing pocket aces");

            stopwatch.Start();
            Calculator.Results results = Calculator.Calculate(iter, 0UL, Utility.CreateCard(12, 0) | Utility.CreateCard(12, 1), 0UL);
            stopwatch.Stop();

            results.Display();
            DisplayStopWatch();
            Console.WriteLine();
        }
        public static void TestOffsuit27(int iter = 2000000)
        {
            Console.WriteLine("Testing offsuit 2 7");

            stopwatch.Start();
            Calculator.Results results = Calculator.Calculate(iter, 0UL, Utility.CreateCard(2, 0) | Utility.CreateCard(7, 1), 0UL);
            stopwatch.Stop();

            results.Display();
            DisplayStopWatch();
            Console.WriteLine();
        }
        public static void Test5050(int iter = 2000000)
        {
            Console.WriteLine("Testing a fair 50 50");

            stopwatch.Start();
            Calculator.Results results = Calculator.Calculate(iter, 0UL, 0UL, 0UL);
            stopwatch.Stop();

            results.Display();
            DisplayStopWatch();
            Console.WriteLine();
        }

        public static void Test252525(int iter = 2000000)
        {
            Console.WriteLine("Testing a fair 25 25 25");

            stopwatch.Start();
            Calculator.Results results = Calculator.Calculate(iter, 0UL, 0UL, 0UL, 0UL, 0UL);
            stopwatch.Stop();

            results.Display();
            DisplayStopWatch();
            Console.WriteLine();
        }
    }
    public static class Constants
    {
        public static readonly ulong SECTION = 0x1FFF;
        public static readonly int SECTIONSIZE = 13;
        public static readonly int SUITES = 4;
        public static readonly int RANKS = 13;

        public static readonly ulong[] STRAIGHTMASKS =
        {
            0b00001111100000000UL, // A-high straight (10, J, Q, K, A)
            0b00000111110000000UL, // K-high straight (9, 10, J, Q, K)
            0b00000011111000000UL, // Q-high straight (8, 9, 10, J, Q)
            0b00000001111100000UL, // J-high straight (7, 8, 9, 10, J)
            0b00000000111110000UL, // 10-high straight (6, 7, 8, 9, 10)
            0b00000000011111000UL, // 9-high straight (5, 6, 7, 8, 9)
            0b00000000001111100UL, // 8-high straight (4, 5, 6, 7, 8)
            0b00000000000111110UL, // 7-high straight (3, 4, 5, 6, 7)
            0b00000000000011111UL, // 6-high straight (2, 3, 4, 5, 6)
            0b00001000000001111UL, // 5-high straight (A, 2, 3, 4, 5)
        };
        // binary normilized values for the straights

        //public static readonly Func<ulong, int>[] HANDEVALUATORS = new Func<ulong, int>[]
        //{
        //    Calculator.RoyalFlush, Calculator.StraightFlush, Calculator.FourOfAKind, Calculator.FullHouse,
        //    Calculator.Flush, Calculator.Straight, Calculator.ThreeOfAKind, Calculator.TwoPair, Calculator.Pair, Calculator.HighCard
        //};
    }
    public enum Suit
    {
        Hearts,
        Diamonds,
        Clubs,
        Spades,
        SUITS
    }
    public enum Rank
    {
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Jack,
        Queen,
        King,
        Ace,
        RANKS
    }
    public static class Calculator
    {
        public struct Card
        {
            public Rank Rank;
            public Suit Suit;

            public Card(Rank rank, Suit suit)
            {
                Rank = rank;
                Suit = suit;
            }

            public Card(ulong card)
            {
                Suit = (Suit)((ulong)BitOperations.TrailingZeroCount(card) / (int)Rank.RANKS);
                Rank = (Rank)(ulong)BitOperations.TrailingZeroCount(Utility.HandToNormalized(card));
            }

            public readonly ulong ToUlong()
            {
                return Utility.SetBit(Rank, Suit);
            }

            public readonly override string ToString()
            {
                return $"{Ranks[(int)Rank]}{Suits[(int)Suit]}";
            }

            static readonly char[] Ranks = { '2', '3', '4', '5', '6', '7', '8', '9', 'T', 'J', 'Q', 'K', 'A' };
            static readonly char[] Suits = { 'H', 'D', 'C', 'S' };
        }
        public enum WinTypes {
            RoyalFlush,
            StraightFlush,
            FourOfAKind,
            FullHouse,
            Flush,
            Straight,
            ThreeOfAKind,
            TwoPair,
            OnePair,
            HighCard
        }
        public struct Results
        {
            public int Total;
            public int Ties;
            public int[] Wins;
            //public int[] RoyalFlush;
            //public int[] StraightFlush;
            //public int[] FourOfAKind;
            //public int[] FullHouse;
            //public int[] Flush;
            //public int[] Straight;
            //public int[] ThreeOfAKind;
            //public int[] TwoPair;
            //public int[] OnePair;
            //public int[] HighCard;

            //public void InitializeArays(int numHands)
            //{
            //    Wins = new int[numHands];
            //    RoyalFlush = new int[numHands];
            //    StraightFlush = new int[numHands];
            //    FourOfAKind = new int[numHands];
            //    FullHouse = new int[numHands];
            //    Flush = new int[numHands];
            //    Straight = new int[numHands];
            //    ThreeOfAKind = new int[numHands];
            //    TwoPair = new int[numHands];
            //    OnePair = new int[numHands];
            //    HighCard = new int[numHands];
            //}
            public readonly int GetLosses(int index)
            {
                if (index < 0)
                {
                    throw new IndexOutOfRangeException("Index cannot be negative");
                }
                if (index > Wins.Length)
                {
                    throw new IndexOutOfRangeException("Index cannot be bigger than the Wins[]");
                }

                return Total - Ties - Wins[index];
            }
            public readonly void Display()
            {
                for (int i = 0; i < Wins.Length; i++)
                {
                    Console.WriteLine($"Wins for hand {i + 1}: {Math.Round(((float)Wins[i] / (float)Total) * 10000) / 100:F1}%");
                }
                Console.WriteLine($"Ties: {Math.Round(((float)Ties / (float)Total) * 10000) / 100:F2}%");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"Total Simulations: {Total}");
                Console.ForegroundColor = ConsoleColor.White;
            }
            // display all the win% and tie% for the Results
            // uses xx.x format for the win% and xx.xx for the tie%
        }
        public struct CalculatorInformation()
        {
            public int Iterations;
            public ulong[] Hands;
            public ulong CommunityCards;
            public Results Results;
            
            public CalculatorInformation(int Iterations, ulong CommunityCards, ulong[] Hands) : this()
            {
                this.Hands = Hands;
                this.CommunityCards = CommunityCards;
                this.Iterations = Iterations;

                this.Results = new Results();
                this.Results.Total = 0;
                this.Results.Wins = new int[Hands.Length];
            }
        }
        delegate int HandEvaluator(ulong hand);
        public static CalculatorInformation SetupCalculator(int iterations = 500, ulong communityCardsPreset = 0UL, params ulong[] hands)
        {
            CalculatorInformation info = new(iterations, communityCardsPreset,hands);
            return info;
        }
        public static void UpdateCalculator(ref CalculatorInformation info)
        {
            Results results = Calculate(info.Iterations,info.CommunityCards,info.Hands);

            info.Results.Total += results.Total;
            info.Results.Ties += results.Ties;

            if (info.Results.Wins.Length != results.Wins.Length)
            {
                throw new InvalidOperationException("The amount of hands in the results does not match the amount of hands in the info");
            }

            for (int i = 0; i < results.Wins.Length; i++)
            {
                info.Results.Wins[i] += results.Wins[i];
            }
        }

        public static void Setup()
        {
            for (int i = 0; i < (int)Rank.RANKS; i++)
            {
                Highcards[i] = Utility.ApplyNormalized(Utility.CreateNomilized((Rank)i));
            }
            // precalculate the normalized values for the highcards
        }
        public static ulong[] Highcards = new ulong[(int)Rank.RANKS];
        public static Results Calculate(int iterations = 100000, ulong communityCardsPreset = 0UL, params ulong[] hands)
        {

            Results results = new Results();
            results.Total = iterations;
            results.Ties = 0;
            results.Wins = new int[hands.Length];

            ulong[] playerHands = new ulong[hands.Length];
            ulong communityCards = communityCardsPreset;


            for (int i = 0; i < iterations; i++)
            {
                ulong activePool = Generator.FullDeck;
                // get a pool of all the valid poker cards

                for (int j = 0; j < hands.Length; j++)
                {
                    if (hands[j] == 0UL)
                    // if the hand is empty then we generate a random hand, empty meaning that the user has not set a hand.
                    {
                        playerHands[j] = Generator.GenerateRandomHand(2, ref activePool);
                    }
                    else
                    // the user has set a hand so we use that hand and remove it from the active pool.
                    {
                        activePool ^= hands[j];
                        playerHands[j] = hands[j];
                    }
                }

                int bitcount = (int)ulong.PopCount(communityCardsPreset);
                if (bitcount < 5)
                {
                    communityCards = Generator.GenerateRandomHand(5 - bitcount, ref activePool) | communityCardsPreset;
                }
                // here we generate the community cards, if the user has any set community cards we keep it and generate the rest.

                int winner = Winner(communityCards, playerHands);
                if (winner == -1)
                {
                    results.Ties++;
                }
                else
                {
                    results.Wins[winner]++;
                }
            }
            return results;
        }
        public static int Winner(ulong communityCards, ulong[] playerCards)
        {
            int pcount = playerCards.Length;

            int p = 0;

            int bestPlayer = -1;
            int bestHandScore = -1;
            int score = -1;

            Span<int> scores = stackalloc int[pcount];
            // All the scores, only needed once beacuse we can only tie at the first type we find.
            

            Span<int> scoresOnlyHand = stackalloc int[pcount];
            // This gives the players hand a score, this is used to determine the winner if we have a tie.

            for (int i = 0; i < pcount; i++)
            {
                scores[i] = -1;
                scoresOnlyHand[i] = -1;
            }
            // this is set beacse -1 is the default value, so if we have a hand that does comply with the type we now that when we dont have a -1.

            for (int t = 0; t < 10; t++)
            {
                // Evaluate RoyalFlush
                for (p = 0; p < pcount; p++)
                {
                    score = -1;
                    switch (t)
                    {
                        case 0:
                            score = Calculator.RoyalFlush(communityCards | playerCards[p]);
                            break;
                        case 1:
                            score = Calculator.StraightFlush(communityCards | playerCards[p]);
                            break;
                        case 2:
                            score = Calculator.FourOfAKind(communityCards | playerCards[p]);
                            break;
                        case 3:
                            score = Calculator.FullHouse(communityCards | playerCards[p]);
                            break;
                        case 4:
                            score = Calculator.Flush(communityCards | playerCards[p]);
                            break;
                        case 5:
                            score = Calculator.Straight(communityCards | playerCards[p]);
                            break;
                        case 6:
                            score = Calculator.ThreeOfAKind(communityCards | playerCards[p]);
                            break;
                        case 7:
                            score = Calculator.TwoPair(communityCards | playerCards[p]);
                            break;
                        case 8:
                            score = Calculator.Pair(communityCards | playerCards[p]);
                            break;
                        case 9:
                            score = Calculator.HighCard(communityCards | playerCards[p]);
                            break;
                    }
                    if (score != -1)
                    {
                        scores[p] = score;
                        scoresOnlyHand[p] = GetHandScore(playerCards[p]);
                    }
                    // if we have a score then we set the score for the player and also the score for the players hand.
                    // we only have a score if the player has a hand that complies with the type.

                    if (score > bestHandScore)
                    {
                        bestHandScore = score;
                        bestPlayer = p;
                    }
                    // if we have a higher score, 
                    // higher score means higher score based on the hand and community cards.
                    // based on the functions, so a acePair is higer score than a 4Pair or 7pair.
                }

                if (IsTie(scores))
                // if there is a tie between the full hands from any players.
                {
                    int highestHandScore = Utility.GetHighest(scores);
                    for (int i = 0; i < pcount; i++)
                    {
                        if (scores[i] != highestHandScore)
                        {
                            scoresOnlyHand[i] = -1;
                        }
                    }
                    // Only calculate the tie values for the players that passes the first tie check; meaning that only the players in the tie will have a score.

                    if (IsTie(scoresOnlyHand))
                    // check if there is a tie in the players hand ONLY, if so then we have an actual game tie.
                    {
                        return -1;
                    }
                    else
                    {
                        return Utility.GetHighestIndex(scoresOnlyHand);
                        // else return the index of the player with the highest only hand score. this is then the winner.
                    }
                }

                if (bestHandScore != -1)
                {
                    return bestPlayer;
                    // if we have a winner then return winners index ofc.
                }
            }

            throw new InvalidOperationException("[No winner found or Tie] bug");
            // A big has happend here please no
        }

        #region Tie helpers
        //public static int TieUsingHighcards(ulong[] playerHands)
        //{
        //    int count = playerHands.Length;
        //    ulong[] playerHandsCopy = playerHands.ToArray();

        //    Span<int> scores = stackalloc int[count];
        //    int highest = -1;

        //    for (int i = 0; i < count; i++)
        //    {
        //        scores[i] = SingleHighcardRemove(ref playerHands[i]);
        //        if (scores[i] > highest)
        //        {
        //            highest = scores[i];
        //        }
        //    }

        //    int ties = 0;
        //    int bestPlayer = -1;
        //    for (int i = 0; i < count; i++)
        //    {
        //        if (scores[i] == highest)
        //        {
        //            ties++;
        //            bestPlayer = i;
        //        }
        //    }
        //    if (ties == 1)
        //    {
        //        return bestPlayer;
        //    }
        //    if (ties > 1)
        //    {
        //        return -1;
        //    }

        //    return -1;
        //}

        // currently not in use.

        public static int SingleHighcardRemove(ref ulong hand)
        {
            if (hand == 0UL)
            {
                return -1;
            }

            for (int i = (int)Rank.Ace; i >= (int)Rank.Two; i--)
            {
                for (int j = 0; j < Constants.SUITES; j++)
                {
                    ulong mask = Utility.SetBit((Rank)i, (Suit)j);
                    if ((hand & mask) == mask)
                    {
                        hand ^= mask;
                        return i;
                    }
                }
            }

            // for each rank and suite we check if the hand has the card, if so we remove it and return the rank.

            return -1;
        }
        public static int GetHandScore(ulong hand)
        {
            int score = SingleHighcardRemove(ref hand)+Constants.RANKS*2;

            // we need to give the highest card some priority based on the texas holdem rules.

            score += HighCard(hand);

            // than if the first card is the same, the second card gives a smaller score, meaning it can still shift tie situations.

            return score;
        }
        public static bool IsTie(Span<int> scores, bool ignoreNegatives = true)
        {
            int highest = Utility.GetHighest(scores);
            int count = 0;
            for (int i = 0; i < scores.Length; i++)
            {
                if (scores[i] == highest)
                {
                    if (ignoreNegatives && scores[i] == -1)
                    {
                        continue;
                    }
                    count++;
                }
            }

            return (count > 1);
        }
        #endregion

        #region Hand values
        public static int HighCard(ulong hand)
        {
            Utility.ThrowBasedOnHand(hand);

            for (int i = (int)Rank.Ace; i >= (int)Rank.Two; i--)
            {
                // this just checks if we have any of the (rank) cards from ace to two, if we have return the (rank)
                if ((hand & Highcards[i]) != 0)
                {
                    return i;
                }
            }

            throw new ArgumentException($"Hand does not contain any valid cards: {Utility.ToBinaryString(hand)};");
        }
        public static int Pair(ulong hand)
        {
            if (hand == 0UL)
            {
                throw new ArgumentException("Hand cannot be empty");
            }

            for (int i = (int)Rank.Ace; i >= (int)Rank.Two; i--)
            {
                if (ulong.PopCount(hand & Highcards[i]) >= 2)
                {
                    return i;
                }
            }

            return -1;
        }
        public static int TwoPair(ulong hand)
        {
            Utility.ThrowBasedOnHand(hand);

            int highest = -1;
            int pairCount = 0;
            ulong count;
            for (int i = (int)Rank.Ace; i >= (int)Rank.Two; i--)
            {
                count = ulong.PopCount(hand & Highcards[i]);
                // counts the bits in the full applied normalied of the rank (:

                if (count >= 2)
                {
                    pairCount++;
                    if (i > highest)
                    {
                        highest = i;
                    }
                }
                if (count == (ulong)Constants.SUITES)
                // if we lets say have 4 aces we actually also have 2 pairs of aces
                {
                    return i;
                }
            }
            if (pairCount >= 2)
            // if we have 2 or more pairs then return the highest pair
            {
                return highest;
            }

            return -1;
        }
        public static int ThreeOfAKind(ulong hand)
        {
            Utility.ThrowBasedOnHand(hand);

            for (int i = (int)Rank.Ace; i >= (int)Rank.Two; i--)
            {
                if (ulong.PopCount(hand & Highcards[i]) >= 3)
                {
                    return i;
                }
            }

            return -1;
        }
        public static int Straight(ulong hand)
        {
            Utility.ThrowBasedOnHand(hand);
            hand = Utility.HandToNormalized(hand);

            for (int i = 0; i < Constants.STRAIGHTMASKS.Length; i++)
            {
                if ((hand & Constants.STRAIGHTMASKS[i]) == Constants.STRAIGHTMASKS[i])
                // we can check if the hand has the straight by checking if the hand has the straight mask. These masks are explained in the Constants class.
                {
                    return (int)Rank.Ace - i;
                }
                // why i reverse it beacuse the straight masks are in reverse order, so the first mask is the highest straight.
            }

            return -1;
        }
        public static int Flush(ulong hand)
        {
            Utility.ThrowBasedOnHand(hand);
            ulong section;
            for (int i = 0; i < Constants.SUITES; i++)
            {
                section = hand & Utility.CreateSection((Suit)i);
                if (ulong.PopCount(section) >= 5)
                {
                    return HighCard(section);
                }
            }
            return -1;
        }
        public static int FullHouse(ulong hand)
        {
            Utility.ThrowBasedOnHand(hand);
            int three = ThreeOfAKind(hand);
            if (three == -1)
            {
                return -1;
            }

            int pair = Pair(hand & ~Highcards[three]);
            // here we remove all the cards that are the same suit as the three of a kind, so we dont get a false positive.

            if (pair == -1)
            {
                return -1;
            }

            if (three >= pair)
            {
                return three;
            }
            else
            {
                return pair;
            }
            // here we return the biggest of the 2 "sets", is needed for tie handling.

        }
        public static int FourOfAKind(ulong hand)
        {
            Utility.ThrowBasedOnHand(hand);

            for (int i = (int)Rank.Ace; i >= (int)Rank.Two; i--)
            {
                if (ulong.PopCount(hand & Highcards[i]) >= 4)
                {
                    return i;
                }
            }

            return -1;
        }
        public static int StraightFlush(ulong hand)
        {
            Utility.ThrowBasedOnHand(hand);
            int highest = -1;
            int score;
            ulong section;
            for (int i = 0; i < Constants.SUITES; i++)
            {
                section = hand & Utility.CreateSection((Suit)i);
                // we split the hand into 4 sections , one for each suite.
                // so no handtonormilzed beacuse that would remove the suit depth and thus we would not be able to check for a StraightFlush.

                if (section == 0UL)
                {
                    continue;
                }
                score = Straight(section);
                if (score > highest)
                {
                    highest = score;
                }
                // !!! 
                //  /// /// NEEDS TESTING: I might be able to just return here but I am not sure if that would work 100%.
                // !!!
            }

            return highest;
        }
        public static int RoyalFlush(ulong hand)
        {
            Utility.ThrowBasedOnHand(hand);
            ulong section;
            for (int i = 0; i < Constants.SUITES; i++)
            {
                section = hand & Utility.CreateSection((Suit)i);
                section = Utility.HandToNormalized(section);
                if (ulong.PopCount(section) >= 5)
                {
                    if ((section & Constants.STRAIGHTMASKS[0]) == Constants.STRAIGHTMASKS[0])
                    {
                        return (int)Rank.Ace;
                    }
                }
                // this is the same as the straight flush but we only get the highest straight flush and if so then we return an ace.
            }
            return -1;
        }
        #endregion
    }
    public static class Utility
    {
        #region Array
        //public static int[] GetAllTheHighest(Span<int> array, int highest)
        //{
        //    int count = 0;
        //    for (int i = 0; i < array.Length; i++)
        //    {
        //        if (array[i] == highest)
        //        {
        //            count++;
        //        }
        //    }

        //    int[] result = new int[count];
        //    int index = 0;
        //    for (int i = 0; i < array.Length; i++)
        //    {
        //        if (array[i] == highest)
        //        {
        //            result[index] = i;
        //            index++;
        //        }
        //    }
        //    return result;
        //}

        // unsed code

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetHighest(Span<int> array)
        {
            int highest = -1;
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] > highest)
                {
                    highest = array[i];
                }
            }
            return highest;
        }
        public static int GetHighestIndex(Span<int> array)
        {
            int highest = -1;
            int highestIndex = -1;
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] > highest)
                {
                    highest = array[i];
                    highestIndex = i;
                }
            }
            return highestIndex;
        }
        #endregion
        #region Bit
        public static ulong CreateCard(Rank rank, Suit suit)
        {
            return SetBit(rank, suit);
        }
        public static ulong CreateCard(int rank, int suit, bool DO_NOT_USE = true)
        {
            return SetBit((Rank)rank, (Suit)suit);
        }
        public static void AddCard(ref ulong cards, Rank rank, Suit suit)
        {
            cards |= CreateCard(rank, suit);
        }
        public static bool HasCard(ulong cards, Rank rank, Suit suit)
        {
            return (cards & SetBit(rank, suit)) != 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static ulong SetBit(int pos)
        {
            return (1UL << pos);
        }
        internal static ulong SetBit(Rank rank, Suit suit)
        {
            return SetBit((int)rank + ((int)suit * Constants.SECTIONSIZE));
        }
        internal static string ToBinaryString(ulong value)
        {
            return Convert.ToString((long)value, 2).PadLeft(64, '0');
        }
        internal static string ToNiceBinaryString(ulong value)
        {
            StringBuilder sb = new();
            for (int s = 0; s < Constants.SUITES; s++)
            {
                for (int r = 0; r < Constants.RANKS; r++)
                {
                    if ((Utility.SetBit((Rank)r, (Suit)s) & value) == Utility.SetBit((Rank)r, (Suit)s))
                    {
                        sb.Append("1");
                    }
                    else
                    {
                        sb.Append("0");
                    }
                }
                if (s < Constants.SUITES - 1)
                {
                    sb.Append("\n");
                }
            }
            return sb.ToString();
        }
        #endregion

        #region Normalization
        public static ulong CreateNomilized(Rank k)
        {
            return (1UL << (int)k);
        }
        #endregion
        #region Section
        public static ulong CreateSection(Suit section)
        {
            return Constants.SECTION << ((int)section * Constants.SECTIONSIZE);
        }
        #endregion
        #region Conversion
        public static ulong NormalizedToSection(ulong normalize, int section)
        {
            normalize &= CreateSection((Suit)0);
            return normalize << (section * Constants.SECTIONSIZE);
        }
        public static ulong SectionToNormalized(ulong section, Suit offset)
        {
            section &= CreateSection(offset);
            return section >> ((int)offset * Constants.SECTIONSIZE);
        }
        public static ulong ApplyNormalized(ulong normalized)
        {
            ulong result = 0UL;
            for (int i = 0; i < Constants.SUITES; i++)
            {
                result |= NormalizedToSection(normalized, i);
            }
            return result;
        }
        public static ulong HandToNormalized(ulong hand)
        {
            ulong result = 0UL;
            for (int i = 0; i < Constants.SUITES; i++)
            {
                result |= SectionToNormalized(hand, (Suit)i);
            }
            result &= Constants.SECTION;
            return result;
        }
        #endregion
        #region Exception
        public static void ThrowBasedOnHand(ulong hand)
        {
            if (hand == 0UL)
            {
                throw new ArgumentException("Hand cannot be empty");
            }
        }
        #endregion
    }
    public static class Generator
    {
        public static Random R = new Random();

        public static readonly ulong FullDeck = GenerateFullDeck();

        public static ulong GenerateRandomHand(int count, ref ulong activePool)
        {
            if (ulong.PopCount(activePool) < (ulong)count)
            {
                throw new ArgumentException("Not enough cards in the pool");
            }

            ulong hand = 0UL;
            for (int i = 0; i < count; i++)
            {
                int bitIndex = GetRandomSetBit(activePool, R);
                ulong selectedCard = 1UL << bitIndex;

                hand |= selectedCard;
                activePool ^= selectedCard;
            }
            return hand;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int GetRandomSetBit(ulong bitmask, Random rand)
        {
            int popCount = (int)ulong.PopCount(bitmask);
            int target = rand.Next(popCount) + 1;
            int bitIndex = 0;

            while (target > 0)
            {
                int tzCount = BitOperations.TrailingZeroCount(bitmask);
                bitIndex += tzCount;
                bitmask >>= tzCount;
                target--;

                if (target > 0)
                {
                    bitmask >>= 1;
                    bitIndex++;
                }
            }
            return bitIndex;
        }
        private static ulong GenerateFullDeck()
        {
            ulong deck = 0UL;
            for (int suit = 0; suit < Constants.SUITES; suit++)
            {
                for (int rank = 0; rank < Constants.RANKS; rank++)
                {
                    deck |= Utility.CreateCard((Rank)rank, (Suit)suit);
                }
            }
            return deck;
        }
    }
}
