using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockAPI.Exceptions
{
    public class NotFoundException : Exception //Dziedziczy po Exception, po to aby można ją wywołać w instrukcji throw
    {
        public NotFoundException(string message) : base(message) // base(message) - konstruktor bazowy z klasy exceptions
        {

        }
    }
}
