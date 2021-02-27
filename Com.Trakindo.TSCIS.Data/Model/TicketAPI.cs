using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Com.Trakindo.TSICS.Data.Model
{
    public class TicketAPI
    {
        public int TicketId { get; set; }
        public int TicketCategoryId { get; set; }
        public string TicketNo { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Submiter { get; set; }
        public string SubmiterName { get; set; }
        public int Responder { get; set; }
        public string ResponderName { get; set; }
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
        public String ReferenceTicketNo { get; set; }
        public int ReferenceTicket { get; set; }
        public string DPPMno { get; set; }
        public string MepBranch { get; set; }
        public DateTime? SMUDate { get; set; }
        public string SoftwarePartNumber { get; set; }
        public string FromInterlock { get; set; }
        public string ToInterlock { get; set; }
        public string TestSpec { get; set; }
        public string TestSpecBrakeSaver { get; set; }
        public int DelegateId { get; set; }
        public bool IsEscalated { get; set; }
        public int SubmiterRating { get; set; }
        public int ResponderRating { get; set; }
        public String WarrantyCategoryName { get; set; }

        public List<TicketAttachmentsAPI> Attachments { get; set; }
        public List<TicketTagsAPI> Tags { get; set; }
        public List<MobileUser> Participants { get; set; }
        public MobileUser ResponderDetails { get; set; }
        public List<TicketAttachmentsAPI> WholeAttachments { get; set; }
        public JsonResolution Resolution { get; set; }
        public DateTime? RecentDate { get; set; }
        public String RatingDescResponder { get; set; }
        public String RatingDescSubmiter { get; set; }
    }

    public class TicketAPIDashboardOverview
    {
        public String TicketId { get; set; }
        public String TicketNo { get; set; }
        public String Title { get; set; }
        public int Submiter { get; set; }
        public int Responder { get; set; }
        public int NextCommenter { get; set; }
        public String Resolution { get; set; }
        public DateTime? CreatedAt { get; set; }
        public String Description { get; set; }
        public int Status { get; set; }

    }
}
