using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace CigarsAndGunsHouse
{
    public class SocketServer {
        private readonly TcpListener _listener;
        private static AuctionHouse _auctionHouse;
        
        public SocketServer(int port)
        {
            _auctionHouse = new AuctionHouse();
            _listener = new TcpListener(IPAddress.Any, port);
        }

        private void AcceptClients() {
            try {
                while (true) {
                    TcpClient client = _listener.AcceptTcpClient();
                    Console.WriteLine($"Client {((IPEndPoint)client.Client.RemoteEndPoint).Address} connected.");

                    ClientHandler handler = new ClientHandler(client, _auctionHouse);
                    Thread clientThread = new Thread(() => handler.RunClient());
                    clientThread.Start();
                }
            }
            catch (Exception ex) {
                Console.WriteLine($"Error accepting clients: {ex.Message}");
            }
        }

        public void Start() {
            _listener.Start();
            Console.WriteLine($"Server started. Listening on port {((IPEndPoint)_listener.LocalEndpoint).Port}...");

            AcceptClients();
        }
    }
}