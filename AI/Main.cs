using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace AI
{
    internal class AI
    {
        private static string address;
        private static int port;
        private static int AI_number;

        private static bool my_strcmp(String s1, String s2)
        {
            int i = s2.Length - 1;
            int c = s1.Length - 1;
            try
            {
                while (c != -1)
                {
                    if (s1[c] != s2[i])
                        return false;
                    i -= 1;
                    c -= 1;
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        private static void Run(int i)
        {
            try
            {
                Server.NetworkUser server = new Server.NetworkUser(new TcpClient(address, port).Client);
                int a = 0;
                int GameNumber = 1;
                Console.WriteLine("Game number " + GameNumber + " of the AI number " + i + " started!");
                string buffer;
                while (true)
                {
                    buffer = server.GetReader().ReadLine();
                    if (buffer.Equals("") || buffer[0] != 'W' || buffer.Equals("alive"))
                        continue;
                    if (my_strcmp("5->pass", buffer))
                    {
                        GameNumber += 1;
                        Console.WriteLine("Game number " + GameNumber + " of the AI number " + i + " started!");
                        server.GetWriter().WriteLine("5");
                    }
                    else if (my_strcmp("play?", buffer))
                    {
                        a = 0;
                        server.GetWriter().WriteLine(a);
                    }
                    else if (my_strcmp("card!", buffer))
                    {
                        a += 1;
                        server.GetWriter().WriteLine(a);
                    }
                }
            }
            catch (SocketException)
            {
                Console.Error.WriteLine("AI number " + i + ": No server found...");
            }
            catch (Exception)
            {
                Console.Error.WriteLine("AI number " + i + ": Server disconnected...");
            }
        }

        public static int Main(string[] args)
        {
            try
            {
                try
                {
                    IPAddress.Parse(args[0]);
                    address = args[0];
                    port = Int32.Parse(args[1]);
                    AI_number = Int32.Parse(args[2]);
                    if (AI_number <= 0 || port <= 0)
                    {
                        Console.Error.WriteLine(
                            "Wrong parameters...\nHELP:\n1st parameter: ip address\n2nd parameter: port\n3rd parameter: number of AI to launch");
                        return 84;
                    }
                }
                catch
                {
                    Console.Error.WriteLine(
                        "Wrong parameters...\nHELP:\n1st parameter: ip address\n2nd parameter: port\n3rd parameter: number of AI to launch");
                    return 84;
                }
                int i = 0;
                while (i != AI_number)
                {
                    Thread thread = new Thread(
                        () =>
                        {
                            try
                            {
                                Run(i);
                            }
                            catch (Exception)
                            {
                                Console.Error.WriteLine("Something happened...");
                            }
                        });
                    thread.Start();
                    i += 1;
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