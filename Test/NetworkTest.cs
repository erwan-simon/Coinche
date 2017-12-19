using System;
using System.Threading;
using Xunit;
using Server;
using Client;

namespace Test
{
    public class NetworkTest
    {
        private static GameServer _server = new GameServer();
        private static GameClient _client = new GameClient();
        private static Thread _serverThread;
        private static Thread _clientThread1;
        private static Thread _clientThread2;
        private static Thread _clientThread3;
        private static Thread _clientThread4;

        [Fact]
        public void Test()
        {
            _serverThread = new Thread(runServer);
            _clientThread1 = new Thread(runClient);
            _clientThread2 = new Thread(runClient);
            _clientThread3 = new Thread(runClient);
            _clientThread4 = new Thread(runClient);
            _serverThread.Start();
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
            _clientThread1.Start();
            _clientThread2.Start();
            _clientThread3.Start();
            _clientThread4.Start();
            try
            {
                Thread.Sleep(5000);
                Console.WriteLine("\n---------------------------" +
                                  "Network Test succeed" +
                                  "---------------------------\n");
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);
                if ((_serverThread != null && _serverThread.IsAlive ||
                     (_clientThread1 != null && _clientThread1.IsAlive) ||
                     (_clientThread2 != null && _clientThread2.IsAlive) ||
                     (_clientThread3 != null && _clientThread3.IsAlive) ||
                     (_clientThread4 != null && _clientThread4.IsAlive)))
                {
                    _serverThread.Interrupt();
                    _clientThread1.Interrupt();
                    _clientThread2.Interrupt();
                    _clientThread3.Interrupt();
                    _clientThread4.Interrupt();
                    throw;
                }
                if (_clientThread1.IsAlive)
                    _clientThread1.Interrupt();
                if (_clientThread2.IsAlive)
                    _clientThread2.Interrupt();
                if (_clientThread3.IsAlive)
                    _clientThread3.Interrupt();
                if (_clientThread4.IsAlive)
                    _clientThread4.Interrupt();
                }

            void runServer()
            {
                try
                {
                    if (_serverThread == null) return;
                    GameServer.Main(null);
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
}
     