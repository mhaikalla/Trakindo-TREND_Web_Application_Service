using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Trakindo.TSICS.Data.Model
{
    [Table("ArticleCategory")]
    public class ArticleCategory
    {
        [Key]
        public int ArticleCategoryId { get; set; }
        public int Parent { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Status { get; set; }
        public String Icon { get; set; }
        public int Position { get; set; }
    }
          
    public class ArticleCategoryMostUsed
    {
        public int ArticleCategoryId { get; set; }
        public int Parent { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Status { get; set; }
        public String Icon { get; set; }
        public int CountUsed { get; set; }
        public int Subcategory { get; set; }
        public int Position { get; set; }
    }
}
