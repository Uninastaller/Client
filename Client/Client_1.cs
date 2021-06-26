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
        private byte[] buffer = new byte[1024];
        private string data = null;
        Computer computer;
        public Client_1()
        {

        }
        public void ClientStart()
        {
            Set();
            Create();
            Connect();
            //Send();
            //Clean();
            _Send();
            Console.ReadLine();
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
            Console.WriteLine("Client 1, Socket connected to {0} THREAD[{1}]", socket.RemoteEndPoint.ToString(),Thread.CurrentThread.ManagedThreadId);
        }
        void _Send()
        {
            while (true)
            {
                Console.WriteLine("Write a request: ");
                data = Console.ReadLine();
                if (data.ToLower() == "send json")
                {
                    SendJson();
                    Receive();
                }
                else
                {
                    buffer = Encoding.ASCII.GetBytes(data);
                    socket.Send(buffer);
                    Receive();
                }
            }
        }
        void Receive()
        {
            buffer = new byte[1024];
            amountOfBytes = socket.Receive(buffer);
            msg = new byte[amountOfBytes];
            Array.Copy(buffer, msg, amountOfBytes);
            data = Encoding.ASCII.GetString(msg);
            Console.WriteLine("[Received]  " + data);

            if (JsonValidation(data))
            {
                Console.WriteLine("[SERVER] Valid json of Computer object was sent.");
            }

        }
        void SendJson()
        {
            computer = new Computer("Asus", "FG126", 600);
            string stringjson = JsonSerializer.Serialize(computer);
            msg = Encoding.ASCII.GetBytes(stringjson);
            amountOfBytes = socket.Send(msg);
        }
        void Clean()
        {
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }
        bool JsonValidation(String data)
        {
            try
            {
                computer = JsonSerializer.Deserialize<Computer>(data);
            }
            catch (System.Text.Json.JsonException)
            {
                return false;
            }
            return true;
        }
    }
}
