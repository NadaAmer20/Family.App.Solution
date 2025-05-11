using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Family.Core.Entities
{
    public class Photo : BaseEntity
    {
        public string Title { get; set; } = string.Empty; // New property for photo title
        public string PhotoUrl { get; set; } = string.Empty; // Photo URL
        public string Description { get; set; } = string.Empty; // Photo description
        public DateTime DateTaken { get; set; } // Date the photo was taken


    }
}
