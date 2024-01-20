using System;
using System.Collections.Generic;

#nullable disable

namespace Enigma_Client_V2.Model
{
    public partial class VideoCard
    {
        public VideoCard()
        {
            Computers = new HashSet<Computer>();
        }

        public int IdVideoCard { get; set; }
        public string Model { get; set; }
        public int Memory { get; set; }

        public virtual ICollection<Computer> Computers { get; set; }
    }
}
