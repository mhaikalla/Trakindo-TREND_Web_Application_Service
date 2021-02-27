using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Com.Trakindo.TSICS.Data.Model
{
    public class MobileDiscussionPostAPI
    {
        [Required(ErrorMessage = "Type is required")]
        public int Type { get; set; }
        [Required(ErrorMessage = "TicketId is required")]
        public int TicketId { get; set; }
        [Required(ErrorMessage = "UserId is required")]
        public int UserId { get; set; }
        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }
        public List<DiscussionAttachment> Attachments { get; set; }
    }
}
