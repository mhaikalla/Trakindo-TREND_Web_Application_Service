using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Trakindo.TSICS.Data.Model
{
    public class ApiJsonCommentTR
    {
        [Key]
        public int id { get; set; }
        public string type { get; set; }
        public virtual ApiJsonCommentDateTR date { get; set; }
        public virtual ApiJsonCommentSenderTR sender { get; set; }
        public string text { get; set; }
        public virtual List<ApiJsonCommentImageTR> image { get; set; }
        public virtual List<ApiJsonCommentTR> children { get; set; }
        public bool isRemovable { get; set; }
        public string nama { get; set; }
    }
}
