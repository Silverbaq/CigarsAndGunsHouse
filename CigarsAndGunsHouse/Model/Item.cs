using System;

namespace CigarsAndGunsHouse.Model
{
    public class Item
    {
        public string id;
        public string title;
        string description;
        private Profile owner;

        public Item(string title, string description, Profile owner)
        {
            this.id = GenerateId();
            this.title = title;
            this.description = description;
            this.owner = owner;
        }

        private string GenerateId()
        {
            Random res = new Random();
            
            String str = "abcdefghijklmnopqrstuvwxyz";
            int size = 5;
            
            String id = "";
  
            for (int i = 0; i < size; i++)
            {
                int x = res.Next(26);
                id += str[x];
            }

            return id;
        }

        public override string ToString()
        {
            return $"id: {id}, title: {title}, description: {description}";
        }
    }
}