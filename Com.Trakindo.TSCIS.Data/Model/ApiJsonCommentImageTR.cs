using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Trakindo.TSICS.Data.Model
{
    public class ApiJsonCommentImageTR
    {
        [Key]
        public int id { get; set; }
        public string src { get; set; }
        public string Type { get; set; }
        public string nama { get; set; }
    }
}
