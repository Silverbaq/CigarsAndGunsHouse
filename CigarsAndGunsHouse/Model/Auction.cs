namespace CigarsAndGunsHouse.Model
{
    public class Auction
    {
        public string id;
        public Profile seller;
        public Profile winner;
        public int currentBid;
        public int startingPrice;
        public int timeRunning;
        public Item item;
        public bool finished;

        public Auction(int startingPrice, int timeRunning, Item item, Profile seller)
        {
            winner = null;
            currentBid = startingPrice;
            this.startingPrice = startingPrice;
            this.timeRunning = timeRunning;
            this.item = item;
            this.seller = seller;
            finished = false;
        }

        public bool Bid(Profile profile, int amount)
        {
            if (currentBid < amount)
            {
                winner = profile;
                currentBid = amount;
                return true;
            }

            return false;
        }

        public override string ToString()
        {
            return $"Auction: {item.title}\n" +
                   $"Current bid: {currentBid}\n" +
                   $"Current winner {winner.name}\n";
        }
    }
}