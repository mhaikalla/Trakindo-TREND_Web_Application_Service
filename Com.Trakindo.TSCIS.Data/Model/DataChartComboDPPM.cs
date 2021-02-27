using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Trakindo.TSICS.Data.Model
{
    public class DataChartComboDPPM
    {
        public List<string> Model { get; set; }
        public decimal DataTotalCost { get; set; }
        public decimal DataTotalSO { get; set; }
        public string SerialNumber { get; set; }
    }
}
