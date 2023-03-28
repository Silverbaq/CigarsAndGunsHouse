using System.Collections.Generic;
using CigarsAndGunsHouse.Model;

namespace CigarsAndGunsHouse.Controllers
{
    public class ProfileController
    {
        private List<Profile> profiles = new List<Profile>();

        public bool AddProfile(string name, string password)
        {
            bool profileExists = profiles.Exists((x) => x.name == name);

            if (profileExists) return false;
            profiles.Add(new Profile(name, password));
            return true;
        }

        public Profile Login(string name, string password)
        {
            return profiles.Find((x) => x.name == name && x.password == password);
        }
    }
}