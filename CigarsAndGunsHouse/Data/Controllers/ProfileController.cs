using System;
using System.Collections.Generic;
using CigarsAndGunsHouse.Model;
using CigarsAndGunsHouse.Repositories;

namespace CigarsAndGunsHouse.Controllers
{
    public class ProfileController
    {
        //private List<Profile> profiles = new List<Profile>();
        private ProfileRepository _profileRepository;
        private ItemRepository _itemRepository;

        public ProfileController(ProfileRepository profileRepository, ItemRepository itemRepository)
        {
            _profileRepository = profileRepository;
            _itemRepository = itemRepository;
        }
        
        
        public bool AddProfile(string name, string password)
        {
            Profile profile = new Profile(name, password);

            try
            {
                _profileRepository.Add(profile);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
            
            return true;
        }

        public Profile Login(string name, string password)
        {
            Profile profile = _profileRepository.GetById(name);
            if (profile != null && profile.password == password) return profile;
            return null;
        }

        public Item AddItem(string title, string description, string profileName)
        {
            Profile profile = _profileRepository.GetById(profileName);
            Item item = new Item(title, description, profile);
            
            _itemRepository.Add(item);
            
            profile.AddItem(item);
            return item;
        }
    }
}