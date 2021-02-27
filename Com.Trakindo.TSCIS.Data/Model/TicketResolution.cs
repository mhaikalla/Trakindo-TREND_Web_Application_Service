using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Trakindo.TSICS.Data.Model
{
    [Table("TicketResolution")]
    public class TicketResolution
    {
        [Key]
        public int TicketResolutionId { get; set; }
        public int TicketId { get; set; }
        public int UserId { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int Status { get; set; }
        public string RespondTime { get; set; }
    }
}
