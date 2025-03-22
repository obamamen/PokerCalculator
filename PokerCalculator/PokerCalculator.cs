using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using System.Net.Security;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PokerCalculator
{
    public enum Suit
    {
        Hearts,
        Diamonds,
        Clubs,
        Spades
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
        Ace
    }
    public static class Calculator
    {
        public static int Calculate()
        {
            return -1;
        }
    }
    public static class Utility
    {
        #region Publics
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
        #endregion

        #region Privates
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static ulong SetBit(int pos)
        {
            return (1UL << pos);
        }
        internal static ulong SetBit(Rank rank, Suit suit)
        {
            return SetBit((int)rank + ((int)suit * 16));
        }
        internal static string ToBinaryString(ulong value)
        { 
            return Convert.ToString((long)value, 2).PadLeft(64, '0');
        }
        #endregion
    }
}
