using StockExchange.Data.Entities;
using System.Collections.Generic;
using System.Data.Entity;

namespace StockExchange.Data.Data
{
    public class StockDbInitializer : DropCreateDatabaseAlways<StockDbContext>
    {
        protected override void Seed(StockDbContext context)
        {
            var stocks = new List<Stock>
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

            foreach (var s in stocks)
            {
                context.StockSet.Add(s);
            }

            base.Seed(context);
        }
    }
}