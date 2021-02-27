using Com.Trakindo.Framework;
using Com.Trakindo.TSICS.Data.Context;
using Com.Trakindo.TSICS.Data.Model;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;

namespace Com.Trakindo.TSICS.Business.Service
{
    public class TicketResolutionBusinessService
    {
        private readonly TsicsContext _dBtsics = Factory.Create<TsicsContext>("TsicsContext", ClassType.clsTypeDataContext);
        public TicketResolution Add(TicketResolution ticketResolution)
        {
            _dBtsics.TicketResolution.Add(ticketResolution);
            _dBtsics.SaveChanges();
            return ticketResolution;
        }
        public TicketResolution GetDetail(int id)
        {
            return _dBtsics.TicketResolution.Find(id);
        }
        public TicketResolution GetDatebyTicketandUserId(int id, int userid)
        {
            return _dBtsics.TicketResolution.OrderByDescending(t => t.RespondTime)
                .FirstOrDefault(t => t.TicketId == id && t.UserId.Equals(userid));
        }
        public TicketResolution GetByTicket(int ticketId)
        {
            return _dBtsics.TicketResolution.FirstOrDefault(resolution => resolution.TicketId.Equals(ticketId));
        }
        public TicketResolution GetLastCommentDataByTicket(int ticketId)
        {
            return _dBtsics.TicketResolution.Where(t => t.TicketId.Equals(ticketId)).OrderByDescending(t => t.CreatedAt).FirstOrDefault();
        }
        public bool isExist(int ticketid)
        {
            return _dBtsics.TicketResolution.Count(t => t.TicketId ==ticketid) > 0;
        }
        public TicketResolution GetListforEmail(int id)
        {
            return _dBtsics.TicketResolution.OrderByDescending(e => e.CreatedAt)
               .FirstOrDefault(t => t.TicketId == id);
        }
    }
}
