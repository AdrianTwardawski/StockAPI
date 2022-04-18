using HtmlAgilityPack;
using StockAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockAPI
{
    public interface IStockScraper
    {
        IEnumerable<Market> AddStocks();
    }

    public class StockScraper : IStockScraper
    {
        private readonly ApplicationDbContext _dbContext;
        private const string BaseUrl = "https://www.bankier.pl/gielda/notowania/akcje";

        public StockScraper(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;         
        }


        public IEnumerable<Market> AddStocks()
        {
            var web = new HtmlWeb();
            var document = web.Load(BaseUrl);
            var tableRows = document.QuerySelectorAll("table tr").Skip(1).Skip(11);

            var markets = new List<Market>();

            foreach (var tableRow in tableRows)
            {
                var tds = tableRow.QuerySelectorAll("td");
                var stock = tds[0].QuerySelector("a").InnerText;
                var price = float.Parse(tds[1].InnerText.Replace(",", ".").Replace("&nbsp;", ""));
                var change = MathF.Round((price * 100) / (price - (float.Parse(tds[2].InnerText.Replace(",", ".").Replace("&nbsp;", "")))) - 100, 2);
                var tradesValue = tds[5].InnerText.Replace("&nbsp;", " ");
                var time = tds[9].InnerText;

                var itemInDb = _dbContext.Market.FirstOrDefault(i => i.Name == stock);
             
                if (itemInDb == null)
                {
                    var market = new Market
                    {
                        Name = stock,
                        Price = price,
                        Change = change,
                        TradesValue = tradesValue,
                        Time = time
                    };
                    markets.Add(market);
                }
                else
                {
                    itemInDb.Name = stock;
                    itemInDb.Change = change;
                    itemInDb.Price = price;
                    itemInDb.TradesValue = tradesValue;
                    itemInDb.Time = time;
                }
            }
            _dbContext.SaveChanges();
            return markets;
        }
    }
}
