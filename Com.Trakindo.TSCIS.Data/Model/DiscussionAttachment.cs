using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Trakindo.TSICS.Data.Model
{
    [Table("DiscussionAttachment")]
    public class DiscussionAttachment
    {
        [Key]
        public int DiscussionAttachmentId { get; set; }
        public int TicketDiscussionId { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }
    }
}
