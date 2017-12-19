using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Xml.Schema;
using Server;
using Xunit;

namespace Test
{
    public class ComTest
    {
        [Fact]
        public void AddUserTest()
        {
            Thread serverThread = new Thread(() =>
            {
                try
                {
                    Server.Network network = new Server.Network("127.0.0.1", 5006);

                    NetworkUser user = null;
                    while (user == null)
                        user = Server.Network.AddUser();
                    Assert.NotNull(user);
                }
                catch (Exception)
                {
                    Assert.False(true);
                }
            });
            Thread clientThread = new Thread(() =>
            {
                try
                {
                    Server.NetworkUser server = new Server.NetworkUser((new TcpClient("127.0.0.1", 5006)).Client);
                }
                catch (Exception)
                {
                    
                    Assert.False(true);
                }
            });
            try
            {
                serverThread.Start();
                Thread.Sleep(5000);
                clientThread.Start();
            }
            catch (UserQuit)
            {
                Console.WriteLine("Test ok");
            }
            catch (Exception)
            {
                Console.WriteLine("Something happened...");
            }
        }
        
        [Fact]
        public void SendPrivateTest()
        {
            Thread serverThread = new Thread(() =>
            {
                try
                {
                    Server.Network network = new Server.Network("127.0.0.1", 5009);
                    
                    NetworkUser user = null;
                    while (user == null)
                        user = Server.Network.AddUser();
                    Player player = new Player(0, Team.FIRST, user);
                    Network.SendPrivate(player, "this is a test");
                }
                catch (Exception)
                {
                    Assert.False(true);
                }
            });
            Thread clientThread = new Thread(() =>
            {
                try
                {
                    Server.NetworkUser server = new Server.NetworkUser((new TcpClient("127.0.0.1", 5009)).Client);
                    String str = server.GetReader().ReadLine();
                    Assert.Equal(str, "this is a test");
                }
                catch (Exception)
                {
                    Assert.False(true);
                }
            });
            try
            {
                serverThread.Start();
                Thread.Sleep(5000);
                clientThread.Start();
            }
            catch (UserQuit)
            {
                Console.WriteLine("Test ok");
            }
            catch (Exception)
            {
                Console.WriteLine("Something happened...");
            }
        }
        
        [Fact]
        public void SendAllTest()
        {
            Thread serverThread = new Thread(() =>
            {
                try
                {
                    Server.Network network = new Server.Network("127.0.0.1", 5002);
    
                    NetworkUser user = null;
                    while (user == null)
                        user = Server.Network.AddUser();
                    NetworkUser user2 = null;
                    while (user2 == null)
                        user2 = Server.Network.AddUser();
                    List<Player> players = new List<Player>();
                    players.Add(new Player(0, Team.FIRST, user));
                    players.Add(new Player(0, Team.FIRST, user2));
                    Network.SendAll(players, "this is a test");
                }
                catch (Exception)
                {
                    Assert.False(true);
                }
            });
            Thread clientThread = new Thread(() =>
            {
                try
                {
                Server.NetworkUser server = new Server.NetworkUser((new TcpClient("127.0.0.1", 5002)).Client);
                Assert.Equal(server.GetReader().ReadLine(), "this is a test");
                }
                catch (Exception)
                {
                    Assert.False(true);
                }
            });
            Thread clientThread2 = new Thread(() =>
            {
                try
                {
                Server.NetworkUser server = new Server.NetworkUser((new TcpClient("127.0.0.1", 5002)).Client);
                Assert.Equal(server.GetReader().ReadLine(), "this is a test");
                }
                catch (Exception)
                {
                    Assert.False(true);
                }
            });
            try
            {
                serverThread.Start();
                Thread.Sleep(5000);
                clientThread.Start();
                clientThread2.Start();
            }
            catch (UserQuit)
            {
                Console.WriteLine("Test ok");
            }
            catch (Exception)
            {
                Console.WriteLine("Something happened...");
            }
        }
        
        [Fact]
        public void SendNetworksTest()
        {
            Thread serverThread = new Thread(() =>
            {
                try
                {
                    Server.Network network = new Server.Network("127.0.0.1", 5003);
                    List<NetworkUser> users = new List<NetworkUser>();
                    NetworkUser user = null;
                    while (user == null)
                        user = Server.Network.AddUser();
                    users.Add(user);
                    user = null;
                    while (user == null)
                        user = Server.Network.AddUser();
                    users.Add(user);
                    Network.SendNetworks(users, "this is a test");
                }
                catch (Exception)
                {
                    Assert.False(true);
                }
            });
            Thread clientThread = new Thread(() =>
            {
                try
                {
                Server.NetworkUser server = new Server.NetworkUser((new TcpClient("127.0.0.1", 5003)).Client);
                Assert.Equal(server.GetReader().ReadLine(), "this is a test");
                }
                catch (Exception)
                {
                    Assert.False(true);
                }
            });
            Thread clientThread2 = new Thread(() =>
            {
                try
                {
                Server.NetworkUser server = new Server.NetworkUser((new TcpClient("127.0.0.1", 5003)).Client);
                Assert.Equal(server.GetReader().ReadLine(), "this is a test");
                }
                catch (Exception)
                {
                    Assert.False(true);
                }
            });
            try
            {
                serverThread.Start();
                Thread.Sleep(5000);
                clientThread.Start();
                clientThread2.Start();
            }
            catch (UserQuit)
            {
                Console.WriteLine("Test ok");
            }
            catch (Exception)
            {
                Console.WriteLine("Something happened...");
            }
        }
    }
}