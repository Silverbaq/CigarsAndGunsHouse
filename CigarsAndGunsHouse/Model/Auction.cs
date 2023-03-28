namespace CigarsAndGunsHouse.Model
{
    public class Auction
    {
        Profile seller;
        Profile winner;
        int currentBid;
        int startingPrice;
        int timeRunning;
        Item item;

        public Auction(int startingPrice, int timeRunning, Item item, Profile seller)
        {
            winner = null;
            currentBid = startingPrice;
            this.startingPrice = startingPrice;
            this.timeRunning = timeRunning;
            this.item = item;
            this.seller = seller;
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
    }
}