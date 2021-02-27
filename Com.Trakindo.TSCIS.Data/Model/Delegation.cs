using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Trakindo.TSICS.Data.Model
{
    [Table("Delegation")]
    public class Delegation
    {
        [Key]
        public int DelegateId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int ToUser { get; set; }
        public int FromUser { get; set; }
        public int Status { get; set; }
        public DateTime? CreatedAt { get; set; }

    }
    public class DelegationAPI
    {
        [Key]
        public int DelegateId { get; set; }
        public String StartDate { get; set; }
        public String EndDate { get; set; }
        public User ToUser { get; set; }
        public User FromUser { get; set; }
        public int Status { get; set; }
        public String CreatedAt { get; set; }

    }
}
