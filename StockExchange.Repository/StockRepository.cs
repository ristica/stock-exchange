using StockExchange.Data.Entities;
using StockExchange.Repository.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace StockExchange.Repository
{
    public class StockRepository
    {
        public IEnumerable<Stock> Get()
        {
            return Data.StockDatabase.GetStocks();
        }

        public Stock GetStockByShare(string share)
        {
            return this.Get().SingleOrDefault(s => s.Share.ToUpper() == share.ToUpper());
        }

        public Stock UpdateCurrentPrice(string share, decimal price)
        {
            var stock = this.Get().SingleOrDefault(s => s.Share.ToUpper() == share.ToUpper());
            if (stock != null)
            {
                stock.CurrentPrice = price;
                return stock;
            }

            throw new NotFoundException(string.Format("Share {0} not found!", share));
        }
    }
}
