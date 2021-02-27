using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Trakindo.TSICS.Data.Model
{
    public class DataChartPSLCompletionPSP
    {
        public string Area { get; set; }
        public int StatusCompleted { get; set; }
        public int StatusOutstanding { get; set; }
        public decimal RawDataStatusCompleted { get; set; }
        public decimal RawDataStatusOutstanding { get; set; }
        public decimal AverageComplete { get; set; }
        public decimal AverageOutstanding { get; set; }
        public double TotalData { get; set; }  

    }
}
