using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Trakindo.TSICS.Data.Model
{
    public class GetFormCollectionPSLCompletionPip
    {
        public string DateRangeFrom { get; set; }
        public string DateRangeEnd { get; set; }
        public string Inventory { get; set; }
        public string Rental { get; set; }
        public string HID { get; set; }
        public string Others { get; set; }
        public string Area { get; set; }
        public string StoreName { get; set; }
        public string PSLId { get; set; }

        public string PSLType { get; set; }
    }
}
