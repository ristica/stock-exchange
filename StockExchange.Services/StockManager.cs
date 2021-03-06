﻿using StockExchange.Contracts;
using System.Collections.Generic;
using StockExchange.Contracts.DataContracts;
using StockExchange.Repository;
using System.Linq;

namespace StockExchange.Services
{
    public class StockManager : IStockService
    {
        private readonly decimal _amount = new decimal(0.75);

        public IEnumerable<StockData> Get()
        {
            var data = new List<StockData>();

            IStockRepository repository = new StockRepository();
            var stocks = repository.Get();
            if (stocks == null) return data;
            data.AddRange(
                stocks.Select(s => new StockData
                {
                    Share = s.Share,
                    Company = s.Trader,
                    CurrentPrice = s.CurrentPrice
                }));

            return data;
        }

        public StockData GetStock(string shape)
        {
            var stock = this.Get().SingleOrDefault(s => s.Share.ToUpper() == shape.ToUpper());
            return stock == null ? null : new StockData { Share = stock.Share, Company = stock.Company, CurrentPrice = stock.CurrentPrice };
        }

        public decimal UpdateStockPrice(string share, decimal oldPrice, bool up)
        {
            var stock = this.Get().SingleOrDefault(s => s.Share.ToUpper() == share.ToUpper());
            if (stock == null) return oldPrice;

            var newPrice = up ? oldPrice + this._amount : oldPrice - this._amount;

            IStockRepository repository = new StockRepository();
            var result = repository.UpdateCurrentPrice(share, newPrice);

            PubSubManager.NotifySubscribersAboutStockPriceChange(result.Share, result.CurrentPrice);

            return newPrice;
        }
    }
}
