using StockExchange.Contracts;
using System.Collections.Generic;
using System.ServiceModel;
using StockExchange.Contracts.DataContracts;

namespace StockExchange.Proxies
{
    public class StockClient : ClientBase<IStockService>, IStockService
    {
        public IEnumerable<StockData> Get()
        {
            return Channel.Get();
        }

        public StockData GetStock(string shape)
        {
            return Channel.GetStock(shape);
        }

        public decimal UpdateStockPrice(string shape, decimal oldPrice, bool up)
        {
            return Channel.UpdateStockPrice(shape, oldPrice, up);
        }
    }
}
