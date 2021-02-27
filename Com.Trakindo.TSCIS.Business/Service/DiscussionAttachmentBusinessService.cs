using System.Collections.Generic;
using System.Linq;
using Com.Trakindo.Framework;
using Com.Trakindo.TSICS.Data.Context;
using Com.Trakindo.TSICS.Data.Model;

namespace Com.Trakindo.TSICS.Business.Service
{
    public class DiscussionAttachmentBusinessService
    {
        private readonly TsicsContext _dBtsics = Factory.Create<TsicsContext>("TsicsContext", ClassType.clsTypeDataContext);
        
        public DiscussionAttachment Add(DiscussionAttachment discussionAttachment)
        {
            _dBtsics.DiscussionAttachment.Add(discussionAttachment);
            _dBtsics.SaveChanges();
            return discussionAttachment;
        }
        
        public List<TicketAttachmentsAPI> GetByDiscussionId(int discussionId)
        {
            var attachments = _dBtsics.DiscussionAttachment.Where(q => q.TicketDiscussionId == discussionId).ToList();
            List<TicketAttachmentsAPI> attachmentList = new List<TicketAttachmentsAPI>();
            if (attachments.Count != 0)
            {
                foreach (var attachment in attachments)
                {
                    TicketAttachmentsAPI ticketAttachmentApi = new TicketAttachmentsAPI {Name = attachment.Name};

                    attachmentList.Add(ticketAttachmentApi);
                }
            }
            return attachmentList;
        }
        public List<DiscussionAttachment> GetFullByDiscussionId(int discussionId)
        {
            var attachments = _dBtsics.DiscussionAttachment.Where(q => q.TicketDiscussionId == discussionId).ToList();
            List<DiscussionAttachment> attachmentList = new List<DiscussionAttachment>();
            if (attachments.Count != 0)
            {
                foreach (var attachment in attachments)
                {
                    DiscussionAttachment discussionAttachment = new DiscussionAttachment {Name = attachment.Name};

                    attachmentList.Add(discussionAttachment);
                }
            }
            return attachmentList;
        }
        public int CountByDiscussion(int discussionId)
        {
            return _dBtsics.DiscussionAttachment.Count(q => q.TicketDiscussionId == discussionId);
        }
    }
}
