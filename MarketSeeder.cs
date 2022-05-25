using Microsoft.EntityFrameworkCore;
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
        private readonly IStockScraper _stockScraper;

        public MarketSeeder(ApplicationDbContext dbContext, IStockScraper stockScraper)
        {
            _dbContext = dbContext;
            _stockScraper = stockScraper;
        }
        public void Seed()
        {
            if(_dbContext.Database.CanConnect())
            {
                var pendingMigrations = _dbContext.Database.GetPendingMigrations();
                if(pendingMigrations != null && pendingMigrations.Any())
                {
                    _dbContext.Database.Migrate();
                }
                if(!_dbContext.Roles.Any())
                {
                    var roles = GetRoles();
                    _dbContext.Roles.AddRange(roles);
                    _dbContext.SaveChanges();
                }
                if(!_dbContext.Market.Any())
                {
                    var markets = GetMarkets();
                    _dbContext.Market.AddRange(markets);
                    _dbContext.SaveChanges();
                }
            }
           
        }


        private IEnumerable<Role> GetRoles()
        {
            var roles = new List<Role>()
            {
                new Role()
                {
                    Name = "User"
                }, 
                new Role()
                {
                    Name = "Manager"
                },
                 new Role()
                {
                    Name = "Admin"
                }
            };
            return roles;
        }

        private IEnumerable<Market> GetMarkets()
        {
            var result = _stockScraper.AddStocks();
            return result;
        }
    }
}
