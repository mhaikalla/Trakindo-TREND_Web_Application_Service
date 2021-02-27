using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Com.Trakindo.TSICS.Data.Model
{
    [Table("WarrantyType")]
    public class WarrantyType
    {
        [Key]
        public int WarrantyTypeId { get; set; }
        public string WarrantyTypeName { get; set; }
    }
}
