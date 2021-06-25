using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;


namespace Client
{
    class StartClient
    {
        private Socket socket;
        private IPHostEntry host;
        private IPAddress ipAddress;
        private IPEndPoint remoteEP;
        public StartClient()
        {
            Set();
            Create();
            Connect();
            Send();
            Clean();
        }
        void Set()
        {
            host = Dns.GetHostEntry("127.0.0.1");
            ipAddress = host.AddressList[0];
            remoteEP = new IPEndPoint(ipAddress, 7777);
        }
        void Create()
        {
            socket = new Socket(ipAddress.AddressFamily,SocketType.Stream, ProtocolType.Tcp);
            if (socket == null)
            {
                Console.WriteLine("Socket creation error");
            }
            Console.WriteLine("Socket created");
        }
        void Connect()
        {
            socket.Connect(remoteEP);
            Console.WriteLine("Socket connected to {0}",socket.RemoteEndPoint.ToString());
        }
        void Send()
        {
            Computer computer = new Computer("Asus", "FG126", 600);
            string stringjson = JsonSerializer.Serialize(computer);
            byte[] msg = Encoding.ASCII.GetBytes(stringjson);
            int bitesSend = socket.Send(msg);
        }
        void Clean()
        {
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }
    }
}
