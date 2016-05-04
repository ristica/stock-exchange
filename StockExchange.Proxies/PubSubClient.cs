using StockExchange.Contracts;
using System.ServiceModel;

namespace StockExchange.Proxies
{
    public class PubSubClient : DuplexClientBase<IPubSubService>, IPubSubService
    {
        public PubSubClient(InstanceContext callbackContext)
            : base(callbackContext) { }

        public int Subscribe(string share)
        {
            return Channel.Subscribe(share);
        }

        public void Unsubscribe(string share)
        {
            Channel.Unsubscribe(share);
        }
    }
}
