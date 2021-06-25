using System;
using System.Text;
using Newtonsoft.Json;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
           // StartClient client = new StartClient();
            Computer computer = new Computer("Asus","FG126",600);
            string stringjson = JsonConvert.SerializeObject(computer);
            Console.WriteLine(stringjson);
        }
    }
}
