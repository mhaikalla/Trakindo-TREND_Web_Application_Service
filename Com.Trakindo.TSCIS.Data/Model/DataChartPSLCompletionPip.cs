using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Trakindo.TSICS.Data.Model
{
    public class DataChartPSLCompletionPip
    {
        public string Area { get; set; }
        public List<int> StatusCompleted { get; set; }
        public List<int> StatusOutstanding { get; set; }
        public List<string> MonthName { get; set; } 
        public List<decimal> RawDataStatusCompleted { get; set; }
        public List<decimal> RawDataStatusOutstanding { get; set; }
        public List<decimal> AverageComplete { get; set; }
        public List<decimal> AverageOutstanding { get; set; } 
    }
}
