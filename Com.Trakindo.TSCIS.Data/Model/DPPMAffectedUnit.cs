using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Trakindo.TSICS.Data.Model
{
    [Table("DPPMAffectedUnit")]
    public class DPPMAffectedUnit
    {
        [Key]
        public int DPPMAffectedUnitId { get; set; }
        public string DealerPPM { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string BrandAffiliation { get; set; }
        public DateTime? BuildDate { get; set; }
        public string Comment { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedByDelegate { get; set; }
        public string CustomerName { get; set; }
        public DateTime? DateFailure { get; set; }
        public DateTime? DateOfRepair { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string EventType { get; set; }
        public string HoursAtFailure { get; set; } 
        public string ModifiedBy { get; set; }
        public string ModifiedByDelegate { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string Owner { get; set; }
        public string PrimaryAffectedUnit { get; set; }
        public int PrimeProductApplicationCategoryCode { get; set; }
        public string PrimeProductApplicationCategoryName { get; set; }
        public string PrimeProductApplicationCode { get; set; }
        public string PrimeProductApplicationName { get; set; }
        public string PrimeProductFamily { get; set; }
        public string PrimeProductFamilyCode { get; set; }
        public string PrimeProductGeneralArrangmentNumber { get; set; }
        public string PrimeProductModel { get; set; }
        public string PrimeProductSourceFacility { get; set; }
        public string ProductSNP { get; set; }
        public DateTime? RecordCreatedOn { get; set; }
        public string SerialNumber { get; set; }
        public string ServiceMeterUnit { get; set; }
        public string SNP { get; set; }
        public string SourcePlantCode { get; set; }
        public string Status { get; set; }
        public string StatusReason { get; set; }
        public string WorkOrderNumber { get; set; }
    }
}
