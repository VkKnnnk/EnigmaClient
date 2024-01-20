using System;
using System.Collections.Generic;

#nullable disable

namespace Enigma_Client_V2.Model
{
    public partial class User
    {
        public User()
        {
            Sessions = new HashSet<Session>();
        }

        public int IdUser { get; set; }
        public string Name { get; set; }
        public int IdAuth { get; set; }
        public int? PersonalDiscount { get; set; }
        public float? Cash { get; set; }
        public string AvatarImg { get; set; }
        public bool AuthStatus { get; set; }

        public virtual Authdatum IdAuthNavigation { get; set; }
        public virtual ICollection<Session> Sessions { get; set; }
    }
}
