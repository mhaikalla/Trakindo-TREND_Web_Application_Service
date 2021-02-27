using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Trakindo.TSICS.Data.Model
{
    public class DataChartPSLCompletionPIPFilterSafetyCompletion
    {
        public List<string> LabelFilterSafetyCompletion { get; set; }
        public List<decimal> DataFilterSafetyCompletion { get; set; }
        public List<string> Area { get; set; }
        public List<string> SalesOffice { get; set; }
        public List<string> PSLId { get; set; }
        public decimal ElementScoreFilterSafetyCompletion { get; set; } 
        public decimal Completed { get; set; }

        public int CountPending { get; set; }
    }
}
