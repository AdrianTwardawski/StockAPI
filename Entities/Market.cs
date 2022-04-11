using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StockAPI.Entities
{
    public class Market
    {   
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }       
        public float Change { get; set; }
        public string TradesValue { get; set; }
        public string Time { get; set; }
    }
}
