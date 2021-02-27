using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Trakindo.TSICS.Data.Model
{
    [Table("Ticket")]
    public class Ticket
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
        public int MasterAreaId { get; set; }
        public string MasterAreaName { get; set; }
        public int MasterBranchId { get; set; }
        public string MasterBranchName { get; set; }
        public int NextCommenter { get; set; }
        public DateTime? DueDateAnswer { get; set; }
        public int ReferenceTicket { get; set; }
        public string DPPMno { get; set; }
        public string MepBranch { get; set; }
        public DateTime? SMUDate { get; set; }
        public string SoftwarePartNumber { get; set; }
        public string FromInterlock{ get; set; }
        public string ToInterlock { get; set; }
        public string TestSpec { get; set; }
        public string TestSpecBrakeSaver { get; set; }
        public int DelegateId { get; set; }
        public int EscalateId { get; set; }
        public DateTime? LastReply { get; set; }
        public DateTime? LastStatusDate { get; set; }
        public long SubmiterStatus { get; set; }
        public long ResponderStatus { get; set; }
    }
}
