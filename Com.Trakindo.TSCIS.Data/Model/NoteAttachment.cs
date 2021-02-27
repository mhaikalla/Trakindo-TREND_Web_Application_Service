using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Trakindo.TSICS.Data.Model
{
    [Table("NoteAttachment")]
    public class NoteAttachment
    {
        [Key]
        public int NoteAttachmentId { get; set; }
        public int TicketNoteId { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }
    }
}
