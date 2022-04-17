using AutoMapper;
using HtmlAgilityPack;
using StockAPI.Entities;
using StockAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StockAPI.Services
{
    public interface IMarketService
    {
        PagedResult<MarketDto> GetStocks(MarketQuery query);
        MarketDto GetMarketById(int id);
    }

    public class MarketService : IMarketService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IStockScraper _stockScrapper;
        private readonly IMapper _mapper;

        public MarketService(ApplicationDbContext dbContext, IStockScraper stockScraper, IMapper mapper)
        {
            _dbContext = dbContext;
            _stockScrapper = stockScraper;
            _mapper = mapper;
        }


        public PagedResult<MarketDto> GetStocks(MarketQuery query)
        {
            _stockScrapper.AddStocks();

            var baseQuery = _dbContext.Market.Where(r => query.SearchPhrase == null ||
                                                (r.Name.ToLower().Contains(query.SearchPhrase.ToLower())));

            var markets = baseQuery
               .Skip(query.PageSize * (query.PageNumber - 1))
               .Take(query.PageSize)
               .ToList();

            var marketsDtos = _mapper.Map<List<MarketDto>>(markets);

            var totalItemsCount = baseQuery.Count();

            var result = new PagedResult<MarketDto>(marketsDtos, totalItemsCount, query.PageSize, query.PageNumber);

            return result;
        }

        public MarketDto GetMarketById(int id)
        {
            _stockScrapper.AddStocks();
            var stock = _dbContext.Market.FirstOrDefault(m => m.Id == id);

            var marketDto = _mapper.Map<MarketDto>(stock);
            return marketDto;
        }
    }
}

