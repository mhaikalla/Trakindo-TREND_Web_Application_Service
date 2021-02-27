using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Trakindo.TSICS.Data.Model
{
    [Table("Comment")]
    public class CommentUpload
    {
        [Key] 
        public int CommentId { get; set; }
        public string PSLSN { get; set; }
        public string Comment { get; set; }
    }
}
