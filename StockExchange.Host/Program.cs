using StockExchange.Services;
using System;
using System.ServiceModel;

namespace StockExchange.Host
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "STOCK EXCHANGE - HOST";

            Console.WriteLine("### Starting StockManager service ###");
            var stockHost = new ServiceHost(typeof(StockManager));
            stockHost.Open();

            Console.WriteLine("### Starting StockManager service ###");
            var pubSubHost = new ServiceHost(typeof(PubSubManager));
            pubSubHost.Open();

            Console.WriteLine("");
            Console.WriteLine("\tStockManager => started");
            Console.WriteLine("\tPubSubManager => started");
            Console.WriteLine("");
            Console.WriteLine("Press <Enter> to close the service");
            Console.WriteLine("");
            Console.ReadKey();

            stockHost.Close();
            pubSubHost.Close();
        }
    }
}
