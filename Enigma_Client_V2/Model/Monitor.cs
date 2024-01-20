using System;
using System.Collections.Generic;

#nullable disable

namespace Enigma_Client_V2.Model
{
    public partial class Monitor
    {
        public Monitor()
        {
            Computers = new HashSet<Computer>();
        }

        public int IdMonitor { get; set; }
        public string Model { get; set; }
        public int Frequency { get; set; }
        public string Resolution { get; set; }

        public virtual ICollection<Computer> Computers { get; set; }
    }
}
