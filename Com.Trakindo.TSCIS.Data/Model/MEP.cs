using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Com.Trakindo.TSICS.Data.Model
{
    [Table("MEP")]
    public class MEP
    {
        [Key]
        public int IdMEP { get; set; }
        public string Area { get; set; }
        public string Region { get; set; }
        public string Plant { get; set; }
        public string SalesOffice { get; set; }
        public string SalesOfficeDescription { get; set; }
        public string CustId { get; set; }
        public string CustomerName { get; set; }
        public string ShipToID { get; set; }
        public string GroupName { get; set; }
        public string RepsName { get; set; }
        public string IndCode { get; set; }
        public string DefinitionIndustry { get; set; }
        public string DefinitionSubIndustry { get; set; }
        public string SerialNumber { get; set; }
        public string ENGMACLTR { get; set; }
        public string ENGMACLTRDescription { get; set; }
        public string Make { get; set; }
        public string PT { get; set; }
        public string PTDescription { get; set; }
        public string Model { get; set; }
        public string Qty { get; set; }
        public string ArrNumber { get; set; }
        public string EquipmentNumber { get; set; }
        public string EngineMake { get; set; }
        public string EngineModel { get; set; }
        public string EngineSerialNumber { get; set; }
        public string PWCCode { get; set; }
        public string SMU { get; set; }
        public string LifeToDateSMU { get; set; }
        public DateTime? SMUUpDate { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string ShipToAddress { get; set; }
        public string LocationDetail { get; set; }
        public string Comment { get; set; }
        public string EquipmentStatus { get; set; }
    }
}
