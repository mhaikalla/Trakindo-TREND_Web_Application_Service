using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Trakindo.TSICS.Data.Model
{
    public class MobileUser
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
    }
}
