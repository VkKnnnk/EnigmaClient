using System;
using System.Collections.Generic;

#nullable disable

namespace Enigma_Client_V2.Model
{
    public partial class Computer
    {
        public Computer()
        {
            Sessions = new HashSet<Session>();
        }

        public int IdComp { get; set; }
        public int IdTypeTariffs { get; set; }
        public int? IdProcessor { get; set; }
        public int? IdVideoCard { get; set; }
        public int? Ram { get; set; }
        public int? IdMonitor { get; set; }
        public int? IdKeyboard { get; set; }
        public int? IdMouse { get; set; }

        public virtual Keyboard IdKeyboardNavigation { get; set; }
        public virtual Monitor IdMonitorNavigation { get; set; }
        public virtual Mouse IdMouseNavigation { get; set; }
        public virtual Processor IdProcessorNavigation { get; set; }
        public virtual TypeTariff IdTypeTariffsNavigation { get; set; }
        public virtual VideoCard IdVideoCardNavigation { get; set; }
        public virtual ICollection<Session> Sessions { get; set; }
    }
}
