using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Trakindo.TSICS.Data.Model
{
    [Table("TicketCategory")]
    public class TicketCategory
    {
        [Key]
        public int TicketCategoryId { get; set; }
        public string Name { get; set; }
        public int Sort { get; set; }
        public int Status { get; set; }
    }
}
