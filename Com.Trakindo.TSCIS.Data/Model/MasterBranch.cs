using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Trakindo.TSICS.Data.Model
{
    [Table("MasterBranch")]
    public class MasterBranch
    {
        [Key]

        public int MasterBranchId { get; set; }
        public int MasterAreaId { get; set; }
        public  string Name { get; set; }
        public int Status { get; set; }
        public string SalesOffice { get; set; }
    }
}
