using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace CigarsAndGunsHouse
{
    public class ClientHandler
    {
        private readonly AuctionHouse _auctionHouse;
        private readonly TcpClient _client;
        private string _clientName;

        public ClientHandler(TcpClient client, AuctionHouse auctionHouse)
        {
            _auctionHouse = auctionHouse;
            _client = client;

            // connects to the broadcast
            _auctionHouse.BroadcastEvent += _broadcastEvent;
        }

        public void RunClient()
        {
            try {
                NetworkStream stream = _client.GetStream();
                StreamReader reader = new StreamReader(stream);
                StreamWriter writer = new StreamWriter(stream);

                // Welcome message
                writer.WriteLine("Welcome to the auction house. Type your name press enter to join the auctions.");
                writer.Write("Name: ");
                writer.Flush();
                
                _clientName = reader.ReadLine();
                
                // Notify other clients that a new user has joined
                _auctionHouse.BroadcastMessage($"{_clientName} has joined the auction.");

                while (true) {
                    // Read client's command
                    string command = reader.ReadLine();
                    if (command == null) {
                        break;
                    }

                    // ******
                    // TODO: Implement user commands
                    // ******
                    switch (command)
                    {
                        case "":
                            _auctionHouse.BroadcastMessage($"{_clientName}: {command}");
                            break;
                        default:
                            _auctionHouse.BroadcastMessage($"{_clientName}: {command}");
                            break;
                    }

                    // Exit if client types "exit" command
                    if (command.Equals("exit", StringComparison.OrdinalIgnoreCase)) {
                        break;
                    }
                }

                _client.Close();
                Console.WriteLine($"Client {((IPEndPoint)_client.Client.RemoteEndPoint).Address} disconnected.");

                // Notify other clients that a user has left
                _auctionHouse.BroadcastMessage($"User {((IPEndPoint)_client.Client.RemoteEndPoint).Address} has left the auction.");
            }
            catch (IOException ex) {
                // Handle connection reset by peer exception
                if (ex.InnerException != null && ex.InnerException.GetType() == typeof(SocketException)) {
                    SocketException socketEx = (SocketException)ex.InnerException;
                    if (socketEx.ErrorCode == 10054) {
                        Console.WriteLine($"Client {((IPEndPoint)_client.Client.RemoteEndPoint).Address} disconnected.");
                    }
                }
                else {
                    Console.WriteLine($"Error handling client {((IPEndPoint)_client.Client.RemoteEndPoint).Address}: {ex.Message}");
                }
            }
            catch (Exception ex) {
                Console.WriteLine($"Error handling client {((IPEndPoint)_client.Client.RemoteEndPoint).Address}: {ex.Message}");
            }
            finally {
                _client.Close();
                Console.WriteLine($"Client {((IPEndPoint)_client.Client.RemoteEndPoint).Address} disconnected.");

                _auctionHouse.BroadcastEvent -= _broadcastEvent;
            }
        }
        
        private void _broadcastEvent(string message)
        {
            // Setup streams for input and output between the client and the server
            var stream = _client.GetStream();
            var writer = new StreamWriter(stream);
            
            // Sends message
            writer.WriteLine(message);
            writer.Flush();
        }
    }
}