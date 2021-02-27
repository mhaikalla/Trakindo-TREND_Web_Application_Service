using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Trakindo.TSICS.Data.Model
{
    public class MobileLogin
    {
        [Key]
        public string Xupj { get; set; }
        public string Password { get; set; }
        public string PlayerId { get; set; }

    }
}
