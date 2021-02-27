using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Com.Trakindo.TSICS.Data.Model
{
    [Table("ECC.PPSC_INVENTORY_WEEKLY")]
    public class PPSC_Inventory_Weekly
    {
        [Key]
        public string Product_Id { get; set; }
        public string Product { get; set; }
        public string Material_Number { get; set; }
        public string Model { get; set; }
        public string Serial_Number { get; set; }
        public string Customer_Number { get; set; }
        public string Customer_Name { get; set; }
        public string Plant { get; set; }
        public string Plant_Desc { get; set; }

    }
    public class TableModelOverviewModel
    {
        public string Model { get; set; }
        public int Count { get; set; }

    }
}
