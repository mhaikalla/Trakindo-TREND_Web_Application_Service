using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Trakindo.TSICS.Data.Model
{
    public class ApiJsonArticle 
    {
        public string link { get; set; }
        public ApiJsonArticleImg img { get; set; }
        public string title { get; set; }
        public string date { get; set; }
        public string type { get; set; }
        public string text { get; set; }
    }
}
