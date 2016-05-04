using System.ServiceModel;

namespace StockExchange.Contracts
{
    [ServiceContract(CallbackContract = typeof(IStockCallback))]
    public interface IPubSubService
    {
        [OperationContract]
        int Subscribe(string share);

        [OperationContract]
        void Unsubscribe(string share);
    }
}
