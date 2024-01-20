using System;
using System.Collections.Generic;

#nullable disable

namespace Enigma_Client_V2.Model
{
    public partial class Authdatum
    {
        public Authdatum()
        {
            Users = new HashSet<User>();
        }

        public int IdAuth { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
