using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Trakindo.TSICS.Data.Model
{
    [Table("PartResponsibleCostImpactAnalysis")]
    public class PartResponsibleCostImpactAnalysis
    {
        [Key]
        public int PartResponsibleId { get; set; }
        public string ServiceOrder { get; set; }
        public string SegmentNumber { get; set; }
        public string JobControl { get; set; }
        public string PartCausingFailure { get; set; }
        public string PartCausingFailureDescription { get; set; }
        public string RepairingDealer { get; set; }
        public string UpdateIndicator { get; set; }
        public DateTime? RepairDate { get; set; }
        public string CATProduct { get; set; }
        public string SerialNumber { get; set; }
        public string PrefixSN { get; set; }
        public string Model { get; set; }
        public int ServiceMeterMeasurement { get; set; }
        public string UnitMes { get; set; }
        public string EmployeeId { get; set; }
        public string ReportType { get; set; }
        public string PartNumberResponsibleCATProduct { get; set; }
        public int Quantity { get; set; }
        public string PartResponsibleGroupCATProduct { get; set; }
        public string GroupNumber { get; set; }
        public string GroupDescription { get; set; }
        public string ProductProblem { get; set; }
        public string ProductProblemDescription { get; set; }
        public string InoperableIndicator { get; set; }
        public string Comments { get; set; }
        public string ServiceOrderStatus { get; set; }
        public string DurabilityFailure { get; set; }
        public string PartResponsibleFailureComponentSN { get; set; }
        public int PartResponsibleFailureComponentHours { get; set; }
        public string PartResponsibleFailureComponentUniMes { get; set; }
        public string SIMSReportStatus { get; set; }
        public string ServiceClaim { get; set; }
        public string DealerClaim { get; set; }
        public decimal InitialClaim { get; set; }
        public string SalesOffice { get; set; }
        public string SalesOfficeDescription { get; set; }
        public string Customer { get; set; }
        public string CustomerDescription { get; set; }
        public decimal ActualCostPart { get; set; }
        public decimal ActualCostLabor { get; set; }
        public decimal ActualCostMiscellaneous { get; set; }
        public decimal TotalAmount { get; set; }
        public string Currency { get; set; }
        public decimal TotalSO { get; set; }
        public decimal TotalClaim { get; set; }
        public decimal TotalSettled { get; set; }
        public decimal Settled { get; set; }
        public decimal Recovery { get; set; }
        public string Plant { get; set; }
        public string HIDTaskId { get; set; }
        public string RentStatus { get; set; }
    }
    public class PartResponsibleCostImpactAnalysisCustom
    {
        public string PartNumber{ get; set; }
        public decimal Total_SO { get; set; }
        public decimal Total_Claim { get; set; }
        public decimal Total_Settled { get; set; }

    }
}
