using StockExchange.Contracts;
using System.Collections.Generic;
using System.ServiceModel;
using StockExchange.Contracts.DataContracts;
using System;

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
    }
}
