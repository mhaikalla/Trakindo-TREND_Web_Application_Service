using Com.Trakindo.Framework;
using Com.Trakindo.TSICS.Data.Context;
using Com.Trakindo.TSICS.Data.Model;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Com.Trakindo.TSICS.Business.Service
{
    public class TicketCategoryBusinessService
    {
        private readonly TsicsContext _dBtsics = Factory.Create<TsicsContext>("TsicsContext", ClassType.clsTypeDataContext);
        public List<TicketCategory> GetList()
        {
            List<TicketCategory> result = _dBtsics.TicketCategory.OrderBy(t => t.TicketCategoryId)
                .ToList();

            return result;
        }
        
        public TicketCategory Add(TicketCategory ticketCategory)
        {
            _dBtsics.TicketCategory.Add(ticketCategory);
            _dBtsics.SaveChanges();
            return ticketCategory;
        }
        public TicketCategory Edit(TicketCategory ticketCategory)
        {
            _dBtsics.Entry(ticketCategory).State = EntityState.Modified;
            _dBtsics.SaveChanges();
            return ticketCategory;
        }

        public TicketCategory GetDetail(int id)
        {
            TicketCategory ticketCategory = _dBtsics.TicketCategory.Find(id);
            return (ticketCategory);
        }
        public TicketCategory GetByName(string name)
        {
            TicketCategory ticketCategory = _dBtsics.TicketCategory
                .FirstOrDefault(q => q.Name == name);
            return ticketCategory;
        }
        public List<TicketCategory> GetListParent()
        {
            List<TicketCategory> result = _dBtsics.TicketCategory.Where(t => t.TicketCategoryId.Equals(0)).ToList();
            return result;
        }
        public List<TicketCategory> GetListChild(int id)
        {
            List<TicketCategory> result = _dBtsics.TicketCategory.Where(t => t.TicketCategoryId.Equals(id)).ToList();
            return result;
        }
    }
}
