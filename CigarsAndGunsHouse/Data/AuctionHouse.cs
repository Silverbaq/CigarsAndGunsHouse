using CigarsAndGunsHouse.Controllers;
using CigarsAndGunsHouse.Model;

namespace CigarsAndGunsHouse
{
    public class AuctionHouse
    {
        public delegate void BroadcastDelegate(string message);
        public event BroadcastDelegate BroadcastEvent;

        private ProfileController _profileController = new ProfileController();
        
        
        // TODO: implement auction house 
        public string CreateProfile(string name, string password)
        {
            bool created = _profileController.AddProfile(name, password);
            if (created) return $"{name} was created. Please login";
            return "Profile already exists";
        }

        public string Login(string name, string password)
        {
            Profile profile = _profileController.Login(name, password);
            if (profile == null) return "Incorrect login";
            return $"Welcome {profile.name}";
        }
        
        public void BroadcastMessage(string message)
        {
            BroadcastEvent?.Invoke(message);
        }
    }
}