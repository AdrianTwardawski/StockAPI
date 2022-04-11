using AutoMapper;
using StockAPI.Entities;
using StockAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockAPI
{
    public class MarketMappingProfile : Profile
    {
        public MarketMappingProfile()
        {
            CreateMap<Market, MarketDto>();
            CreateMap<Observed, ObservedDto>()
                .ForMember(m => m.CurrentPrice, c => c.MapFrom(s => s.Market.Price));
            CreateMap<CreateObservedDto, Observed>();          
        }

        
    }
}
