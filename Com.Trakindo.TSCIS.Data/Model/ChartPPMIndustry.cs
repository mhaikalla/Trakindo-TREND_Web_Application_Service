using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Trakindo.TSICS.Data.Model
{
    public class ChartPPMIndustry
    {
        public decimal CompactMachines { get; set; }
        public decimal EarthMove { get; set; }
        public decimal MarineEngAux{ get; set; }
        public decimal Mining { get; set; }
        public decimal Blank { get; set; }
        public decimal Other { get; set; }

        public int CountCompactMachines { get; set; }
        public int CountEarthMove { get; set; }
        public int CountMarineEngAux { get; set; }
        public int CountMining { get; set; }
        public int CountBlank { get; set; }
        public int CountOther { get; set; }
    }
}
