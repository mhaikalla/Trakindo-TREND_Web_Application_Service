using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Trakindo.TSICS.Data.Model
{
    [Table("UserDevices")]
    public class UserDevices
    {
        public int Id { get;set; }
        public int UserId { get; set; }
        public string PlayerId { get; set; }
        public int Status { get; set; }
    }
}
