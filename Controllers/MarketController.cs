using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StockAPI.Entities;
using StockAPI.Models;
using StockAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MarketController : ControllerBase
    {

        private readonly IMarketService _marketService;
        public MarketController(IMarketService marketService)
        {
            _marketService = marketService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<MarketDto>> GetAllMarkets([FromQuery]MarketQuery query)
        {
            var marketsDtos = _marketService.GetStocks(query);
  
            return Ok(marketsDtos);
        }

        [HttpGet("{id}")]
        public ActionResult<MarketDto> GetMarket([FromRoute]int id)
        {
            var marketDto = _marketService.GetMarketById(id);
         
            if (marketDto is null)
            {
                return NotFound();
            }
            return Ok(marketDto);          
        }
    }
}
