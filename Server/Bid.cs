namespace Server
{
    public enum Status {
        AMOUNT,
        COINCH,
        SURCOINCH,
        CAPOT
    }
    
    public class            Bid {

        public Bid() {
            amount = 80;
            suit = Suit.HEART;
            status = Status.AMOUNT;
            team = Team.FIRST;
        }

        private int         amount;
        private Suit   suit;
        private Status      status;
        private Team        team;

        public int getAmount() {
            return amount;
        }

        public Suit getSuit() {
            return suit;
        }

        public Status getStatus() {
            return status;
        }

        public Team getTeam() {
            return team;
        }

        public bool setAmount(int amount) {
            if (this.amount >= amount || amount < 80)
                return false;
            this.amount = amount;
            return true;
        }

        public bool setSuit(int suit) {
            switch (suit) {
                case 0:
                    this.suit = Suit.CLUB;
                    break;
                case 1:
                    this.suit = Suit.DIAMOND;
                    break;
                case 2:
                    this.suit = Suit.HEART;
                    break;
                case 3:
                    this.suit = Suit.SPADE;
                    break;
            }
            return true;
        }

        public bool setStatus(Status status) {
            this.status = status;
            return true;
        }

        public bool setTeam(Team team) {
            this.team = team;
            return true;
        }
    }
}