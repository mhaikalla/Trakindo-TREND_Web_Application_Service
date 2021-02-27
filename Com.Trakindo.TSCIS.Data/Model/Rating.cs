using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Trakindo.TSICS.Data.Model
{
    [Table("Rating")]
    public class Rating
    {
        [Key]
        public int UserRatingId { get; set; }
        public int TicketId { get; set; }
        public int UserId { get; set; }
        public int Rate { get; set; }
        public string Description { get; set; }
        public int RatedFrom { get; set; }
        public DateTime CreatedAt { get; set; }

        public String RespondTime { get; set; }
    }
}
