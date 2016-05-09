using System.ServiceModel;

namespace StockExchange.Contracts
{
    [ServiceContract]
    public interface IStockCallback
    {
        [OperationContract]
        void RefreshSubscriptions(string share, int subscribers);

        [OperationContract]
        void RefreshStockState(string share, decimal price);
    }
}
