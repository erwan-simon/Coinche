using System;
using System.Collections.Generic;
using System.Threading;
using Xunit;
using Server;
using Client;

namespace Test
{
    public class LobbyTest
    {
        //private static GameServer _server = new GameServer();
        //private static GameClient _client = new GameClient();
        private static Thread _serverThread;
        private List<Thread> _cliThreads = new List<Thread>();

        [Fact]
        public void Test()
        {
            int i = 0;
            _serverThread = new Thread(runServer);
            _serverThread.Start();
            while (i <= 11)
            {
                Thread thread = new Thread(runClient);
                if (i < 4)
                    thread.Start();
                _cliThreads.Add(thread);
                i += 1;
            }
            try
            {
                Thread.Sleep(1000);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                if (_serverThread == null || _serverThread.IsAlive)
                    return;
                _serverThread.Interrupt();
                throw;
            }
            try
            {
                Thread.Sleep(2000);
                if (_cliThreads[0].IsAlive)
                    _cliThreads[0].Interrupt();
                Thread.Sleep(2000);
                _cliThreads[5].Start();
                if (_cliThreads[2].IsAlive)
                    _cliThreads[2].Interrupt();
                if (_cliThreads[3].IsAlive)
                    _cliThreads[3].Interrupt();
                Thread.Sleep(2000);
                _cliThreads[6].Start();
                _cliThreads[7].Start();
                if (_cliThreads[5].IsAlive)
                    _cliThreads[5].Interrupt();
                if (_cliThreads[6].IsAlive)
                    _cliThreads[6].Interrupt();
                if (_cliThreads[4].IsAlive)
                    _cliThreads[4].Interrupt();
                Thread.Sleep(2000);
                _cliThreads[8].Start();
                _cliThreads[9].Start();
                _cliThreads[10].Start();
                foreach (Thread t in _cliThreads)
                {
                    if (t.IsAlive)
                        t.Interrupt();
                }
                Console.WriteLine("\n---------------------------" +
                                  "Lobby Test succeed" +
                                  "---------------------------\n");
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);
                foreach (Thread t in _cliThreads)
                {
                    if (t.IsAlive)
                        t.Interrupt();
                }
                throw;
            }

        }
        
        void runServer()
            {
                try
                {
                    if (_serverThread != null)
                    {
                        GameServer.Main(null);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

            void runClient()
            {
                try
                {
                    if (_serverThread != null)
                    {
                        GameClient.Main(null);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        } 
}
