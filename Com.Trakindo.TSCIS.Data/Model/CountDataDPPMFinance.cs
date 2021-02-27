using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Trakindo.TSICS.Data.Model
{
    public class CountDataDPPMFinance
    {
        public decimal TotalServiceOrderCost { get; set; }
        public int TotalUnitImpacted { get; set; }
        public decimal FinancialSummary { get; set; }
        public int QuantitySummary { get; set; }
    }
}
