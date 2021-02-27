using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Trakindo.TSICS.Data.Model
{
    public class TablePSLExecution
    {
        public int Row { get; set; }
        public string SerialNo { get; set; }
        public string Area { get; set; }
        public string SalesOffice { get; set; }
        public string Customer { get; set; }
        public string Model { get; set; }
        public List<StatusTablePSLExecution> Status { get; set; } 
        public List<StatusTablePSLExecution> StatusCheckValueSearch { get; set; }

    }
}
