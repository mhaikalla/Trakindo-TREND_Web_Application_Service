using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Trakindo.TSICS.Data.Model
{
    [Table("TicketAttachment")]
    public class TicketAttachment
    {
        [Key]
        public int TicketAttachmentId	{ get; set; }
        public int TicketId	{ get; set; }
        public string Name	{ get; set; }
        public string LevelUser { get; set; }
        public int Status { get; set; }
    }
}
