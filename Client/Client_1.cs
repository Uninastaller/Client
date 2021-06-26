using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;

namespace Client
{
    class Client_1
    {
        private Socket socket;
        private IPHostEntry host;
        private IPAddress ipAddress;
        private IPEndPoint remoteEP;

        private byte[] msg;
        private int identificator = 1;
        private int amountOfBytes;
        private byte[] buffer;
        private string data = null;
        public Client_1()
        {

        }
        public void ClientStart()
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
            Console.WriteLine("Client 1, Socket created THREAD[{0}]", Thread.CurrentThread.ManagedThreadId);
        }
        void Connect()
        {
            socket.Connect(remoteEP);
            Console.WriteLine("Client 1,Socket connected to {0} THREAD[{1}]",socket.RemoteEndPoint.ToString(),Thread.CurrentThread.ManagedThreadId);
        }
        void Send()
        {
            amountOfBytes = socket.Send(BitConverter.GetBytes(identificator));
            buffer = new byte[10];
            amountOfBytes = socket.Receive(buffer);
            data = Encoding.ASCII.GetString(buffer, 0, amountOfBytes);
            if(data.Equals("Send_Json"))
            {
                SendJson();
            }
        }
        void SendJson()
        {
            Computer computer = new Computer("Asus", "FG126", 600);
            string stringjson = JsonSerializer.Serialize(computer);
            msg = Encoding.ASCII.GetBytes(stringjson);
            amountOfBytes = socket.Send(msg);
        }
        void Clean()
        {
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }
    }
}
