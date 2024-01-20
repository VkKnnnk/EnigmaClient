using System;
using System.Collections.Generic;

#nullable disable

namespace Enigma_Client_V2.Model
{
    public partial class Tariff
    {
        public Tariff()
        {
            Sessions = new HashSet<Session>();
        }

        public int IdTariff { get; set; }
        public string Name { get; set; }
        public float Duration { get; set; }
        public int IdTypeTariffs { get; set; }
        public float? FixPrice { get; set; }
        public int? AppearanceHour { get; set; }
        public int? AppearanceDuration { get; set; }

        public virtual TypeTariff IdTypeTariffsNavigation { get; set; }
        public virtual ICollection<Session> Sessions { get; set; }
    }
}
