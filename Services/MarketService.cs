using AutoMapper;
using HtmlAgilityPack;
using StockAPI.Entities;
using StockAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

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
        private readonly IMapper _mapper;

        public MarketService(ApplicationDbContext dbContext, IStockScraper stockScraper, IMapper mapper)
        {
            _dbContext = dbContext;          
            _mapper = mapper;
            stockScraper.AddStocks();
        }


        public PagedResult<MarketDto> GetStocks(MarketQuery query)
        {
            var baseQuery = _dbContext.Market
                .Where(r => query.SearchPhrase == null ||
                                                (r.Name.ToLower().Contains(query.SearchPhrase.ToLower())));

            if(!string.IsNullOrEmpty(query.SortBy)) // sorting is optional
            {
                var columnsSelector = new Dictionary<string, Expression<Func<Market, object>>>
                {
                    {nameof(Market.Name), r => r.Name }, // {key, value}
                    {nameof(Market.Change), r => r.Change },
                    {nameof(Market.TradesValue), r => r.TradesValue },
                };

                var selectedColumn = columnsSelector[query.SortBy];

                baseQuery = query.SortDirection == SortDirection.ASC
                    ? baseQuery.OrderBy(selectedColumn)
                    : baseQuery.OrderByDescending(selectedColumn);
            }

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
            var stock = _dbContext.Market.FirstOrDefault(m => m.Id == id);

            var marketDto = _mapper.Map<MarketDto>(stock);
            return marketDto;
        }
    }
}

