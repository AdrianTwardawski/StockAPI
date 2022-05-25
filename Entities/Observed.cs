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
        public int? CreatedById { get; set; } //referencja do tabeli User
        public virtual User CreatedBy { get; set; } //obiekt User, do którego można się bezpośrednio odnieść
        public int MarketId { get; set; }
        public virtual Market Market { get; set; }

    }
}
