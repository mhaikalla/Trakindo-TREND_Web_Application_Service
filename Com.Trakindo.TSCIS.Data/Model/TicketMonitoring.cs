using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Trakindo.TSICS.Data.Model
{
    public class TicketMonitoring
    {
        public int Row { get; set; }
        public int TicketId { get; set; }
        public string TicketNo { get; set; }
        public string Category { get; set; }
        public string Title { get; set; } 
        public string Submitter { get; set; }
        public string Responder { get; set; }
        public string TicketCreated { get; set; }
        public string TicketClosed { get; set; }
        public string TicketUpdated { get; set; }
        public string TicketStatus { get; set; }
        public string UserStatus { get; set; }
    }
}
