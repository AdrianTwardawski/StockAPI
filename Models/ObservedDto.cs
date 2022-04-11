using StockAPI.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace StockAPI.Models
{
    public class ObservedDto
    {       
        public string Name { get; set; }
        [DisplayName("Current Price")]
        public float CurrentPrice { get; set; }
        [DisplayName("Purchase Price")]
        public float PurchasePrice { get; set; }
        [DisplayName("Number of actions")]
        public int NumberOfActions { get; set; }
        public float Profit { get; set; }     
       
    }
}
