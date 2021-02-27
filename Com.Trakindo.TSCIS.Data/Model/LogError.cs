namespace Com.Trakindo.TSICS.Data.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [Table("LogError")]

    public class LogError
    {
        [Key]
        public int LogErrorID { get; set; }
        public string Project { get; set; }
        public string UserXupj { get; set; }
        public string Source { get; set; }
        public string Description { get; set; }
        public DateTime? LogDate { get; set; }
    }
}
