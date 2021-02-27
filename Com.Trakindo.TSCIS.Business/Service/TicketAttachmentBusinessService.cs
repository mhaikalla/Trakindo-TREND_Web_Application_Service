using Com.Trakindo.Framework;
using Com.Trakindo.TSICS.Data.Context;
using Com.Trakindo.TSICS.Data.Model;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Com.Trakindo.TSICS.Business.Service
{
    public class TicketAttachmentBusinessService
    {
        private readonly TsicsContext _dBtsics = Factory.Create<TsicsContext>("TsicsContext", ClassType.clsTypeDataContext);
        public List<TicketAttachment> GetList()
        {
            List<TicketAttachment> result = _dBtsics.TicketAttachment
                .ToList();

            return result;
        }
        public TicketAttachment Add(TicketAttachment ticketAttachment)
        {
            _dBtsics.TicketAttachment.Add(ticketAttachment);
            _dBtsics.SaveChanges();
            return ticketAttachment;
        }
        public TicketAttachment Edit(TicketAttachment ticketAttachment)
        {
            _dBtsics.Entry(ticketAttachment).State = EntityState.Modified;
            _dBtsics.SaveChanges();
            return ticketAttachment;
        }
        public TicketAttachment GetDetail(int id)
        {
            TicketAttachment ticketAttachment = _dBtsics.TicketAttachment.Find(id);
            return (ticketAttachment);
        }
        public List<TicketAttachmentsAPI> GetByTicketId(int ticketId)
        {
            var attachments = _dBtsics.TicketAttachment.Where(q => q.TicketId == ticketId).ToList();
            List<TicketAttachmentsAPI> attachmentList = new List<TicketAttachmentsAPI>();
            if (attachments.Count != 0)
            {
                foreach (var attachment in attachments)
                {
                    TicketAttachmentsAPI ticketAttachmentApi = new TicketAttachmentsAPI
                    {
                        Id = attachment.TicketAttachmentId, Name = attachment.Name, Level = attachment.LevelUser
                    };

                    attachmentList.Add(ticketAttachmentApi);
                }
            }
            return attachmentList;
        }
        public List<TicketAttachment> GetFullByTicketId(int ticketId)
        {
            var attachments = _dBtsics.TicketAttachment.Where(q => q.TicketId == ticketId).ToList();
            List<TicketAttachment> attachmentList = new List<TicketAttachment>();
            if (attachments.Count != 0)
            {
                foreach (var attachment in attachments)
                {
                    TicketAttachment ticketAttachment = new TicketAttachment
                    {
                        TicketAttachmentId = attachment.TicketAttachmentId,
                        Name = attachment.Name,
                        LevelUser = attachment.LevelUser
                    };

                    attachmentList.Add(ticketAttachment);
                }
            }
            return attachmentList;
        }
        public int CountByTicket(int ticketId)
        {
            return _dBtsics.TicketAttachment.Count(q => q.TicketId == ticketId);
        }

        public int Delete(TicketAttachment ticketAttachment)
        {
            _dBtsics.TicketAttachment.Remove(ticketAttachment);
            var DeleteResult = _dBtsics.SaveChanges();
            return DeleteResult;
        }
        public void DeleteAllAttachmentInTicket(int TicketId)
        {
            using ( TsicsContext db = new TsicsContext() )
            {
                db.Database.ExecuteSqlCommand("Delete from TicketAttachment where TicketId = '" + TicketId + "'");
            } 
        }
        public void DeleteAllTagsInTicket(int TicketId)
        {
            using (TsicsContext db = new TsicsContext())
            {
                db.Database.ExecuteSqlCommand("Delete from ArticleTag where TicketId = '" + TicketId + "'");
            }
        }
    }
}
