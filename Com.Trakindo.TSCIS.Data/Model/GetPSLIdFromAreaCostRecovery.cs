using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Trakindo.TSICS.Data.Model
{
    public class GetPSLIdFromAreaCostRecovery
    {
        public string PSLId { get; set; }
        public DateTime? TerminationDate { get; set; }
        public DateTime? LetterDate { get; set; }
        public string DaysToExpired { get; set; }
        public string Validation { get; set; }
        public List<GetModelFromPSLIdAreaCostRecovery> Model { get; set; }
        public List<GetModelFromPSLIdAreaCostRecovery> ModelCheck { get; set; }
    }
}
