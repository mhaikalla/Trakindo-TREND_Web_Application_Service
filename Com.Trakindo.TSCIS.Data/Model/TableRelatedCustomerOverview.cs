using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Trakindo.TSICS.Data.Model
{
    public class TableRelatedCustomerOverview
    {
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string Location { get; set; }
        public string Model { get; set; }
        public int CountOfSN { get; set; }
    }
}
