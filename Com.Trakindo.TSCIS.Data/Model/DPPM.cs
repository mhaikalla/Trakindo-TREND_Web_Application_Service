using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Trakindo.TSICS.Data.Model
{
    [Table("DPPM")]
    public class DPPM
    {
        [Key]
        public int DPPMId { get; set; }
        public string SRNumber { get; set; }
        public string Title { get; set; }
        public string PrimeProductModel { get; set; }
        public string PrimeProductFamily { get; set; }
        public string ProblemDescription { get; set; }
        public string SRNotes { get; set; }
        public string Status { get; set; }
        public string SubStatus { get; set; }
        public string CPINumber { get; set; }
        public string DealerContactName { get; set; }
        public string TechRep { get; set; }
        public string z_Industry { get; set; }
        public string PrimeProductGroupName { get; set; }
        public decimal CountAffectedUnit { get; set; }
        public decimal DPPMQuickScore { get; set; }
        public decimal TotalDetailedStore { get; set; }
        public string PCA { get; set; }
        public DateTime? PCADate { get; set; }
        public string DealerPPMCurrentStatus { get; set; }
        public string ICA { get; set; }
        public DateTime? ICADate { get; set; }
        public DateTime? CreateOn { get; set; }
        public DateTime? DateDealerOpen { get; set; }
        public DateTime? DateLastUpdated { get; set; }
        public DateTime? DateDealerClosed { get; set; }
        public string DealerCode { get; set; }
        public string DealershipName { get; set; }
        public string ParentDealershipCode { get; set; }
        public string CaterpillarPPMStatus { get; set; }
        public string GroupNumber { get; set; }
        public string PartNumber { get; set; }
        public string PDCode { get; set; }
        public int PartHours { get; set; }
        public string TRNumber { get; set; }
        public string PSLType { get; set; }
        public string Description { get; set; }
        public DateTime? LetterDate { get; set; }
        public DateTime? TerminationDate { get; set; }
        public string Validation { get; set; }
        public int Completed { get; set; }
        public int TotalDowntime { get; set; }
        public int TotalSettlement { get; set; }
        public int TotalExpected { get; set; }
        public int TotalInvoice { get; set; }
        public int TotalActualSOCost { get; set; }
        public int ServiceOrderCount { get; set; }
        public int AverageDowntime { get; set; }
        public int Settlement { get; set; }
        public int Recovery { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
