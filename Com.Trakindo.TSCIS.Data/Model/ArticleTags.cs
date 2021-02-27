using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Com.Trakindo.TSICS.Data.Model
{
    [Table("ArticleTags")]
    public class ArticleTags
    {
        [Key]
        public int ArticleTagsId { get; set; }
        public string Name { get; set; }
        public int ArticleId { get; set; }
    }
}
