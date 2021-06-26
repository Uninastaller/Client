using System;
using System.Text.Json;
using System.Threading;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            
            Client_1 client_1 = new Client_1();
            Thread thread_1 = new Thread(new ThreadStart(client_1.ClientStart));          
            thread_1.Start();
            
            Client_2 client_2 = new Client_2();
            Thread thread_2 = new Thread(new ThreadStart(client_2.ClientStart));
            thread_2.Start();
            
        }
    }
}
