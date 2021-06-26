using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;

namespace Client
{
    class Client_2
    {
        private Socket socket;
        private IPHostEntry host;
        private IPAddress ipAddress;
        private IPEndPoint remoteEP;

        private int identificator = 2;
        private int amountOfBytes;
        private byte[] buffer;
        private string data = null;

        Computer computer;

        public Client_2()
        {
        }
        public void ClientStart()
        {
            Set();
            Create();
            Connect();
            Receive();
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
            socket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            Console.WriteLine("Client 2, Socket created THREAD[{0}]", Thread.CurrentThread.ManagedThreadId);
        }
        void Connect()
        {
            socket.Connect(remoteEP);
            Console.WriteLine("Client 2, Socket connected to {0} THREAD[{1}]", socket.RemoteEndPoint.ToString(), Thread.CurrentThread.ManagedThreadId);
        }

        void Receive()
        {
            amountOfBytes = socket.Send(BitConverter.GetBytes(identificator));

            buffer = new byte[1024];
            amountOfBytes = socket.Receive(buffer);
            data = Encoding.ASCII.GetString(buffer, 0, amountOfBytes);
            computer = JsonSerializer.Deserialize<Computer>(data);
            Console.WriteLine("Json received THREAD[{0}]", Thread.CurrentThread.ManagedThreadId);

            Console.WriteLine(computer.name);
           // CloseTheThread(connectId);

        }

        void Clean()
        {
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }

    }
}
