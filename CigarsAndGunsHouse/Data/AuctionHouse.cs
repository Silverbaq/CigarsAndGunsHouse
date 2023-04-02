using System.Threading;
using CigarsAndGunsHouse.Controllers;
using CigarsAndGunsHouse.Model;

namespace CigarsAndGunsHouse
{
    public class AuctionHouse
    {
        public delegate void BroadcastDelegate(string message);
        public event BroadcastDelegate BroadcastEvent;

        private Auction _currentAuction = null;
        private int _currentAuctionTimeLeft = 0;
        
        private ProfileController _profileController = new ProfileController();
        private AuctionController _auctionController = new AuctionController();

        public AuctionHouse()
        {
            TestData();
        }

        private void TestData()
        {
            _profileController.AddProfile("steffen", "123");
            Profile profile = _profileController.Login("steffen", "123");
            _profileController.AddItem("something", "it works fine!", profile);
        }

        // TODO: implement auction house 
        public string CreateProfile(string name, string password)
        {
            bool created = _profileController.AddProfile(name, password);
            if (created) return $"{name} was created. Please login";
            return "Profile already exists";
        }

        public Profile Login(string name, string password)
        {
            return _profileController.Login(name, password);
        }

        public bool CreateItem(string title, string description, Profile profile)
        {
            Item item = _profileController.AddItem(title, description, profile);
            if (item == null) return false;
            return true;
        }
        
        public bool AddAuction(int startingPrice, int timeRunning, string itemId, Profile profile)
        {
            Item item = profile.items.Find((x) => x.id == itemId);
            Auction auction = _auctionController.AddAuction(startingPrice: startingPrice, timeRunning: timeRunning, item: item, profile);
            return auction != null;
        }

        public string GetCurrentAuction()
        {
            string message = $"Auction: {_currentAuction.item.title}\n" +
                             $"Current bid: {_currentAuction.currentBid}\n" +
                             $"Current winner {_currentAuction.winner.name}\n" +
                             $"Time left: {_currentAuctionTimeLeft}\n\n";
            return message;
        }

        public bool Bid(int amount, Profile profile)
        {
            bool result = _currentAuction.Bid(profile, amount);
            if (result)
            {
                BroadcastMessage($"{profile.name} just bid {amount} on {_currentAuction.item.title}");
            }
            if (result && _currentAuctionTimeLeft < 10)
            {
                _currentAuctionTimeLeft = 15;
            }

            return result;
        }
        
        public void RunAuctions()
        {
            while (true)
            {
                _currentAuction = _auctionController.GetNextAuction();
                if (_currentAuction == null)
                {
                    BroadcastMessage("There is no current auctions. We will get back in 1 min.");
                    Thread.Sleep(60_000);
                }
                else
                {
                    BroadcastMessage($"A new auction is starting\n" +
                                     $"Item: {_currentAuction.item.title}\n" +
                                     $"Starting at: {_currentAuction.startingPrice}\n" +
                                     $"Is running for: {_currentAuction.timeRunning}");

                    _currentAuctionTimeLeft = _currentAuction.timeRunning;
                    while (!_currentAuction.finished)
                    {
                        _currentAuctionTimeLeft -= 1;
                        if (_currentAuctionTimeLeft == 0)
                        {
                            _currentAuction.finished = true;
                            BroadcastMessage($"Gravel: Sold!");
                            BroadcastMessage($"{_currentAuction.winner} won {_currentAuction.item.title} for {_currentAuction.currentBid}");
                        }
                        else
                        {
                            if (_currentAuctionTimeLeft == 10)
                            {
                                BroadcastMessage("Gravel: Going once!");
                            } else if (_currentAuctionTimeLeft == 5)
                            {
                                BroadcastMessage($"Gravel: Going Twice!");
                            }

                            Thread.Sleep(1_000);
                        }
                    }
                }
            }
        }
        
        public void BroadcastMessage(string message)
        {
            BroadcastEvent?.Invoke(message);
        }
    }
}