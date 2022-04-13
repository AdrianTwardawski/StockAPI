using AutoMapper;
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
    [Route("api/[controller]")]
    public class MarketController : ControllerBase
    {

        private readonly IMarketService _marketService;
        private readonly IMapper _mapper;
        public MarketController(IMarketService marketService, IMapper mapper)
        {
            _marketService = marketService;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<MarketDto>> GetAllMarkets()
        {
            var markets = _marketService.GetStocks();

            var marketsDtos = _mapper.Map<List<MarketDto>>(markets);
     
            return Ok(marketsDtos);
        }



        [HttpGet("{id}")]
        public ActionResult<MarketDto> GetMarket([FromRoute]int id)
        {
            var market = _marketService.GetMarketById(id);

            var marketDto = _mapper.Map<MarketDto>(market);

            if (market is null)
            {
                return NotFound();
            }
            return Ok(marketDto);          
        }
    }
}
