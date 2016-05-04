using System.ServiceModel;

namespace StockExchange.Contracts
{
    [ServiceContract]
    public interface IStockCallback
    {
        [OperationContract]
        void UpdateSubscriptions(string share, int subscribers);

        [OperationContract]
        void NotifyClientsStockChanged(string share, decimal price);
    }
}
