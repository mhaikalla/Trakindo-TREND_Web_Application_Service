using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Trakindo.TSICS.Data.Model
{
    [Table("EmployeeMaster")]
    public partial class EmployeeMaster
    {
        [Key]
        public int Employee_Id { get; set; }

        [StringLength(80)]
        public string Employee_Name { get; set; }
        public string Employee_xupj { get; set; }
        public int Superior_ID { get; set; }
        public string Branch_Id { get; set; }
        public string Location_Id { get; set; }
        public string Location_Name { get; set; }
        public string Birth_date { get; set; }
        public string Position_Name { get; set; }
        public string Email { get; set; }
        public string POH_Name { get; set; }
        public string POH_Id { get; set; }
        public string Business_Area { get; set; }
        public string Business_Area_Desc { get; set; }
        public string Gender_Key { get; set; }
    }
}
