using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Trakindo.TSICS.Data.Model
{
    [Table("WarrantyList")]
    public class WarrantyList
    {
        [Key]
        public int WarrantyListId { get; set; }
        public string ServiceOrder { get; set; }
        public string WarrantyClaim { get; set; }
        public DateTime? PostingDate { get; set; }
        public string Currency { get; set; }
        public string TransactionType { get; set; }
        public string TransactionDescription { get; set; }
        public string RegistrationNumber { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public string SettlementNumber { get; set; }
        public DateTime? SettlementDate { get; set; }
        public string Status { get; set; }
        public string SerialNumber { get; set; }
        public string CustomerName { get; set; }
        public string SalesOffice { get; set; }
        public string Area { get; set; }
        public string Region { get; set; }
        public string JobControl { get; set; }
        public string ClaimClass { get; set; }
        public int InvoicedParts { get; set; }
        public int InvoicedLabor { get; set; }
        public int InvoicedVehicle { get; set; }
        public int InvoicedTravel { get; set; }
        public int InvoicedMisc { get; set; }
        public int InvoicedTotal { get; set; }
        public int ExpectedSettledParts { get; set; }
        public int ExpectedSettledLabor { get; set; }
        public int ExpectedSettledVehicle { get; set; }
        public int ExpectedSettledTravel { get; set; }
        public int ExpectedSettledMisc { get; set; }
        public int ClaimedParts { get; set; }
        public int ClaimedLabor { get; set; }
        public int ClaimedVehicle { get; set; }
        public int ClaimedTravel { get; set; }
        public int ClaimedMisc { get; set; }
        public int ClaimedTotal { get; set; }
        public int SettledTotal { get; set; }
        public int ExchangeRate { get; set; }

    }
}
