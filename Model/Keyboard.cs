using System;
using System.Collections.Generic;

#nullable disable

namespace Enigma_Client_V2.Model
{
    public partial class Keyboard
    {
        public Keyboard()
        {
            Computers = new HashSet<Computer>();
        }

        public int IdKeyboard { get; set; }
        public string Model { get; set; }

        public virtual ICollection<Computer> Computers { get; set; }
    }
}
