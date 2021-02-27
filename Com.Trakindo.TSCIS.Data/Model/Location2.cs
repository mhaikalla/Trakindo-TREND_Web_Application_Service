using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Com.Trakindo.TSICS.Data.Model
{
    [Table("Location2")]
    public class Location2
    { 
        [Key]
        public int Location2Id { get; set; }
        public string JCode { get; set; }
        public string DealerDescription { get; set; }
        public string Area { get; set; }
        public string Region { get; set; }
        public string SalesOffice { get; set; }
    }
}
