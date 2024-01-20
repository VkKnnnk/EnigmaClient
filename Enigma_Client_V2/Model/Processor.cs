using System;
using System.Collections.Generic;

#nullable disable

namespace Enigma_Client_V2.Model
{
    public partial class Processor
    {
        public Processor()
        {
            Computers = new HashSet<Computer>();
        }

        public int IdProcessor { get; set; }
        public string Model { get; set; }
        public string CoreAmount { get; set; }
        public string Frequency { get; set; }

        public virtual ICollection<Computer> Computers { get; set; }
    }
}
