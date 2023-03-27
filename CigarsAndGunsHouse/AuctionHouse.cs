namespace CigarsAndGunsHouse
{
    public class AuctionHouse
    {
        public delegate void BroadcastDelegate(string message);
        public event BroadcastDelegate BroadcastEvent;
        
        // TODO: implement auction house 
        
        public void BroadcastMessage(string message)
        {
            BroadcastEvent?.Invoke(message);
        }
    }
}