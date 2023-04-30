using CigarsAndGunsHouse.Data;

namespace CigarsAndGunsHouse
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            SocketServer server = new SocketServer(9001);
            server.Start();
        }
    }
}