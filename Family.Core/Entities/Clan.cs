using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Family.Core.Entities
{
    public class Clan : BaseEntity
    {
        public string Name { get; set; } = string.Empty; // Clan name
        public string PhotoUrl { get; set; } = string.Empty; // Clan photo
        public string Region { get; set; } = string.Empty; // Clan region

        // Navigation property
        public ICollection<Branch> Branches { get; set; } = new HashSet<Branch>();
    }


}

