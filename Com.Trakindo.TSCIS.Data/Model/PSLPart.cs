using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Com.Trakindo.TSICS.Data.Model
{
    [Table("PSLPart")]
    public class PSLPart
    {
        [Key]
        public int PSLPartId { get; set; }
        public string PSLNo { get; set; }
        public string PartNumber { get; set; }
        public string Description { get; set; }
        public string RequiredQuantity { get; set; }
    }
}
