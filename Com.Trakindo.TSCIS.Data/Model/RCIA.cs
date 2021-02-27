using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Trakindo.TSICS.Data.Model
{
    [Table("CS.RCIA")]
    public class RCIA
    {
        [Key]
        public string Srv_Order { get; set; }
        public string Segment_Number { get; set; }
        public string Job_Control { get; set; }
        public string Comments { get; set; } 
        public int SMU { get; set; }
        public string SMU_Unit { get; set; }
        public string Sales_Office_Desc { get; set; }
        public string Part_Causing_Failure_Desc { get; set; }
        public string Part_Causing_Failure { get; set; }
        public DateTime? Repair_Date { get; set; }
        public string Currency { get; set; }
        public decimal Tot_Amount { get; set; }

    }
}
