using System.Collections.Generic;

namespace CigarsAndGunsHouse.Model
{
    public class Profile
    {
        public string name;
        public string password;
        public List<Auction> auctions;
        public List<Item> items;

        public Profile(string name, string password)
        {
            this.name = name;
            this.password = password;
            auctions = new List<Auction>();
            items = new List<Item>();
        }

        public void AddAuction(Auction auction)
        {
            auctions.Add(auction);
        }

        public void AddItem(Item item)
        {
            items.Add(item);
        }
    }
}