using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using CigarsAndGunsHouse.Model;

namespace CigarsAndGunsHouse
{
    public class ClientHandler
    {
        private readonly AuctionHouse _auctionHouse;
        private readonly TcpClient _client;
        private Profile _profile;

        public ClientHandler(TcpClient client, AuctionHouse auctionHouse)
        {
            _auctionHouse = auctionHouse;
            _client = client;

            // connects to the broadcast
            _auctionHouse.BroadcastEvent += _writeToClient;
        }

        public void RunClient()
        {
            try {
                NetworkStream stream = _client.GetStream();
                StreamReader reader = new StreamReader(stream);
                StreamWriter writer = new StreamWriter(stream);
                writer.AutoFlush = true;

                // Welcome message
                writer.WriteLine("Welcome to the auction house. Type your name press enter to join the auctions.");

                while (true) {
                    if (_profile == null)
                    {
                        writer.WriteLine("****** MENU ******\n" +
                                         "You have the following options:\n" +
                                         "[-]: create-profile\n" +
                                         "[-]: login\n" +
                                         "[-]: exit\n");
                        
                        // Read client's command
                        writer.Write("$: ");
                        string command = reader.ReadLine();
                        if (command == null) {
                            break;
                        }
                    
                        switch (command)
                        {
                            case "create-profile":
                                writer.Write("Name: ");
                                string createName = reader.ReadLine();
                                writer.Write("Password: ");
                                string createPassword = reader.ReadLine();
                                string createResult = _auctionHouse.CreateProfile(createName, createPassword);
                                writer.WriteLine(createResult);
                                break;
                            case "login":
                                writer.Write("Name: ");
                                string loginName = reader.ReadLine();
                                writer.Write("Password: ");
                                string loginPassword = reader.ReadLine();
                                _profile = _auctionHouse.Login(loginName, loginPassword);

                                writer.WriteLine(_profile == null ? "Incorrect login" : $"Welcome {_profile.name}!");
                                Thread.Sleep(2000);
                                break;
                            default:
                                _auctionHouse.BroadcastMessage($"{_profile.name}: {command}");
                                break;
                        }

                        // Exit if client types "exit" command
                        if (command.Equals("exit", StringComparison.OrdinalIgnoreCase)) {
                            break;
                        }
                    }
                    else
                    {
                        writer.WriteLine("****** Main menu ******\n" +
                                         "You have the following options:\n" +
                                         "[+]: create item\n" +
                                         "[+]: list items\n" +
                                         "[+]: current auction\n" +
                                         "[+]: add auction\n" +
                                         "[+]: bid\n" +
                                         "[+]: show wins\n" +
                                         "[+]: exit\n");
                        
                        // Read client's command
                        writer.Write("$: ");
                        string command = reader.ReadLine();
                        if (command == null) {
                            break;
                        }
                    
                        switch (command)
                        {
                            case "create item":
                                writer.Write("Title: ");
                                string title = reader.ReadLine();
                                writer.Write("Description: ");
                                string description = reader.ReadLine();
                                bool createResult = _auctionHouse.CreateItem(title: title, description: description, _profile);
                                if (createResult) writer.WriteLine($"Created successfully");
                                else writer.WriteLine("Something is wrong, please try again");
                                Thread.Sleep(1000);
                                break;
                            case "list items":
                                _profile.items.ForEach((item) =>
                                {
                                    writer.WriteLine(item);    
                                });
                                
                                Thread.Sleep(2000);
                                break;
                            case "current auction":
                                string currentAuctionMessage = _auctionHouse.GetCurrentAuction();
                                writer.Write($"{currentAuctionMessage}");
                                Thread.Sleep(1000);
                                break;
                            case "add auction":
                                writer.Write("Starting price: ");
                                int startingPrice = int.Parse(reader.ReadLine() ?? string.Empty);
                                
                                writer.Write("Auction time running, in seconds: ");
                                int timeRunning = int.Parse(reader.ReadLine() ?? string.Empty);

                                writer.Write("Item id: ");
                                string itemId = reader.ReadLine();
                                
                                bool result = _auctionHouse.AddAuction(startingPrice, timeRunning, itemId, _profile);
                                if (result) writer.WriteLine("Auction was created!");
                                else writer.WriteLine("Error! Try again.");
                                break;
                            case "bid":
                                writer.Write("Amount: ");
                                int amount = int.Parse(reader.ReadLine() ?? string.Empty);

                                bool bidResult = _auctionHouse.Bid(amount, _profile);
                                writer.WriteLine(bidResult ? "Bid was accepted!" : "Bid was to low");
                                break;
                            case "show wins":
                                break;
                            default:
                                _auctionHouse.BroadcastMessage($"{_profile.name}: {command}");
                                break;
                        }

                        // Exit if client types "exit" command
                        if (command.Equals("exit", StringComparison.OrdinalIgnoreCase)) {
                            break;
                        }
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

                _auctionHouse.BroadcastEvent -= _writeToClient;
            }
        }
        
        private void _writeToClient(string message)
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