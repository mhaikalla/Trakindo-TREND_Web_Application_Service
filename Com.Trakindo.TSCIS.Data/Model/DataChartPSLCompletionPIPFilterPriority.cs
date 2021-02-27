using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Trakindo.TSICS.Data.Model
{
    public class DataChartPSLCompletionPIPFilterPriority
    {
        public List<string> LabelFilterPriority { get; set; }
        public List<decimal> DataFilterPriority { get; set; }
        public List<string> Area { get; set; }
        public List<string> SalesOffice { get; set; }
        public List<string> PSLId { get; set; } 
        public decimal ElementScoreFilterPriority { get; set; }
        public decimal Completed { get; set; }
        public int CountPending { get; set; }
    }
}
