using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Trakindo.TSICS.Data.Model
{
    public class MobileEscalate
    {
        [Required(ErrorMessage = "TicketId is required")]
        public int TicketId { get; set; }
        [Required(ErrorMessage = "Responder is required")]
        public string Responder { get; set; }
    }
}
