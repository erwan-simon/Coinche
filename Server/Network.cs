using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace Server
{
    public class Network
    {
        private static TcpListener myListner;

        public Network(string address, int port)
        {
            myListner = new TcpListener(IPAddress.Parse(address), port);
            myListner.Start();            
        }
        
        public static void SendPrivate(Player player, string message)
        {
            try
            {
                player.getNetworkUser().GetWriter().WriteLine(message);
            }
            catch (Exception)
            {
                UserQuit e = new UserQuit("A player quitted...");
                throw e;
            }
        }

        public static void SendAll(List<Player> players, string message) {
            try
            {
                foreach (Player p in players)
                {
                    p.getNetworkUser().GetWriter().WriteLine(message);
                }
                Console.WriteLine("To all: " + message);
            }
            catch (Exception)
            {
                UserQuit e = new UserQuit("A player quitted...");
                throw e;
            }
        }

        public static NetworkUser AddUser()
        {
            if (myListner.Pending())
                return new NetworkUser(myListner.AcceptSocket());
            return null;
        }

        public static List<NetworkUser> SendNetworks(List<NetworkUser> users, string message)
        {
            List<NetworkUser> usersCopy = users.ToList();
            
            foreach (NetworkUser n in usersCopy)
            {
                try
                {
                    n.GetWriter().WriteLine(message);
                    if (message != "alive")
                        Console.WriteLine("message to networkUser send: " + message);
                }
                catch (Exception)
                {
                    
                    users.Remove(n);
                }
            }
            return users;
        }

        public TcpListener GetMyListener()
        {
            return myListner;
        }
    }
}