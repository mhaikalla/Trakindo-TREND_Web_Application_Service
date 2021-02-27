namespace Com.Trakindo.TSICS.Data.Context
{
    using Com.Trakindo.TSICS.Data.Model;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;


    public partial class TsicsContext : DbContext
    {
        public TsicsContext() 
            : base("name=TSICSdbContext")
        { 
        }

        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserRole> UserRole { get; set; }
        public virtual DbSet<UserMessage> UserMessage { get; set; }
        public virtual DbSet<Ticket> Ticket { get; set; }
        public virtual DbSet<TicketAttachment> TicketAttachment { get; set; }
        public virtual DbSet<TicketCategory> TicketCategory { get; set; }
        public virtual DbSet<TicketDiscussion> TicketDiscussion { get; set; }
        public virtual DbSet<TicketNote> TicketNote { get; set; }
        public virtual DbSet<TicketParcipant> TicketParcipant { get; set; }
        public virtual DbSet<TicketResolution> TicketResolution { get; set; }
        public virtual DbSet<EdwUnit> EdwUnit { get; set; }
        public virtual DbSet<ArticleTag> ArticleTag { get; set; }
        public virtual DbSet<ArticleTags> ArticleTags { get; set; }
        public virtual DbSet<WarrantyType> WarrantyType { get; set; }
        public virtual DbSet<MEP> MEP { get; set; }

        public virtual DbSet<MobileLogin> MobileLogin { get; set; }
        public virtual DbSet<ApiJsonCommentTR> ApiJsonCommentTR { get; set; }
        public virtual DbSet<ApiJsonCommentDateTR> ApiJsonCommentDateTR { get; set; }
        public virtual DbSet<ApiJsonCommentSenderTR> ApiJsonCommentSenderTR { get; set; }
        public virtual DbSet<ApiJsonCommentImageTR> ApiJsonCommentImageTR { get; set; }
        public virtual DbSet<LogReport> LogReport { get; set; }
        public virtual DbSet<MasterArea> MasterArea { get; set; }
        public virtual DbSet<MasterBranch> MasterBranch { get; set; }
        public virtual DbSet<Article> Article { get; set; }
        public virtual DbSet<ArticleCategory> ArticleCategory { get; set; }
        public virtual DbSet<LogError> LogError { get; set; }
		public virtual DbSet<DiscussionAttachment> DiscussionAttachment { get; set; }
		public virtual DbSet<ArticleFile> ArticleFile { get; set; }
		public virtual DbSet<NoteAttachment> NoteAttachment { get; set; }
        public virtual DbSet<Rating> Rating { get; set; }
        public virtual DbSet<UserTsManager> UserTsManager { get; set; }
        public virtual DbSet<UserDevices> UserDevices { get; set; }
        public virtual DbSet<EscalateLog> EscalateLog { get; set; }
        public virtual DbSet<Delegation> Delegation {get; set; }
        public virtual DbSet<PIS> PIS { get; set; }
        public virtual DbSet<PSLMaster> PSLMaster { get; set; }
        public virtual DbSet<Location2> Location2 { get; set; }
        public virtual DbSet<CommentUpload> CommentUpload { get; set; }
        public virtual DbSet<CalcPIS> CalcPIS { get; set; }
        public virtual DbSet<WarrantyList> WarrantyList { get; set; }
        public virtual DbSet<DPPMSummary> DPPMSummary { get; set; }
        public virtual DbSet<DPPM> DPPM { get; set; }
        public virtual DbSet<PartResponsibleCostImpactAnalysis> PartResponsibleCostImpactAnalysis { get; set; }
        public virtual DbSet<DPPMAffectedUnit> DPPMAffectedUnit { get; set; }
        public virtual DbSet<DPPMAffectedPart> DPPMAffectedPart { get; set; }
        public virtual DbSet<SIMSErrorSummary> SIMSErrorSummary { get; set; }
        public virtual DbSet<Organization> Organization { get; set; }
        public virtual DbSet<RCIA> RCIA { get; set; } 
        public virtual DbSet<PSLPart> PSLPart { get; set; }
        public virtual DbSet<V_TR_MEP> V_TR_MEP { get; set; }
        public virtual DbSet<PPSC_Inventory_Weekly> Inventory_Weekly{ get; set; }
        public virtual DbSet<Organization2> Organization2 { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Customer>()
            //    .Property(e => e.PAR)
            //    .IsUnicode(false);
        }
    }
}
