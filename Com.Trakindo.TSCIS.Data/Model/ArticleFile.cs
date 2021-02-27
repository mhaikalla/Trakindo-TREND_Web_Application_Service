using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Trakindo.TSICS.Data.Model
{
    [Table("ArticleFile")]
    public class ArticleFile
    {
        [Key]
        public int ArticleFileId { get; set; }
        public int ArticleId { get; set; }
        public string Name { get; set; }
        public string LevelUser { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
    public class ArticleFileData
    {
        public List<String> Name { get; set; }
        public List<String> Level { get; set; }
        public int idFile { get; set; }
    }
}
