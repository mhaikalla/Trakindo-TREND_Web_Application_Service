using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Trakindo.TSICS.Data.Model
{
    [Table("PSLMaster")]
    public class PSLMaster
    {
        [Key]
        public int PSLMasterId { get; set; } 
        public string PSLId { get; set; } 
        public string PSLSN { get; set; }
        public string Description { get; set; }
        public int EquipmentId { get; set; }
        public string SerialNumber { get; set; }
        public string Model { get; set; }
        public string ModelMEP { get; set; }
        public string AssignDealer { get; set; }
        public string CatCustomer { get; set; }
        public string SalesOfficeCode { get; set; }
        public string SalesOffice { get; set; }
        public string Area { get; set; }
        public string Region { get; set; }
        public int CustomerNo { get; set; }
        public string SapCustomerName { get; set; }
        public string PSSR { get; set; }
        public string PSLType { get; set; }
        public string SapPSLStatus { get; set; }
        public string CatPSLStatus { get; set; }
        public DateTime? LetterDate { get; set; }
        public DateTime? TerminationDate { get; set; }
        public DateTime? RepairDate { get; set; }
        public decimal LaborHours { get; set; }
        public DateTime? CatDeliveryDate { get; set; }
        public int ServiceClaimAllowanceGroup { get; set; }
        public string ProductSmuRange1 { get; set; }
        public string ProductAgeRange1 { get; set; }
        public string ProductSmuRange2 { get; set; }
        public string ProductAgeRange2 { get; set; }
        public string ProductSmuRange3 { get; set; }
        public string ProductAgeRange3 { get; set; }
        public string ProductSmuRange4 { get; set; }
        public string ProductAgeRange4 { get; set; }
        public string PCR { get; set; }
        public string ServiceRequestNo { get; set; }
        public DateTime? ServiceRequestDate { get; set; }
        public string QuotationNo { get; set; }
        public DateTime? QuotationApprovalDate { get; set; }
        public string ServiceOrderNo { get; set; }
        public string ServiceOrderStatusDesc { get; set; }
        public DateTime? ServiceOrderOpenDate { get; set; }
        public DateTime? ServiceOrderReleaseDate { get; set; }
        public DateTime? ServiceOrderClosedDate { get; set; }
        public string LastSmu { get; set; }
        public DateTime? SapDeliveryDate { get; set; }
        public string TerritoryIndicator { get; set; }
        public string CatCompleteIndicator { get; set; }
        public string RemovedByCat { get; set; }
        public string PSPSMU { get; set; }
        public string Plant { get; set; }
        public string HIDStatus { get; set; }
        public int PslAge { get; set; }
        public string DaysToExpired { get; set; }
        public string SmuToOverlimitRange1 { get; set; }
        public string MonthToAgeOverlimitRange1 { get; set; }
        public string Range1Overlimit { get; set; }
        public string SmuToMaxOverlimit { get; set; }
        public string MonthToMaxAgeOverlimit { get; set; }
        public string MaxOverlimit { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public int UnitAge { get; set; }
        public int FollowUpDuration { get; set; }
        public int WipAge { get; set; }
        public string Validation { get; set; }
        public string Comment { get; set; }
        public string PriorityLevel { get; set; }
        public string EngMacLtrDescription { get; set; }
        public string DefinitionIndustry { get; set; }
        public string DefinitionSubIndustry { get; set; }
        public string PTDescription { get; set; }
        public string MakeDescription { get; set; }
        public string PwcCode { get; set; }
        public bool SimsAvailable { get; set; }
        public string WarClaimNo { get; set; }
        public decimal TotalActualSoCost { get; set; }
        public decimal SoTotalCost { get; set; }
        public string ServiceClaim { get; set; }
        public decimal TotalAmount { get; set; } 
        public decimal WarrantyClaimTotal { get; set; }
        public decimal TotalSettled { get; set; }
        public decimal Settled { get; set; }
        public decimal Recovery { get; set; }
        public string CompleteYear { get; set; }
        public string RentStatus { get; set; }
    }
}
