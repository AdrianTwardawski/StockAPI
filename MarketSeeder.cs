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
            var result = _marketServie.AddStocks();
            return result;
        }
    }
}
