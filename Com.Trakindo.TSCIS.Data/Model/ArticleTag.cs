using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Trakindo.TSICS.Data.Model
{
    [Table("ArticleTag")]
    public class ArticleTag
    {
        [Key]
        public int ArticleTagId { get; set; }
        public string Name { get; set; }
        public int Freq { get; set; }
        public int TicketId { get; set; }
    }
}
