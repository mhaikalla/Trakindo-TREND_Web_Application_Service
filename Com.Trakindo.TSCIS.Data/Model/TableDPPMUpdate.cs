using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Trakindo.TSICS.Data.Model
{
    public class TableDPPMUpdate
    {
        public int Row { get; set; }
        public string DPPMNo { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }
        public string Status { get; set; }
        public string DealerContact { get; set; }
        public string CatReps { get; set; }
        public string ICA { get; set; }
        public string PCA { get; set; }
        public string DateCreated { get; set; }
        public string DateLastUpdate { get; set; }
    }
}
