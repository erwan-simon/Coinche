using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;

namespace Server
{
    public class        Player 
    {
        public Player(int id, Team team, NetworkUser networkUser)
        {
            passed = false;
            this.id = id;
            cards = new List<Card>();
            this.team = team;
            wonTricks = new List<Card>();
            score = 0;
            this.networkUser = networkUser;
        }
    
        private int           id;
        private List<Card>    cards;
        private Team          team;
        private List<Card>    wonTricks;
        private bool          passed;
        private int           score;
        private NetworkUser   networkUser;
    
    
        public NetworkUser getNetworkUser()
        {
            return networkUser;
        }
    
        public void setNetworkUser(NetworkUser networkUser)
        {
            this.networkUser = networkUser;
        }
    
        public bool  subCard(Card card)
        {
            cards.Remove(card);
            return true;
        }
    
        public void  addCard(Card card)
        {
            cards.Add(card);
        }
    
        public int getScore()
        {
            return score;
        }
    
        public void setScore(int score)
        {
            this.score = score;
        }
    
        public int      getId()
        {
            return id;
        }
    
        public List<Card>   getCards()
        {
            return cards;
        }
    
        public Team getTeam()
        {
            return team;
        }

        public bool didYouPassed()
        {
            return passed;
        }
    
        public void addWonTrick(List<Card> trick)
        {
            foreach (Card i in trick)
            {
                wonTricks.Add(i);
            }
        }
    
        public int WaitForMessage()
        {
            string answer = null;
            int res = 0;
            while (true)
            {
                answer = networkUser.GetReader().ReadLine();
                if (Int32.TryParse(answer, out res))
                    break;
                Network.SendPrivate(this, "What are you trying to do?");
            }
            return res;
        }
    
        private bool playCheck(List<Card> board, int n, Bid bid)
        {
            try
            {
                cards.ElementAt(n);
                if (board.Count == 0)
                    return true;
                if (cards.ElementAt(n).getSuit() == board.ElementAt(0).getSuit())
                    return true;
                if (board.ElementAt(0).doYouHaveSuit(cards))
                    return false;
                if (cards.ElementAt(n).getSuit() == bid.getSuit())
                {
                    /*if (cards.elementAt(n).isItBiggest(board, true))
                        return true;
                    else
                        return (!cards.elementAt(n).isItBiggest(cards, true));*/
                    return true;
                }
                Card card = new Card(bid.getSuit(), Value.ACE);
                return !card.doYouHaveSuit(cards);
            }
            catch (Exception)
            {
                Network.SendPrivate(this, "Out of range!");
                return false;
            }
        }
    
        public Card play(List<Card> board, Bid bid)
        {
            if (board.Count != 0)
                Network.SendPrivate(this, "The current board is composed of :");
            foreach (Card i in board)
            {
                Network.SendPrivate(this, " - " + i.getValue() + " of " + i.getSuit());
            }
            Network.SendPrivate(this, "Player " + this.id + "! Your turn! The asset's suit is " + bid.getSuit() + ".");
            Network.SendPrivate(this, "Your current cards:");
            for (int j = 0; j != cards.Count; j += 1)
            {
                Network.SendPrivate(this, j + " -> " + cards.ElementAt(j).getValue() + " of " + cards.ElementAt(j).getSuit());
            }
            Network.SendPrivate(this, "What is the number of the card you want to play?");
            int n = 0;
            bool end = false;
            while (!end)
            {
                n = WaitForMessage();
                if (!(end = playCheck(board, n, bid)))
                    Network.SendPrivate(this, "Wait... You can't play this card!");
            }
            Network.SendPrivate(this, "Great!");
            Card res = cards.ElementAt(n);
            cards.Remove(cards.ElementAt(n));
            return res;
        }
    
        public Bid auction(Bid bid) {
            int n;
            bool end = false;
            if (bid.getAmount() != 0)
                Network.SendPrivate(this, "The actual bill is " + bid.getAmount() + " " + bid.getSuit() + " from " + bid.getTeam() + " team.\n");
            else
                Network.SendPrivate(this, "You are the first one to bid!");
            Network.SendPrivate(this, "Player " + id + " turn to bid:");
            while (!end)
            {
                string str = "What do you want to do? ";
                switch (bid.getStatus()) {
                    case Status.AMOUNT:
                        str += "1->bid ";
                        goto case Status.CAPOT;
                    case Status.CAPOT:
                        str += "2->coinche ";
                        goto case Status.COINCH;
                    case Status.COINCH:
                        if (bid.getStatus() == Status.COINCH)
                            str += "3->surcoinche ";
                        break;
                    case Status.SURCOINCH:
                        Network.SendPrivate(this, "You can't bid anymore.");
                        passed = true;
                        return (bid);
                }
                if (bid.getStatus() == Status.AMOUNT)
                    str += "4->capot ";
                Network.SendPrivate(this, str + "5->pass");
                n = WaitForMessage();
                end = true;
                switch (n) {
                    case 1:
                        Network.SendPrivate(this, "What is the amount of your bid ? (>= " + (bid.getAmount() == 0 ? 80 : bid.getAmount() + 10) + ")");
                        if (!bid.setAmount(WaitForMessage()) || bid.getStatus() != Status.AMOUNT) {
                            Network.SendPrivate(this, "Incorrect input");
                            end = false;
                            break;
                        }
                        Network.SendPrivate(this, "What is the suit ? (0->CLUB | 1->DIAMOND | 2->HEART | 3->SPADE)");
                        n = WaitForMessage();
                        if (n < 0 || n > 3) {
                            Network.SendPrivate(this, "Incorrect input");
                            end = false;
                            break;
                        }
                        bid.setSuit(n);
                        bid.setTeam(this.team);
                        passed = false;
                        break;
                    case 2:
                        if (bid.getStatus() != Status.AMOUNT && bid.getStatus() != Status.CAPOT) {
                            Network.SendPrivate(this, "Incorrect input");
                            end = false;
                            break;
                        }
                        bid.setStatus(Status.COINCH);
                        passed = false;
                        break;
                    case 3:
                        if (bid.getStatus() != Status.COINCH) {
                            Network.SendPrivate(this, "Incorrect input");
                            end = false;
                            break;
                        }
                        bid.setStatus(Status.SURCOINCH);
                        passed = false;
                        break;
                    case 4:
                        if (bid.getStatus() == Status.COINCH || bid.getStatus() == Status.SURCOINCH || bid.getStatus() == Status.CAPOT) {
                            Network.SendPrivate(this, "Incorrect input");
                            end = false;
                            break;
                        }
                        bid.setStatus(Status.CAPOT);
                        bid.setTeam(team);
                        passed = false;
                        break;
                    case 5:
                        passed = true;
                        break;
                    default:
                        Network.SendPrivate(this, "Incorrect input");
                        end = false;
                        break;
                }
            }
            Network.SendPrivate(this, "Great!");
            return bid;
        }
    
        public void countScore()
        {
            foreach (Card i in wonTricks)
            {
                score += Card.scoreValue[Array.IndexOf(Card.withAsset, i.getValue())];
            }
            wonTricks.Clear();
        }
    }
}