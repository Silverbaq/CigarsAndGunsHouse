using System.Collections.Generic;

namespace CigarsAndGunsHouse.Model
{
    public class Profile
    {
        public string name;
        public string password;
        public List<Auction> auctions;

        public Profile(string name, string password)
        {
            this.name = name;
            this.password = password;
            auctions = new List<Auction>();
        }

        public void AddAuction(Auction auction)
        {
            auctions.Add(auction);
        }
    }
}