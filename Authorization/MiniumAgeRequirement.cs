using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockAPI.Authorization
{
    public class MiniumAgeRequirement : IAuthorizationRequirement
    {
        public int MinimumAge { get;}
        public MiniumAgeRequirement(int minimumAge)
        {
            MinimumAge = minimumAge;
        }
    }
}
