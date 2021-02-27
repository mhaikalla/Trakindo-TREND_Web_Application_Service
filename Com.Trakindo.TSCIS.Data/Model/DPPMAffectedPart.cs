using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Trakindo.TSICS.Data.Model
{
    [Table("DPPMAffectedPart")]
    public class DPPMAffectedPart
    {
        [Key]
        public int DPPMAffectedPartId { get; set; }
        public string PartNumber { get; set; }
        public string CreatedBy { get; set; }
        public string DealerPPM { get; set; }
        public int PartHours { get; set; }
        public string Parts { get; set; }
        public string PDCode { get; set; }
        public string PrimaryPart { get; set; }
        public int FailureQuantity { get; set; }
        public string RegionText { get; set; }
        public string BranchText { get; set; }
        public int TotalDowntime { get; set; }
        public int AverageDowntime { get; set; }
        public int ServiceOrderCount { get; set; }
        public int TotalActualSOCost { get; set; }
        public int TotalInvoice { get; set; }
        public int TotalExpected { get; set; }
        public int TotalSettlement { get; set; }
    }
}
