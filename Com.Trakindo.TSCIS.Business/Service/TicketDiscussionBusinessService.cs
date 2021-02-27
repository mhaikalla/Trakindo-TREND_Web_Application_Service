using Com.Trakindo.Framework;
using Com.Trakindo.TSICS.Data.Context;
using Com.Trakindo.TSICS.Data.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Com.Trakindo.TSICS.Business.Service
{
    public class TicketDiscussionBusinessService
    {
        private readonly TsicsContext _dBtsics = Factory.Create<TsicsContext>("TsicsContext", ClassType.clsTypeDataContext);
        public List<TicketDiscussion> GetListByTicket(int id)
        {
            List<TicketDiscussion> result = _dBtsics.TicketDiscussion
                .Where(t => t.TicketId == id)
                .Where(t => t.Status != 0)
                .OrderByDescending(t => t.CreatedAt)
                .ToList();

            return result;
        }
        public TicketDiscussion Add(TicketDiscussion ticketDiscussion)
        {
            _dBtsics.TicketDiscussion.Add(ticketDiscussion);
            _dBtsics.SaveChanges();
            return ticketDiscussion;
        }
        public TicketDiscussion GetDatebyTicketandUserId(int id, int userid)
        {
            return _dBtsics.TicketDiscussion.OrderByDescending(t => t.RespondTime)
                .FirstOrDefault(t => t.TicketId == id && t.UserId.Equals(userid));
        }
        public void SetNote(int ticketId, int ticketNoteId)
        {
            _dBtsics.Database.ExecuteSqlCommand("UPDATE TicketDiscussion SET TicketNoteId = {0} WHERE TicketId = {1} AND TicketNoteId = {2}", ticketNoteId, ticketId, 0);
        }
        public IQueryable<TicketDiscussion> GetGroupedByNote(int ticketId)
        {

            return _dBtsics.TicketDiscussion
                .Where(discussion => discussion.TicketId.Equals(ticketId) && discussion.Status == 1);
        }

        public TicketDiscussion GetDetail(int id)
        {
            TicketDiscussion  result = _dBtsics.TicketDiscussion
                .FirstOrDefault(t => t.TicketDiscussionId == id);

            return result;
        }
        public TicketDiscussion DeleteDiscussion(TicketDiscussion ticketDiscussion)
        {
            ticketDiscussion.Status = 0;
            _dBtsics.Entry(ticketDiscussion).State = EntityState.Modified;
            _dBtsics.SaveChanges();
            //DBtsics.TicketParcipant.Remove(TicketParcipant);
            //var deleteParticipant = DBtsics.SaveChanges();
            TicketDiscussion getDetailDiscussion = GetDetail(ticketDiscussion.TicketDiscussionId);
            return getDetailDiscussion;
        }
        # region Monthly
        public decimal GetValuePercentage(int userId, string type, int month, int year)
        {


            var discussionList = (from ticketDiscussion in _dBtsics.TicketDiscussion
                                  from ticket in _dBtsics.Ticket.Where(w => w.TicketId == ticketDiscussion.TicketId).DefaultIfEmpty()
                                  where ticket.Status == 3 && ticket.Responder == userId && ticketDiscussion.UserId == userId
                                  select new
                                  {
                                      idTicketDiscussion = ticketDiscussion.TicketDiscussionId,
                                      discussionDate = ticketDiscussion.CreatedAt,
                                      prevDataDiscussion = _dBtsics.TicketDiscussion.Where(w => w.TicketId == ticket.TicketId && w.CreatedAt < ticketDiscussion.CreatedAt).OrderByDescending(obd => obd.TicketDiscussionId).FirstOrDefault(),
                                      ticketCreated = ticket.CreatedAt,
                                      totalRowTicketRating = _dBtsics.Rating.Count(w => w.TicketId == ticket.TicketId),
                                      lastRowRating = _dBtsics.Rating.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(obd=>obd.UserRatingId).FirstOrDefault()
                                  });

            if (type == "submitter")
            {
                 discussionList = (from ticketDiscussion in _dBtsics.TicketDiscussion
                                      from ticket in _dBtsics.Ticket.Where(w => w.TicketId == ticketDiscussion.TicketId).DefaultIfEmpty()
                                      where ticket.Status == 3 && ticket.Submiter == userId && ticketDiscussion.UserId == userId
                                      select new
                                      {
                                          idTicketDiscussion = ticketDiscussion.TicketDiscussionId,
                                          discussionDate = ticketDiscussion.CreatedAt,
                                          prevDataDiscussion = _dBtsics.TicketDiscussion.Where(w => w.TicketId == ticket.TicketId && w.CreatedAt < ticketDiscussion.CreatedAt).OrderByDescending(obd => obd.TicketDiscussionId).FirstOrDefault(),
                                          ticketCreated = ticket.CreatedAt,
                                          totalRowTicketRating = _dBtsics.Rating.Count(w => w.TicketId == ticket.TicketId),
                                          lastRowRating = _dBtsics.Rating.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(obd => obd.UserRatingId).FirstOrDefault()
                                      });
            }


            var listDiscussionCount = (from listDiscussion in discussionList
                                       where listDiscussion.totalRowTicketRating == 2 && listDiscussion.lastRowRating.CreatedAt.Month == month && listDiscussion.lastRowRating.CreatedAt.Year == year   
                              select new
                              {
                                  listDiscussion.idTicketDiscussion,
                                  listDiscussion.discussionDate,
                                  listDiscussion.prevDataDiscussion,
                                  listDiscussion.ticketCreated,
                                  responseDay = (listDiscussion.prevDataDiscussion != null) ? DbFunctions.DiffDays(listDiscussion.discussionDate .Value,listDiscussion.prevDataDiscussion.CreatedAt.Value) : DbFunctions.DiffDays(listDiscussion.discussionDate.Value,listDiscussion.ticketCreated.Value),
                                  isNotMoreThanThree = (((listDiscussion.prevDataDiscussion != null) ? DbFunctions.DiffDays(listDiscussion.discussionDate.Value, listDiscussion.prevDataDiscussion.CreatedAt.Value) : DbFunctions.DiffDays(listDiscussion.discussionDate.Value, listDiscussion.ticketCreated.Value)) <= 3)? 1 : 0
                              }
                              ).ToList();

            var avarage = new decimal();
            var totalRow = listDiscussionCount.Count;

            foreach (var item in listDiscussionCount)
            {
                avarage = avarage + item.isNotMoreThanThree;
            }

            if(avarage != 0)
            {
                avarage = (avarage / totalRow) * 100;
            }

            return avarage;
        }
        #endregion

        # region Yearly
        public decimal GetValuePercentageYearly(int userId, string type, int year)
        {


            var discussionList = (from ticketDiscussion in _dBtsics.TicketDiscussion
                                  from ticket in _dBtsics.Ticket.Where(w => w.TicketId == ticketDiscussion.TicketId).DefaultIfEmpty()
                                  where ticket.Status == 3 && ticket.Responder == userId && ticketDiscussion.UserId == userId
                                  select new
                                  {
                                      idTicketDiscussion = ticketDiscussion.TicketDiscussionId,
                                      discussionDate = ticketDiscussion.CreatedAt,
                                      prevDataDiscussion = _dBtsics.TicketDiscussion.Where(w => w.TicketId == ticket.TicketId && w.CreatedAt < ticketDiscussion.CreatedAt).OrderByDescending(obd => obd.TicketDiscussionId).FirstOrDefault(),
                                      ticketCreated = ticket.CreatedAt,
                                      totalRowTicketRating = _dBtsics.Rating.Count(w => w.TicketId == ticket.TicketId),
                                      lastRowRating = _dBtsics.Rating.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(obd => obd.UserRatingId).FirstOrDefault()
                                  });

            if (type == "submitter")
            {
                discussionList = (from ticketDiscussion in _dBtsics.TicketDiscussion
                                  from ticket in _dBtsics.Ticket.Where(w => w.TicketId == ticketDiscussion.TicketId).DefaultIfEmpty()
                                  where ticket.Status == 3 && ticket.Submiter == userId && ticketDiscussion.UserId == userId
                                  select new
                                  {
                                      idTicketDiscussion = ticketDiscussion.TicketDiscussionId,
                                      discussionDate = ticketDiscussion.CreatedAt,
                                      prevDataDiscussion = _dBtsics.TicketDiscussion.Where(w => w.TicketId == ticket.TicketId && w.CreatedAt < ticketDiscussion.CreatedAt).OrderByDescending(obd => obd.TicketDiscussionId).FirstOrDefault(),
                                      ticketCreated = ticket.CreatedAt,
                                      totalRowTicketRating = _dBtsics.Rating.Count(w => w.TicketId == ticket.TicketId),
                                      lastRowRating = _dBtsics.Rating.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(obd => obd.UserRatingId).FirstOrDefault()
                                  });
            }


            var listDiscussionCount = (from listDiscussion in discussionList
                                       where listDiscussion.totalRowTicketRating == 2 && listDiscussion.lastRowRating.CreatedAt.Year == year
                                       select new
                                       {
                                           listDiscussion.idTicketDiscussion,
                                           listDiscussion.discussionDate,
                                           listDiscussion.prevDataDiscussion,
                                           listDiscussion.ticketCreated,
                                           responseDay = (listDiscussion.prevDataDiscussion != null) ? DbFunctions.DiffDays(listDiscussion.discussionDate.Value, listDiscussion.prevDataDiscussion.CreatedAt.Value) : DbFunctions.DiffDays(listDiscussion.discussionDate.Value, listDiscussion.ticketCreated.Value),
                                           isNotMoreThanThree = (((listDiscussion.prevDataDiscussion != null) ? DbFunctions.DiffDays(listDiscussion.discussionDate.Value, listDiscussion.prevDataDiscussion.CreatedAt.Value) : DbFunctions.DiffDays(listDiscussion.discussionDate.Value, listDiscussion.ticketCreated.Value)) <= 3) ? 1 : 0
                                       }
                              ).ToList();

            var avarage = new decimal();
            var totalRow = listDiscussionCount.Count;

            foreach (var item in listDiscussionCount)
            {
                avarage = avarage + item.isNotMoreThanThree;
            }

            if (avarage != 0)
            {
                avarage = (avarage / totalRow) * 100;
            }

            return avarage;
        }

        public decimal GetValuePercentageYearlyJan(int userId, string type, int year)
        {


            var discussionList = (from ticketDiscussion in _dBtsics.TicketDiscussion
                                  from ticket in _dBtsics.Ticket.Where(w => w.TicketId == ticketDiscussion.TicketId).DefaultIfEmpty()
                                  where ticket.Status == 3 && ticket.Responder == userId && ticketDiscussion.UserId == userId
                                  select new
                                  {
                                      idTicketDiscussion = ticketDiscussion.TicketDiscussionId,
                                      discussionDate = ticketDiscussion.CreatedAt,
                                      prevDataDiscussion = _dBtsics.TicketDiscussion.Where(w => w.TicketId == ticket.TicketId && w.CreatedAt < ticketDiscussion.CreatedAt).OrderByDescending(obd => obd.TicketDiscussionId).FirstOrDefault(),
                                      ticketCreated = ticket.CreatedAt,
                                      totalRowTicketRating = _dBtsics.Rating.Count(w => w.TicketId == ticket.TicketId),
                                      lastRowRating = _dBtsics.Rating.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(obd => obd.UserRatingId).FirstOrDefault()
                                  });

            if (type == "submitter")
            {
                discussionList = (from ticketDiscussion in _dBtsics.TicketDiscussion
                                  from ticket in _dBtsics.Ticket.Where(w => w.TicketId == ticketDiscussion.TicketId).DefaultIfEmpty()
                                  where ticket.Status == 3 && ticket.Submiter == userId && ticketDiscussion.UserId == userId
                                  select new
                                  {
                                      idTicketDiscussion = ticketDiscussion.TicketDiscussionId,
                                      discussionDate = ticketDiscussion.CreatedAt,
                                      prevDataDiscussion = _dBtsics.TicketDiscussion.Where(w => w.TicketId == ticket.TicketId && w.CreatedAt < ticketDiscussion.CreatedAt).OrderByDescending(obd => obd.TicketDiscussionId).FirstOrDefault(),
                                      ticketCreated = ticket.CreatedAt,
                                      totalRowTicketRating = _dBtsics.Rating.Count(w => w.TicketId == ticket.TicketId),
                                      lastRowRating = _dBtsics.Rating.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(obd => obd.UserRatingId).FirstOrDefault()
                                  });
            }


            var listDiscussionCount = (from listDiscussion in discussionList
                                       where listDiscussion.totalRowTicketRating == 2 && listDiscussion.lastRowRating.CreatedAt.Month == 01 && listDiscussion.lastRowRating.CreatedAt.Year == year
                                       select new
                                       {
                                           listDiscussion.idTicketDiscussion,
                                           listDiscussion.discussionDate,
                                           listDiscussion.prevDataDiscussion,
                                           listDiscussion.ticketCreated,
                                           responseDay = (listDiscussion.prevDataDiscussion != null) ? DbFunctions.DiffDays(listDiscussion.discussionDate.Value, listDiscussion.prevDataDiscussion.CreatedAt.Value) : DbFunctions.DiffDays(listDiscussion.discussionDate.Value, listDiscussion.ticketCreated.Value),
                                           isNotMoreThanThree = (((listDiscussion.prevDataDiscussion != null) ? DbFunctions.DiffDays(listDiscussion.discussionDate.Value, listDiscussion.prevDataDiscussion.CreatedAt.Value) : DbFunctions.DiffDays(listDiscussion.discussionDate.Value, listDiscussion.ticketCreated.Value)) <= 3) ? 1 : 0
                                       }
                              ).ToList();

            var avarage = new decimal();
            var totalRow = listDiscussionCount.Count;

            foreach (var item in listDiscussionCount)
            {
                avarage = avarage + item.isNotMoreThanThree;
            }

            if (avarage != 0)
            {
                avarage = (avarage / totalRow) * 100 / 20;
            }

            return avarage;
        }
        public decimal GetValuePercentageYearlyFeb(int userId, string type, int year)
        {


            var discussionList = (from ticketDiscussion in _dBtsics.TicketDiscussion
                                  from ticket in _dBtsics.Ticket.Where(w => w.TicketId == ticketDiscussion.TicketId).DefaultIfEmpty()
                                  where ticket.Status == 3 && ticket.Responder == userId && ticketDiscussion.UserId == userId
                                  select new
                                  {
                                      idTicketDiscussion = ticketDiscussion.TicketDiscussionId,
                                      discussionDate = ticketDiscussion.CreatedAt,
                                      prevDataDiscussion = _dBtsics.TicketDiscussion.Where(w => w.TicketId == ticket.TicketId && w.CreatedAt < ticketDiscussion.CreatedAt).OrderByDescending(obd => obd.TicketDiscussionId).FirstOrDefault(),
                                      ticketCreated = ticket.CreatedAt,
                                      totalRowTicketRating = _dBtsics.Rating.Count(w => w.TicketId == ticket.TicketId),
                                      lastRowRating = _dBtsics.Rating.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(obd => obd.UserRatingId).FirstOrDefault()
                                  });

            if (type == "submitter")
            {
                discussionList = (from ticketDiscussion in _dBtsics.TicketDiscussion
                                  from ticket in _dBtsics.Ticket.Where(w => w.TicketId == ticketDiscussion.TicketId).DefaultIfEmpty()
                                  where ticket.Status == 3 && ticket.Submiter == userId && ticketDiscussion.UserId == userId
                                  select new
                                  {
                                      idTicketDiscussion = ticketDiscussion.TicketDiscussionId,
                                      discussionDate = ticketDiscussion.CreatedAt,
                                      prevDataDiscussion = _dBtsics.TicketDiscussion.Where(w => w.TicketId == ticket.TicketId && w.CreatedAt < ticketDiscussion.CreatedAt).OrderByDescending(obd => obd.TicketDiscussionId).FirstOrDefault(),
                                      ticketCreated = ticket.CreatedAt,
                                      totalRowTicketRating = _dBtsics.Rating.Count(w => w.TicketId == ticket.TicketId),
                                      lastRowRating = _dBtsics.Rating.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(obd => obd.UserRatingId).FirstOrDefault()
                                  });
            }


            var listDiscussionCount = (from listDiscussion in discussionList
                                       where listDiscussion.totalRowTicketRating == 2 && listDiscussion.lastRowRating.CreatedAt.Month == 02 && listDiscussion.lastRowRating.CreatedAt.Year == year
                                       select new
                                       {
                                           listDiscussion.idTicketDiscussion,
                                           listDiscussion.discussionDate,
                                           listDiscussion.prevDataDiscussion,
                                           listDiscussion.ticketCreated,
                                           responseDay = (listDiscussion.prevDataDiscussion != null) ? DbFunctions.DiffDays(listDiscussion.discussionDate.Value, listDiscussion.prevDataDiscussion.CreatedAt.Value) : DbFunctions.DiffDays(listDiscussion.discussionDate.Value, listDiscussion.ticketCreated.Value),
                                           isNotMoreThanThree = (((listDiscussion.prevDataDiscussion != null) ? DbFunctions.DiffDays(listDiscussion.discussionDate.Value, listDiscussion.prevDataDiscussion.CreatedAt.Value) : DbFunctions.DiffDays(listDiscussion.discussionDate.Value, listDiscussion.ticketCreated.Value)) <= 3) ? 1 : 0
                                       }
                              ).ToList();

            var avarage = new decimal();
            var totalRow = listDiscussionCount.Count;

            foreach (var item in listDiscussionCount)
            {
                avarage = avarage + item.isNotMoreThanThree;
            }

            if (avarage != 0)
            {
                avarage = (avarage / totalRow) * 100 / 20;
            }

            return avarage;
        }
        public decimal GetValuePercentageYearlyMar(int userId, string type, int year)
        {


            var discussionList = (from ticketDiscussion in _dBtsics.TicketDiscussion
                                  from ticket in _dBtsics.Ticket.Where(w => w.TicketId == ticketDiscussion.TicketId).DefaultIfEmpty()
                                  where ticket.Status == 3 && ticket.Responder == userId && ticketDiscussion.UserId == userId
                                  select new
                                  {
                                      idTicketDiscussion = ticketDiscussion.TicketDiscussionId,
                                      discussionDate = ticketDiscussion.CreatedAt,
                                      prevDataDiscussion = _dBtsics.TicketDiscussion.Where(w => w.TicketId == ticket.TicketId && w.CreatedAt < ticketDiscussion.CreatedAt).OrderByDescending(obd => obd.TicketDiscussionId).FirstOrDefault(),
                                      ticketCreated = ticket.CreatedAt,
                                      totalRowTicketRating = _dBtsics.Rating.Count(w => w.TicketId == ticket.TicketId),
                                      lastRowRating = _dBtsics.Rating.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(obd => obd.UserRatingId).FirstOrDefault()
                                  });

            if (type == "submitter")
            {
                discussionList = (from ticketDiscussion in _dBtsics.TicketDiscussion
                                  from ticket in _dBtsics.Ticket.Where(w => w.TicketId == ticketDiscussion.TicketId).DefaultIfEmpty()
                                  where ticket.Status == 3 && ticket.Submiter == userId && ticketDiscussion.UserId == userId
                                  select new
                                  {
                                      idTicketDiscussion = ticketDiscussion.TicketDiscussionId,
                                      discussionDate = ticketDiscussion.CreatedAt,
                                      prevDataDiscussion = _dBtsics.TicketDiscussion.Where(w => w.TicketId == ticket.TicketId && w.CreatedAt < ticketDiscussion.CreatedAt).OrderByDescending(obd => obd.TicketDiscussionId).FirstOrDefault(),
                                      ticketCreated = ticket.CreatedAt,
                                      totalRowTicketRating = _dBtsics.Rating.Count(w => w.TicketId == ticket.TicketId),
                                      lastRowRating = _dBtsics.Rating.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(obd => obd.UserRatingId).FirstOrDefault()
                                  });
            }


            var listDiscussionCount = (from listDiscussion in discussionList
                                       where listDiscussion.totalRowTicketRating == 2 && listDiscussion.lastRowRating.CreatedAt.Month == 03 && listDiscussion.lastRowRating.CreatedAt.Year == year
                                       select new
                                       {
                                           listDiscussion.idTicketDiscussion,
                                           listDiscussion.discussionDate,
                                           listDiscussion.prevDataDiscussion,
                                           listDiscussion.ticketCreated,
                                           responseDay = (listDiscussion.prevDataDiscussion != null) ? DbFunctions.DiffDays(listDiscussion.discussionDate.Value, listDiscussion.prevDataDiscussion.CreatedAt.Value) : DbFunctions.DiffDays(listDiscussion.discussionDate.Value, listDiscussion.ticketCreated.Value),
                                           isNotMoreThanThree = (((listDiscussion.prevDataDiscussion != null) ? DbFunctions.DiffDays(listDiscussion.discussionDate.Value, listDiscussion.prevDataDiscussion.CreatedAt.Value) : DbFunctions.DiffDays(listDiscussion.discussionDate.Value, listDiscussion.ticketCreated.Value)) <= 3) ? 1 : 0
                                       }
                              ).ToList();

            var avarage = new decimal();
            var totalRow = listDiscussionCount.Count;

            foreach (var item in listDiscussionCount)
            {
                avarage = avarage + item.isNotMoreThanThree;
            }

            if (avarage != 0)
            {
                avarage = (avarage / totalRow) * 100 / 20;
            }

            return avarage;
        }
        public decimal GetValuePercentageYearlyApr(int userId, string type, int year)
        {


            var discussionList = (from ticketDiscussion in _dBtsics.TicketDiscussion
                                  from ticket in _dBtsics.Ticket.Where(w => w.TicketId == ticketDiscussion.TicketId).DefaultIfEmpty()
                                  where ticket.Status == 3 && ticket.Responder == userId && ticketDiscussion.UserId == userId
                                  select new
                                  {
                                      idTicketDiscussion = ticketDiscussion.TicketDiscussionId,
                                      discussionDate = ticketDiscussion.CreatedAt,
                                      prevDataDiscussion = _dBtsics.TicketDiscussion.Where(w => w.TicketId == ticket.TicketId && w.CreatedAt < ticketDiscussion.CreatedAt).OrderByDescending(obd => obd.TicketDiscussionId).FirstOrDefault(),
                                      ticketCreated = ticket.CreatedAt,
                                      totalRowTicketRating = _dBtsics.Rating.Count(w => w.TicketId == ticket.TicketId),
                                      lastRowRating = _dBtsics.Rating.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(obd => obd.UserRatingId).FirstOrDefault()
                                  });

         if (type == "submitter")
            {
                discussionList = (from ticketDiscussion in _dBtsics.TicketDiscussion
                                  from ticket in _dBtsics.Ticket.Where(w => w.TicketId == ticketDiscussion.TicketId).DefaultIfEmpty()
                                  where ticket.Status == 3 && ticket.Submiter == userId && ticketDiscussion.UserId == userId
                                  select new
                                  {
                                      idTicketDiscussion = ticketDiscussion.TicketDiscussionId,
                                      discussionDate = ticketDiscussion.CreatedAt,
                                      prevDataDiscussion = _dBtsics.TicketDiscussion.Where(w => w.TicketId == ticket.TicketId && w.CreatedAt < ticketDiscussion.CreatedAt).OrderByDescending(obd => obd.TicketDiscussionId).FirstOrDefault(),
                                      ticketCreated = ticket.CreatedAt,
                                      totalRowTicketRating = _dBtsics.Rating.Count(w => w.TicketId == ticket.TicketId),
                                      lastRowRating = _dBtsics.Rating.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(obd => obd.UserRatingId).FirstOrDefault()
                                  });
            }


            var listDiscussionCount = (from listDiscussion in discussionList
                                       where listDiscussion.totalRowTicketRating == 2 && listDiscussion.lastRowRating.CreatedAt.Month == 04 && listDiscussion.lastRowRating.CreatedAt.Year == year
                                       select new
                                       {
                                           listDiscussion.idTicketDiscussion,
                                           listDiscussion.discussionDate,
                                           listDiscussion.prevDataDiscussion,
                                           listDiscussion.ticketCreated,
                                           responseDay = (listDiscussion.prevDataDiscussion != null) ? DbFunctions.DiffDays(listDiscussion.discussionDate.Value, listDiscussion.prevDataDiscussion.CreatedAt.Value) : DbFunctions.DiffDays(listDiscussion.discussionDate.Value, listDiscussion.ticketCreated.Value),
                                           isNotMoreThanThree = (((listDiscussion.prevDataDiscussion != null) ? DbFunctions.DiffDays(listDiscussion.discussionDate.Value, listDiscussion.prevDataDiscussion.CreatedAt.Value) : DbFunctions.DiffDays(listDiscussion.discussionDate.Value, listDiscussion.ticketCreated.Value)) <= 3) ? 1 : 0
                                       }
                              ).ToList();

            var avarage = new decimal();
            var totalRow = listDiscussionCount.Count;

            foreach (var item in listDiscussionCount)
            {
                avarage = avarage + item.isNotMoreThanThree;
            }

            if (avarage != 0)
            {
                avarage = (avarage / totalRow) * 100 / 20;
            }

            return avarage;
        }
        public decimal GetValuePercentageYearlyMei(int userId, string type, int year)
        {


            var discussionList = (from ticketDiscussion in _dBtsics.TicketDiscussion
                                  from ticket in _dBtsics.Ticket.Where(w => w.TicketId == ticketDiscussion.TicketId).DefaultIfEmpty()
                                  where ticket.Status == 3 && ticket.Responder == userId && ticketDiscussion.UserId == userId
                                  select new
                                  {
                                      idTicketDiscussion = ticketDiscussion.TicketDiscussionId,
                                      discussionDate = ticketDiscussion.CreatedAt,
                                      prevDataDiscussion = _dBtsics.TicketDiscussion.Where(w => w.TicketId == ticket.TicketId && w.CreatedAt < ticketDiscussion.CreatedAt).OrderByDescending(obd => obd.TicketDiscussionId).FirstOrDefault(),
                                      ticketCreated = ticket.CreatedAt,
                                      totalRowTicketRating = _dBtsics.Rating.Count(w => w.TicketId == ticket.TicketId),
                                      lastRowRating = _dBtsics.Rating.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(obd => obd.UserRatingId).FirstOrDefault()
                                  });

            if (type == "submitter")
            {
                discussionList = (from ticketDiscussion in _dBtsics.TicketDiscussion
                                  from ticket in _dBtsics.Ticket.Where(w => w.TicketId == ticketDiscussion.TicketId).DefaultIfEmpty()
                                  where ticket.Status == 3 && ticket.Submiter == userId && ticketDiscussion.UserId == userId
                                  select new
                                  {
                                      idTicketDiscussion = ticketDiscussion.TicketDiscussionId,
                                      discussionDate = ticketDiscussion.CreatedAt,
                                      prevDataDiscussion = _dBtsics.TicketDiscussion.Where(w => w.TicketId == ticket.TicketId && w.CreatedAt < ticketDiscussion.CreatedAt).OrderByDescending(obd => obd.TicketDiscussionId).FirstOrDefault(),
                                      ticketCreated = ticket.CreatedAt,
                                      totalRowTicketRating = _dBtsics.Rating.Count(w => w.TicketId == ticket.TicketId),
                                      lastRowRating = _dBtsics.Rating.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(obd => obd.UserRatingId).FirstOrDefault()
                                  });
            }


            var listDiscussionCount = (from listDiscussion in discussionList
                                       where listDiscussion.totalRowTicketRating == 2 && listDiscussion.lastRowRating.CreatedAt.Month == 05 && listDiscussion.lastRowRating.CreatedAt.Year == year
                                       select new
                                       {
                                           listDiscussion.idTicketDiscussion,
                                           listDiscussion.discussionDate,
                                           listDiscussion.prevDataDiscussion,
                                           listDiscussion.ticketCreated,
                                           responseDay = (listDiscussion.prevDataDiscussion != null) ? DbFunctions.DiffDays(listDiscussion.discussionDate.Value, listDiscussion.prevDataDiscussion.CreatedAt.Value) : DbFunctions.DiffDays(listDiscussion.discussionDate.Value, listDiscussion.ticketCreated.Value),
                                           isNotMoreThanThree = (((listDiscussion.prevDataDiscussion != null) ? DbFunctions.DiffDays(listDiscussion.discussionDate.Value, listDiscussion.prevDataDiscussion.CreatedAt.Value) : DbFunctions.DiffDays(listDiscussion.discussionDate.Value, listDiscussion.ticketCreated.Value)) <= 3) ? 1 : 0
                                       }
                              ).ToList();

            var avarage = new decimal();
            var totalRow = listDiscussionCount.Count;

            foreach (var item in listDiscussionCount)
            {
                avarage = avarage + item.isNotMoreThanThree;
            }

            if (avarage != 0)
            {
                avarage = (avarage / totalRow) * 100 / 20;
            }

            return avarage;
        }
        public decimal GetValuePercentageYearlyJun(int userId, string type, int year)
        {


            var discussionList = (from ticketDiscussion in _dBtsics.TicketDiscussion
                                  from ticket in _dBtsics.Ticket.Where(w => w.TicketId == ticketDiscussion.TicketId).DefaultIfEmpty()
                                  where ticket.Status == 3 && ticket.Responder == userId && ticketDiscussion.UserId == userId
                                  select new
                                  {
                                      idTicketDiscussion = ticketDiscussion.TicketDiscussionId,
                                      discussionDate = ticketDiscussion.CreatedAt,
                                      prevDataDiscussion = _dBtsics.TicketDiscussion.Where(w => w.TicketId == ticket.TicketId && w.CreatedAt < ticketDiscussion.CreatedAt).OrderByDescending(obd => obd.TicketDiscussionId).FirstOrDefault(),
                                      ticketCreated = ticket.CreatedAt,
                                      totalRowTicketRating = _dBtsics.Rating.Count(w => w.TicketId == ticket.TicketId),
                                      lastRowRating = _dBtsics.Rating.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(obd => obd.UserRatingId).FirstOrDefault()
                                  });

            if (type == "submitter")
            {
                discussionList = (from ticketDiscussion in _dBtsics.TicketDiscussion
                                  from ticket in _dBtsics.Ticket.Where(w => w.TicketId == ticketDiscussion.TicketId).DefaultIfEmpty()
                                  where ticket.Status == 3 && ticket.Submiter == userId && ticketDiscussion.UserId == userId
                                  select new
                                  {
                                      idTicketDiscussion = ticketDiscussion.TicketDiscussionId,
                                      discussionDate = ticketDiscussion.CreatedAt,
                                      prevDataDiscussion = _dBtsics.TicketDiscussion.Where(w => w.TicketId == ticket.TicketId && w.CreatedAt < ticketDiscussion.CreatedAt).OrderByDescending(obd => obd.TicketDiscussionId).FirstOrDefault(),
                                      ticketCreated = ticket.CreatedAt,
                                      totalRowTicketRating = _dBtsics.Rating.Count(w => w.TicketId == ticket.TicketId),
                                      lastRowRating = _dBtsics.Rating.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(obd => obd.UserRatingId).FirstOrDefault()
                                  });
            }


            var listDiscussionCount = (from listDiscussion in discussionList
                                       where listDiscussion.totalRowTicketRating == 2 && listDiscussion.lastRowRating.CreatedAt.Month == 06 && listDiscussion.lastRowRating.CreatedAt.Year == year
                                       select new
                                       {
                                           listDiscussion.idTicketDiscussion,
                                           listDiscussion.discussionDate,
                                           listDiscussion.prevDataDiscussion,
                                           listDiscussion.ticketCreated,
                                           responseDay = (listDiscussion.prevDataDiscussion != null) ? DbFunctions.DiffDays(listDiscussion.discussionDate.Value, listDiscussion.prevDataDiscussion.CreatedAt.Value) : DbFunctions.DiffDays(listDiscussion.discussionDate.Value, listDiscussion.ticketCreated.Value),
                                           isNotMoreThanThree = (((listDiscussion.prevDataDiscussion != null) ? DbFunctions.DiffDays(listDiscussion.discussionDate.Value, listDiscussion.prevDataDiscussion.CreatedAt.Value) : DbFunctions.DiffDays(listDiscussion.discussionDate.Value, listDiscussion.ticketCreated.Value)) <= 3) ? 1 : 0
                                       }
                              ).ToList();

            var avarage = new decimal();
            var totalRow = listDiscussionCount.Count;

            foreach (var item in listDiscussionCount)
            {
                avarage = avarage + item.isNotMoreThanThree;
            }

            if (avarage != 0)
            {
                avarage = (avarage / totalRow) * 100 / 20;
            }

            return avarage;
        }
        public decimal GetValuePercentageYearlyJul(int userId, string type, int year)
        {


            var discussionList = (from ticketDiscussion in _dBtsics.TicketDiscussion
                                  from ticket in _dBtsics.Ticket.Where(w => w.TicketId == ticketDiscussion.TicketId).DefaultIfEmpty()
                                  where ticket.Status == 3 && ticket.Responder == userId && ticketDiscussion.UserId == userId
                                  select new
                                  {
                                      idTicketDiscussion = ticketDiscussion.TicketDiscussionId,
                                      discussionDate = ticketDiscussion.CreatedAt,
                                      prevDataDiscussion = _dBtsics.TicketDiscussion.Where(w => w.TicketId == ticket.TicketId && w.CreatedAt < ticketDiscussion.CreatedAt).OrderByDescending(obd => obd.TicketDiscussionId).FirstOrDefault(),
                                      ticketCreated = ticket.CreatedAt,
                                      totalRowTicketRating = _dBtsics.Rating.Count(w => w.TicketId == ticket.TicketId),
                                      lastRowRating = _dBtsics.Rating.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(obd => obd.UserRatingId).FirstOrDefault()
                                  });

            if (type == "submitter")
            {
                discussionList = (from ticketDiscussion in _dBtsics.TicketDiscussion
                                  from ticket in _dBtsics.Ticket.Where(w => w.TicketId == ticketDiscussion.TicketId).DefaultIfEmpty()
                                  where ticket.Status == 3 && ticket.Submiter == userId && ticketDiscussion.UserId == userId
                                  select new
                                  {
                                      idTicketDiscussion = ticketDiscussion.TicketDiscussionId,
                                      discussionDate = ticketDiscussion.CreatedAt,
                                      prevDataDiscussion = _dBtsics.TicketDiscussion.Where(w => w.TicketId == ticket.TicketId && w.CreatedAt < ticketDiscussion.CreatedAt).OrderByDescending(obd => obd.TicketDiscussionId).FirstOrDefault(),
                                      ticketCreated = ticket.CreatedAt,
                                      totalRowTicketRating = _dBtsics.Rating.Count(w => w.TicketId == ticket.TicketId),
                                      lastRowRating = _dBtsics.Rating.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(obd => obd.UserRatingId).FirstOrDefault()
                                  });
            }


            var listDiscussionCount = (from listDiscussion in discussionList
                                       where listDiscussion.totalRowTicketRating == 2 && listDiscussion.lastRowRating.CreatedAt.Month == 07 && listDiscussion.lastRowRating.CreatedAt.Year == year
                                       select new
                                       {
                                           listDiscussion.idTicketDiscussion,
                                           listDiscussion.discussionDate,
                                           listDiscussion.prevDataDiscussion,
                                           listDiscussion.ticketCreated,
                                           responseDay = (listDiscussion.prevDataDiscussion != null) ? DbFunctions.DiffDays(listDiscussion.discussionDate.Value, listDiscussion.prevDataDiscussion.CreatedAt.Value) : DbFunctions.DiffDays(listDiscussion.discussionDate.Value, listDiscussion.ticketCreated.Value),
                                           isNotMoreThanThree = (((listDiscussion.prevDataDiscussion != null) ? DbFunctions.DiffDays(listDiscussion.discussionDate.Value, listDiscussion.prevDataDiscussion.CreatedAt.Value) : DbFunctions.DiffDays(listDiscussion.discussionDate.Value, listDiscussion.ticketCreated.Value)) <= 3) ? 1 : 0
                                       }
                              ).ToList();

            var avarage = new decimal();
            var totalRow = listDiscussionCount.Count;

            foreach (var item in listDiscussionCount)
            {
                avarage = avarage + item.isNotMoreThanThree;
            }

            if (avarage != 0)
            {
                avarage = (avarage / totalRow) * 100 / 20;
            }

            return avarage;
        }
        public decimal GetValuePercentageYearlyAgu(int userId, string type, int year)
        {


            var discussionList = (from ticketDiscussion in _dBtsics.TicketDiscussion
                                  from ticket in _dBtsics.Ticket.Where(w => w.TicketId == ticketDiscussion.TicketId).DefaultIfEmpty()
                                  where ticket.Status == 3 && ticket.Responder == userId && ticketDiscussion.UserId == userId
                                  select new
                                  {
                                      idTicketDiscussion = ticketDiscussion.TicketDiscussionId,
                                      discussionDate = ticketDiscussion.CreatedAt,
                                      prevDataDiscussion = _dBtsics.TicketDiscussion.Where(w => w.TicketId == ticket.TicketId && w.CreatedAt < ticketDiscussion.CreatedAt).OrderByDescending(obd => obd.TicketDiscussionId).FirstOrDefault(),
                                      ticketCreated = ticket.CreatedAt,
                                      totalRowTicketRating = _dBtsics.Rating.Count(w => w.TicketId == ticket.TicketId),
                                      lastRowRating = _dBtsics.Rating.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(obd => obd.UserRatingId).FirstOrDefault()
                                  });

            if (type == "submitter")
            {
                discussionList = (from ticketDiscussion in _dBtsics.TicketDiscussion
                                  from ticket in _dBtsics.Ticket.Where(w => w.TicketId == ticketDiscussion.TicketId).DefaultIfEmpty()
                                  where ticket.Status == 3 && ticket.Submiter == userId && ticketDiscussion.UserId == userId
                                  select new
                                  {
                                      idTicketDiscussion = ticketDiscussion.TicketDiscussionId,
                                      discussionDate = ticketDiscussion.CreatedAt,
                                      prevDataDiscussion = _dBtsics.TicketDiscussion.Where(w => w.TicketId == ticket.TicketId && w.CreatedAt < ticketDiscussion.CreatedAt).OrderByDescending(obd => obd.TicketDiscussionId).FirstOrDefault(),
                                      ticketCreated = ticket.CreatedAt,
                                      totalRowTicketRating = _dBtsics.Rating.Count(w => w.TicketId == ticket.TicketId),
                                      lastRowRating = _dBtsics.Rating.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(obd => obd.UserRatingId).FirstOrDefault()
                                  });
            }


            var listDiscussionCount = (from listDiscussion in discussionList
                                       where listDiscussion.totalRowTicketRating == 2 && listDiscussion.lastRowRating.CreatedAt.Month == 08 && listDiscussion.lastRowRating.CreatedAt.Year == year
                                       select new
                                       {
                                           listDiscussion.idTicketDiscussion,
                                           listDiscussion.discussionDate,
                                           listDiscussion.prevDataDiscussion,
                                           listDiscussion.ticketCreated,
                                           responseDay = (listDiscussion.prevDataDiscussion != null) ? DbFunctions.DiffDays(listDiscussion.discussionDate.Value, listDiscussion.prevDataDiscussion.CreatedAt.Value) : DbFunctions.DiffDays(listDiscussion.discussionDate.Value, listDiscussion.ticketCreated.Value),
                                           isNotMoreThanThree = (((listDiscussion.prevDataDiscussion != null) ? DbFunctions.DiffDays(listDiscussion.discussionDate.Value, listDiscussion.prevDataDiscussion.CreatedAt.Value) : DbFunctions.DiffDays(listDiscussion.discussionDate.Value, listDiscussion.ticketCreated.Value)) <= 3) ? 1 : 0
                                       }
                              ).ToList();

            var avarage = new decimal();
            var totalRow = listDiscussionCount.Count;

            foreach (var item in listDiscussionCount)
            {
                avarage = avarage + item.isNotMoreThanThree;
            }

            if (avarage != 0)
            {
                avarage = (avarage / totalRow) * 100 / 20;
            }

            return avarage;
        }
        public decimal GetValuePercentageYearlySep(int userId, string type, int year)
        {


            var discussionList = (from ticketDiscussion in _dBtsics.TicketDiscussion
                                  from ticket in _dBtsics.Ticket.Where(w => w.TicketId == ticketDiscussion.TicketId).DefaultIfEmpty()
                                  where ticket.Status == 3 && ticket.Responder == userId && ticketDiscussion.UserId == userId
                                  select new
                                  {
                                      idTicketDiscussion = ticketDiscussion.TicketDiscussionId,
                                      discussionDate = ticketDiscussion.CreatedAt,
                                      prevDataDiscussion = _dBtsics.TicketDiscussion.Where(w => w.TicketId == ticket.TicketId && w.CreatedAt < ticketDiscussion.CreatedAt).OrderByDescending(obd => obd.TicketDiscussionId).FirstOrDefault(),
                                      ticketCreated = ticket.CreatedAt,
                                      totalRowTicketRating = _dBtsics.Rating.Count(w => w.TicketId == ticket.TicketId),
                                      lastRowRating = _dBtsics.Rating.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(obd => obd.UserRatingId).FirstOrDefault()
                                  });

            if (type == "submitter")
            {
                discussionList = (from ticketDiscussion in _dBtsics.TicketDiscussion
                                  from ticket in _dBtsics.Ticket.Where(w => w.TicketId == ticketDiscussion.TicketId).DefaultIfEmpty()
                                  where ticket.Status == 3 && ticket.Submiter == userId && ticketDiscussion.UserId == userId
                                  select new
                                  {
                                      idTicketDiscussion = ticketDiscussion.TicketDiscussionId,
                                      discussionDate = ticketDiscussion.CreatedAt,
                                      prevDataDiscussion = _dBtsics.TicketDiscussion.Where(w => w.TicketId == ticket.TicketId && w.CreatedAt < ticketDiscussion.CreatedAt).OrderByDescending(obd => obd.TicketDiscussionId).FirstOrDefault(),
                                      ticketCreated = ticket.CreatedAt,
                                      totalRowTicketRating = _dBtsics.Rating.Count(w => w.TicketId == ticket.TicketId),
                                      lastRowRating = _dBtsics.Rating.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(obd => obd.UserRatingId).FirstOrDefault()
                                  });
            }


            var listDiscussionCount = (from listDiscussion in discussionList
                                       where listDiscussion.totalRowTicketRating == 2 && listDiscussion.lastRowRating.CreatedAt.Month == 09 && listDiscussion.lastRowRating.CreatedAt.Year == year
                                       select new
                                       {
                                           listDiscussion.idTicketDiscussion,
                                           listDiscussion.discussionDate,
                                           listDiscussion.prevDataDiscussion,
                                           listDiscussion.ticketCreated,
                                           responseDay = (listDiscussion.prevDataDiscussion != null) ? DbFunctions.DiffDays(listDiscussion.discussionDate.Value, listDiscussion.prevDataDiscussion.CreatedAt.Value) : DbFunctions.DiffDays(listDiscussion.discussionDate.Value, listDiscussion.ticketCreated.Value),
                                           isNotMoreThanThree = (((listDiscussion.prevDataDiscussion != null) ? DbFunctions.DiffDays(listDiscussion.discussionDate.Value, listDiscussion.prevDataDiscussion.CreatedAt.Value) : DbFunctions.DiffDays(listDiscussion.discussionDate.Value, listDiscussion.ticketCreated.Value)) <= 3) ? 1 : 0
                                       }
                              ).ToList();

            var avarage = new decimal();
            var totalRow = listDiscussionCount.Count;

            foreach (var item in listDiscussionCount)
            {
                avarage = avarage + item.isNotMoreThanThree;
            }

            if (avarage != 0)
            {
                avarage = (avarage / totalRow) * 100 / 20;
            }

            return avarage;
        }
        public decimal GetValuePercentageYearlyOkt(int userId, string type, int year)
        {


            var discussionList = (from ticketDiscussion in _dBtsics.TicketDiscussion
                                  from ticket in _dBtsics.Ticket.Where(w => w.TicketId == ticketDiscussion.TicketId).DefaultIfEmpty()
                                  where ticket.Status == 3 && ticket.Responder == userId && ticketDiscussion.UserId == userId
                                  select new
                                  {
                                      idTicketDiscussion = ticketDiscussion.TicketDiscussionId,
                                      discussionDate = ticketDiscussion.CreatedAt,
                                      prevDataDiscussion = _dBtsics.TicketDiscussion.Where(w => w.TicketId == ticket.TicketId && w.CreatedAt < ticketDiscussion.CreatedAt).OrderByDescending(obd => obd.TicketDiscussionId).FirstOrDefault(),
                                      ticketCreated = ticket.CreatedAt,
                                      totalRowTicketRating = _dBtsics.Rating.Count(w => w.TicketId == ticket.TicketId),
                                      lastRowRating = _dBtsics.Rating.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(obd => obd.UserRatingId).FirstOrDefault()
                                  });

            if (type == "submitter")
            {
                discussionList = (from ticketDiscussion in _dBtsics.TicketDiscussion
                                  from ticket in _dBtsics.Ticket.Where(w => w.TicketId == ticketDiscussion.TicketId).DefaultIfEmpty()
                                  where ticket.Status == 3 && ticket.Submiter == userId && ticketDiscussion.UserId == userId
                                  select new
                                  {
                                      idTicketDiscussion = ticketDiscussion.TicketDiscussionId,
                                      discussionDate = ticketDiscussion.CreatedAt,
                                      prevDataDiscussion = _dBtsics.TicketDiscussion.Where(w => w.TicketId == ticket.TicketId && w.CreatedAt < ticketDiscussion.CreatedAt).OrderByDescending(obd => obd.TicketDiscussionId).FirstOrDefault(),
                                      ticketCreated = ticket.CreatedAt,
                                      totalRowTicketRating = _dBtsics.Rating.Count(w => w.TicketId == ticket.TicketId),
                                      lastRowRating = _dBtsics.Rating.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(obd => obd.UserRatingId).FirstOrDefault()
                                  });
            }


            var listDiscussionCount = (from listDiscussion in discussionList
                                       where listDiscussion.totalRowTicketRating == 2 && listDiscussion.lastRowRating.CreatedAt.Month == 10 && listDiscussion.lastRowRating.CreatedAt.Year == year
                                       select new
                                       {
                                           listDiscussion.idTicketDiscussion,
                                           listDiscussion.discussionDate,
                                           listDiscussion.prevDataDiscussion,
                                           listDiscussion.ticketCreated,
                                           responseDay = (listDiscussion.prevDataDiscussion != null) ? DbFunctions.DiffDays(listDiscussion.discussionDate.Value, listDiscussion.prevDataDiscussion.CreatedAt.Value) : DbFunctions.DiffDays(listDiscussion.discussionDate.Value, listDiscussion.ticketCreated.Value),
                                           isNotMoreThanThree = (((listDiscussion.prevDataDiscussion != null) ? DbFunctions.DiffDays(listDiscussion.discussionDate.Value, listDiscussion.prevDataDiscussion.CreatedAt.Value) : DbFunctions.DiffDays(listDiscussion.discussionDate.Value, listDiscussion.ticketCreated.Value)) <= 3) ? 1 : 0
                                       }
                              ).ToList();

            var avarage = new decimal();
            var totalRow = listDiscussionCount.Count;

            foreach (var item in listDiscussionCount)
            {
                avarage = avarage + item.isNotMoreThanThree;
            }

            if (avarage != 0)
            {
                avarage = (avarage / totalRow) * 100 / 20;
            }

            return avarage;
        }
        public decimal GetValuePercentageYearlyNov(int userId, string type, int year)
        {


            var discussionList = (from ticketDiscussion in _dBtsics.TicketDiscussion
                                  from ticket in _dBtsics.Ticket.Where(w => w.TicketId == ticketDiscussion.TicketId).DefaultIfEmpty()
                                  where ticket.Status == 3 && ticket.Responder == userId && ticketDiscussion.UserId == userId
                                  select new
                                  {
                                      idTicketDiscussion = ticketDiscussion.TicketDiscussionId,
                                      discussionDate = ticketDiscussion.CreatedAt,
                                      prevDataDiscussion = _dBtsics.TicketDiscussion.Where(w => w.TicketId == ticket.TicketId && w.CreatedAt < ticketDiscussion.CreatedAt).OrderByDescending(obd => obd.TicketDiscussionId).FirstOrDefault(),
                                      ticketCreated = ticket.CreatedAt,
                                      totalRowTicketRating = _dBtsics.Rating.Count(w => w.TicketId == ticket.TicketId),
                                      lastRowRating = _dBtsics.Rating.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(obd => obd.UserRatingId).FirstOrDefault()
                                  });

            if (type == "submitter")
            {
                discussionList = (from ticketDiscussion in _dBtsics.TicketDiscussion
                                  from ticket in _dBtsics.Ticket.Where(w => w.TicketId == ticketDiscussion.TicketId).DefaultIfEmpty()
                                  where ticket.Status == 3 && ticket.Submiter == userId && ticketDiscussion.UserId == userId
                                  select new
                                  {
                                      idTicketDiscussion = ticketDiscussion.TicketDiscussionId,
                                      discussionDate = ticketDiscussion.CreatedAt,
                                      prevDataDiscussion = _dBtsics.TicketDiscussion.Where(w => w.TicketId == ticket.TicketId && w.CreatedAt < ticketDiscussion.CreatedAt).OrderByDescending(obd => obd.TicketDiscussionId).FirstOrDefault(),
                                      ticketCreated = ticket.CreatedAt,
                                      totalRowTicketRating = _dBtsics.Rating.Count(w => w.TicketId == ticket.TicketId),
                                      lastRowRating = _dBtsics.Rating.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(obd => obd.UserRatingId).FirstOrDefault()
                                  });
            }


            var listDiscussionCount = (from listDiscussion in discussionList
                                       where listDiscussion.totalRowTicketRating == 2 && listDiscussion.lastRowRating.CreatedAt.Month == 11 && listDiscussion.lastRowRating.CreatedAt.Year == year
                                       select new
                                       {
                                           listDiscussion.idTicketDiscussion,
                                           listDiscussion.discussionDate,
                                           listDiscussion.prevDataDiscussion,
                                           listDiscussion.ticketCreated,
                                           responseDay = (listDiscussion.prevDataDiscussion != null) ? DbFunctions.DiffDays(listDiscussion.discussionDate.Value, listDiscussion.prevDataDiscussion.CreatedAt.Value) : DbFunctions.DiffDays(listDiscussion.discussionDate.Value, listDiscussion.ticketCreated.Value),
                                           isNotMoreThanThree = (((listDiscussion.prevDataDiscussion != null) ? DbFunctions.DiffDays(listDiscussion.discussionDate.Value, listDiscussion.prevDataDiscussion.CreatedAt.Value) : DbFunctions.DiffDays(listDiscussion.discussionDate.Value, listDiscussion.ticketCreated.Value)) <= 3) ? 1 : 0
                                       }
                              ).ToList();

            var avarage = new decimal();
            var totalRow = listDiscussionCount.Count;

            foreach (var item in listDiscussionCount)
            {
                avarage = avarage + item.isNotMoreThanThree;
            }

            if (avarage != 0)
            {
                avarage = (avarage / totalRow) * 100 / 20;
            }

            return avarage;
        }
        public decimal GetValuePercentageYearlyDes(int userId, string type, int year)
        {


            var discussionList = (from ticketDiscussion in _dBtsics.TicketDiscussion
                                  from ticket in _dBtsics.Ticket.Where(w => w.TicketId == ticketDiscussion.TicketId).DefaultIfEmpty()
                                  where ticket.Status == 3 && ticket.Responder == userId && ticketDiscussion.UserId == userId
                                  select new
                                  {
                                      idTicketDiscussion = ticketDiscussion.TicketDiscussionId,
                                      discussionDate = ticketDiscussion.CreatedAt,
                                      prevDataDiscussion = _dBtsics.TicketDiscussion.Where(w => w.TicketId == ticket.TicketId && w.CreatedAt < ticketDiscussion.CreatedAt).OrderByDescending(obd => obd.TicketDiscussionId).FirstOrDefault(),
                                      ticketCreated = ticket.CreatedAt,
                                      totalRowTicketRating = _dBtsics.Rating.Count(w => w.TicketId == ticket.TicketId),
                                      lastRowRating = _dBtsics.Rating.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(obd => obd.UserRatingId).FirstOrDefault()
                                  });

            if (type == "submitter")
            {
                discussionList = (from ticketDiscussion in _dBtsics.TicketDiscussion
                                  from ticket in _dBtsics.Ticket.Where(w => w.TicketId == ticketDiscussion.TicketId).DefaultIfEmpty()
                                  where ticket.Status == 3 && ticket.Submiter == userId && ticketDiscussion.UserId == userId
                                  select new
                                  {
                                      idTicketDiscussion = ticketDiscussion.TicketDiscussionId,
                                      discussionDate = ticketDiscussion.CreatedAt,
                                      prevDataDiscussion = _dBtsics.TicketDiscussion.Where(w => w.TicketId == ticket.TicketId && w.CreatedAt < ticketDiscussion.CreatedAt).OrderByDescending(obd => obd.TicketDiscussionId).FirstOrDefault(),
                                      ticketCreated = ticket.CreatedAt,
                                      totalRowTicketRating = _dBtsics.Rating.Count(w => w.TicketId == ticket.TicketId),
                                      lastRowRating = _dBtsics.Rating.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(obd => obd.UserRatingId).FirstOrDefault()
                                  });
            }


            var listDiscussionCount = (from listDiscussion in discussionList
                                       where listDiscussion.totalRowTicketRating == 2 && listDiscussion.lastRowRating.CreatedAt.Month == 12 && listDiscussion.lastRowRating.CreatedAt.Year == year
                                       select new
                                       {
                                           listDiscussion.idTicketDiscussion,
                                           listDiscussion.discussionDate,
                                           listDiscussion.prevDataDiscussion,
                                           listDiscussion.ticketCreated,
                                           responseDay = (listDiscussion.prevDataDiscussion != null) ? DbFunctions.DiffDays(listDiscussion.discussionDate.Value, listDiscussion.prevDataDiscussion.CreatedAt.Value) : DbFunctions.DiffDays(listDiscussion.discussionDate.Value, listDiscussion.ticketCreated.Value),
                                           isNotMoreThanThree = (((listDiscussion.prevDataDiscussion != null) ? DbFunctions.DiffDays(listDiscussion.discussionDate.Value, listDiscussion.prevDataDiscussion.CreatedAt.Value) : DbFunctions.DiffDays(listDiscussion.discussionDate.Value, listDiscussion.ticketCreated.Value)) <= 3) ? 1 : 0
                                       }
                              ).ToList();

            var avarage = new decimal();
            var totalRow = listDiscussionCount.Count;

            foreach (var item in listDiscussionCount)
            {
                avarage = avarage + item.isNotMoreThanThree;
            }

            if (avarage != 0)
            {
                avarage = (avarage / totalRow) * 100 / 20;
            }

            return avarage;
        }
        #endregion 


        public List<decimal> GetValuePercentageWeek1(int userId, string type, int month, int year)
        {


            var discussionList = (from ticketDiscussion in _dBtsics.TicketDiscussion
                                  from ticket in _dBtsics.Ticket.Where(w => w.TicketId == ticketDiscussion.TicketId).DefaultIfEmpty()
                                  where ticket.Status == 3 && ticket.Responder == userId && ticketDiscussion.UserId == userId
                                  select new
                                  {
                                      idTicketDiscussion = ticketDiscussion.TicketDiscussionId,
                                      discussionDate = ticketDiscussion.CreatedAt,
                                      prevDataDiscussion = _dBtsics.TicketDiscussion.Where(w => w.TicketId == ticket.TicketId && w.CreatedAt < ticketDiscussion.CreatedAt).OrderByDescending(obd => obd.TicketDiscussionId).FirstOrDefault(),
                                      ticketCreated = ticket.CreatedAt,
                                      totalRowTicketRating = _dBtsics.Rating.Count(w => w.TicketId == ticket.TicketId),
                                      lastRowRating = _dBtsics.Rating.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(obd => obd.UserRatingId).FirstOrDefault()
                                  });

            if (type == "submitter")
            {
                discussionList = (from ticketDiscussion in _dBtsics.TicketDiscussion
                                  from ticket in _dBtsics.Ticket.Where(w => w.TicketId == ticketDiscussion.TicketId).DefaultIfEmpty()
                                  where ticket.Status == 3 && ticket.Submiter == userId && ticketDiscussion.UserId == userId
                                  select new
                                  {
                                      idTicketDiscussion = ticketDiscussion.TicketDiscussionId,
                                      discussionDate = ticketDiscussion.CreatedAt,
                                      prevDataDiscussion = _dBtsics.TicketDiscussion.Where(w => w.TicketId == ticket.TicketId && w.CreatedAt < ticketDiscussion.CreatedAt).OrderByDescending(obd => obd.TicketDiscussionId).FirstOrDefault(),
                                      ticketCreated = ticket.CreatedAt,
                                      totalRowTicketRating = _dBtsics.Rating.Count(w => w.TicketId == ticket.TicketId),
                                      lastRowRating = _dBtsics.Rating.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(obd => obd.UserRatingId).FirstOrDefault()
                                  });
            }

            var listDiscussionCount = (from listDiscussion in discussionList
                                       where listDiscussion.totalRowTicketRating == 2 && listDiscussion.lastRowRating.CreatedAt.Month == month && listDiscussion.lastRowRating.CreatedAt.Year == year 
                                       select new
                                       {
                                           listDiscussion.idTicketDiscussion,
                                           listDiscussion.discussionDate,
                                           listDiscussion.prevDataDiscussion,
                                           listDiscussion.ticketCreated,
                                           lastRowRating = listDiscussion.lastRowRating.CreatedAt,
                                           responseDay = (listDiscussion.prevDataDiscussion != null) ? DbFunctions.DiffDays(listDiscussion.discussionDate.Value, listDiscussion.prevDataDiscussion.CreatedAt.Value) : DbFunctions.DiffDays(listDiscussion.discussionDate.Value, listDiscussion.ticketCreated.Value),
                                           isNotMoreThanThree = (((listDiscussion.prevDataDiscussion != null) ? DbFunctions.DiffDays(listDiscussion.discussionDate.Value, listDiscussion.prevDataDiscussion.CreatedAt.Value) : DbFunctions.DiffDays(listDiscussion.discussionDate.Value, listDiscussion.ticketCreated.Value)) <= 3) ? 1 : 0
                                       }
                              ).ToList();

            var valueWeek = (from item in listDiscussionCount
                                select new
                                {
                                    week = GetWeekNumberOfMonth(item.lastRowRating), item.isNotMoreThanThree
                                }).ToList();
            var getWeek1 = (from item in valueWeek
                            where item.week == 1
                            select new
                            {
                                item.isNotMoreThanThree
                            }).ToList();
            var getWeek2 = (from item in valueWeek
                            where item.week == 2
                            select new
                            {
                                item.isNotMoreThanThree
                            }).ToList();
            var getWeek3 = (from item in valueWeek
                            where item.week == 3
                            select new
                            {
                                item.isNotMoreThanThree
                            }).ToList();
            var getWeek4 = (from item in valueWeek
                            where item.week == 4
                            select new
                            {
                                item.isNotMoreThanThree
                            }).ToList();
            #region Week1
            var avarageWeek1 = new decimal();
            var totalRowWeek1 = getWeek1.Count;

            foreach (var item in getWeek1)
            {
                avarageWeek1 = avarageWeek1 + item.isNotMoreThanThree;
            }

            if (avarageWeek1 != 0)
            {
                avarageWeek1 = (avarageWeek1 / totalRowWeek1) * 100 / 20;
            }
            #endregion
            #region Week 2
            var avarageWeek2 = new decimal();
            var totalRowWeek2 = getWeek2.Count;

            foreach (var item in getWeek2)
            {
                avarageWeek2 = avarageWeek2 + item.isNotMoreThanThree;
            }

            if (avarageWeek2 != 0)
            {
                avarageWeek2 = (avarageWeek2 / totalRowWeek2) * 100 / 20;
            }
            #endregion
            #region Week 3
            var avarageWeek3 = new decimal();
            var totalRowWeek3 = getWeek1.Count;

            foreach (var item in getWeek3)
            {
                avarageWeek3 = avarageWeek3 + item.isNotMoreThanThree;
            }

            if (avarageWeek3 != 0)
            {
                avarageWeek3 = (avarageWeek3 / totalRowWeek3) * 100 / 20;
            }
            #endregion
            #region Week 4
            var avarageWeek4 = new decimal();
            var totalRowWeek4 = getWeek4.Count;

            foreach (var item in getWeek4)
            {
                avarageWeek4 = avarageWeek4 + item.isNotMoreThanThree;
            }

            if (avarageWeek4 != 0)
            {
                avarageWeek4 = (avarageWeek4 / totalRowWeek4) * 100 / 20;
            }
            #endregion

            var listWeek = new List<decimal> {avarageWeek1, avarageWeek2, avarageWeek3, avarageWeek4};

            return listWeek;
        }

        public int GetWeekNumberOfMonth(DateTime date)
        {
            date = date.Date;
            DateTime firstMonthDay = new DateTime(date.Year, date.Month, 1);
            DateTime firstMonthMonday = firstMonthDay.AddDays((DayOfWeek.Monday + 7 - firstMonthDay.DayOfWeek) % 7);
            if (firstMonthMonday > date)
            {
                firstMonthDay = firstMonthDay.AddMonths(-1);
                firstMonthMonday = firstMonthDay.AddDays((DayOfWeek.Monday + 7 - firstMonthDay.DayOfWeek) % 7);
            }
            var getWeek = (date - firstMonthMonday).Days / 7 + 1;
            return getWeek;
        }
    }
}
