using System;
using System.Collections.Generic;

#nullable disable

namespace Enigma_Client_V2.Model
{
    public partial class Session
    {
        public int IdSession { get; set; }
        public int IdUser { get; set; }
        public int IdTariff { get; set; }
        public DateTime StartSession { get; set; }
        public DateTime EndSession { get; set; }
        public int IdComputer { get; set; }
        public int Cost { get; set; }
        public bool Status { get; set; }

        public virtual Computer IdComputerNavigation { get; set; }
        public virtual Tariff IdTariffNavigation { get; set; }
        public virtual User IdUserNavigation { get; set; }
    }
}
