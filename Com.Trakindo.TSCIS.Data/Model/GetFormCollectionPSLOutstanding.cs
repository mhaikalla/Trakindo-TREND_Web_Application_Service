using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Trakindo.TSICS.Data.Model
{
    public class GetFormCollectionPSLOutstanding
    {
        public string PipSafety { get; set; }
        public string PipPriority { get; set; }
        public string PspProactive { get; set; }
        public string PspAfterFailure { get; set; }
        public string ReleaseDateFrom { get; set; }
        public string ReleaseDateEnd { get; set; }
        public string TerminationDateFrom { get; set; }
        public string TerminationDateEnd { get; set; }
        public string Inventory { get; set; }
        public string Rental { get; set; }
        public string HID { get; set; }
        public string Others { get; set; }
        public string Area { get; set; }
        public string SalesOffice { get; set; }
        public string PSLStatus { get; set; }
        public string PSLNo { get; set; }
        public string AgeIndicator { get; set; }
        public string SerialNumber { get; set; }
        public string Model { get; set; }
        public string Priority { get; set; }
        public string ChecklistArea { get; set; }
        public string ChecklistSalesOffice { get; set; }
        public string ChecklistPSLStatus { get; set; }
        public string ChecklistPSLNo { get; set; }
        public string ChecklistAgeIndicator { get; set; }
        public string ChecklistSerialNumber { get; set; }
        public string ChecklistModel { get; set; }
        public string ChecklistPriority { get; set; }
        public string PostAreaFilter { get; set; }
    }
}
