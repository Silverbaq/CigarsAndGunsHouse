namespace CigarsAndGunsHouse.Model
{
    public class Item
    {
        string title;
        string description;
        private Profile owner;

        public Item(string title, string description, Profile owner)
        {
            this.title = title;
            this.description = description;
            this.owner = owner;
        }
    }
}