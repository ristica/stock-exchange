using System.Collections.Generic;
using StockExchange.Data.Entities;

namespace StockExchange.Repository
{
    public interface IStockRepository
    {
        IEnumerable<Stock> Get();
        Stock GetStockByShare(string share);
        Stock UpdateCurrentPrice(string share, decimal price);
    }
}