using System.Threading;
using CigarsAndGunsHouse.Data.Controllers;
using CigarsAndGunsHouse.Data.Repositories;
using CigarsAndGunsHouse.Model;

namespace CigarsAndGunsHouse.Data
{
    public class AuctionHouse
    {
        public delegate void BroadcastDelegate(string message);
        public event BroadcastDelegate BroadcastEvent;
        
        private int _currentAuctionTimeLeft = 0;

        private static ProfileRepository _profileRepository = new ProfileRepository();
        private static ItemRepository _itemRepository = new ItemRepository();
        private static AuctionRepository _auctionRepository = new AuctionRepository();
        
        private ProfileController _profileController = new ProfileController(_profileRepository, _itemRepository);
        private AuctionController _auctionController = new AuctionController(_auctionRepository);

        public AuctionHouse()
        {
            TestData();
        }

        private void TestData()
        {
            _profileController.AddProfile("steffen", "123");
            Profile profile = _profileController.Login("steffen", "123");
            _profileController.AddItem("something", "it works fine!", profile.name);
            _profileController.AddItem("something else", "it works great!", profile.name);
            _profileController.AddItem("something completely different", "Fu*k! I does not work...", profile.name);
            
            profile.items.ForEach(item => _auctionController.AddAuction(100, 60, item, profile));
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
            Item item = _profileController.AddItem(title, description, profile.name);
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
            string title = _auctionController.GetCurrentAuction().item.title;
            string bid = _auctionController.GetCurrentAuction().currentBid.ToString(); 
            string name = _auctionController.GetCurrentAuction().winner?.name;
            
            string message = $"Auction: {title}\n" +
                             $"Current bid: {bid}\n" +
                             $"Current winner: {name}\n" +
                             $"Time left: {_currentAuctionTimeLeft}\n\n";
            return message;
        }

        public bool Bid(int amount, Profile profile)
        {
            bool result = _auctionController.GetCurrentAuction().Bid(profile, amount);
            if (result)
            {
                BroadcastMessage($"{profile.name} just bid {amount} on {_auctionController.GetCurrentAuction().item.title}");
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
                if (!_auctionController.StartNextAuction())
                {
                    BroadcastMessage("There is no current auctions. We will get back in 1 min.");
                    Thread.Sleep(60_000);
                }
                else
                {
                    BroadcastMessage($"A new auction is starting\n" +
                                     $"Item: {_auctionController.GetCurrentAuction().item.title}\n" +
                                     $"Starting at: {_auctionController.GetCurrentAuction().startingPrice}\n" +
                                     $"Is running for: {_auctionController.GetCurrentAuction().timeRunning}");

                    _currentAuctionTimeLeft = _auctionController.GetCurrentAuction().timeRunning;
                    while (!_auctionController.GetCurrentAuction().finished)
                    {
                        _currentAuctionTimeLeft -= 1;
                        if (_currentAuctionTimeLeft == 0)
                        {
                            _auctionController.GetCurrentAuction().finished = true;
                            BroadcastMessage($"Gravel: Sold!");
                            BroadcastMessage($"{_auctionController.GetCurrentAuction().winner} won {_auctionController.GetCurrentAuction().item.title} for {_auctionController.GetCurrentAuction().currentBid}");
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