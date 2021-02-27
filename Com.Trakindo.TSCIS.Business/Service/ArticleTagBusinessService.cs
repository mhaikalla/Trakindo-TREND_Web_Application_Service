using System;
using System.Collections.Generic;
using System.Linq;
using Com.Trakindo.Framework;
using Com.Trakindo.TSICS.Data.Context;
using Com.Trakindo.TSICS.Data.Model;

namespace Com.Trakindo.TSICS.Business.Service
{
    public class ArticleTagBusinessService
    {
        private readonly TsicsContext _ctx = Factory.Create<TsicsContext>("TsicsContext", ClassType.clsTypeDataContext);

        public ArticleTag Add(ArticleTag articleTag)
        {
            _ctx.ArticleTag.Add(articleTag);
            _ctx.SaveChanges();
            return articleTag;
        }

        public List<ArticleTag> GetTagsByTicket(int ticketId)
        {
            return _ctx.ArticleTag.Where(q => q.TicketId == ticketId).ToList();
        }

        public void Delete(int ticketId, string name)
        {
            ArticleTag articleTag = _ctx.ArticleTag.FirstOrDefault(q => q.TicketId == ticketId && q.Name == name);
            if(articleTag != null)
            {
                _ctx.ArticleTag.Remove(articleTag);
                _ctx.SaveChanges();
            }
        }
        public void DeleteAllTagsInTicket(int TicketId)
        {
            using (TsicsContext db = new TsicsContext())
            {
                db.Database.ExecuteSqlCommand("Delete from ArticleTag where TicketId = '" + TicketId + "'");
            }
        }
        public Boolean IsThisTagExists(int ticketId, string name)
        {
            return _ctx.ArticleTag.Where(q => q.TicketId == ticketId && q.Name == name).ToList().Count > 0;
        }

        public void BulkDeleteByTicket(int ticketId)
        {
            using (TsicsContext db = new TsicsContext())
            {
                db.Database.ExecuteSqlCommand("DELETE FROM ArticleTag WHERE TicketId = {0}", ticketId);
            }
        }
        public List<int> searchTagTicket(string search)
        {
            return _ctx.ArticleTag.Where(e => e.Name.Contains(search)).Select(t => t.TicketId).ToList();
        }
    }
}
