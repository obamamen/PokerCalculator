using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace PokerCalculator
{
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
        public static void Setup()
        {
            for (int i = 0; i < (int)Rank.RANKS; i++)
            {
                Highcards[i] = Utility.ApplyNormalized(Utility.CreateNomilized((Rank)i));
            }
        }
        public static ulong[] Highcards = new ulong[(int)Rank.RANKS];
        public static int Calculate()
        {
            return -1;
        }
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
                count = ulong.PopCount(hand & Highcards[i]); // counts the bits in the full applied normalied of the rank (:

                if (count >= 2)
                {
                    pairCount++;
                    if (i > highest)
                    {
                        highest = i;
                    }
                }
                if (count == (ulong)Constants.SUITES)// if we lets say have 4 aces we actually also have 2 pairs of aces
                {
                    return i;
                }
            }
            if (pairCount >= 2)// if we have 2 or more pairs then return the highest pair
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
                {
                    return (int)Rank.Ace - i;
                }
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
        }
        #endregion
    }
    public static class Utility
    {
        #region Bit
        public static ulong CreateCard(Rank rank, Suit suit)
        {
            return SetBit(rank, suit);
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
}
