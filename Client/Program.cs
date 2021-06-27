using System;
using System.Text.Json;
using System.Threading;

namespace Client
{
    class Program
    {
        const int PORT = 7777;
        const string IP_ADDRESS = "127.0.0.1";
        static void Main(string[] args)
        {
            
            new Client_1(IP_ADDRESS,PORT);

        }
    }
}
