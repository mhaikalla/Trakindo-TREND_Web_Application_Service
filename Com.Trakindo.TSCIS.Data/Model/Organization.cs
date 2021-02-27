using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Trakindo.TSICS.Data.Model
{
    [Table("Organization")]
    public class Organization
    {
        [Key]
        public int OrganizationId { get; set; }
        public string SalesOfficeCode { get; set; }
        public string SalesOfficeDescription { get; set; }
        public string Region { get; set; }
        public string Area { get; set; }
    }

    [Table("Organization2")]
    public class Organization2
    {
        [Key]
        public int OrganizationId { get; set; }
        public string SalesOfficeCode { get; set; }
        public string SalesOfficeDescription { get; set; }
        public string Region { get; set; }
        public string Area { get; set; }
        public string Lattitude { get; set; }
        public string Longitude { get; set; }
    }
}
