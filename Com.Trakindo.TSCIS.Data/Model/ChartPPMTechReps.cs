using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Trakindo.TSICS.Data.Model
{
    public class ChartPPMTechReps
    {
        public decimal ChenShuehSy { get; set; }
        public decimal ChouglanZaneStephen { get; set; }
        public decimal JoniYohanis { get; set; }
        public decimal TamaAntonFirman { get; set; }
        public decimal Blank { get; set; }
        public decimal Other { get; set; }

        public int CountChenShuehSy { get; set; }
        public int CountChouglanZaneStephen { get; set; }
        public int CountJoniYohanis { get; set; }
        public int CountTamaAntonFirman { get; set; }
        public int CountBlank { get; set; }
        public int CountOther { get; set; }
    }
}
