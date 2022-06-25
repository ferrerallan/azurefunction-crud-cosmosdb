using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureFunctionCosmosDB.Models
{
    public class Guitar
    {
        public string id { get; set; }
        public string Manufacturer { get; set; }
        public int NumberOfStrings { get; set; }
        public bool ActivePickup { get; set; }

        public string GetInfo()
        {
            return $"Manufacturer:{this.Manufacturer} | NumberOfStrings:{this.NumberOfStrings} | ActivePickup:{this.ActivePickup}";
        }
    }
}
