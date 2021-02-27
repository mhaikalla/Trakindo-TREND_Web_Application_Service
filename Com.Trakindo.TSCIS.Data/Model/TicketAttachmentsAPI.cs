using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Trakindo.TSICS.Data.Model
{
    public class TicketAttachmentsAPI
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Level { get; set; }
        public string Uri { get; set; }
        public string Type { get; set; }
        public string NameWithOutBase64 { get; set; }
    }
}
