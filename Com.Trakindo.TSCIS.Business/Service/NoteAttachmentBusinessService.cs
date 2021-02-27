using Com.Trakindo.Framework;
using Com.Trakindo.TSICS.Data.Context;
using Com.Trakindo.TSICS.Data.Model;
using System.Collections.Generic;
using System.Linq;

namespace Com.Trakindo.TSICS.Business.Service
{
    public class NoteAttachmentBusinessService
    {
        private readonly TsicsContext _dBtsics = Factory.Create<TsicsContext>("TsicsContext", ClassType.clsTypeDataContext);
        
        public NoteAttachment Add(NoteAttachment noteAttachment)
        {
            _dBtsics.NoteAttachment.Add(noteAttachment);
            _dBtsics.SaveChanges();
            return noteAttachment;
        }
        
        public List<TicketAttachmentsAPI> GetByNoteId(int noteId)
        {
            var attachments = _dBtsics.NoteAttachment.Where(q => q.TicketNoteId == noteId).ToList();
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
        public List<NoteAttachment> GetFullByNoteId(int noteId)
        {
            var attachments = _dBtsics.NoteAttachment.Where(q => q.TicketNoteId == noteId).ToList();
            List<NoteAttachment> attachmentList = new List<NoteAttachment>();
            if (attachments.Count != 0)
            {
                foreach (var attachment in attachments)
                {
                    NoteAttachment noteAttachment = new NoteAttachment {Name = attachment.Name};

                    attachmentList.Add(noteAttachment);
                }
            }
            return attachmentList;
        }
        public int CountByNote(int noteId)
        {
            return _dBtsics.NoteAttachment.Count(q => q.TicketNoteId == noteId);
        }
    }
}
