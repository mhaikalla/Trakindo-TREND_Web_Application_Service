using Com.Trakindo.Framework;
using Com.Trakindo.TSICS.Data.Context;
using Com.Trakindo.TSICS.Data.Model;
using System.Linq;

namespace Com.Trakindo.TSICS.Business.Service
{
    public class RatingBusinessService
    {
        private readonly TsicsContext _ctx = Factory.Create<TsicsContext>("TsicsContext", ClassType.clsTypeDataContext);
        
        public Rating Add(Rating rating)
        {
            _ctx.Rating.Add(rating);
            _ctx.SaveChanges();
            return rating;
        }
        
        public Rating GetRatingFromSubmiter(int ticketId, int submiterId)
        {
            return _ctx.Rating.FirstOrDefault(q => q.TicketId == ticketId && q.RatedFrom == submiterId);
        }

        public Rating GetRatingFromResponder(int ticketId, int responderId)
        {
            return _ctx.Rating.FirstOrDefault(q => q.TicketId == ticketId && q.RatedFrom == responderId);
        }
    }
}
