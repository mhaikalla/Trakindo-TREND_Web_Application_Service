using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Com.Trakindo.TSICS.Data.Model
{
    public class MobileResolutionPostAPI
    {
        [Required(ErrorMessage ="TicketId is required")]
        public int TicketId { get; set; }
        [Required(ErrorMessage = "UserId is required")]
        public int UserId { get; set; }
        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }
        [Required(ErrorMessage = "UserToRate is required")]
        public int UserToRate { get; set; }
        [Required(ErrorMessage = "Rate is required")]
        public int Rate { get; set; }
        public string RateDescription { get; set; }
    }
}
