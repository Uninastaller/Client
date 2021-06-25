using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Computer
    {
        public string _name { get; set; }
        public string _version { get; set; }
        public int _price { get; set; }

        

        public Computer(string name, string version, int price)
        {
            _name = name;
            _version = version;
            _price = price;
        }

       
    }

}
