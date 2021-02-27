using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Trakindo.TSICS.Data.Model
{
    public class StatusTablePSLExecution
    {
        public string PSLId { get; set; }
        public string PSLStatus { get; set; }
        public string SOStatus { get; set; }
        public string SRStatus { get; set; }
        public string QuotNo { get; set; }
        public string SONo { get; set; }
        public int WipAge { get; set; }
        public int FollowUpDuration { get; set; }
        public string Validation { get; set; }
        public int PSLAge { get; set; }
        public string DayToExpired { get; set; }
        public DateTime? TerminationDate { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string PSLType { get; set; }
    }
}
