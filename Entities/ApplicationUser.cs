using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockAPI.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public List<Observed> Items { get; set; }
    }

}
