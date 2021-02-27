using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Trakindo.TSICS.Data.Model
{
    public class TablePSLCompletionPIP
    {
        public int Row { get; set; }
        public string Area { get; set; }
        public string PSLNo { get; set; }
        public string StoreName { get; set; }
        public string Model { get; set; }
        public string SerialNo { get; set; }
        public string SRNo { get; set; }
        public string QuotNo { get; set; }
        public string SoNo { get; set; }
        public string PSLStatus { get; set; }
        public string DaysToExpired { get; set; }
        public int WipAge { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public DateTime? TerminationDate { get; set; }
        public string ReleaseDateString { get; set; }
        public string TerminationDateString { get; set; }

        public string Validation { get; set; }
        public int PslAge { get; set; }
        public DateTime? LetterDate { get; set; }
        public string CatPSLStatus { get; set; }
    }
}
