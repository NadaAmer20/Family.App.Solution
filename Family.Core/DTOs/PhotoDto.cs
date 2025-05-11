using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Family.Core.DTOs
{
    public class PhotoDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty; // New property for photo title
        public string PhotoUrl { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string DateTaken { get; set; } = string.Empty; // Formatted date
    }
}
