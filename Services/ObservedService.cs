using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using StockAPI.Authorization;
using StockAPI.Entities;
using StockAPI.Exceptions;
using StockAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace StockAPI.Services
{
    public interface IObservedService
    {
        int CreateObserved(CreateObservedDto dto);
        IEnumerable<ObservedDto> GetAll();
        ObservedDto GetById(int id);
        void DeleteById(int id);
        void Update(UpdateObservedDto dto, int id);
    }

    public class ObservedService : IObservedService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<ObservedService> _logger;
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserContextService _userContextService;
        public ObservedService(
            ApplicationDbContext dbContext,
            IMapper mapper,
            ILogger<ObservedService> logger,
            IAuthorizationService authorizationService,
            IUserContextService userContextService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _authorizationService = authorizationService;
            _userContextService = userContextService;
        }

        public int CreateObserved(CreateObservedDto dto)
        {
            var marketItem = _dbContext.Market.FirstOrDefault(i => i.Name == dto.Name);
            if (marketItem is null)
                throw new NotFoundException($"Stock {dto.Name} not found");

            var observed = _mapper.Map<Observed>(dto);

            observed.CreatedById = _userContextService.GetUserId;

            observed.MarketId = marketItem.Id;
            observed.Profit = (observed.PurchasePrice - marketItem.Price) * observed.NumberOfActions;

            _dbContext.Observed.Add(observed);
            _dbContext.SaveChanges();

            return observed.Id;
        }

        public IEnumerable<ObservedDto> GetAll()
        {

            var observed = _dbContext.Observed.ToList();
            if (observed is null)
                throw new NotFoundException("You are not observing any stock yet");

            var observedDtos = _mapper.Map<List<ObservedDto>>(observed);
            return observedDtos;

        }



        public ObservedDto GetById(int id)
        {
           var observed = _dbContext.Observed.FirstOrDefault(s => s.Id == id);
           if (observed is null)
               throw new NotFoundException("Stock not found");

            var result = _mapper.Map<ObservedDto>(observed);
            return result;
        }

        public void DeleteById(int id)
        {
            _logger.LogError($"Stock with id: {id} DELETE action invoked");

            var observed = _dbContext.Observed.FirstOrDefault(s => s.Id == id);
            if (observed is null)
                throw new NotFoundException("Stock not found");

            var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, observed,
              new ResourceOperationRequirement(ResourceOperation.Delete)).Result;

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException();
            }

            _dbContext.Observed.Remove(observed);
            _dbContext.SaveChanges();         
        }

        public void Update(UpdateObservedDto dto, int id)
        {

            var observed = _dbContext.Observed.FirstOrDefault(s => s.Id == id);
            if (observed is null)
                throw new NotFoundException("Stock not found");

            var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, observed,
                new ResourceOperationRequirement(ResourceOperation.Update)).Result;

            if(!authorizationResult.Succeeded)
            {
                throw new ForbidException();
            }

            observed.NumberOfActions = dto.NumberOfActions;
            observed.PurchasePrice = dto.PurchasePrice;

            _dbContext.SaveChanges();         
        }
    }
}
