using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Trakindo.TSICS.Data.Model
{
    public class ApiJsonCommentDateTR
    {
        [Key]
        public int id { get; set; }
        public string day { get; set; }
        public string time { get; set; } 
    }
}
