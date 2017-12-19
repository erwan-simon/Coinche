using System;
using System.Net;
using System.Net.Sockets;

namespace Server
{
    public class GameServer
    {
        private static string address;
        private static int port;
        
        public static int Main(string[] args)
        {
            Network network;
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
            try
            {
                Console.WriteLine("Hello world!\nServer is running on address " + address + ":" + port + ".\nPress escape to quit (some say that a platypus will appear...)\nLauching lobby!");
                Console.WriteLine();
                network = new Network(address, port);
                Lobby lobby = new Lobby();
                lobby.Run();
                Console.WriteLine(
                    "      .--..-'''-''''-._\n  ___/%   ) )      \\ i-;;,_\n((:___/--/ /--------\\ ) `'-'\n         \"\"          \"\"");
                return 0;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Something happened... " + e.Message);
                return 84;
            }
        }
    }
}