using StockExchange.Data.Data;
using StockExchange.Data.Entities;
using StockExchange.Repository.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StockExchange.Repository
{
    public class StockRepository : IStockRepository, IDisposable
    {
        private StockDbContext _ctx = new StockDbContext();

        public void Dispose()
        {
            this.Dispose();
        }

        public IEnumerable<Stock> Get()
        {
            return this._ctx.StockSet.ToList();
        }

        public Stock GetStockByShare(string share)
        {
            return this._ctx.StockSet.SingleOrDefault(s => s.Share.ToUpper() == share.ToUpper());
        }

        public Stock UpdateCurrentPrice(string share, decimal price)
        {
            var stock = this._ctx.StockSet.SingleOrDefault(s => s.Share.ToUpper() == share.ToUpper());
            if (stock != null)
            {
                stock.CurrentPrice = price;
                this._ctx.SaveChanges();

                return stock;
            }

            throw new NotFoundException(string.Format("Share {0} not found!", share));
        }
    }
}
