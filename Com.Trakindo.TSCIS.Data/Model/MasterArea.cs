using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Trakindo.TSICS.Data.Model
{
    [Table("MasterArea")]
    public class MasterArea
    {
        [Key]
        public int MasterAreaId { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }
    }
}
