using Family.Core.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Family.Core.Entities
{
    public class Branch : BaseEntity
    {
        public string Name { get; set; } = string.Empty; // Branch name
        public string PhotoUrl { get; set; } = string.Empty; // Branch photo
        public string Region { get; set; } = string.Empty; // Branch region
        public int ClanId { get; set; }
        public Clan Clan { get; set; } = null!;
        public ICollection<AppUser>? Persons { get; set; }

    }
}
