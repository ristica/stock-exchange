using System.Runtime.Serialization;

namespace StockExchange.Contracts.DataContracts
{
    [DataContract]
    public class StockData
    {
        [DataMember]
        public string Share { get; set; }

        [DataMember]
        public decimal CurrentPrice { get; set; }
    }
}
