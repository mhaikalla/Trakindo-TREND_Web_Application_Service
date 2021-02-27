using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Trakindo.TSICS.Data.Model
{
    [Table("TicketDiscussion")]
    public class TicketDiscussion
    {
        [Key]
        public int TicketDiscussionId { get; set; }
        public int TicketId { get; set; }
        public int UserId { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int TicketNoteId { get; set; }
        public int Status { get; set; }
        public string RespondTime { get; set; }
    }
    public class CustomTicketDiscussion
    {
        public int TicketDiscussionId { get; set; }
        public int TicketId { get; set; }
        public int UserId { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int TicketNoteId { get; set; }
        public int Status { get; set; }
        public Double RespondTime { get; set; }
        public bool IsInserted { get; set; }
    }
    public class TicketDiscussionRemoveChat
    {
        public int removeChat { get; set; }
    } 
}
