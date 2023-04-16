using System.Collections.Generic;
using CigarsAndGunsHouse.Model;

namespace CigarsAndGunsHouse.Repositories
{
    public class AuctionRepository : IRepository<Auction>
    {
        private List<Auction> _auctions = new List<Auction>();
        private int _idCount = 0;
        
        public void Add(Auction value)
        {
            value.id = _idCount.ToString();
            _auctions.Add(value);
            _idCount++;
        }

        public Auction GetById(string id)
        {
            return _auctions.Find(auction => auction.id == id);
        }

        public List<Auction> GetAll()
        {
            return _auctions;
        }

        public Auction Update(Auction value)
        {
            Auction auction = GetById(value.id);
            auction.timeRunning = value.timeRunning;
            auction.item = value.item;
            auction.currentBid = value.currentBid;
            auction.finished = value.finished;
            auction.seller = value.seller;
            auction.winner = value.winner;
            auction.startingPrice = value.startingPrice;
            return auction;
        }

        public Auction Remove(Auction value)
        {
            _auctions.Remove(value);
            return value;
        }
    }
}