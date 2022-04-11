using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StockAPI.Models
{
    public class UpdateObservedDto
    {
        [DisplayName("Purchase Price")]
        public float PurchasePrice { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Number of actions must not be empty")]
        [DisplayName("Number of actions")]
        public int NumberOfActions { get; set; }
    }
}
