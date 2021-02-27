using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Trakindo.TSICS.Data.Model
{
    [Table("UserTsManager")]
    public class UserTsManager
    {
        [Key]
        public int UserTsManagerId { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public int EmployeeId { get; set; }
        public string Position { get; set; }
        public string Email { get; set; }
        public int LevelTs { get; set; }        
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
