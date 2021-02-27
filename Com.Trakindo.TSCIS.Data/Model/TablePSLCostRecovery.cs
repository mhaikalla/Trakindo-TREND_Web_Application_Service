using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Trakindo.TSICS.Data.Model
{
    public class TablePSLCostRecovery
    {
        public int Row { get; set; }
        public string Area { get; set; }
        public string PSLId { get; set; } 
        public string Model { get; set; }
        public decimal UnitQty { get; set; }
        public decimal Completed { get; set; }
        public decimal TotalSO { get; set; }
        public decimal TotalClaim { get; set; }
        public decimal Settled { get; set; } 
        public decimal Settlement { get; set; }
        public decimal Recovery { get; set; }
        public int TotalData { get; set; }
        public string DaysToExpired { get; set; }
        public int WipAge { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public DateTime? TerminationDate { get; set; }
        public string Validation { get; set; }
        public int PslAge { get; set; }

        public decimal TotalAmount { get; set; }

        public List<GetPSLIdFromAreaCostRecovery> PSLNo { get; set; }

        public List<GetPSLIdFromAreaCostRecovery> PSLNoCheck { get; set; }
        public List<GetPSLIdFromAreaCostRecovery> ModelCostRevocery { get; set; }
        public List<GetModelFromPSLIdAreaCostRecovery> ModelCostRevoceryCheck { get; set; }

        public int CountData { get; set; }
    }
}
