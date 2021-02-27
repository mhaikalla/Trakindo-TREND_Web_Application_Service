using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Trakindo.TSICS.Data.Model
{
    public class GetFormCollectionOverview
    {
        public string Area { get; set; }
        public string SalesOffice { get; set; }
        public string Customer { get; set; }
        public string Customer_Name { get; set; }
        public string Family { get; set; }
        public string Industry { get; set; }
        public string Model { get; set; }
        public string SerialNumber { get; set; }
        public int SMURangeFrom { get; set; }
        public int SMURangeTo { get; set; }
        public string DeliveryDateFrom { get; set; } 
        public string PurchaseDateEnd { get; set; }
        public string PurchaseDateFrom { get; set; }
        public string LastRepairDateEnd { get; set; }
        public string LastRepairDateFrom { get; set; }
        public string DeliveryDateEnd { get; set; }
        public string HID { get; set;  }
        public string Rental { get; set; }
        public string Inventory { get; set; }
        public string Others { get; set; }
        public string Product { get; set; }
        public string Plant { get; set; }
        public string[] SerialNumberData { get; set; }
        public string paramsModel { get; set; }
        public string paramsSerialNumber { get; set; }
        public int FilterType { get; set; }

        public bool btnPOST { get; set; }
    }
}
