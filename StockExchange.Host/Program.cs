using StockExchange.Services;
using System;
using System.ServiceModel;

namespace StockExchange.Host
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost stockHost = null;
            ServiceHost pubSubHost = null;
            try
            {
                Console.Title = "STOCK EXCHANGE - HOST";

                Console.WriteLine("### Starting StockManager service ###");
                stockHost = new ServiceHost(typeof(StockManager));
                stockHost.Open();
                Console.WriteLine("\tStockManager => started");

                Console.WriteLine("");

                Console.WriteLine("### Starting StockManager service ###");
                pubSubHost = new ServiceHost(typeof(PubSubManager));
                pubSubHost.Open();
                Console.WriteLine("\tPubSubManager => started");

                Console.WriteLine("");
                Console.WriteLine("Press <Enter> to close the service");
                Console.WriteLine("");
                Console.ReadKey();

                stockHost.Close();
                pubSubHost.Close();
            }
            catch(Exception ex)
            {
                if (stockHost != null && stockHost.State == CommunicationState.Faulted)
                {
                    stockHost.Close();
                }

                if (pubSubHost != null && pubSubHost.State == CommunicationState.Faulted)
                {
                    pubSubHost.Close();
                }

                Console.WriteLine("Exception occured: ");
                Console.WriteLine("");
                Console.WriteLine(ex.Message);
                Console.ReadKey();
                Console.WriteLine("");
                Console.WriteLine("Press any key to exit");
            }
        }
    }
}
