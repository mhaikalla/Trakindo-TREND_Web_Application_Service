using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Trakindo.TSICS.Data.Model
{
    [Table("EdwUnit")]
    public class EdwUnit
    {
        [Key]
        public int EdwUnitId { get; set; }
        public string UnitSN { get; set; }
        public string Customer { get; set; }
        public string Location { get; set; }
        public string Make { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string ArrangementNo { get; set; }
        public string Family { get; set; }
        public string Model { get; set; }
        public string SMU { get; set; }

    }
}
