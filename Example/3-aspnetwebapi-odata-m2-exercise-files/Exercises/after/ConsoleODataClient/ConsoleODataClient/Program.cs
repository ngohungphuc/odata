using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace ConsoleODataClient
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpClient client = new HttpClient();
            client.GetAsync("http://localhost.fiddler:2112/Customers");

            Console.WriteLine("Press Enter to Exit...");
            Console.ReadLine();
        }
    }
}
