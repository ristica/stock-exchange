using StockExchange.Contracts;
using System.Collections.Generic;
using StockExchange.Contracts.DataContracts;
using StockExchange.Repository;
using System.Linq;

namespace StockExchange.Services
{
    public class StockManager : IStockService
    {
        public IEnumerable<StockData> Get()
        {
            var data = new List<StockData>();

            IStockRepository repository = new StockRepository();
            var stocks = repository.Get(); 
            if (stocks != null)
            {
                foreach (var s in stocks)
                {
                    data.Add(
                    new StockData
                    {
                        Share = s.Share,
                        Company = s.Trader,
                        CurrentPrice = s.CurrentPrice
                    });
                }
            }

            return data;
        }

        public StockData GetStock(string shape)
        {
            var stock = this.Get().SingleOrDefault(s => s.Share.ToUpper() == shape.ToUpper());
            if (stock == null) return null;

            return new StockData { Share = stock.Share, Company = stock.Company, CurrentPrice = stock.CurrentPrice };
        }
    }
}
