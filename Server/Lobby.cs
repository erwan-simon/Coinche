using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Server
{
    public class Lobby
    {
        private volatile List<NetworkUser> waiters = new List<NetworkUser>();
        private List<Thread> threads = new List<Thread>();
        
        private List<NetworkUser> GameLaunching()
        {
            waiters = Network.SendNetworks(waiters, "alive");
            List<NetworkUser> temp = new List<NetworkUser>();
            while (temp.Count != 4)
            {
                if (waiters.Count == 0)
                    return temp;
                if (waiters.First().IsAlive())
                {
                    temp.Add(waiters.First());
                    waiters.Remove(waiters.First());
                }
                else
                {
                    waiters.Remove(waiters.First());
                }
            }
            return temp;
        }

        public void Run()
        {
            int gamesId = 0;

            do
            {
                while (!Console.KeyAvailable)
                {
                    if (waiters.Count < 4)
                    {
                        waiters = Network.SendNetworks(waiters, "alive");
                        NetworkUser user = Network.AddUser();
                        if (user != null)
                            waiters.Add(user);
                        else
                        {
                            Thread.Sleep(100);
                            continue;
                        }
                        waiters = Network.SendNetworks(waiters, "A new user came!");
                    }
                    if (waiters.Count >= 4)
                    {
                        List<NetworkUser> temp = GameLaunching();
                        if (temp.Count != 4)
                        {
                            temp = Network.SendNetworks(temp, "Somebody quitted during user collection...");
                            foreach (NetworkUser n in temp)
                            {
                                if (n.IsAlive())
                                    waiters.Add(n);
                            }
                            temp.Clear();
                            continue;
                        }
                        Game game = null;
                        try
                        {
                            gamesId += 1;
                            game = new Game(gamesId, temp);
                        }
                        catch (UserQuit)
                        {
                            temp = Network.SendNetworks(temp, "Somebody quitted during game creation...");
                            foreach (NetworkUser n in temp)
                            {
                                if (n.IsAlive())
                                    waiters.Add(n);
                            }
                            temp.Clear();
                            continue;
                        }
                        Thread thread = new Thread(
                            () =>
                            {
                                try
                                {
                                    temp = Network.SendNetworks(temp, "Begining of the game!");
                                    try
                                    {
                                        temp = game.runGame();
                                    }
                                    catch (UserQuit)
                                    {
                                        temp = Network.SendNetworks(temp, "Somebody quitted during game...");
                                    }
                                    if (temp.Count == 0)
                                        Console.WriteLine("Nobody survived from the game...");
                                    else if (temp.Count == 4)
                                        temp = Network.SendNetworks(temp, "Game over!");
                                    else
                                        temp = Network.SendNetworks(temp,
                                            "Somebody quitted the game... Back to the lobby!");
                                    foreach (NetworkUser n in temp)
                                    {
                                        if (n.IsAlive())
                                            waiters.Add(n);
                                    }
                                    temp.Clear();
                                }
                                catch (Exception)
                                {
                                    Console.Error.WriteLine("Something happened...");
                                }
                            }
                        );
                        thread.Start();
                        threads.Add(thread);
                    }
                }
            } while (Console.ReadKey(true).Key != ConsoleKey.Escape);
            try
            {
                List<Thread> threads2 = threads;
                foreach (Thread t in threads2)
                {
                    if (t.IsAlive)
                        t.Interrupt();
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Something happened...");
            }
        }
    }
}