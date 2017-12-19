using System;
using System.Collections.Generic;
using System.Numerics;
using Xunit;
using Server;

namespace Test
{
    public class CardTest
    {
        [Fact]
        public void TestSuit()
        {
            Card card= new Card(Suit.HEART, Value.ACE);
            List<Card> deck = new List<Card>();
            deck.Add(new Card(Suit.HEART, Value.ACE));
            deck.Add(new Card(Suit.SPADE, Value.EIGHT));
            deck.Add(new Card(Suit.DIAMOND, Value.QUEEN));
            deck.Add(new Card(Suit.CLUB, Value.ACE));
            Assert.True(card.doYouHaveSuit(deck));
            Console.WriteLine("\n---------------------------" +
                              "Suit Test succeed" +
                              "---------------------------\n");
        }

        [Fact]
        public void TestNoSuit()
        {
            Card card= new Card(Suit.HEART, Value.ACE);
            List<Card> deck = new List<Card>();
            deck.Add(new Card(Suit.SPADE, Value.EIGHT));
            deck.Add(new Card(Suit.DIAMOND, Value.QUEEN));
            deck.Add(new Card(Suit.CLUB, Value.ACE));
            Assert.False(card.doYouHaveSuit(deck));
            Console.WriteLine("\n---------------------------" +
                              "NoSuit Test succeed" +
                              "---------------------------\n");
        }
    }
}