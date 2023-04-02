using System.Collections.Generic;
using CigarsAndGunsHouse.Model;

namespace CigarsAndGunsHouse.Controllers
{
    public class AuctionController
    {
        private Queue<Auction> auctions = new Queue<Auction>();

        public Auction AddAuction(int startingPrice, int timeRunning, Item item, Profile profile)
        {
            var auction = new Auction(startingPrice: startingPrice, timeRunning: timeRunning, item: item, seller: profile);
            auctions.Enqueue(auction);
            return auction;
        }

        public Auction GetNextAuction()
        {
            if (auctions.Count == 0) return null;
            return auctions.Dequeue();
        }
    }
}