using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Trakindo.TSICS.Data.Model
{
    public class TableRelateTRDPPMFinancial
    {
        public int Row { get; set; }
        public int TicketId { get; set; }
        public string TicketNo { get; set; }
        public string Industry { get; set; }
        public string Submitter { get; set; }
        public string Responder { get; set; }
        public string Familiy { get; set; }
        public string Model { get; set; }
        public string Category { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }
        public string Resolution { get; set; }
        public string DateCreated { get; set; }
        public string DateClosed { get; set; }
        public string LastUpdate { get; set; }
        public int StatusTR { get; set; }
    }
}
