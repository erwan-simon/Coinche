using System;
using System.Collections.Generic;

namespace Server
{
    public class Card
    {
        public Card(Suit suit, Value value)
        {
            this.suit = suit;
            this.value = value;
        }

        private readonly Suit suit;
        private readonly Value value;

        public static readonly Value[] withAsset =
            {Value.SEVEN, Value.EIGHT, Value.NINE, Value.TEN, Value.JACK, Value.QUEEN, Value.KING, Value.ACE};

        public static readonly Value[] withoutAsset =
            {Value.SEVEN, Value.EIGHT, Value.QUEEN, Value.KING, Value.TEN, Value.ACE, Value.NINE, Value.JACK};

        public static readonly int[] scoreValue = {0, 0, 0, 2, 3, 4, 10, 19};

        public Suit getSuit()
        {
            return suit;
        }

        public Value getValue()
        {
            return value;
        }

        public bool doYouHaveSuit(List<Card> deck)
        {
            foreach (Card i in deck)
            {
                if (i.getSuit() == suit)
                    return true;
            }
            return false;
        }

        public bool isItBiggest(List<Card> deck, bool asset)
        {
            foreach (Card i in deck)
            {
                if (asset)
                {
                    if (Array.IndexOf(withAsset, value) <= Array.IndexOf(withAsset, i.getValue()))
                        return true;
                }
                else
                {
                    if (Array.IndexOf(withAsset, value) <= Array.IndexOf(withAsset, i.getValue()))
                        return true;
                }
            }
            return false;
        }
    }
}