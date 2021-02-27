using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Trakindo.TSICS.Data.Model
{
    public class ArticleListJson
    {
        public string link { get; set; }
        public ArticleListJsonImg img { get; set; }
        public string title { get; set; }
        public string date { get; set; }
        public string type { get; set; }
        public string text { get; set; }
        public string group_url { get; set; }

    }

    public class ArticleListJsonImg
    {
        public string src { get; set; }
        public string alt { get; set; }
        public string label { get; set; }

    }
}
