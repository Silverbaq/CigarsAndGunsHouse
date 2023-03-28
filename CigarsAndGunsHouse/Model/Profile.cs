namespace CigarsAndGunsHouse.Model
{
    public class Profile
    {
        public string name;
        public string password;

        public Profile(string name, string password)
        {
            this.name = name;
            this.password = password;
        }
    }
}