using StockAPI.Entities;
using StockAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockAPI
{
    public class MarketSeeder
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMarketService _marketServie;

        public MarketSeeder(ApplicationDbContext dbContext, IMarketService marketService)
        {
            _dbContext = dbContext;
            _marketServie = marketService;
        }
        public void Seed()
        {
            if(_dbContext.Database.CanConnect())
            {
                if(!_dbContext.Market.Any())
                {
                    var markets = GetMarkets();
                    _dbContext.Market.AddRange(markets);
                    _dbContext.SaveChanges();
                }
            }
           
        }

        private IEnumerable<Market> GetMarkets()
        {
            var result = _marketServie.AddStocks();
            return result;
        }
    }
}
