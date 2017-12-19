using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace Client
{
    public class GameClient
    {
        private static string address;
        private static int port;
        
        public static int Main(string[] args)
        {
            try
            {
                try
                {
                    IPAddress.Parse(args[0]);
                    address = args[0];
                    port = Int32.Parse(args[1]);
                }
                catch
                {
                    Console.Error.WriteLine("Wrong parameters...\nHELP:\n1st parameter: ip address\n2nd parameter: port");
                    return 84;
                }
                Server.NetworkUser server = new Server.NetworkUser((new TcpClient(address, port)).Client);
                Console.WriteLine("Hello World! Trying to reach server on address " + address + ":" + port + ".\nType 'quit' to make a platypus appear!");
                while (true)
                {
                    string message = server.GetReader().ReadLine();
                    if (message.Equals("alive"))
                        continue;
                    bool answer = (message != "" && message[0] == 'W');
                    Console.WriteLine(message);
                    if (answer)
                    {
                        string input = Console.ReadLine();
                        if (input == "quit")
                        {
                            Console.WriteLine(
                                "      .--..-'''-''''-._\n  ___/%   ) )      \\ i-;;,_\n((:___/--/ /--------\\ ) `'-'\n         \"\"          \"\"");
                            break;
                        }
                        server.GetWriter().WriteLine(input);
                    }
                }
            }
            catch (SocketException)
            {
                Console.Error.WriteLine("No server found...");
                return 84;
            }
            catch (Exception)
            {
                Console.Error.WriteLine("Server disconnected...");
                return 84;
            }
            return 0;
        }
    }
}