using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Trakindo.TSICS.Data.Model
{
    [Table("SIMSErrorSummary")]
    public class SIMSErrorSummary
    {
        [Key]
        public int SIMSErrorSummaryId { get; set; }
        public string ErrorDescription { get; set; }
        public string RepairingDealer { get; set; }
        public string WorkOrder { get; set; }
        public int WorkOrderSegment { get; set; }
        public string SerialNumber { get; set; }
        public DateTime? RepairDate { get; set; }
        public string PartCausingFailure { get; set; }
        public string PartCausingFailureDescription { get; set; }
        public string GroupNumber { get; set; }
        public string GroupDescription { get; set; }
        public string Comment { get; set; }
        public int EmployeeId { get; set; }
    }
}
