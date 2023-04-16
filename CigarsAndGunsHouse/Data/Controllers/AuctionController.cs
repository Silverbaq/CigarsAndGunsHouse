using System.Collections.Generic;
using System.Linq;
using CigarsAndGunsHouse.Model;
using CigarsAndGunsHouse.Repositories;

namespace CigarsAndGunsHouse.Controllers
{
    public class AuctionController
    {
        private Queue<Auction> auctions = new Queue<Auction>();
        private Auction _currentAuction = null;
        private AuctionRepository _auctionRepository;
        
        
        public AuctionController(AuctionRepository auctionRepository)
        {
            _auctionRepository = auctionRepository;
        }

        public Auction AddAuction(int startingPrice, int timeRunning, Item item, Profile profile)
        {
            var auction = new Auction(startingPrice: startingPrice, timeRunning: timeRunning, item: item, seller: profile);
            
            _auctionRepository.Add(auction);
            auctions.Enqueue(auction);
            return auction;
        }

        public Auction GetCurrentAuction()
        {
            return _currentAuction;
        }

        public bool StartNextAuction()
        {
            _currentAuction = _auctionRepository.GetAll().First(auction => auction.finished == false);
            if (_currentAuction == null) return false;
            return true;
        }
    }
}