using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Com.Trakindo.TSICS.Data.Model
{
    [Table("CRM.V_TR_MEP")]
    public class V_TR_MEP
    {
        [Key]
        public string Area_ID { get; set; }
        public string Area { get; set; }
        public string Region { get; set; }
        public string Plant { get; set; }
        public string LOC { get; set; }
        public string Customer_ID { get; set; }
        public string Customer_Name{ get; set; }
        public string Sales_Reps_ID { get; set; }
        public string Sales_Reps_Name { get; set; }
        public string CIC_ID { get; set; }
        public string CIC_Group { get; set; }
        public string CIC_Description { get; set; }
        public string Serial_Number { get; set; }
        public string Product_Category_Code { get; set; }
        public string Product_Category_Description { get; set; }
        public string MAKE { get; set; }
        public double SMU{ get; set; }
        public string SMU_Update { get; set; }
        public string Purchase_Date { get; set; }
        public string Delivery_Date { get; set; }
        public string Ship_To_Address { get; set; }
        public string Product_Family { get; set; }
        public string Product_Family_Description { get; set; }
        public string Product_Model { get; set; }
        public int HidTaskId { get; set; }
        public string RentStatus { get; set; }
        public string Last_Work_Order { get; set; }
    }

    public class TableMEPOverviewModel
    {
        public string Serial_Number { get; set; }
        public string SMU { get; set; }
        public string SMU_Update { get; set; }
        public string LastServiced { get; set; }
        public string DeliveryDate { get; set; }
        public string PurchaseDate { get; set; }
        public string Industry { get; set; }
    }
}
