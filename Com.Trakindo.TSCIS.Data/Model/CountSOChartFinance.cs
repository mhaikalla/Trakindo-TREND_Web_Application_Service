using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Trakindo.TSICS.Data.Model
{
    public class CountSOChartFinance
    {
        public decimal TotalSoCost { get; set; }
        public int TotalSO { get; set; }
        public List<string> PrefixSN {get; set;  }
    }
}
