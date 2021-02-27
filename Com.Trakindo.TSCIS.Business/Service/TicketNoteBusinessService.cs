using Com.Trakindo.Framework;
using Com.Trakindo.TSICS.Data.Context;
using Com.Trakindo.TSICS.Data.Model;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Com.Trakindo.TSICS.Business.Service
{
    public class TicketNoteBusinessService
    {
        private readonly TsicsContext _dBtsics = Factory.Create<TsicsContext>("TsicsContext", ClassType.clsTypeDataContext);

        public List<TicketNote> GetListByTicket(int id)
        {
            List<TicketNote> result = _dBtsics.TicketNote
                .Where(t => t.TicketId == id).OrderByDescending(t=> t.CreatedAt)
                .ToList();

            return result;
        }
        public TicketNote GetDatebyTicketandUserId(int id, int userid)
        {
                return _dBtsics.TicketNote.OrderByDescending(t => t.RespondTime)
               .FirstOrDefault(t => t.TicketId == id && t.UserId.Equals(userid));
        }
        public TicketNote Add(TicketNote ticketNote)
        {
            _dBtsics.TicketNote.Add(ticketNote);
            _dBtsics.SaveChanges();
            return ticketNote;
        }
        public TicketNote Edit(TicketNote ticketNote)
        {
            _dBtsics.Entry(ticketNote).State = EntityState.Modified;
            _dBtsics.SaveChanges();
            return ticketNote;
        }

        public TicketNote GetDetail(int id)
        {
            TicketNote ticketNote = _dBtsics.TicketNote.Find(id);
            return (ticketNote);
        }
        public TicketNote GetDetailbyTicketIdAndResponder(int id, int responder)
        {
            TicketNote ticketNote = _dBtsics.TicketNote.Where(t => t.TicketId.Equals(id) && t.UserId == responder).OrderByDescending(t => t.CreatedAt).FirstOrDefault();
            return (ticketNote);
        }
        public TicketNote GetLastCommentDataByTicket(int ticketId)
        {
           return _dBtsics.TicketNote.Where(t => t.TicketId.Equals(ticketId)).OrderByDescending(t => t.CreatedAt).FirstOrDefault();
        }
        public List<TicketNote> GetListforEmail(int id)
        {
            List<TicketNote> result = _dBtsics.TicketNote
                .OrderByDescending(t=> t.CreatedAt).Where(t => t.TicketId == id)
                .ToList();

            return result;
        }

        public bool isExist(int ticketid)
        {
            return _dBtsics.TicketNote.Count(t => t.TicketId == ticketid) > 0;
        }
       
    }
}
