using System.Collections.Generic;
using CigarsAndGunsHouse.Model;

namespace CigarsAndGunsHouse.Data.Repositories
{
    public class ProfileRepository: IRepository<Profile>
    {
        private List<Profile> _profiles = new List<Profile>();
        
        public void Add(Profile value)
        {
            _profiles.Add(value);
        }

        public Profile GetById(string id)
        {
            return _profiles.Find((profile) => profile.name == id);
        }

        public List<Profile> GetAll()
        {
            return _profiles;
        }

        public Profile Update(Profile value)
        {
            Profile profile = GetById(value.name);
            profile.name = value.name;
            profile.password = value.password;
            profile.auctions = value.auctions;
            profile.items = value.items;
            return profile;
        }

        public Profile Remove(Profile value)
        {
            _profiles.Remove(value);
            return value;
        }
    }
}