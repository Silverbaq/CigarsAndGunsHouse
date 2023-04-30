using System.Collections.Generic;
using CigarsAndGunsHouse.Model;

namespace CigarsAndGunsHouse.Data.Repositories
{
    public class ItemRepository : IRepository<Item>
    {
        private List<Item> _items = new List<Item>();
        
        public void Add(Item value)
        {
            _items.Add(value);
        }

        public Item GetById(string id)
        {
            return _items.Find(item => item.id == id);
        }

        public List<Item> GetAll()
        {
            return _items;
        }

        public Item Update(Item value)
        {
            Item item = GetById(value.id);
            item.title = value.title;
            item.description = value.description;
            item.owner = value.owner;
            return item;
        }

        public Item Remove(Item value)
        {
            _items.Remove(value);
            return value;
        }
    }
}