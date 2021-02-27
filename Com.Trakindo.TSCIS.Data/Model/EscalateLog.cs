using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Trakindo.TSICS.Data.Model
{
    [Table("EscalateLog")]
    public class EscalateLog
    {
        [Key]
        public int IdEscalateLog { get; set; }

        public int EscalateFrom { get; set; }

        public int EscalateTo { get; set; }

        public int TicketId { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
