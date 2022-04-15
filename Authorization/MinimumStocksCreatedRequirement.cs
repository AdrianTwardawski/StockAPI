using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockAPI.Authorization
{
    public class MinimumStocksCreatedRequirement : IAuthorizationRequirement
    {
        public int StocksNumber { get; }

        public MinimumStocksCreatedRequirement(int stocksNumber)
        {
            stocksNumber = StocksNumber;
        }


    }
}
