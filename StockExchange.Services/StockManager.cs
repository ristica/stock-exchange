using StockExchange.Contracts;
using System.Collections.Generic;
using StockExchange.Contracts.DataContracts;
using StockExchange.Repository;

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
                        CurrentPrice = s.CurrentPrice
                    });
                }
            }

            return data;
        }
    }
}
