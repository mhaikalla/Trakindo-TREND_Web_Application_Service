using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Com.Trakindo.TSICS.Data.Model
{
    [Table("PIS")]
    public class PIS
    { 
        [Key]
        public int PisId{ get; set; }
        public string ProgramNo { get; set; }
        public string Description { get; set; }
        public string SerialNumber { get; set; }
        public string Model { get; set; }
        public string AssignDealer { get; set; }
        public string RepairingDealer { get; set; }
        public string CatCustomer { get; set; }
        public string PslType { get; set; }
        public string CatPslStatus { get; set; }
        public DateTime? LetterDate { get; set; }
        public DateTime? TerminationDate { get; set; }
        public DateTime? RepairDate { get; set; }
        public int LaborHours { get; set; }
        public DateTime? CatDeliveryDate { get; set; }
        public int ServiceClaimAllowanceGroup { get; set; }
        public string ProductSmuAgeRange1 { get; set; }
        public string ProductSmuAgeRange2 { get; set; }
        public string ProductSmuAgeRange3 { get; set; }
        public string ProductSmuAgeRange4 { get; set; }
        
    }
}