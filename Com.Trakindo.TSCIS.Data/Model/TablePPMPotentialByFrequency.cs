using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Trakindo.TSICS.Data.Model
{
    public class TablePPMPotentialByFrequency
    {
        public string Row { get; set; }
        public string SerialNumber { get; set; }
        public string Model { get; set; }
        public string ProductProblemDescription { get; set; }
        public string Comment { get; set; }
        public string ServiceOrder { get; set; }
        public int ServiceMeterMeasurement { get; set; }
        public string UnitMes { get; set; }
        public string SalesOffice { get; set; }
        public string PartNo { get; set; }
        public string PartDescription { get; set; }
        public DateTime? RepairDate { get; set; }
        public string Currency { get; set; }
        public decimal TotalSoCost { get; set; }
        public List<string> GroupNo { get; set; }
        public string GroupDesc { get; set; }
        public int CountOfRepair { get; set; }
    }
}
