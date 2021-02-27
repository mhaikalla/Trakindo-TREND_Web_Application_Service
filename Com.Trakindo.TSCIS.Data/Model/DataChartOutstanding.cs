using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Trakindo.TSICS.Data.Model
{
    public class DataChartOutstanding
    {
        public string Area { get; set; }
        public int StatusOpen { get; set; }
        public int StatusRelease { get; set; }
        public int StatusOutstanding { get; set; }
        public int StatusInProgress { get; set; }
        public List<int> Data { get; set; }
        public int TotalData { get; set; }
        public string SAPPslStatus { get; set; }
        public int PSLAge { get; set; }
        public string DayToExpired { get; set; }
    }
}
