using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Trakindo.TSICS.Data.Model
{
    [Table("Article")]
    public class Article
    {
        [Key]
        public int ArticleId { get; set; }
        public int Category1Id { get; set; }
        public string Category1 { get; set; }
        public int Category2Id { get; set; }
        public string Category2 { get; set; }
        public int Category3Id { get; set; }
        public string Category3 { get; set; }
        public int Category4Id { get; set; }
        public string Category4 { get; set; }
        public int Category5Id { get; set; }
        public string Category5 { get; set; }
        public int Category6Id { get; set; }
        public string Category6 { get; set; }
        public int Category7Id { get; set; }
        public string Category7 { get; set; }
        public string HeaderImage { get; set; }
        public string Title { get; set; }
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public string Tag { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int Aproved1By { get; set; }
        public DateTime? Aproved1At { get; set; }
        public int Aproved2By { get; set; }
        public DateTime? Aproved2At { get; set; }
        public int AprovedAdminBy { get; set; }
        public DateTime? AprovedAdminAt { get; set; }
        public string StatusName { get; set; }
        public int Status { get; set; }
        public string LevelUser { get; set; }
        public int Type { get; set; }
        public string Token { get; set; }
        public int Position { get; set; }

    }
    public class CustomArticleTags
    {
        public int ArticleTagsId { get; set; }
        public string Name { get; set; }
        public int ArticleId { get; set; }
        public  List<ArticleTags> Object { get; set; }
    }
}
