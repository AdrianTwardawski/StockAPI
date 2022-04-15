using Microsoft.AspNetCore.Authorization;
using StockAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace StockAPI.Authorization
{
    public class MinimumStocksCreatedRequirementHandler : AuthorizationHandler<MinimumStocksCreatedRequirement>
    {
        private readonly ApplicationDbContext _context;
        public MinimumStocksCreatedRequirementHandler(ApplicationDbContext context)
        {
            _context = context;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumStocksCreatedRequirement requirement)
        {
            var userId = int.Parse(context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);

            var matchingId = _context.Observed.Count(o => o.CreatedById == userId);

            if(matchingId >= requirement.StocksNumber)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
