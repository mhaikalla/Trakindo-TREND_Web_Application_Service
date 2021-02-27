using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Trakindo.TSICS.Data.Model
{
    public class ArticleImageUpload
    {
        public int uploaded { get; set; }
        public string fileName { get; set; }
        public string url { get; set; }
        public string error { get; set; }
    }
}
