using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Trakindo.TSICS.Data.Model
{
    public class WebStatus
    {
        public int code { get; set; }

        [StringLength(255)]
        public string description { get; set; }
    }
}
