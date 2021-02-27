using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Trakindo.TSICS.Data.Model
{
    public class EmailJson
    {
        [Key]
        public int EmailJsonID { get; set; }
        public string To { get; set; }
        public string Cc { get; set; }
        public string Tag { get; set; }
        public List<EmailJsonAttachments> Attachments { get; set; }
        public List<EmailJsonkeyValues> KeyValues { get; set; }
    }
}
