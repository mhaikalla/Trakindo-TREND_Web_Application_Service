using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Trakindo.TSICS.Data.Model
{
    public class TableRelatePSLDPPMFinancial
    {
        public int Row { get; set; }
        public string PSLNo { get; set; }
        public string DaysToExpired { get; set; }
        public int WipAge { get; set; }
        public string Validation { get; set; }
        public int PslAge { get; set; }
        public string Desc { get; set; }
        public string PSLType { get; set; }
        public DateTime? IssueDate { get; set; }
        public decimal Completion { get; set; }
        public DateTime? TerminationDate { get; set; }
        public string IssueDateString { get; set; }
        public string TerminationDateString { get; set; }
    }
}
