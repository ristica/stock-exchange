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
    }
}
