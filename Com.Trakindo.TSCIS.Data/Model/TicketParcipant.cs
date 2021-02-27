using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Trakindo.TSICS.Data.Model
{
    [Table("TicketParcipant")]
    public class TicketParcipant
    {
        [Key]
        public int TicketParcipantId { get; set; }
        public int TicketId { get; set; }
        public int UserId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int Status { get; set; }
    }

    public class TicketParticipantWithName
    {
        public TicketParcipant TicketParcipant { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }
    }
}
