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
                    writer.WriteLine("****** MENU ******\n" +
                                     "You have the following options:\n" +
                                     "[-]: create-profile\n" +
                                     "[-]: login\n" +
                                     "[-]: exit\n");
                    
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
                            string loginResult = _auctionHouse.Login(loginName, loginPassword);
                            writer.WriteLine(loginResult);
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