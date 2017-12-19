using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Server
{
    public class        Game
    {
        public Game(int id, List<NetworkUser> users)
        {
            this.id = id;
            deck = new List<Card>();
            players = new List<Player>();
            foreach (NetworkUser n in users)
            {
                players.Add(new Player(players.Count, (players.Count % 2 == 0 ? Team.FIRST : Team.SECOND), n));
                Network.SendPrivate(players.Last(), "Hello there! You are player " + (players.Count - 1)+ " and you are in the " + ((players.Count - 1) % 2 == 0 ? Team.FIRST : Team.SECOND) + " team.");
                Network.SendAll(players, "Player " + (players.Count - 1) + " joined and he is in the " + ((players.Count - 1)% 2 == 0 ? Team.FIRST : Team.SECOND) + " team.");
            }
            bid = new Bid();
        }
    
        private List<Card>    deck;
        private List<Player>  players;
        private Bid           bid;
        private readonly int  id;

        public int GetId()
        {
            return id;
        }
    
        public List<Player> getPlayers()
        {
            return players;
        }
    
        public void addPlayer(Player player)
        {
            players.Add(player);
        }
    
        public List<NetworkUser> runGame()
        {
            List<NetworkUser> res = new List<NetworkUser>();
            try
            {
                distribution();
                auction();
                playTheGame();
                countScore();
            }
            catch (UserQuit e)
            {
                Console.Error.WriteLine(e.Message);
            }
            finally
            {
                foreach (Player p in players)
                {
                    if (p.getNetworkUser().IsAlive())
                        res.Add(p.getNetworkUser());
                }
            }
            return res;
        }
    
        private void distribution() {
            foreach (Value i in Enum.GetValues(typeof(Value))) {
                foreach (Suit j in Enum.GetValues(typeof(Suit))) {
                    deck.Add(new Card(j, i));
                }
            }
            Random r = new Random();
            foreach (Player player in players) {
                for (int a = 0; a < 8; a += 1) {
                    int x = r.Next(0, deck.Count);
                    player.addCard(deck.ElementAt(x));
                    deck.Remove(deck.ElementAt(x));
                }
            }
        }
    
        private void auction()
        {
            int passed = 0;
            while (passed != 4) {
                passed = 0;
                foreach (Player player in players) {
                    bid = player.auction(bid);
                    if (player.didYouPassed())
                        passed += 1;
                }
            }
            Network.SendAll(players, "Auction is over!");
        }
    
        private int whoWon(List<Card> board)
        {
            int winner = -1;
            for (int i = 0; i != board.Count; i += 1)
            {
                if (i == 0)
                    winner = i;
                else
                {
                    if (board.ElementAt(i).getSuit() == bid.getSuit())
                    {
                        if (board.ElementAt(winner).getSuit() == bid.getSuit())
                        {
                            if (Array.IndexOf(Card.withAsset, board.ElementAt(i).getValue()) > Array.IndexOf(Card.withAsset, board.ElementAt(winner).getValue()))
                                winner = i;
                        }
                        else
                            winner = i;
                    }
                    else if (board.ElementAt(winner).getSuit() != bid.getSuit() && Array.IndexOf(Card.withAsset, board.ElementAt(i).getValue()) > Array.IndexOf(Card.withoutAsset, board.ElementAt(winner).getValue()))
                        winner = i;
                }
            }
            Network.SendAll(players, "  --  Player " + winner + " won this trick!  --  ");
            players.ElementAt(winner).addWonTrick(board);
            return (winner);
        }
    
        private void playTheGame()
        {
            List<Card> board = new List<Card>();
            int b = 0;
            for (int a = 0; a != 8; a += 1)
            {
                board.Clear();
                int c = 0;
                while (c != 4) {
                    board.Add(players.ElementAt(b).play(board, bid));
                    c += 1;
                    b = (b == 3 ? 0 : b + 1);
                }
                b = whoWon(board);
            }
        }
    
        private void countScore()
        {
            int team1 = 0;
            int team2 = 0;
            foreach (Player i in players) {
                i.countScore();
                Network.SendAll(players, "Player " + i.getId() + " of the " + i.getTeam() + " team got " + i.getScore() + " points.");
                if (i.getTeam() == Team.FIRST)
                    team1 += i.getScore();
                else
                    team2 += i.getScore();
            }
            Network.SendAll(players, Team.FIRST  + " team got " + team1 + " points.");
            Network.SendAll(players, Team.SECOND + " team got " + team2 + " points.");
            switch (bid.getStatus()) {
                case Status.CAPOT:
                    Network.SendAll(players, "The bid was capot by " + bid.getTeam() + " team.");
                    if ((bid.getTeam() == Team.FIRST ? team2 : team1) != 0)
                        Network.SendAll(players, bid.getTeam() + " failed their capot. " + (bid.getTeam() == Team.FIRST ? Team.SECOND : Team.FIRST) + " won.");
                    else
                        Network.SendAll(players, bid.getTeam() + " achieved their capot. They won.");
                    break;
                case Status.COINCH:
                    Network.SendAll(players, "The bid was a coinch by the " + bid.getTeam() + " team on a bet of " + bid.getAmount() + " points.");
                    if ((bid.getTeam() == Team.FIRST ? team2 : team1) <= bid.getAmount())
                        Network.SendAll(players, (bid.getTeam() == Team.FIRST ? Team.SECOND : Team.FIRST) + " failed their contract and " + bid.getTeam() + " conched. " + bid.getTeam() + " won.");
                    else
                        Network.SendAll(players, (bid.getTeam() == Team.FIRST ? Team.SECOND : Team.FIRST) + " achieved their contract and " + bid.getTeam() + " conched. " + (bid.getTeam() == Team.FIRST ? Team.SECOND : Team.FIRST) + " won.");
                    break;
                case Status.SURCOINCH:
                    Network.SendAll(players, "The bid was a surcoinch by the " + bid.getTeam() + " team on a bet of " + bid.getAmount() + " points.");
                    if ((bid.getTeam() == Team.FIRST ? team1 : team2) < bid.getAmount())
                        Network.SendAll(players, bid.getTeam() + " failed their contract and they surcoinched. " + (bid.getTeam() == Team.FIRST ? Team.SECOND : Team.FIRST) + " won.");
                    else
                        Network.SendAll(players, bid.getTeam() + " achieved their contract and they surcoinched. "  + bid.getTeam() + " won!");
                    break;
                case Status.AMOUNT:
                    Network.SendAll(players, "The bid was " + bid.getAmount() + " points by the " + bid.getTeam() + " team.");
                    if ((bid.getTeam() == Team.FIRST ? team1 : team2) >= bid.getAmount())
                        Network.SendAll(players, bid.getTeam() + " managed to fulfill their contract. They won!");
                    else
                        Network.SendAll(players, bid.getTeam() + " failed to fulfill their contract. " + (bid.getTeam() == Team.FIRST ? Team.SECOND : Team.FIRST) + " won!");
                    break;
            }
        }
    }
}