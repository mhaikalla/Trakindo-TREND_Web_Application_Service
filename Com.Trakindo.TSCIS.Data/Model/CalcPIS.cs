using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Trakindo.TSICS.Data.Model
{
    [Table("CalcPIS")]
    public class CalcPIS
    {
        [Key]
        public int CalcPisId { get; set; }
        public string PSLSN { get; set; }
        public string ProgramNo { get; set; }
        public string Description { get; set; }
        public string SerialNumber { get; set; }
        public string Model { get; set; }
        public string AssignDealer { get; set; }
        public string CatCustomer { get; set; }
        public string Area { get; set; }
        public string Region { get; set; }
        public string SalesOffice { get; set; }
        public string PSLType { get; set; }
        public string CatPSLStatus { get; set; }
        public DateTime? LetterDate { get; set; }
        public DateTime? TerminationDate { get; set; }
        public DateTime? RepairDate { get; set; }
        public int LaborHours { get; set; }
        public DateTime? CatDeliveryDate { get; set; }
        public int ServiceClaimAllowanceGroup { get; set; }
        public string ProductSMURange1 { get; set; }
        public string ProductAgeRange1 { get; set; }
        public string ProductSMURange2 { get; set; }
        public string ProductAgeRange2{ get; set; }
        public string ProductSMURange3 { get; set; }
        public string ProductAgeRange3 { get; set; }
        public string ProductSMURange4 { get; set; }
        public string ProductAgeRange4 { get; set; }

    }
}
