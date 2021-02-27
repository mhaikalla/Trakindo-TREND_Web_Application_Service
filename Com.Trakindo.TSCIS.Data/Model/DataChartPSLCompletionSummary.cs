using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Trakindo.TSICS.Data.Model
{
    public class DataChartPSLCompletionSummary
    {
        public List<string> MonthName { get; set; }
        public List<decimal> DataPercentPipSafety { get; set; }
        public List<decimal> DataPercentPipPriority { get; set; }
        public List<decimal> DataPercentPspProActive { get; set; }
        public List<decimal> DataPercentPspAfterFailure { get; set; }
        public List<decimal> RawDataPipSafetyCompAndOuts { get; set; }
        public List<decimal> RawDataPipPriorityCompAndOuts { get; set; }
        public List<decimal> RawDataPspProActiveCompAndOuts { get; set; }  
        public List<decimal> RawDataPspAfterFailureCompAndOuts { get; set; }

    }
}
