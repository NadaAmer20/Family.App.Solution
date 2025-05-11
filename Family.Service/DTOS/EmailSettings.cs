using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Family.Service.DTOS
{
    public class EmailSettings
    {
        public Guid Id { get; set; }
        public string SMTPHost { get; set; }
        public int SMTPPort { get; set; }
        public string SMTPUsername { get; set; }
        public string SMTPPassword { get; set; }
        public bool EnableSSL { get; set; }
        public string FromEmail { get; set; }
        public string FromName { get; set; }
    }
}
