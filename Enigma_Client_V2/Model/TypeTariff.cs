using System;
using System.Collections.Generic;

#nullable disable

namespace Enigma_Client_V2.Model
{
    public partial class TypeTariff
    {
        public TypeTariff()
        {
            Computers = new HashSet<Computer>();
            Tariffs = new HashSet<Tariff>();
        }

        public int IdTypeTariffs { get; set; }
        public string Name { get; set; }
        public int PriceHour { get; set; }

        public virtual ICollection<Computer> Computers { get; set; }
        public virtual ICollection<Tariff> Tariffs { get; set; }
    }
}
