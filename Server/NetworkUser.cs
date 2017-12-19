using System;
using System.IO;
using System.Net.Sockets;

namespace Server
{
    public class NetworkUser
    {
        private Socket socket;
        private Stream stream;
        private StreamReader reader;
        private StreamWriter writer;
        
        public NetworkUser(Socket socket)
        {
            this.socket = socket; 
            stream = new NetworkStream(socket);
            reader = new StreamReader(stream);
            writer = new StreamWriter(stream)
                {AutoFlush = true};
        }

        public StreamReader GetReader()
        {
            return reader;
        }

        public StreamWriter GetWriter()
        {
            return writer;
        }

        public bool IsAlive()
        {
            try
            {
                writer.WriteLine("alive");
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}