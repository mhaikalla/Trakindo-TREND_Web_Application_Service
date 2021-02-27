using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Trakindo.TSICS.Data.Model
{
    [Table("DPPMSummary")]
    public class DPPMSummary
    {
        [Key]
        public int DPPMSummaryId { get; set; }
        public string SRNumber { get; set; }
        public string Title { get; set; }
        public string ProblemDescription { get; set; }
        public string SRNotes { get; set; }
        public string Status { get; set; }
        public string SubStatus { get; set; }
        public string CPINumber { get; set; }
        public string DealerContactName { get; set; }
        public string TechRep { get; set; }
        public string Z_Industry { get; set; }
        public string PrimeProductGroupName { get; set; }
        public decimal CountAffectedUnit { get; set; }
        public decimal DPPMQuickScore { get; set; }
        public decimal TotalDetailedScore { get; set; }
        public string DealerPPMCurrentStatus { get; set; }
        public string PCA { get; set; }
        public DateTime? PCADate { get; set; }
        public string ICA { get; set; }
        public DateTime? ICADate { get; set; }
        public DateTime? CreateOn { get; set; }
        public DateTime? DateDealerOpen { get; set; }
        public DateTime? DateLastUpdated { get; set; }
        public DateTime? DateDealerClosed { get; set; }
        public string DealerCode { get; set; }
        public string DealershipName { get; set; }
        public string ParentDealerCode { get; set; }
        public string CaterpillarPPMStatus { get; set; }
        public string GroupName { get; set; }
    }

    public class DPPMOverview
    {
        public int DPPMSummaryId { get; set; }
        public string SRNumber { get; set; }
        public string Title { get; set; }
        public string ProblemDescription { get; set; }
        public string SRNotes { get; set; }
        public string Status { get; set; }
        public string SubStatus { get; set; }
        public string CPINumber { get; set; }
        public string DealerContactName { get; set; }
        public string TechRep { get; set; }
        public string Z_Industry { get; set; }
        public string PrimeProductGroupName { get; set; }
        public decimal CountAffectedUnit { get; set; }
        public decimal DPPMQuickScore { get; set; }
        public decimal TotalDetailedScore { get; set; }
        public string DealerPPMCurrentStatus { get; set; }
        public string PCA { get; set; }
        public DateTime? PCADate { get; set; }
        public string ICA { get; set; }
        public DateTime? ICADate { get; set; }
        public DateTime? CreateOn { get; set; }
        public DateTime? DateDealerOpen { get; set; }
        public DateTime? DateLastUpdated { get; set; }
        public DateTime? DateDealerClosed { get; set; }
        public string DealerCode { get; set; }
        public string DealershipName { get; set; }
        public string ParentDealerCode { get; set; }
        public string CaterpillarPPMStatus { get; set; }
        public string GroupName { get; set; }
    }
}
