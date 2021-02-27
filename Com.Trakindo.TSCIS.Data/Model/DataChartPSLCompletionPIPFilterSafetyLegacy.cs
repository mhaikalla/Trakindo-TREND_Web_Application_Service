using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Trakindo.TSICS.Data.Model
{
    public class DataChartPSLCompletionPIPFilterSafetyLegacy
    {
        public List<string> LabelFilterSafetyLegacy { get; set; }
        public List<decimal> DataFilterSafetyLegacy { get; set; }
        public List<string> Area { get; set; }
        public List<string> SalesOffice { get; set; }
        public List<string> PSLId { get; set; }
        public decimal ElementScoreFilterSafetyLegacy { get; set; } 
        public int Completed { get; set; }
    }
}
