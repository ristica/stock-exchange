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
        #region Fields

        private StockDbContext _ctx = new StockDbContext();
        private bool _isDisposed;

        #endregion

        #region IStockRepository implementation

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

        #endregion

        #region IDsposable implementation

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Helpers

        private void Dispose(bool disposing)
        {
            if (this._isDisposed) return;

            if (disposing)
            {
                this._ctx.Dispose();
            }
            this._isDisposed = true;
        }

        #endregion        
    }
}
