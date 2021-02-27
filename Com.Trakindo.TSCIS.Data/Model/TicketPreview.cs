using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Trakindo.TSICS.Data.Model
{
    public class TicketPreview
    {
        [Key]
        public int TicketId { get; set; }
        public int TicketCategoryId { get; set; }
        public string TicketNo { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Submiter { get; set; }
        public int Responder { get; set; }
        public int SubmiterFlag { get; set; }
        public int ResponderFlag { get; set; }
        public string SerialNumber { get; set; }
        public string Customer { get; set; }
        public string Location { get; set; }
        public string Make { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public string ArrangementNo { get; set; }
        public string Family { get; set; }
        public string Model { get; set; }
        public string SMU { get; set; }
        public string PartCausingFailure { get; set; }
        public string PartsDescription { get; set; }
        public string EmailCC { get; set; }
        public string Manufacture { get; set; }
        public string PartsNumber { get; set; }
        public string ServiceToolSN { get; set; }
        public string EngineSN { get; set; }
        public string EcmSN { get; set; }
        public string TotalTattletale { get; set; }
        public string ReasonCode { get; set; }
        public string DiagnosticClock { get; set; }
        public string Password { get; set; }
        public string ServiceOrderNumber { get; set; }
        public string ClaimNumber { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int Status { get; set; }
        public int WarrantyTypeId { get; set; }
        public string RoleName { get; set; }
        public int NextCommenter { get; set; }
        public bool IsEscalated { get; set; }
        public string FromInterlock { get; set; }
        public string ToInterlock { get; set; }
        public string TestSpec { get; set; }
        public string TestSpecBrakeSaver { get; set; }
        public string AreaName { get; set; }
        public DateTime? SolvedDate { get; set; }
        public DateTime? ClosedDate { get; set; }
        public DateTime? DueDateAnswer { get; set; }
        public int Rate { get; set; }
        public string DPPMNumber { get; set; }
        public DateTime? LastReply { get; set; }
    
        public string ticketNoReference { get; set; }

    }
}
