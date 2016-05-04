using StockExchange.Data.Entities;
using System.Collections.Generic;

namespace StockExchange.Data
{
    public static class StockDatabase
    {
        public static IEnumerable<Stock> GetStocks()
        {
            return new List<Stock>
            {
                new Stock
                {
                    Share = "DOW",
                    Trader = "US Global Index",
                    CurrentPrice = new decimal(17750.91)
                },
                new Stock
                {
                    Share = "MSFT",
                    Trader = "Microsoft Corporation",
                    CurrentPrice = new decimal(49.78)
                },
                new Stock
                {
                    Share = "RUBI",
                    Trader = "The Rubicon Project Inc",
                    CurrentPrice = new decimal(10.29)
                },
                new Stock
                {
                    Share = "DPW",
                    Trader = "Digital Power Corp",
                    CurrentPrice = new decimal(0.41)
                },
            };
        }
    }
}
