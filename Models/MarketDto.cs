using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace StockAPI.Models
{
    public class MarketDto
    {       
        public string Name { get; set; }
        public float Price { get; set; }
        [DisplayName("Price [PLN]")]
        public float Change { get; set; }
        public string TradesValue { get; set; }
        public string Time { get; set; }
    }
}
