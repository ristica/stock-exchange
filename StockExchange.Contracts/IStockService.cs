using StockExchange.Contracts.DataContracts;
using System.Collections.Generic;
using System.ServiceModel;

namespace StockExchange.Contracts
{
    [ServiceContract]
    public interface IStockService
    {
        [OperationContract]
        IEnumerable<StockData> Get();

        [OperationContract]
        StockData GetStock(string shape);

        [OperationContract]
        decimal UpdateStockPrice(string shape, decimal oldPrice, bool up);
    }
}
