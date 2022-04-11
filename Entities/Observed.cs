using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StockAPI.Entities
{
    public class Observed
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public float PurchasePrice { get; set; }
        public int NumberOfActions { get; set; }
        public float Profit { get; set; }
        public int MarketId { get; set; }
        //public string ApplicationUserId { get; set; }
        public virtual Market Market { get; set; }
        //public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
