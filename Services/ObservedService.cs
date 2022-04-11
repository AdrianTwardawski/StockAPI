using AutoMapper;
using StockAPI.Entities;
using StockAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockAPI.Services
{
    public interface IObservedService
    {
        int CreateObserved(CreateObservedDto dto);
        ObservedDto GetById(int id);
        bool DeleteById(int id);
        bool Update(UpdateObservedDto dto, int id);
    }

    public class ObservedService : IObservedService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        public ObservedService(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }


        public int CreateObserved(CreateObservedDto dto)
        {
            var marketItem = _dbContext.Market.FirstOrDefault(i => i.Name == dto.Name);

            var observed = _mapper.Map<Observed>(dto);

            if (marketItem != null)
            {
                observed.MarketId = marketItem.Id;
                observed.Profit = (observed.PurchasePrice - marketItem.Price) * observed.NumberOfActions;
                _dbContext.Observed.Add(observed);
                _dbContext.SaveChanges();
            }

            return observed.Id;
        }


        public ObservedDto GetById(int id)
        {
           var observed = _dbContext.Observed.FirstOrDefault(s => s.Id == id);

            var result = _mapper.Map<ObservedDto>(observed);
            return result;
        }

        public bool DeleteById(int id)
        {
            var observed = _dbContext.Observed.FirstOrDefault(s => s.Id == id);
            if (observed is null) return false;

            _dbContext.Observed.Remove(observed);
            _dbContext.SaveChanges();

            return true;
        }

        public bool Update(UpdateObservedDto dto, int id)
        {
            var observed = _dbContext.Observed.FirstOrDefault(s => s.Id == id);
            if (observed is null)
                return false;

            observed.NumberOfActions = dto.NumberOfActions;
            observed.PurchasePrice = dto.PurchasePrice;

            _dbContext.SaveChanges();

            return true;
        }
    }
}
