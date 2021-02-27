using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Trakindo.TSICS.Data.Model
{
    [Table("UserMessage")]
    public class UserMessage
    {
        [Key]
        public int UserMessageId { get; set; }
        public int ToUserId { get; set; }
        public int ToEmployeeId { get; set; }
        public int FromUserId { get; set; }
        public int FromEmployeeId { get; set; }
        public int FromUserType { get; set; }
        public string Message { get; set; }
        public string MessageType { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
