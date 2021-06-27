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
        private string data = null;
        private Computer computer;
        public Client_1(String ipAddress, int port)
        {
            GetIPAddress(ipAddress);
            ClientStart(port);
        }
        void ClientStart(int port)
        {
            Set(port);
            Create();
            Connect();
            Loop();
        }
        void GetIPAddress(String ipAddress)
        {
            host = Dns.GetHostEntry(ipAddress);
            this.ipAddress = host.AddressList[0];
        }

        void Set(int port)
        {
            remoteEP = new IPEndPoint(ipAddress, port);
        }
        void Create()
        {
            socket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            Console.WriteLine("Client 1, Socket created THREAD[{0}]", Thread.CurrentThread.ManagedThreadId);
        }
        void Connect()
        {
            try
            {
                socket.Connect(remoteEP);
            }
            catch(SocketException)
            {
                Console.WriteLine("Failed to connect to server");
                socket.Close();
                Environment.Exit(0);
            }
            Console.WriteLine("Client 1, Socket connected to {0} THREAD[{1}]", socket.RemoteEndPoint.ToString(), Thread.CurrentThread.ManagedThreadId);
        }
        void Loop()
        {
            while (true)
            {
                SendMessage();
                ReceiveMessege();
            }
        }
        void ReceiveMessege()
        {
            byte[] buffer = new byte[2048];
            int received = socket.Receive(buffer, SocketFlags.None);
            if (received == 0) return;
            byte[] msg = new byte[received];
            Array.Copy(buffer, msg, received);
            string data = Encoding.ASCII.GetString(msg);
            Console.WriteLine(data);

            if (JsonValidation(data))
            {
                Console.WriteLine("Valid json of Computer object was received from server.");
            }

        }
        void SendMessage()
        {
            Console.WriteLine("Write a request: ");
            data = Console.ReadLine();

            if (data.ToLower() == "send json")
            {
                SendJson();
            }
            else SendString(data);

            if (data.ToLower() == "exit")
            {
                Exit();
            }
        }
        void Exit()
        {
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
            Environment.Exit(0);
        }
        void SendString(String data)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            socket.Send(buffer, 0, buffer.Length, SocketFlags.None);
        }
        void SendJson()
        {
            computer = new Computer("Asus", "FG126", 600);
            string stringjson = JsonSerializer.Serialize(computer);
            msg = Encoding.ASCII.GetBytes(stringjson);
            socket.Send(msg);
        }

        bool JsonValidation(String data)
        {
            try
            {
                computer = JsonSerializer.Deserialize<Computer>(data);
            }
            catch (JsonException)
            {
                return false;
            }
            return true;
        }
    }
}
