using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Trakindo.TSICS.Data.Model
{
    public class ChartPPMStatus
    {
        public decimal StatusClose { get; set; }
        public int CountStatusClose { get; set; }
        public decimal StatusOpen { get; set; }
        public int CountStatusOpen { get; set; }
        public decimal StatusIssuePending { get; set; }
        public int CountStatusIssuePending { get; set; }
        public decimal StatusUnsubmitted { get; set; }
        public int CountStatusUnsubmitted { get; set; }
    }
}
