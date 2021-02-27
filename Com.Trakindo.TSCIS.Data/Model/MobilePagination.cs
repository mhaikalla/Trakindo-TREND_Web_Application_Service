using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Trakindo.TSICS.Data.Model
{
    public class MobilePagination
    {
        [Required(ErrorMessage = "PerPage is required")]
        public int PerPage { get; set; }
        [Required(ErrorMessage = "PageNum is required")]
        public int PageNum { get; set; }
    }
}
