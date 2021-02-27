using Com.Trakindo.Framework;
using Com.Trakindo.TSICS.Data.Context;
using Com.Trakindo.TSICS.Data.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;

namespace Com.Trakindo.TSICS.Business.Service
{
    public class TicketBusinessService
    {
        private readonly TsicsContext _ctx = Factory.Create<TsicsContext>("TsicsContext", ClassType.clsTypeDataContext);
        private readonly RatingBusinessService _ratingBusinessService = Factory.Create<RatingBusinessService>("Rating", ClassType.clsTypeBusinessService);
        private readonly TicketParcipantBusinessService _ticketParcipantBusinessService = Factory.Create<TicketParcipantBusinessService>("TicketParcipant", ClassType.clsTypeBusinessService);
        private readonly UserBusinessService _userBusinessService = Factory.Create<UserBusinessService>("User", ClassType.clsTypeBusinessService);

        public object Session { get; private set; }

        public List<Ticket> GetList(int perPage, int pageNum)
        {
            int offset = (perPage * pageNum) - perPage;
            List<Ticket> result = _ctx.Ticket
                .Where(ticket => ticket.Status != 5)
                .OrderByDescending(item => new[] { item.CreatedAt, item.UpdatedAt, item.LastReply, item.LastStatusDate}.Max())
                .ToList();
            return result.Skip(offset).Take(perPage).ToList();
        }
        public Ticket Add(Ticket ticket)
        {
            _ctx.Ticket.Add(ticket);
            _ctx.SaveChanges();
            return ticket;
        }
      
        public void DeleteSelectedTicket(String idList)
        {
            if (!String.IsNullOrWhiteSpace(idList))
            {
                using (TsicsContext db = new TsicsContext())
                {
                    db.Database.ExecuteSqlCommand("delete from Ticket where TicketId in (" + idList + ")");
                }
            }
        }

        public Ticket Edit(Ticket ticket)
        {
            _ctx.Entry(ticket).State = EntityState.Modified;
            _ctx.SaveChanges();
            return ticket;
        }
        public Ticket GetDetail(int id)
        {
            Ticket ticket = _ctx.Ticket.Find(id);
            return (ticket);
        }
        public Ticket GetDetailbyTicketNo(String TicketNo)
        {
            Ticket ticket = _ctx.Ticket.Where(w => w.TicketNo == TicketNo).FirstOrDefault();
            return ticket;
        }

        public Ticket GetByIdAsNoTracking(int id)
        {
            return _ctx.Ticket.AsNoTracking().FirstOrDefault(q => q.TicketId.Equals(id));
        }
        public List<Ticket> GetBySubmitter(int submitter, int perPage, int pageNum)
        {
            int offset = (perPage * pageNum) - perPage;
            
            var ticketByParticipant = from ticketParticipant in _ctx.TicketParcipant
                                      join ticket in _ctx.Ticket on ticketParticipant.TicketId equals ticket.TicketId
                                      where ticketParticipant.UserId == submitter && ticket.Status != 5
                                      select ticketParticipant;

            List<int> ticketIdList = new List<int>();

            {
                var ticketParcipants = ticketByParticipant.ToList();
                
                foreach (TicketParcipant participant in ticketParcipants)
                {
                    ticketIdList.Add(participant.TicketId);
                }
            }

            List<Ticket> ticketBySubmiterResponder = _ctx.Ticket
                .Where(q =>
                    q.Submiter == submitter && q.Status != 5 ||
                    q.Responder == submitter && q.Status != 5)
                .ToList();

            if (ticketBySubmiterResponder.Count > 0)
            {
                foreach (Ticket ticketItem in ticketBySubmiterResponder)
                {
                    ticketIdList.Add(ticketItem.TicketId);
                }
            }

            int[] ticketIdArray = ticketIdList.ToArray();
            int[] stat = { 2, 3, 4, 6 };
            var Ticket = _ctx.Ticket
                .Where(q => ticketIdArray.Contains(q.TicketId) && (stat.Contains(q.Status) || (q.Status.Equals(1) && q.Submiter.Equals(submitter))))
               .OrderByDescending(t => new[] { t.CreatedAt, t.LastReply, t.UpdatedAt, t.LastStatusDate }.Max())
                .ToList();
            return Ticket.Skip(offset).Take(perPage).ToList();
        }
        public void Delete(Ticket ticket)
        {
            if (ticket != null)
            {
                ticket.Status = 5;
                _ctx.Entry(ticket).State = EntityState.Modified;
            }
            _ctx.SaveChanges();
        }
        public string GetNewTicketNoByCategory(int idCategory)
        {
            // Get the last record by category
            Ticket ticket = _ctx.Ticket
                .Where(q => q.TicketCategoryId == idCategory)
                .OrderByDescending(a => a.TicketNo).FirstOrDefault();
            // Create DateTime obj
            DateTime dtObj = DateTime.Now;
            // Reformat DateTime
            string now = dtObj.ToString("yyyyMMdd");
            int counter = 0;
            if (ticket != null)
            {
                // Check if the day is same the append the counter
                // If not counter return to zero
                if (ticket.TicketNo.Substring(8, 2).Equals(dtObj.ToString("dd")))
                {
                    // Get counter (last 3 digit) value from TicketNo
                    counter += Convert.ToInt32(ticket.TicketNo.Substring(10));
                }
            }
            // Ser result prefix
            string prefix = "";
            switch (idCategory)
            {
                case 1:
                    prefix = "PH";
                    break;
                case 2:
                    prefix = "PT";
                    break;
                case 3:
                    prefix = "DM";
                    break;
                case 4:
                    prefix = "RF";
                    break;
                case 5:
                    prefix = "WR";
                    break;
                case 6:
                    prefix = "GW";
                    break;
                case 7:
                    prefix = "PW";
                    break;
                case 8:
                    prefix = "TM";
                    break;
                case 9:
                    prefix = "HD";
                    break;
                case 10:
                    prefix = "CM";
                    break;
            }
            string result = prefix + now;
            counter += 1;
            int counterLength = Convert.ToInt32(counter.ToString().Length.ToString());
            switch (counterLength)
            {
                case 1:
                    for (int i = 0; i <= counterLength; i++)
                    {
                        result += "0";
                    }
                    break;
                case 2:
                    for (int i = 0; i < counterLength - 1; i++)
                    {
                        result += "0";
                    }
                    break;
            }
            result += counter.ToString();
            return result;
        }
        public bool IsTicketExists(int ticketId)
        {
            return _ctx.Ticket.Count(ticket => ticket.TicketId == ticketId) > 0;
        }
       
        public IQueryable<Ticket> GetQueryableByUser(int userId)
        {
            return from ticket in _ctx.Ticket
                   where ticket.Status != 5
                   where ticket.Submiter == userId
                   select ticket;
        }
        public string GetCategoryName(string ticketNumber)
        {
            string categoryName = null;
            var ticket = _ctx.Ticket.FirstOrDefault(q => q.TicketNo.Equals(ticketNumber));
            if(ticket != null)
            {
                var categoryObj = _ctx.TicketCategory.Find(ticket.TicketCategoryId);
                if (categoryObj != null) categoryName = Regex.Replace(categoryObj.Name, @"\s", "");
            }

            return categoryName;
        }
        public Ticket GetByTicketNumber(string ticketNo)
        {
            return _ctx.Ticket.FirstOrDefault(q => q.TicketNo.Equals(ticketNo));
        }

        public List<Ticket> GetRecent()
        {
            return _ctx.Ticket.Where(q => q.Status != 5).OrderByDescending(q => q.CreatedAt).Take(5).ToList();
        }

        public IQueryable<Ticket> GetQueryableSummary()
        {
            int[] ids = { 2,3,4,6 };
            return from ticket in _ctx.Ticket
                   where ids.Contains(ticket.Status)
                   select ticket;
           
        }

        public List<Ticket> GetCronTicketSetRed()
        {
            DateTime dt = DateTime.Now.Date;
            List<Ticket> result = _ctx.Ticket
                .Where(q => DbFunctions.TruncateTime(q.DueDateAnswer) <= dt)
                .Where(f => f.SubmiterFlag == 0 && f.ResponderFlag==0)
                .Where(s => s.Status == 2)
                .ToList();

            return result;
        }

        public void SetTheNextCommenter(int ticketId, int userId)
        {
            var ticket = _ctx.Ticket.AsNoTracking().FirstOrDefault(q => q.TicketId == ticketId);
            if (ticket != null)
            {
                ticket.NextCommenter = userId;
                _ctx.Entry(ticket).State = EntityState.Modified;
            }

            _ctx.SaveChanges();
        }

        public void Close(int ticketId)
        {
            var ticket = _ctx.Ticket.Find(ticketId);
            if (ticket != null)
            {
                ticket.Status = 3;
                ticket.LastStatusDate = DateTime.Now;

                _ctx.Entry(ticket).State = EntityState.Modified;
            }

            _ctx.SaveChanges();
        }

        public void AddDppm(int ticketId, string dppmNo)
        {
            Ticket ticket = _ctx.Ticket.AsNoTracking().FirstOrDefault(t => t.TicketId.Equals(ticketId));
            if (ticket != null)
            {
                ticket.DPPMno = dppmNo;

                _ctx.Entry(ticket).State = EntityState.Modified;
                _ctx.SaveChanges();
            }
        }
        

        public List<Ticket> GetAnythingLike(string searchingString)
        {
            int status = 0;
            List<Ticket> result = new List<Ticket>();
            if (searchingString.ToLower().Contains("submit") || searchingString.ToLower().Contains("open"))
            {
                status = 2;
            }
            else if (searchingString.ToLower().Contains("close") || searchingString.ToLower().Contains("finish"))
            {
                status = 3;
            }
            else if (searchingString.ToLower().Contains("escalate"))
            {
                var ticketIdList = _ctx.EscalateLog.Select(e => e.TicketId).Distinct().ToList();

                if (ticketIdList.Count != 0)
                {
                    int[] ticketIdArray = ticketIdList.ToArray();

                    return _ctx.Ticket.Where(t => ticketIdArray.Contains(t.TicketId)).ToList();
                }

                return new List<Ticket>();
            }



            if (!status.Equals(0))
            {
                return _ctx.Ticket
                    .Where(t => t.Status.Equals(status))
                    .ToList();
            }
            var ticketIdListTag = _ctx.ArticleTag.Where(e => e.Name.Contains(searchingString)).Select(e => e.TicketId).ToList();
            int[] stat = { 2, 3, 4, 6 };
            var ticketGenericList = (from ticket in _ctx.Ticket
            join user in _ctx.User on ticket.Responder equals user.UserId
            where (stat.Contains(ticket.Status)) &&
            ticket.Title.Contains(searchingString) ||
                ticket.Description.Contains(searchingString) ||
                ticket.SerialNumber.Contains(searchingString) ||
                ticket.TicketNo.Contains(searchingString) ||
                ticket.Family.Contains(searchingString) ||
                ticket.Customer.Contains(searchingString) ||
                ticket.PartCausingFailure.Contains(searchingString) ||
                ticket.Model.Contains(searchingString) ||
                ticket.SMU.Contains(searchingString) ||
                ticket.ArrangementNo.Contains(searchingString) ||
                  ticket.DPPMno.Contains(searchingString) ||
                user.Name.Contains(searchingString)
            select new
            {
                TicketId = ticket.TicketId,
                TicketCategoryId = ticket.TicketCategoryId,
                TicketNo = ticket.TicketNo,
                Title = ticket.Title,
                Description = ticket.Description,
                Submiter = ticket.Submiter,
                Responder = ticket.Responder,
                SubmiterFlag = ticket.SubmiterFlag,
                ResponderFlag = ticket.ResponderFlag,
                SerialNumber = ticket.SerialNumber,
                Customer = ticket.Customer,
                Location = ticket.Location,
                Make = ticket.Make,
                DeliveryDate = ticket.DeliveryDate,
                ArrangementNo = ticket.ArrangementNo,
                Family = ticket.Family,
                Model = ticket.Model,
                SMU = ticket.SMU,
                PartCausingFailure = ticket.PartCausingFailure,
                PartsDescription = ticket.PartsDescription,
                EmailCC = ticket.EmailCC,
                Manufacture = ticket.Manufacture,
                PartsNumber = ticket.PartsNumber,
                ServiceToolSN = ticket.ServiceToolSN,
                EngineSN = ticket.EngineSN,
                EcmSN = ticket.EcmSN,
                TotalTattletale = ticket.TotalTattletale,
                ReasonCode = ticket.ReasonCode,
                DiagnosticClock = ticket.DiagnosticClock,
                Password = ticket.Password,
                ServiceOrderNumber = ticket.ServiceOrderNumber,
                ClaimNumber = ticket.ClaimNumber,
                InvoiceDate = ticket.InvoiceDate,
                CreatedAt = ticket.CreatedAt,
                UpdatedAt = ticket.UpdatedAt,
                Status = ticket.Status,
                WarrantyTypeId = ticket.WarrantyTypeId,
                MasterAreaId = ticket.MasterAreaId,
                MasterAreaName = ticket.MasterAreaName,
                MasterBranchId = ticket.MasterBranchId,
                MasterBranchName = ticket.MasterBranchName,
                NextCommenter = ticket.NextCommenter,
                DueDateAnswer = ticket.DueDateAnswer,
                ReferenceTicket = ticket.ReferenceTicket,
                DPPMno = ticket.DPPMno,
                MepBranch = ticket.MepBranch,
                SMUDate = ticket.SMUDate,
                SoftwarePartNumber = ticket.SoftwarePartNumber,
                FromInterlock = ticket.FromInterlock,
                ToInterlock = ticket.ToInterlock,
                TestSpec = ticket.TestSpec,
                TestSpecBrakeSaver = ticket.TestSpecBrakeSaver,
                DelegateId = ticket.DelegateId,
                EscalateId = ticket.EscalateId,
                LastReply = ticket.LastReply
            }).ToList();

            if (ticketGenericList != null)
            {
                foreach (var ticket in ticketGenericList)
                {
                    Ticket ticketModel = new Ticket()
                    {
                        TicketId = ticket.TicketId,
                        TicketCategoryId = ticket.TicketCategoryId,
                        TicketNo = ticket.TicketNo,
                        Title = ticket.Title,
                        Description = ticket.Description,
                        Submiter = ticket.Submiter,
                        Responder = ticket.Responder,
                        SubmiterFlag = ticket.SubmiterFlag,
                        ResponderFlag = ticket.ResponderFlag,
                        SerialNumber = ticket.SerialNumber,
                        Customer = ticket.Customer,
                        Location = ticket.Location,
                        Make = ticket.Make,
                        DeliveryDate = ticket.DeliveryDate,
                        ArrangementNo = ticket.ArrangementNo,
                        Family = ticket.Family,
                        Model = ticket.Model,
                        SMU = ticket.SMU,
                        PartCausingFailure = ticket.PartCausingFailure,
                        PartsDescription = ticket.PartsDescription,
                        EmailCC = ticket.EmailCC,
                        Manufacture = ticket.Manufacture,
                        PartsNumber = ticket.PartsNumber,
                        ServiceToolSN = ticket.ServiceToolSN,
                        EngineSN = ticket.EngineSN,
                        EcmSN = ticket.EcmSN,
                        TotalTattletale = ticket.TotalTattletale,
                        ReasonCode = ticket.ReasonCode,
                        DiagnosticClock = ticket.DiagnosticClock,
                        Password = ticket.Password,
                        ServiceOrderNumber = ticket.ServiceOrderNumber,
                        ClaimNumber = ticket.ClaimNumber,
                        InvoiceDate = ticket.InvoiceDate,
                        CreatedAt = ticket.CreatedAt,
                        UpdatedAt = ticket.UpdatedAt,
                        Status = ticket.Status,
                        WarrantyTypeId = ticket.WarrantyTypeId,
                        MasterAreaId = ticket.MasterAreaId,
                        MasterAreaName = ticket.MasterAreaName,
                        MasterBranchId = ticket.MasterBranchId,
                        MasterBranchName = ticket.MasterBranchName,
                        NextCommenter = ticket.NextCommenter,
                        DueDateAnswer = ticket.DueDateAnswer,
                        ReferenceTicket = ticket.ReferenceTicket,
                        DPPMno = ticket.DPPMno,
                        MepBranch = ticket.MepBranch,
                        SMUDate = ticket.SMUDate,
                        SoftwarePartNumber = ticket.SoftwarePartNumber,
                        FromInterlock = ticket.FromInterlock,
                        ToInterlock = ticket.ToInterlock,
                        TestSpec = ticket.TestSpec,
                        TestSpecBrakeSaver = ticket.TestSpecBrakeSaver,
                        DelegateId = ticket.DelegateId,
                        EscalateId = ticket.EscalateId,
                        LastReply = ticket.LastReply
                    };

                    result.Add(ticketModel);
                }
            }

            return result;

        }
        
        public List<Ticket> GetMyTr(string searchingString, int userId)
        {
            List<Ticket> result = new List<Ticket>();
            int status = 0;

            if (searchingString.ToLower().Contains("submit") || searchingString.ToLower().Contains("open"))
            {
                status = 2;
            }
            else if (searchingString.ToLower().Contains("close") || searchingString.ToLower().Contains("finish"))
            {
                status = 3;
            }
            else if (searchingString.ToLower().Contains("escalate"))
            {
                var ticketIdList = _ctx.EscalateLog.Where(q => q.EscalateFrom == userId).Select(e => e.TicketId).Distinct().ToList();

                if (ticketIdList.Count != 0)
                {
                    int[] ticketIdArray = ticketIdList.ToArray();

                    return _ctx.Ticket.Where(t => ticketIdArray.Contains(t.TicketId)).ToList();
                }

                return new List<Ticket>();
            }

            var idTicketParticipant = _ctx.TicketParcipant
                .Where(d => d.UserId == userId)
                .Select(s => s)
                .GroupBy(g => new { g.TicketId })
                .Select(t => t.Key.TicketId).ToArray();

            if (!status.Equals(0))
            {
                return _ctx.Ticket
                    .Where(t => t.Status.Equals(status))
                    .Where(t =>
                        t.Submiter.Equals(userId) || t.Responder.Equals(userId) || idTicketParticipant.Contains(t.TicketId)
                    )
                    .ToList();
            }
            var ticketIdListTag = _ctx.ArticleTag.Where(e => e.Name.Contains(searchingString)).Select(e => e.TicketId).ToList();
            var ticketGenericList = (from ticket in _ctx.Ticket
                                     join user in _ctx.User on ticket.Responder equals user.UserId
                                     where (ticket.Status != 5) &&
                                     ticket.Title.Contains(searchingString) && ticket.Submiter == userId ||
                                     ticket.Title.Contains(searchingString) && ticket.Responder == userId ||
                                     ticket.Description.Contains(searchingString) && ticket.Submiter == userId ||
                                     ticket.Description.Contains(searchingString) && ticket.Responder == userId ||
                                     ticket.SerialNumber.Contains(searchingString) && ticket.Submiter == userId ||
                                     ticket.SerialNumber.Contains(searchingString) && ticket.Responder == userId ||
                                     ticket.TicketNo.Contains(searchingString) && ticket.Submiter == userId ||
                                     ticket.TicketNo.Contains(searchingString) && ticket.Responder == userId ||
                                     ticket.Family.Contains(searchingString) && ticket.Submiter == userId ||
                                     ticket.Family.Contains(searchingString) && ticket.Responder == userId ||
                                     ticket.Customer.Contains(searchingString) && ticket.Submiter == userId ||
                                     ticket.Customer.Contains(searchingString) && ticket.Responder == userId ||
                                     ticket.PartCausingFailure.Contains(searchingString) && ticket.Submiter == userId ||
                                     ticket.PartCausingFailure.Contains(searchingString) && ticket.Responder == userId ||
                                     ticket.Model.Contains(searchingString) && ticket.Submiter == userId ||
                                     ticket.Model.Contains(searchingString) && ticket.Responder == userId ||
                                     ticket.SMU.Contains(searchingString) && ticket.Submiter == userId ||
                                     ticket.SMU.Contains(searchingString) && ticket.Responder == userId ||
                                     ticket.ArrangementNo.Contains(searchingString) && ticket.Submiter == userId ||
                                     ticket.ArrangementNo.Contains(searchingString) && ticket.Responder == userId ||
                                     ticket.DPPMno.Contains(searchingString) ||
                                     user.Name.Contains(searchingString) && ticket.Submiter == userId ||
                                     user.Name.Contains(searchingString) && ticket.Responder == userId
                                     select new
                                     {
                                         TicketId = ticket.TicketId,
                                         TicketCategoryId = ticket.TicketCategoryId,
                                         TicketNo = ticket.TicketNo,
                                         Title = ticket.Title,
                                         Description = ticket.Description,
                                         Submiter = ticket.Submiter,
                                         Responder = ticket.Responder,
                                         SubmiterFlag = ticket.SubmiterFlag,
                                         ResponderFlag = ticket.ResponderFlag,
                                         SerialNumber = ticket.SerialNumber,
                                         Customer = ticket.Customer,
                                         Location = ticket.Location,
                                         Make = ticket.Make,
                                         DeliveryDate = ticket.DeliveryDate,
                                         ArrangementNo = ticket.ArrangementNo,
                                         Family = ticket.Family,
                                         Model = ticket.Model,
                                         SMU = ticket.SMU,
                                         PartCausingFailure = ticket.PartCausingFailure,
                                         PartsDescription = ticket.PartsDescription,
                                         EmailCC = ticket.EmailCC,
                                         Manufacture = ticket.Manufacture,
                                         PartsNumber = ticket.PartsNumber,
                                         ServiceToolSN = ticket.ServiceToolSN,
                                         EngineSN = ticket.EngineSN,
                                         EcmSN = ticket.EcmSN,
                                         TotalTattletale = ticket.TotalTattletale,
                                         ReasonCode = ticket.ReasonCode,
                                         DiagnosticClock = ticket.DiagnosticClock,
                                         Password = ticket.Password,
                                         ServiceOrderNumber = ticket.ServiceOrderNumber,
                                         ClaimNumber = ticket.ClaimNumber,
                                         InvoiceDate = ticket.InvoiceDate,
                                         CreatedAt = ticket.CreatedAt,
                                         UpdatedAt = ticket.UpdatedAt,
                                         Status = ticket.Status,
                                         WarrantyTypeId = ticket.WarrantyTypeId,
                                         MasterAreaId = ticket.MasterAreaId,
                                         MasterAreaName = ticket.MasterAreaName,
                                         MasterBranchId = ticket.MasterBranchId,
                                         MasterBranchName = ticket.MasterBranchName,
                                         NextCommenter = ticket.NextCommenter,
                                         DueDateAnswer = ticket.DueDateAnswer,
                                         ReferenceTicket = ticket.ReferenceTicket,
                                         DPPMno = ticket.DPPMno,
                                         MepBranch = ticket.MepBranch,
                                         SMUDate = ticket.SMUDate,
                                         SoftwarePartNumber = ticket.SoftwarePartNumber,
                                         FromInterlock = ticket.FromInterlock,
                                         ToInterlock = ticket.ToInterlock,
                                         TestSpec = ticket.TestSpec,
                                         TestSpecBrakeSaver = ticket.TestSpecBrakeSaver,
                                         DelegateId = ticket.DelegateId,
                                         EscalateId = ticket.EscalateId,
                                         LastReply = ticket.LastReply
                                     }).ToList();

            if (ticketGenericList != null)
            {
                foreach (var ticket in ticketGenericList)
                {
                    Ticket ticketModel = new Ticket()
                    {
                        TicketId = ticket.TicketId,
                        TicketCategoryId = ticket.TicketCategoryId,
                        TicketNo = ticket.TicketNo,
                        Title = ticket.Title,
                        Description = ticket.Description,
                        Submiter = ticket.Submiter,
                        Responder = ticket.Responder,
                        SubmiterFlag = ticket.SubmiterFlag,
                        ResponderFlag = ticket.ResponderFlag,
                        SerialNumber = ticket.SerialNumber,
                        Customer = ticket.Customer,
                        Location = ticket.Location,
                        Make = ticket.Make,
                        DeliveryDate = ticket.DeliveryDate,
                        ArrangementNo = ticket.ArrangementNo,
                        Family = ticket.Family,
                        Model = ticket.Model,
                        SMU = ticket.SMU,
                        PartCausingFailure = ticket.PartCausingFailure,
                        PartsDescription = ticket.PartsDescription,
                        EmailCC = ticket.EmailCC,
                        Manufacture = ticket.Manufacture,
                        PartsNumber = ticket.PartsNumber,
                        ServiceToolSN = ticket.ServiceToolSN,
                        EngineSN = ticket.EngineSN,
                        EcmSN = ticket.EcmSN,
                        TotalTattletale = ticket.TotalTattletale,
                        ReasonCode = ticket.ReasonCode,
                        DiagnosticClock = ticket.DiagnosticClock,
                        Password = ticket.Password,
                        ServiceOrderNumber = ticket.ServiceOrderNumber,
                        ClaimNumber = ticket.ClaimNumber,
                        InvoiceDate = ticket.InvoiceDate,
                        CreatedAt = ticket.CreatedAt,
                        UpdatedAt = ticket.UpdatedAt,
                        Status = ticket.Status,
                        WarrantyTypeId = ticket.WarrantyTypeId,
                        MasterAreaId = ticket.MasterAreaId,
                        MasterAreaName = ticket.MasterAreaName,
                        MasterBranchId = ticket.MasterBranchId,
                        MasterBranchName = ticket.MasterBranchName,
                        NextCommenter = ticket.NextCommenter,
                        DueDateAnswer = ticket.DueDateAnswer,
                        ReferenceTicket = ticket.ReferenceTicket,
                        DPPMno = ticket.DPPMno,
                        MepBranch = ticket.MepBranch,
                        SMUDate = ticket.SMUDate,
                        SoftwarePartNumber = ticket.SoftwarePartNumber,
                        FromInterlock = ticket.FromInterlock,
                        ToInterlock = ticket.ToInterlock,
                        TestSpec = ticket.TestSpec,
                        TestSpecBrakeSaver = ticket.TestSpecBrakeSaver,
                        DelegateId = ticket.DelegateId,
                        EscalateId = ticket.EscalateId,
                        LastReply = ticket.LastReply
                    };

                    result.Add(ticketModel);
                }
            }
            return result;
        }
        
        public void RequestToClose(int ticketId)
        {
            var ticket = _ctx.Ticket.Find(ticketId);
            if(ticket != null)
            {
                ticket.LastReply = DateTime.Now;
                ticket.LastStatusDate = ticket.LastReply;
                ticket.Status = 6;
                _ctx.Entry(ticket).State = EntityState.Modified;
                _ctx.SaveChanges();
            }
        }
        public IQueryable<Ticket> GetQueryableAllTr()
        {
            return _ctx.Ticket
                .Where(ticket => ticket.Status != 1 && ticket.Status != 5);
        }
        public IQueryable<Ticket> GetQueryableByUserConected(int userId)
        {
            int [] Stat = { 2,3,4,6 };
            var ticketByParticipant = from ticketParticipant in _ctx.TicketParcipant
                    join ticket in _ctx.Ticket on ticketParticipant.TicketId equals ticket.TicketId
                    where ticketParticipant.UserId == userId && (Stat.Contains(ticket.Status) || (ticket.Status == 1 && ticket.Submiter == userId))
                    select ticketParticipant;

            List < int > ticketIdList = new List<int>();

            {
                var ticketParcipants = ticketByParticipant.ToList();

                foreach (TicketParcipant participant in ticketParcipants)
                {
                    ticketIdList.Add(participant.TicketId);
                }
            }

            List<Ticket> ticketBySubmiterResponder = _ctx.Ticket
                .Where(q =>
                    q.Submiter == userId && q.Status != 5 ||
                    q.Responder == userId && q.Status != 5)
                .ToList();

            if(ticketBySubmiterResponder.Count > 0)
            {
                foreach(Ticket ticketItem in ticketBySubmiterResponder)
                {
                    ticketIdList.Add(ticketItem.TicketId);
                }
            }

            int[] ticketIdArray = ticketIdList.ToArray();

            
            return _ctx.Ticket
                .Where(q => ticketIdArray.Contains(q.TicketId));
        }

        public IQueryable<Ticket> GetHelpDeskList()
        {

            return _ctx.Ticket
             .Where(ticket => ticket.TicketCategoryId == 9 && ticket.Status != 1 && ticket.Status != 5)
                  .OrderBy(ticket => ticket.CreatedAt);
            
        }

        public List<string> AutoClose(int maxRateBackDay)
        {
            var tickets = _ctx.Ticket.Where(q => q.Status == 6).ToList();
            DateTime dateTimeNow = new DateTime();
            List<string> ticketClosed = new List<string>();
            foreach (var ticket in tickets)
            {
                Rating responderRating = new Rating();
                if(ticket.Responder != 0)
                {
                    responderRating = _ratingBusinessService.GetRatingFromResponder(ticket.TicketId, ticket.Responder);
                }
                else // Help Desk (responder is admin)
                {
                    List<User> adminList = _userBusinessService.GetListAdmin();
                    foreach(var admin in adminList)
                    {
                        var adminRating = _ratingBusinessService.GetRatingFromResponder(ticket.TicketId, admin.UserId);
                        if(adminRating != null)
                        {
                            responderRating = adminRating;
                        }
                    }
                }
                var submiterRating = _ratingBusinessService.GetRatingFromSubmiter(ticket.TicketId, ticket.Submiter);
                TimeSpan respond = ticket.LastReply == null ? (dateTimeNow.Subtract(ticket.CreatedAt.Value))/*TotalSeconds*/ : (dateTimeNow.Subtract(ticket.LastReply.Value))/*TotalSeconds*/;
                if (responderRating != null && submiterRating == null && (DateTime.Now - responderRating.CreatedAt).Days > maxRateBackDay - 1)
                {
                    Rating rating = new Rating()
                    {
                        UserId = ticket.Responder,
                        TicketId = ticket.TicketId,
                        Rate = 5,
                        RatedFrom = ticket.Submiter,
                        CreatedAt = DateTime.Now,
                        RespondTime = (respond.Days < 1 ? "" : respond.Days.ToString() + ". ") + respond.Hours.ToString() + ":" + respond.Minutes.ToString() + ":" + respond.Seconds.ToString()
                    };

                    _ratingBusinessService.Add(rating);
                    Close(ticket.TicketId);
                    ticketClosed.Add(ticket.TicketNo);
                }
                else if (responderRating != null && submiterRating != null && (DateTime.Now - responderRating.CreatedAt).Days > maxRateBackDay - 1)
                {
                    Close(ticket.TicketId);
                    ticketClosed.Add(ticket.TicketNo);
                }
                
            }

            return ticketClosed;
        }
    
        public Ticket Escalate(int ticketId, int currentResponder, int newResponder)
        {
            var ticket = GetDetail(ticketId);

            if (ticket != null)
            {
                _ticketParcipantBusinessService.CheckExistingParticipants(ticketId, newResponder);

                TicketParcipant ticketParticipant = new TicketParcipant()
                {
                    TicketId = ticketId,
                    UserId = currentResponder,
                    CreatedAt = DateTime.Now,
                    Status = 1
                };

                _ticketParcipantBusinessService.Add(ticketParticipant);

                if (ticket.NextCommenter == ticket.Responder)
                {
                    ticket.NextCommenter = newResponder;
                }
                ticket.Responder = newResponder;
                ticket.LastStatusDate = DateTime.Now;
                var updateResult = Edit(ticket);

                EscalateLog escalateLog = new EscalateLog()
                {
                    EscalateFrom = currentResponder,
                    EscalateTo = newResponder,
                    TicketId = updateResult.TicketId,
                    CreatedAt = DateTime.Now
                };

                _ctx.EscalateLog.Add(escalateLog);
                _ctx.SaveChanges();
            }

            return ticket;
        }

        public bool IsEscalated(int ticketId, int logedInUserId)
        {
            return _ctx.EscalateLog.Count(q => q.TicketId == ticketId && q.EscalateFrom == logedInUserId) > 0;
        }

        public EscalateLog getLastTimeEscalated(int ticketId)
        {
            return _ctx.EscalateLog.OrderByDescending(e => e.CreatedAt).FirstOrDefault(e => e.TicketId.Equals(ticketId));
        }
        public int [] getlistIdEscalatedbyTicket(int ticketId)
        {
            return _ctx.EscalateLog.OrderBy(e => e.CreatedAt).Where(e=> e.TicketId.Equals(ticketId)).Take(3).Select(e => e.IdEscalateLog).ToArray();
        }
        public EscalateLog getEscalatedDetail(int id)
        {
            return _ctx.EscalateLog.FirstOrDefault(e => e.IdEscalateLog.Equals(id));
        }
        public List<Ticket> GetTicketDashboard(int userid)
        {
            List<Ticket> result = _ctx.Ticket
                .Where(ticket => ticket.Submiter == userid && ticket.Status == 3)
                .ToList();
            return result;
        }
        #region Monthly
        public decimal GetCountTicketDashboardMonthSubmitter(int userid,string type, int m, int y)
        {
            var listRating = (from rating in _ctx.Rating
                                    from ticket in _ctx.Ticket.Where(w => w.TicketId == rating.TicketId).DefaultIfEmpty()
                              where ticket.Status == 3 && ticket.Responder == userid && rating.UserId == userid
                                    select new
                                    {
                                        rate = rating.Rate,
                                        totRowRating = _ctx.Rating.Count(w => w.TicketId == ticket.TicketId),
                                        lastRowRating = _ctx.Rating.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(odb => odb.UserRatingId).FirstOrDefault()
                                    });
            if(type == "submitter")
            {
                listRating = (from rating in _ctx.Rating
                              from ticket in _ctx.Ticket.Where(w => w.TicketId == rating.TicketId).DefaultIfEmpty()
                              where ticket.Status == 3 && ticket.Submiter == userid && rating.UserId == userid
                              select new
                              {
                                  rate = rating.Rate,
                                  totRowRating = _ctx.Rating.Count(w => w.TicketId == ticket.TicketId),
                                  lastRowRating = _ctx.Rating.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(odb => odb.UserRatingId).FirstOrDefault()
                              });
            }
            var listRatingCount = (from ratingList in listRating
                                            where ratingList.totRowRating == 2 && ratingList.lastRowRating.CreatedAt.Month == m && ratingList.lastRowRating.CreatedAt.Year == y
                                            select new
                                            {
                                                ratingList.rate
                                            }).ToList();

            var average = new decimal();
            var totRow = listRatingCount.Count;
            foreach (var item in listRatingCount)
            {
                average = average + item.rate;
            }

            if(average > 0)
            {
                average = average / totRow;
            }

            return Math.Round(average, 2);
        }

        public List<decimal> GetCountTicketDashboardMonthSubmitterGetWeek(int userid, string type, int m, int y)
        {
            var listRating = (from rating in _ctx.Rating
                              from ticket in _ctx.Ticket.Where(w => w.TicketId == rating.TicketId).DefaultIfEmpty()
                              where ticket.Status == 3 && ticket.Responder == userid && rating.UserId == userid
                              select new
                              {
                                  rate = rating.Rate,
                                  totRowRating = _ctx.Rating.Count(w => w.TicketId == ticket.TicketId),
                                  lastRowRating = _ctx.Rating.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(odb => odb.UserRatingId).FirstOrDefault()
                              });
            if (type == "submitter")
            {
                listRating = (from rating in _ctx.Rating
                              from ticket in _ctx.Ticket.Where(w => w.TicketId == rating.TicketId).DefaultIfEmpty()
                              where ticket.Status == 3 && ticket.Submiter == userid && rating.UserId == userid
                              select new
                              {
                                  rate = rating.Rate,
                                  totRowRating = _ctx.Rating.Count(w => w.TicketId == ticket.TicketId),
                                  lastRowRating = _ctx.Rating.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(odb => odb.UserRatingId).FirstOrDefault()
                              });
            }
            var listRatingCount = (from ratingList in listRating
                                   where ratingList.totRowRating == 2 && ratingList.lastRowRating.CreatedAt.Month == m && ratingList.lastRowRating.CreatedAt.Year == y
                                   select new
                                   {
                                       ratingList.rate,
                                       lastRowRating = ratingList.lastRowRating.CreatedAt
                                   }).ToList();

            var validWeek = (from item in listRatingCount
                             select new
                             {
                                 item.rate,
                                 week = GetWeekNumberOfMonth(item.lastRowRating)
                             }).ToList();

            var getDataWeek = (from item in validWeek
                               where item.week == 1
                               select new
                               {
                                   item.rate
                               }).ToList();
            var getDataWeek2 = (from item in validWeek
                               where item.week == 2
                               select new
                               {
                                   item.rate
                               }).ToList();
            var getDataWeek3 = (from item in validWeek
                               where item.week == 3
                               select new
                               {
                                   item.rate
                               }).ToList();
            var getDataWeek4 = (from item in validWeek
                               where item.week == 4
                               select new
                               {
                                   item.rate
                               }).ToList();
            #region  Week1
            var averageWeek1 = new decimal();
            var countDataByWeek1 = getDataWeek.Count;
            foreach (var item in getDataWeek)
            {
                averageWeek1 = averageWeek1 + item.rate;
            }

            if (countDataByWeek1 > 0)
            {
                averageWeek1 = averageWeek1 / countDataByWeek1;
                averageWeek1 = Math.Round(averageWeek1, 2);
            }
            else if (countDataByWeek1 == 0)
            {
                averageWeek1 = 0;
            }
            #endregion
            #region  Week2
            var averageWeek2 = new decimal();
            var countDataByWeek2 = getDataWeek2.Count;
            foreach (var item in listRatingCount)
            {
                averageWeek2 = averageWeek2 + item.rate;

            }

            if (countDataByWeek2 > 0)
            {
                averageWeek2 = averageWeek2 / countDataByWeek2;
                averageWeek2 = Math.Round(averageWeek2, 2);
            }
            else if (countDataByWeek2 == 0)
            {
                averageWeek2 = 0;
            }
            #endregion
            #region  Week3
            var averageWeek3 = new decimal();
            var countDataByWeek3 = getDataWeek3.Count;
            foreach (var item in listRatingCount)
            {
                averageWeek3 = averageWeek3 + item.rate;
            }

            if (countDataByWeek3 > 0)
            {
                averageWeek3 = averageWeek3 / countDataByWeek3;
                averageWeek3 = Math.Round(averageWeek3, 2);
            }
            else if (countDataByWeek3 == 0)
            {
                averageWeek3 = 0;
            }
            #endregion
            #region  Week4
            var averageWeek4 = new decimal();
            var countDataByWeek4 = getDataWeek4.Count;
            foreach (var item in listRatingCount)
            {
                averageWeek4 = averageWeek4 + item.rate;
                averageWeek4 = Math.Round(averageWeek4, 2);
            }

            if (countDataByWeek4 > 0)
            {
                averageWeek4 = averageWeek4 / countDataByWeek4;
            }
            else if (countDataByWeek4 == 0)
            {
                averageWeek4 = 0;
            }
            #endregion

            var listDataByWeek = new List<decimal>
            {
                averageWeek1, averageWeek2, averageWeek3, averageWeek4
            };

            return listDataByWeek;
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

        public int GetCountTrMonthResponder(int userId, int month)
        {
            var getTicket = (from rating in _ctx.Rating
                             from ticket in _ctx.Ticket.Where(w => w.TicketId == rating.TicketId).DefaultIfEmpty()
                             where ticket.Status == 3 && ticket.Responder == userId && rating.UserId == userId
                             select new
                             {
                                 rate = rating.Rate,
                                 totRowRating = _ctx.Rating.Count(w => w.TicketId == ticket.TicketId),
                                 lastRowRating = _ctx.Rating.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(odb => odb.UserRatingId).FirstOrDefault()
                             });

            var ticketCloseCount = (from listTicket in getTicket
                                    where listTicket.totRowRating == 2 && listTicket.lastRowRating.CreatedAt.Month == month
                                    select new
                                    {
                                        listTicket.rate
                                    }).ToList();
            var countTicketCloseMonth = ticketCloseCount.Count;
            if(countTicketCloseMonth > 0)
            {
                return countTicketCloseMonth;
            }
            else
            {
                return 0;
            }
        }
        public int GetCountTrMonthSubmitter(int userId, int month)
        {
            var getTicket = (from rating in _ctx.Rating
                             from ticket in _ctx.Ticket.Where(w => w.TicketId == rating.TicketId).DefaultIfEmpty()
                             where ticket.Status == 3 && ticket.Submiter == userId && rating.UserId == userId
                             select new
                             {
                                 rate = rating.Rate,
                                 totRowRating = _ctx.Rating.Count(w => w.TicketId == ticket.TicketId),
                                 lastRowRating = _ctx.Rating.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(odb => odb.UserRatingId).FirstOrDefault()
                             });

            var ticketCloseCount = (from listTicket in getTicket
                                    where listTicket.totRowRating == 2 && listTicket.lastRowRating.CreatedAt.Month == month
                                    select new
                                    {
                                        listTicket.rate
                                    }).ToList();
            var countTicketCloseMonth = ticketCloseCount.Count;
            if (countTicketCloseMonth > 0)
            {
                return countTicketCloseMonth;
            }
            else
            {
                return 0;
            }
        }

        public decimal GetValueTurnAroundMonthLessThanTree(int userId, string type, int month, int year)
        {
            var getLastTicketClose = (from ticket in _ctx.Ticket
                                      from rating in _ctx.Rating.Where(w => w.TicketId == ticket.TicketId).DefaultIfEmpty()
                                      where ticket.Status == 3 && ticket.Submiter == userId && rating.UserId == userId || ticket.Status == 3 && ticket.Responder == userId && rating.UserId == userId
                                      select new
                                      {
                                          createdTicket = ticket.CreatedAt.Value,
                                          lastRating = _ctx.Rating.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(o => o.UserRatingId).FirstOrDefault(),
                                          totalRowTicketRating = _ctx.Rating.Count(w => w.TicketId == ticket.TicketId),
                                      });

            var getLastTicketCloseRebuild = (from data in getLastTicketClose
                                             select new
                                             {
                                                 ticketCreate = data.createdTicket,
                                                 lastRatingTicket = data.lastRating,
                                                 data.totalRowTicketRating,
                                                 getValueCloseTicket = DbFunctions.DiffDays(data.createdTicket,data.lastRating.CreatedAt)
                                             });


            var countTicketClose = (from listTicket in getLastTicketCloseRebuild
                                    where listTicket.totalRowTicketRating == 2 && listTicket.lastRatingTicket.CreatedAt.Month == month && listTicket.lastRatingTicket.CreatedAt.Year == year && listTicket.getValueCloseTicket < 3
                                    select new
                                    {
                                        ticketCreate = listTicket.lastRatingTicket                                    
                                    }).ToList();

            var countTicket = countTicketClose.Count;
            return countTicket;
        }
        public decimal GetValueTurnAroundMonthLessThanFourteen(int userId, string type, int month, int year)
        {
            var getLastTicketClose = (from ticket in _ctx.Ticket
                                      from rating in _ctx.Rating.Where(w => w.TicketId == ticket.TicketId).DefaultIfEmpty()
                                      where ticket.Status == 3 && ticket.Submiter == userId && rating.UserId == userId || ticket.Status == 3 && ticket.Responder == userId && rating.UserId == userId
                                      select new
                                      {
                                          createdTicket = ticket.CreatedAt.Value,
                                          lastRating = _ctx.Rating.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(o => o.UserRatingId).FirstOrDefault(),
                                          totalRowTicketRating = _ctx.Rating.Count(w => w.TicketId == ticket.TicketId),
                                      });

            var getLastTicketCloseRebuild = (from data in getLastTicketClose
                                             select new
                                             {
                                                 ticketCreate = data.createdTicket,
                                                 lastRatingTicket = data.lastRating,
                                                 data.totalRowTicketRating,
                                                 getValueCloseTicket = DbFunctions.DiffDays(data.createdTicket, data.lastRating.CreatedAt)
                                             });

            var countTicketClose = (from listTicket in getLastTicketCloseRebuild
                                    where listTicket.totalRowTicketRating == 2 && listTicket.lastRatingTicket.CreatedAt.Month == month && listTicket.lastRatingTicket.CreatedAt.Year == year && listTicket.getValueCloseTicket >=3 && listTicket.getValueCloseTicket < 14
                                    select new
                                    {
                                        ticketCreate = listTicket.lastRatingTicket                                    
                                    }).ToList();

            var countTicket = countTicketClose.Count;
            return countTicket;
        }
        public decimal GetValueTurnAroundMonthLessThanFourthyTwo(int userId, string type, int month, int year)
        {
            var getLastTicketClose = (from ticket in _ctx.Ticket
                                      from rating in _ctx.Rating.Where(w => w.TicketId == ticket.TicketId).DefaultIfEmpty()
                                      where ticket.Status == 3 && ticket.Submiter == userId && rating.UserId == userId || ticket.Status == 3 && ticket.Responder == userId && rating.UserId == userId
                                      select new
                                      {
                                          createdTicket = ticket.CreatedAt.Value,
                                          lastRating = _ctx.Rating.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(o => o.UserRatingId).FirstOrDefault(),
                                          totalRowTicketRating = _ctx.Rating.Count(w => w.TicketId == ticket.TicketId),
                                      });

            var getLastTicketCloseRebuild = (from data in getLastTicketClose
                                             select new
                                             {
                                                 ticketCreate = data.createdTicket,
                                                 lastRatingTicket = data.lastRating,
                                                 data.totalRowTicketRating,
                                                 getValueCloseTicket = DbFunctions.DiffDays(data.createdTicket, data.lastRating.CreatedAt)
                                             });

            var countTicketClose = (from listTicket in getLastTicketCloseRebuild
                                    where listTicket.totalRowTicketRating == 2 && listTicket.lastRatingTicket.CreatedAt.Month == month && listTicket.lastRatingTicket.CreatedAt.Year == year && listTicket.getValueCloseTicket >= 14 && listTicket.getValueCloseTicket < 42
                                    select new
                                    {
                                        ticketCreate = listTicket.lastRatingTicket
                                    }).ToList();

            var countTicket = countTicketClose.Count;
            return countTicket;
        }
        public decimal GetValueTurnAroundMonthLessThanEightyFour(int userId, string type, int month, int year)
        {
            var getLastTicketClose = (from ticket in _ctx.Ticket
                                      from rating in _ctx.Rating.Where(w => w.TicketId == ticket.TicketId).DefaultIfEmpty()
                                      where ticket.Status == 3 && ticket.Submiter == userId && rating.UserId == userId || ticket.Status == 3 && ticket.Responder == userId && rating.UserId == userId
                                      select new
                                      {
                                          createdTicket = ticket.CreatedAt.Value,
                                          lastRating = _ctx.Rating.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(o => o.UserRatingId).FirstOrDefault(),
                                          totalRowTicketRating = _ctx.Rating.Count(w => w.TicketId == ticket.TicketId),
                                      });

            var getLastTicketCloseRebuild = (from data in getLastTicketClose
                                             select new
                                             {
                                                 ticketCreate = data.createdTicket,
                                                 lastRatingTicket = data.lastRating,
                                                 data.totalRowTicketRating,
                                                 getValueCloseTicket = DbFunctions.DiffDays(data.createdTicket, data.lastRating.CreatedAt)
                                             });

            var countTicketClose = (from listTicket in getLastTicketCloseRebuild
                                    where listTicket.totalRowTicketRating == 2 && listTicket.lastRatingTicket.CreatedAt.Month == month && listTicket.lastRatingTicket.CreatedAt.Year == year && listTicket.getValueCloseTicket >= 42 && listTicket.getValueCloseTicket < 84
                                    select new
                                    {
                                        ticketCreate = listTicket.lastRatingTicket
                                    }).ToList();

            var countTicket = countTicketClose.Count;
            return countTicket;
        }
        public decimal GetValueTurnAroundMonthLessThanTwoQuarter(int userId, string type, int month, int year)
        {
            var getLastTicketClose = (from ticket in _ctx.Ticket
                                      from rating in _ctx.Rating.Where(w => w.TicketId == ticket.TicketId).DefaultIfEmpty()
                                      where ticket.Status == 3 && ticket.Submiter == userId && rating.UserId == userId || ticket.Status == 3 && ticket.Responder == userId && rating.UserId == userId
                                      select new
                                      {
                                          createdTicket = ticket.CreatedAt.Value,
                                          lastRating = _ctx.Rating.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(o => o.UserRatingId).FirstOrDefault(),
                                          totalRowTicketRating = _ctx.Rating.Count(w => w.TicketId == ticket.TicketId),
                                          getDataCloseDate = DbFunctions.DiffDays(ticket.CreatedAt.Value, rating.CreatedAt),
                                      });

            var getLastTicketCloseRebuild = (from data in getLastTicketClose
                                             select new
                                             {
                                                 ticketCreate = data.createdTicket,
                                                 lastRatingTicket = data.lastRating,
                                                 data.totalRowTicketRating,
                                                 getValueCloseTicket = DbFunctions.DiffDays(data.createdTicket, data.lastRating.CreatedAt)
                                             });

            var countTicketClose = (from listTicket in getLastTicketCloseRebuild
                                    where listTicket.totalRowTicketRating == 2 && listTicket.lastRatingTicket.CreatedAt.Month == month && listTicket.lastRatingTicket.CreatedAt.Year == year && listTicket.getValueCloseTicket >= 84 && listTicket.getValueCloseTicket < 180
                                    select new
                                    {
                                        ticketCreate = listTicket.lastRatingTicket
                                    }).ToList();

            var countTicket = countTicketClose.Count;
            return countTicket;
        }
        public decimal GetValueTurnAroundMonthMoreThanTwoQuarter(int userId, string type, int month, int year)
        {
            var getLastTicketClose = (from ticket in _ctx.Ticket
                                      from rating in _ctx.Rating.Where(w => w.TicketId == ticket.TicketId).DefaultIfEmpty()
                                      where ticket.Status == 3 && ticket.Submiter == userId && rating.UserId == userId || ticket.Status == 3 && ticket.Responder == userId && rating.UserId == userId
                                      select new
                                      {
                                          createdTicket = ticket.CreatedAt.Value,
                                          lastRating = _ctx.Rating.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(o => o.UserRatingId).FirstOrDefault(),
                                          totalRowTicketRating = _ctx.Rating.Count(w => w.TicketId == ticket.TicketId),
                                          getDataCloseDate = DbFunctions.DiffDays(ticket.CreatedAt.Value, rating.CreatedAt),
                                      });

            var getLastTicketCloseRebuild = (from data in getLastTicketClose
                                             select new
                                             {
                                                 ticketCreate = data.createdTicket,
                                                 lastRatingTicket = data.lastRating,
                                                 data.totalRowTicketRating,
                                                 getValueCloseTicket = DbFunctions.DiffDays(data.createdTicket, data.lastRating.CreatedAt)
                                             });

            var countTicketClose = (from listTicket in getLastTicketCloseRebuild
                                    where listTicket.totalRowTicketRating == 2 && listTicket.lastRatingTicket.CreatedAt.Month == month && listTicket.lastRatingTicket.CreatedAt.Year == year && listTicket.getValueCloseTicket > 180
                                    select new
                                    {
                                        ticketCreate = listTicket.lastRatingTicket
                                    }).ToList();

            var countTicket = countTicketClose.Count;
            return countTicket;
        }
        #endregion

        #region Yearly
        public decimal GetCountTicketDashboardYearly(int userid, string type, int y)
        {
            var listRating = (from rating in _ctx.Rating
                              from ticket in _ctx.Ticket.Where(w => w.TicketId == rating.TicketId).DefaultIfEmpty()
                              where ticket.Status == 3 && ticket.Responder == userid && rating.UserId == userid
                              select new
                              {
                                  rate = rating.Rate,
                                  totRowRating = _ctx.Rating.Count(w => w.TicketId == ticket.TicketId),
                                  lastRowRating = _ctx.Rating.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(odb => odb.UserRatingId).FirstOrDefault()
                              });
            if (type == "submitter")
            {
                listRating = (from rating in _ctx.Rating
                              from ticket in _ctx.Ticket.Where(w => w.TicketId == rating.TicketId).DefaultIfEmpty()
                              where ticket.Status == 3 && ticket.Submiter == userid && rating.UserId == userid
                              select new
                              {
                                  rate = rating.Rate,
                                  totRowRating = _ctx.Rating.Count(w => w.TicketId == ticket.TicketId),
                                  lastRowRating = _ctx.Rating.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(odb => odb.UserRatingId).FirstOrDefault()
                              });
            }
            var listRatingCount = (from ratingList in listRating
                                   where ratingList.totRowRating == 2 && ratingList.lastRowRating.CreatedAt.Year == y
                                   select new
                                   {
                                       ratingList.rate
                                   }).ToList();

            var average = new decimal();
            var totRow = listRatingCount.Count;
            foreach (var item in listRatingCount)
            {
                average = average + item.rate;
            }

            if (average > 0)
            {
                average = average / totRow;
            }

            return Math.Round(average, 2);
        } 

        public decimal GetCountTicketDashboardYearlyJan(int userid, string type, int y)
        {
            var listRating = (from rating in _ctx.Rating
                              from ticket in _ctx.Ticket.Where(w => w.TicketId == rating.TicketId).DefaultIfEmpty()
                              where ticket.Status == 3 && ticket.Responder == userid && rating.UserId == userid
                              select new
                              {
                                  rate = rating.Rate,
                                  totRowRating = _ctx.Rating.Count(w => w.TicketId == ticket.TicketId),
                                  lastRowRating = _ctx.Rating.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(odb => odb.UserRatingId).FirstOrDefault()
                              });
            if (type == "submitter")
            {
                listRating = (from rating in _ctx.Rating
                              from ticket in _ctx.Ticket.Where(w => w.TicketId == rating.TicketId).DefaultIfEmpty()
                              where ticket.Status == 3 && ticket.Submiter == userid && rating.UserId == userid
                              select new
                              {
                                  rate = rating.Rate,
                                  totRowRating = _ctx.Rating.Count(w => w.TicketId == ticket.TicketId),
                                  lastRowRating = _ctx.Rating.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(odb => odb.UserRatingId).FirstOrDefault()
                              });
            }
            var listRatingCount = (from ratingList in listRating
                                   where ratingList.totRowRating == 2 && ratingList.lastRowRating.CreatedAt.Month == 01
                                   select new
                                   {
                                       ratingList.rate
                                   }).ToList();

            var average = new decimal();
            var totRow = listRatingCount.Count;
            foreach (var item in listRatingCount)
            {
                average = average + item.rate;
            }

            if (average > 0)
            {
                average = average / totRow;
            }

            return Math.Round(average, 2);
        }
        public decimal GetCountTicketDashboardYearlyFeb(int userid, string type, int y)
        {
            var listRating = (from rating in _ctx.Rating
                              from ticket in _ctx.Ticket.Where(w => w.TicketId == rating.TicketId).DefaultIfEmpty()
                              where ticket.Status == 3 && ticket.Responder == userid && rating.UserId == userid
                              select new
                              {
                                  rate = rating.Rate,
                                  totRowRating = _ctx.Rating.Count(w => w.TicketId == ticket.TicketId),
                                  lastRowRating = _ctx.Rating.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(odb => odb.UserRatingId).FirstOrDefault()
                              });
            if (type == "submitter")
            {
                listRating = (from rating in _ctx.Rating
                              from ticket in _ctx.Ticket.Where(w => w.TicketId == rating.TicketId).DefaultIfEmpty()
                              where ticket.Status == 3 && ticket.Submiter == userid && rating.UserId == userid
                              select new
                              {
                                  rate = rating.Rate,
                                  totRowRating = _ctx.Rating.Count(w => w.TicketId == ticket.TicketId),
                                  lastRowRating = _ctx.Rating.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(odb => odb.UserRatingId).FirstOrDefault()
                              });
            }
            var listRatingCount = (from ratingList in listRating
                                   where ratingList.totRowRating == 2 && ratingList.lastRowRating.CreatedAt.Month == 02
                                   select new
                                   {
                                       ratingList.rate
                                   }).ToList();

            var average = new decimal();
            var totRow = listRatingCount.Count;
            foreach (var item in listRatingCount)
            {
                average = average + item.rate;
            }

            if (average > 0)
            {
                average = average / totRow;
            }

            return Math.Round(average, 2);
        }
        public decimal GetCountTicketDashboardYearlyMar(int userid, string type, int y)
        {
            var listRating = (from rating in _ctx.Rating
                              from ticket in _ctx.Ticket.Where(w => w.TicketId == rating.TicketId).DefaultIfEmpty()
                              where ticket.Status == 3 && ticket.Responder == userid && rating.UserId == userid
                              select new
                              {
                                  rate = rating.Rate,
                                  totRowRating = _ctx.Rating.Count(w => w.TicketId == ticket.TicketId),
                                  lastRowRating = _ctx.Rating.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(odb => odb.UserRatingId).FirstOrDefault()
                              });
            if (type == "submitter")
            {
                listRating = (from rating in _ctx.Rating
                              from ticket in _ctx.Ticket.Where(w => w.TicketId == rating.TicketId).DefaultIfEmpty()
                              where ticket.Status == 3 && ticket.Submiter == userid && rating.UserId == userid
                              select new
                              {
                                  rate = rating.Rate,
                                  totRowRating = _ctx.Rating.Count(w => w.TicketId == ticket.TicketId),
                                  lastRowRating = _ctx.Rating.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(odb => odb.UserRatingId).FirstOrDefault()
                              });
            }
            var listRatingCount = (from ratingList in listRating
                                   where ratingList.totRowRating == 2 && ratingList.lastRowRating.CreatedAt.Month == 03
                                   select new
                                   {
                                       ratingList.rate
                                   }).ToList();

            var average = new decimal();
            var totRow = listRatingCount.Count;
            foreach (var item in listRatingCount)
            {
                average = average + item.rate;
            }

            if (average > 0)
            {
                average = average / totRow;
            }

            return Math.Round(average, 2);
        }
        public decimal GetCountTicketDashboardYearlyApr(int userid, string type, int y)
        {
            var listRating = (from rating in _ctx.Rating
                              from ticket in _ctx.Ticket.Where(w => w.TicketId == rating.TicketId).DefaultIfEmpty()
                              where ticket.Status == 3 && ticket.Responder == userid && rating.UserId == userid
                              select new
                              {
                                  rate = rating.Rate,
                                  totRowRating = _ctx.Rating.Count(w => w.TicketId == ticket.TicketId),
                                  lastRowRating = _ctx.Rating.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(odb => odb.UserRatingId).FirstOrDefault()
                              });
            if (type == "submitter")
            {
                listRating = (from rating in _ctx.Rating
                              from ticket in _ctx.Ticket.Where(w => w.TicketId == rating.TicketId).DefaultIfEmpty()
                              where ticket.Status == 3 && ticket.Submiter == userid && rating.UserId == userid
                              select new
                              {
                                  rate = rating.Rate,
                                  totRowRating = _ctx.Rating.Count(w => w.TicketId == ticket.TicketId),
                                  lastRowRating = _ctx.Rating.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(odb => odb.UserRatingId).FirstOrDefault()
                              });
            }
            var listRatingCount = (from ratingList in listRating
                                   where ratingList.totRowRating == 2 && ratingList.lastRowRating.CreatedAt.Month == 04
                                   select new
                                   {
                                       ratingList.rate
                                   }).ToList();

            var average = new decimal();
            var totRow = listRatingCount.Count;
            foreach (var item in listRatingCount)
            {
                average = average + item.rate;
            }

            if (average > 0)
            {
                average = average / totRow;
            }

            return Math.Round(average, 2);
        }
        public decimal GetCountTicketDashboardYearlyMei(int userid, string type, int y)
        {
            var listRating = (from rating in _ctx.Rating
                              from ticket in _ctx.Ticket.Where(w => w.TicketId == rating.TicketId).DefaultIfEmpty()
                              where ticket.Status == 3 && ticket.Responder == userid && rating.UserId == userid
                              select new
                              {
                                  rate = rating.Rate,
                                  totRowRating = _ctx.Rating.Count(w => w.TicketId == ticket.TicketId),
                                  lastRowRating = _ctx.Rating.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(odb => odb.UserRatingId).FirstOrDefault()
                              });
            if (type == "submitter")
            {
                listRating = (from rating in _ctx.Rating
                              from ticket in _ctx.Ticket.Where(w => w.TicketId == rating.TicketId).DefaultIfEmpty()
                              where ticket.Status == 3 && ticket.Submiter == userid && rating.UserId == userid
                              select new
                              {
                                  rate = rating.Rate,
                                  totRowRating = _ctx.Rating.Count(w => w.TicketId == ticket.TicketId),
                                  lastRowRating = _ctx.Rating.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(odb => odb.UserRatingId).FirstOrDefault()
                              });
            }
            var listRatingCount = (from ratingList in listRating
                                   where ratingList.totRowRating == 2 && ratingList.lastRowRating.CreatedAt.Month == 05
                                   select new
                                   {
                                       ratingList.rate
                                   }).ToList();

            var average = new decimal();
            var totRow = listRatingCount.Count;
            foreach (var item in listRatingCount)
            {
                average = average + item.rate;
            }

            if (average > 0)
            {
                average = average / totRow;
            }

            return Math.Round(average, 2);
        }
        public decimal GetCountTicketDashboardYearlyJun(int userid, string type, int y)
        {
            var listRating = (from rating in _ctx.Rating
                              from ticket in _ctx.Ticket.Where(w => w.TicketId == rating.TicketId).DefaultIfEmpty()
                              where ticket.Status == 3 && ticket.Responder == userid && rating.UserId == userid
                              select new
                              {
                                  rate = rating.Rate,
                                  totRowRating = _ctx.Rating.Count(w => w.TicketId == ticket.TicketId),
                                  lastRowRating = _ctx.Rating.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(odb => odb.UserRatingId).FirstOrDefault()
                              });
            if (type == "submitter")
            {
                listRating = (from rating in _ctx.Rating
                              from ticket in _ctx.Ticket.Where(w => w.TicketId == rating.TicketId).DefaultIfEmpty()
                              where ticket.Status == 3 && ticket.Submiter == userid && rating.UserId == userid
                              select new
                              {
                                  rate = rating.Rate,
                                  totRowRating = _ctx.Rating.Count(w => w.TicketId == ticket.TicketId),
                                  lastRowRating = _ctx.Rating.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(odb => odb.UserRatingId).FirstOrDefault()
                              });
            }
            var listRatingCount = (from ratingList in listRating
                                   where ratingList.totRowRating == 2 && ratingList.lastRowRating.CreatedAt.Month == 06
                                   select new
                                   {
                                       ratingList.rate
                                   }).ToList();

            var average = new decimal();
            var totRow = listRatingCount.Count;
            foreach (var item in listRatingCount)
            {
                average = average + item.rate;
            }

            if (average > 0)
            {
                average = average / totRow;
            }

            return Math.Round(average, 2);
        }
        public decimal GetCountTicketDashboardYearlyJul(int userid, string type, int y)
        {
            var listRating = (from rating in _ctx.Rating
                              from ticket in _ctx.Ticket.Where(w => w.TicketId == rating.TicketId).DefaultIfEmpty()
                              where ticket.Status == 3 && ticket.Responder == userid && rating.UserId == userid
                              select new
                              {
                                  rate = rating.Rate,
                                  totRowRating = _ctx.Rating.Count(w => w.TicketId == ticket.TicketId),
                                  lastRowRating = _ctx.Rating.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(odb => odb.UserRatingId).FirstOrDefault()
                              });
            if (type == "submitter")
            {
                listRating = (from rating in _ctx.Rating
                              from ticket in _ctx.Ticket.Where(w => w.TicketId == rating.TicketId).DefaultIfEmpty()
                              where ticket.Status == 3 && ticket.Submiter == userid && rating.UserId == userid
                              select new
                              {
                                  rate = rating.Rate,
                                  totRowRating = _ctx.Rating.Count(w => w.TicketId == ticket.TicketId),
                                  lastRowRating = _ctx.Rating.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(odb => odb.UserRatingId).FirstOrDefault()
                              });
            }
            var listRatingCount = (from ratingList in listRating
                                   where ratingList.totRowRating == 2 && ratingList.lastRowRating.CreatedAt.Month == 07
                                   select new
                                   {
                                       ratingList.rate
                                   }).ToList();

            var average = new decimal();
            var totRow = listRatingCount.Count;
            foreach (var item in listRatingCount)
            {
                average = average + item.rate;
            }

            if (average > 0)
            {
                average = average / totRow;
            }

            return Math.Round(average, 2);
        }
        public decimal GetCountTicketDashboardYearlyAgu(int userid, string type, int y)
        {
            var listRating = (from rating in _ctx.Rating
                              from ticket in _ctx.Ticket.Where(w => w.TicketId == rating.TicketId).DefaultIfEmpty()
                              where ticket.Status == 3 && ticket.Responder == userid && rating.UserId == userid
                              select new
                              {
                                  rate = rating.Rate,
                                  totRowRating = _ctx.Rating.Count(w => w.TicketId == ticket.TicketId),
                                  lastRowRating = _ctx.Rating.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(odb => odb.UserRatingId).FirstOrDefault()
                              });
            if (type == "submitter")
            {
                listRating = (from rating in _ctx.Rating
                              from ticket in _ctx.Ticket.Where(w => w.TicketId == rating.TicketId).DefaultIfEmpty()
                              where ticket.Status == 3 && ticket.Submiter == userid && rating.UserId == userid
                              select new
                              {
                                  rate = rating.Rate,
                                  totRowRating = _ctx.Rating.Count(w => w.TicketId == ticket.TicketId),
                                  lastRowRating = _ctx.Rating.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(odb => odb.UserRatingId).FirstOrDefault()
                              });
            }
            var listRatingCount = (from ratingList in listRating
                                   where ratingList.totRowRating == 2 && ratingList.lastRowRating.CreatedAt.Month == 08
                                   select new
                                   {
                                       ratingList.rate
                                   }).ToList();

            var average = new decimal();
            var totRow = listRatingCount.Count;
            foreach (var item in listRatingCount)
            {
                average = average + item.rate;
            }

            if (average > 0)
            {
                average = average / totRow;
            }

            return Math.Round(average, 2);
        }
        public decimal GetCountTicketDashboardYearlySep(int userid, string type, int y)
        {
            var listRating = (from rating in _ctx.Rating
                              from ticket in _ctx.Ticket.Where(w => w.TicketId == rating.TicketId).DefaultIfEmpty()
                              where ticket.Status == 3 && ticket.Responder == userid && rating.UserId == userid
                              select new
                              {
                                  rate = rating.Rate,
                                  totRowRating = _ctx.Rating.Count(w => w.TicketId == ticket.TicketId),
                                  lastRowRating = _ctx.Rating.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(odb => odb.UserRatingId).FirstOrDefault()
                              });
            if (type == "submitter")
            {
                listRating = (from rating in _ctx.Rating
                              from ticket in _ctx.Ticket.Where(w => w.TicketId == rating.TicketId).DefaultIfEmpty()
                              where ticket.Status == 3 && ticket.Submiter == userid && rating.UserId == userid
                              select new
                              {
                                  rate = rating.Rate,
                                  totRowRating = _ctx.Rating.Count(w => w.TicketId == ticket.TicketId),
                                  lastRowRating = _ctx.Rating.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(odb => odb.UserRatingId).FirstOrDefault()
                              });
            }
            var listRatingCount = (from ratingList in listRating
                                   where ratingList.totRowRating == 2 && ratingList.lastRowRating.CreatedAt.Month == 09
                                   select new
                                   {
                                       ratingList.rate
                                   }).ToList();

            var average = new decimal();
            var totRow = listRatingCount.Count;
            foreach (var item in listRatingCount)
            {
                average = average + item.rate;
            }

            if (average > 0)
            {
                average = average / totRow;
            }

            return Math.Round(average, 2);
        }
        public decimal GetCountTicketDashboardYearlyOkt(int userid, string type, int y)
        {
            var listRating = (from rating in _ctx.Rating
                              from ticket in _ctx.Ticket.Where(w => w.TicketId == rating.TicketId).DefaultIfEmpty()
                              where ticket.Status == 3 && ticket.Responder == userid && rating.UserId == userid
                              select new
                              {
                                  rate = rating.Rate,
                                  totRowRating = _ctx.Rating.Count(w => w.TicketId == ticket.TicketId),
                                  lastRowRating = _ctx.Rating.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(odb => odb.UserRatingId).FirstOrDefault()
                              });
            if (type == "submitter")
            {
                listRating = (from rating in _ctx.Rating
                              from ticket in _ctx.Ticket.Where(w => w.TicketId == rating.TicketId).DefaultIfEmpty()
                              where ticket.Status == 3 && ticket.Submiter == userid && rating.UserId == userid
                              select new
                              {
                                  rate = rating.Rate,
                                  totRowRating = _ctx.Rating.Count(w => w.TicketId == ticket.TicketId),
                                  lastRowRating = _ctx.Rating.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(odb => odb.UserRatingId).FirstOrDefault()
                              });
            }
            var listRatingCount = (from ratingList in listRating
                                   where ratingList.totRowRating == 2 && ratingList.lastRowRating.CreatedAt.Month == 10
                                   select new
                                   {
                                       ratingList.rate
                                   }).ToList();

            var average = new decimal();
            var totRow = listRatingCount.Count;
            foreach (var item in listRatingCount)
            {
                average = average + item.rate;
            }

            if (average > 0)
            {
                average = average / totRow;
            }

            return Math.Round(average, 2);
        }
        public decimal GetCountTicketDashboardYearlyNov(int userid, string type, int y)
        {
            var listRating = (from rating in _ctx.Rating
                              from ticket in _ctx.Ticket.Where(w => w.TicketId == rating.TicketId).DefaultIfEmpty()
                              where ticket.Status == 3 && ticket.Responder == userid && rating.UserId == userid
                              select new
                              {
                                  rate = rating.Rate,
                                  totRowRating = _ctx.Rating.Count(w => w.TicketId == ticket.TicketId),
                                  lastRowRating = _ctx.Rating.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(odb => odb.UserRatingId).FirstOrDefault()
                              });
            if (type == "submitter")
            {
                listRating = (from rating in _ctx.Rating
                              from ticket in _ctx.Ticket.Where(w => w.TicketId == rating.TicketId).DefaultIfEmpty()
                              where ticket.Status == 3 && ticket.Submiter == userid && rating.UserId == userid
                              select new
                              {
                                  rate = rating.Rate,
                                  totRowRating = _ctx.Rating.Count(w => w.TicketId == ticket.TicketId),
                                  lastRowRating = _ctx.Rating.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(odb => odb.UserRatingId).FirstOrDefault()
                              });
            }
            var listRatingCount = (from ratingList in listRating
                                   where ratingList.totRowRating == 2 && ratingList.lastRowRating.CreatedAt.Month == 11
                                   select new
                                   {
                                       ratingList.rate
                                   }).ToList();

            var average = new decimal();
            var totRow = listRatingCount.Count;
            foreach (var item in listRatingCount)
            {
                average = average + item.rate;
            }

            if (average > 0)
            {
                average = average / totRow;
            }

            return Math.Round(average, 2);
        }
        public decimal GetCountTicketDashboardYearlyDes(int userid, string type, int y)
        {
            var listRating = (from rating in _ctx.Rating
                              from ticket in _ctx.Ticket.Where(w => w.TicketId == rating.TicketId).DefaultIfEmpty()
                              where ticket.Status == 3 && ticket.Responder == userid && rating.UserId == userid
                              select new
                              {
                                  rate = rating.Rate,
                                  totRowRating = _ctx.Rating.Count(w => w.TicketId == ticket.TicketId),
                                  lastRowRating = _ctx.Rating.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(odb => odb.UserRatingId).FirstOrDefault()
                              });
            if (type == "submitter")
            {
                listRating = (from rating in _ctx.Rating
                              from ticket in _ctx.Ticket.Where(w => w.TicketId == rating.TicketId).DefaultIfEmpty()
                              where ticket.Status == 3 && ticket.Submiter == userid && rating.UserId == userid
                              select new
                              {
                                  rate = rating.Rate,
                                  totRowRating = _ctx.Rating.Count(w => w.TicketId == ticket.TicketId),
                                  lastRowRating = _ctx.Rating.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(odb => odb.UserRatingId).FirstOrDefault()
                              });
            }
            var listRatingCount = (from ratingList in listRating
                                   where ratingList.totRowRating == 2 && ratingList.lastRowRating.CreatedAt.Month == 12
                                   select new
                                   {
                                       ratingList.rate
                                   }).ToList();

            var average = new decimal();
            var totRow = listRatingCount.Count;
            foreach (var item in listRatingCount)
            {
                average = average + item.rate;
            }

            if (average > 0)
            {
                average = average / totRow;
            }

            return Math.Round(average, 2);
        }

        public int GetCountTrYearResponder(int userId, int year)
        {
            var getTicket = (from rating in _ctx.Rating
                             from ticket in _ctx.Ticket.Where(w => w.TicketId == rating.TicketId).DefaultIfEmpty()
                             where ticket.Status == 3 && ticket.Responder == userId && rating.UserId == userId
                             select new
                             {
                                 rate = rating.Rate,
                                 totRowRating = _ctx.Rating.Count(w => w.TicketId == ticket.TicketId),
                                 lastRowRating = _ctx.Rating.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(odb => odb.UserRatingId).FirstOrDefault()
                             });

            var ticketCloseCount = (from listTicket in getTicket
                                    where listTicket.totRowRating == 2 && listTicket.lastRowRating.CreatedAt.Year == year
                                    select new
                                    {
                                        listTicket.rate
                                    }).ToList();
            var countTicketCloseMonth = ticketCloseCount.Count;
            if (countTicketCloseMonth > 0)
            {
                return countTicketCloseMonth;
            }
            else
            {
                return 0;
            }
        }
        public int GetCountTrYearSubmitter(int userId, int year)
        {
            var getTicket = (from rating in _ctx.Rating
                             from ticket in _ctx.Ticket.Where(w => w.TicketId == rating.TicketId).DefaultIfEmpty()
                             where ticket.Status == 3 && ticket.Submiter == userId && rating.UserId == userId
                             select new
                             {
                                 rate = rating.Rate,
                                 totRowRating = _ctx.Rating.Count(w => w.TicketId == ticket.TicketId),
                                 lastRowRating = _ctx.Rating.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(odb => odb.UserRatingId).FirstOrDefault()
                             });

            var ticketCloseCount = (from listTicket in getTicket
                                    where listTicket.totRowRating == 2 && listTicket.lastRowRating.CreatedAt.Year == year
                                    select new
                                    {
                                        listTicket.rate
                                    }).ToList();
            var countTicketCloseMonth = ticketCloseCount.Count;
            if (countTicketCloseMonth > 0)
            {
                return countTicketCloseMonth;
            }
            else
            {
                return 0;
            }
        }

        public decimal GetValueTurnAroundYearLessThanTree(int userId, string type, int year)
        {
            var getLastTicketClose = (from ticket in _ctx.Ticket
                                      from rating in _ctx.Rating.Where(w => w.TicketId == ticket.TicketId).DefaultIfEmpty()
                                      where ticket.Status == 3 && ticket.Submiter == userId && rating.UserId == userId || ticket.Status == 3 && ticket.Responder == userId && rating.UserId == userId
                                      select new
                                      {
                                          createdTicket = ticket.CreatedAt.Value,
                                          lastRating = _ctx.Rating.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(o => o.UserRatingId).FirstOrDefault(),
                                          totalRowTicketRating = _ctx.Rating.Count(w => w.TicketId == ticket.TicketId),
                                      });

            var getLastTicketCloseRebuild = (from data in getLastTicketClose
                                             select new
                                             {
                                                 ticketCreate = data.createdTicket,
                                                 lastRatingTicket = data.lastRating,
                                                 data.totalRowTicketRating,
                                                 getValueCloseTicket = DbFunctions.DiffDays(data.createdTicket, data.lastRating.CreatedAt)
                                             });

            var countTicketClose = (from listTicket in getLastTicketCloseRebuild
                                    where listTicket.totalRowTicketRating == 2 && listTicket.lastRatingTicket.CreatedAt.Year == year && listTicket.getValueCloseTicket < 3
                                    select new
                                    {
                                        ticketCreate = listTicket.lastRatingTicket
                                    }).ToList();

            var countTicket = countTicketClose.Count;
            return countTicket;
        }
        public decimal GetValueTurnAroundYearLessThanFourteen(int userId, string type, int year)
        {
            var getLastTicketClose = (from ticket in _ctx.Ticket
                                      from rating in _ctx.Rating.Where(w => w.TicketId == ticket.TicketId).DefaultIfEmpty()
                                      where ticket.Status == 3 && ticket.Submiter == userId && rating.UserId == userId || ticket.Status == 3 && ticket.Responder == userId && rating.UserId == userId
                                      select new
                                      {
                                          createdTicket = ticket.CreatedAt.Value,
                                          lastRating = _ctx.Rating.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(o => o.UserRatingId).FirstOrDefault(),
                                          totalRowTicketRating = _ctx.Rating.Count(w => w.TicketId == ticket.TicketId),
                                      });

            var getLastTicketCloseRebuild = (from data in getLastTicketClose
                                             select new
                                             {
                                                 ticketCreate = data.createdTicket,
                                                 lastRatingTicket = data.lastRating,
                                                 data.totalRowTicketRating,
                                                 getValueCloseTicket = DbFunctions.DiffDays(data.createdTicket, data.lastRating.CreatedAt)
                                             });

            var countTicketClose = (from listTicket in getLastTicketCloseRebuild
                                    where listTicket.totalRowTicketRating == 2 && listTicket.lastRatingTicket.CreatedAt.Year == year && listTicket.getValueCloseTicket >= 3 && listTicket.getValueCloseTicket < 14
                                    select new
                                    {
                                        ticketCreate = listTicket.lastRatingTicket
                                    }).ToList();

            var countTicket = countTicketClose.Count;
            return countTicket;
        }
        public decimal GetValueTurnAroundYearLessThanFourthyTwo(int userId, string type, int year)
        {
            var getLastTicketClose = (from ticket in _ctx.Ticket
                                      from rating in _ctx.Rating.Where(w => w.TicketId == ticket.TicketId).DefaultIfEmpty()
                                      where ticket.Status == 3 && ticket.Submiter == userId && rating.UserId == userId || ticket.Status == 3 && ticket.Responder == userId && rating.UserId == userId
                                      select new
                                      {
                                          createdTicket = ticket.CreatedAt.Value,
                                          lastRating = _ctx.Rating.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(o => o.UserRatingId).FirstOrDefault(),
                                          totalRowTicketRating = _ctx.Rating.Count(w => w.TicketId == ticket.TicketId),
                                      });

            var getLastTicketCloseRebuild = (from data in getLastTicketClose
                                             select new
                                             {
                                                 ticketCreate = data.createdTicket,
                                                 lastRatingTicket = data.lastRating,
                                                 data.totalRowTicketRating,
                                                 getValueCloseTicket = DbFunctions.DiffDays(data.createdTicket, data.lastRating.CreatedAt)
                                             });

            var countTicketClose = (from listTicket in getLastTicketCloseRebuild
                                    where listTicket.totalRowTicketRating == 2 && listTicket.lastRatingTicket.CreatedAt.Year == year && listTicket.getValueCloseTicket >= 14 && listTicket.getValueCloseTicket < 42
                                    select new
                                    {
                                        ticketCreate = listTicket.lastRatingTicket
                                    }).ToList();

            var countTicket = countTicketClose.Count;
            return countTicket;
        }
        public decimal GetValueTurnAroundYearLessThanEightyFour(int userId, string type, int year)
        {
            var getLastTicketClose = (from ticket in _ctx.Ticket
                                      from rating in _ctx.Rating.Where(w => w.TicketId == ticket.TicketId).DefaultIfEmpty()
                                      where ticket.Status == 3 && ticket.Submiter == userId && rating.UserId == userId || ticket.Status == 3 && ticket.Responder == userId && rating.UserId == userId
                                      select new
                                      {
                                          createdTicket = ticket.CreatedAt.Value,
                                          lastRating = _ctx.Rating.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(o => o.UserRatingId).FirstOrDefault(),
                                          totalRowTicketRating = _ctx.Rating.Count(w => w.TicketId == ticket.TicketId),
                                      });

            var getLastTicketCloseRebuild = (from data in getLastTicketClose
                                             select new
                                             {
                                                 ticketCreate = data.createdTicket,
                                                 lastRatingTicket = data.lastRating,
                                                 data.totalRowTicketRating,
                                                 getValueCloseTicket = DbFunctions.DiffDays(data.createdTicket, data.lastRating.CreatedAt)
                                             });

            var countTicketClose = (from listTicket in getLastTicketCloseRebuild
                                    where listTicket.totalRowTicketRating == 2 && listTicket.lastRatingTicket.CreatedAt.Year == year && listTicket.getValueCloseTicket >= 42 && listTicket.getValueCloseTicket < 84
                                    select new
                                    {
                                        ticketCreate = listTicket.lastRatingTicket
                                    }).ToList();

            var countTicket = countTicketClose.Count;
            return countTicket;
        }
        public decimal GetValueTurnAroundYearLessThanTwoQuarter(int userId, string type, int year)
        {
            var getLastTicketClose = (from ticket in _ctx.Ticket
                                      from rating in _ctx.Rating.Where(w => w.TicketId == ticket.TicketId).DefaultIfEmpty()
                                      where ticket.Status == 3 && ticket.Submiter == userId && rating.UserId == userId || ticket.Status == 3 && ticket.Responder == userId && rating.UserId == userId
                                      select new
                                      {
                                          createdTicket = ticket.CreatedAt.Value,
                                          lastRating = _ctx.Rating.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(o => o.UserRatingId).FirstOrDefault(),
                                          totalRowTicketRating = _ctx.Rating.Count(w => w.TicketId == ticket.TicketId),
                                      });

            var getLastTicketCloseRebuild = (from data in getLastTicketClose
                                             select new
                                             {
                                                 ticketCreate = data.createdTicket,
                                                 lastRatingTicket = data.lastRating,
                                                 data.totalRowTicketRating,
                                                 getValueCloseTicket = DbFunctions.DiffDays(data.createdTicket, data.lastRating.CreatedAt)
                                             });

            var countTicketClose = (from listTicket in getLastTicketCloseRebuild
                                    where listTicket.totalRowTicketRating == 2 && listTicket.lastRatingTicket.CreatedAt.Year == year && listTicket.getValueCloseTicket >= 84 && listTicket.getValueCloseTicket < 180
                                    select new
                                    {
                                        ticketCreate = listTicket.lastRatingTicket
                                    }).ToList();

            var countTicket = countTicketClose.Count;
            return countTicket;
        }
        public decimal GetValueTurnAroundYearMoreThanTwoQuarter(int userId, string type, int year)
        {
            var getLastTicketClose = (from ticket in _ctx.Ticket
                                      from rating in _ctx.Rating.Where(w => w.TicketId == ticket.TicketId).DefaultIfEmpty()
                                      where ticket.Status == 3 && ticket.Submiter == userId && rating.UserId == userId || ticket.Status == 3 && ticket.Responder == userId && rating.UserId == userId
                                      select new
                                      {
                                          createdTicket = ticket.CreatedAt.Value,
                                          lastRating = _ctx.Rating.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(o => o.UserRatingId).FirstOrDefault(),
                                          totalRowTicketRating = _ctx.Rating.Count(w => w.TicketId == ticket.TicketId),
                                      });

            var getLastTicketCloseRebuild = (from data in getLastTicketClose
                                             select new
                                             {
                                                 ticketCreate = data.createdTicket,
                                                 lastRatingTicket = data.lastRating,
                                                 data.totalRowTicketRating,
                                                 getValueCloseTicket = DbFunctions.DiffDays(data.createdTicket, data.lastRating.CreatedAt)
                                             });

            var countTicketClose = (from listTicket in getLastTicketCloseRebuild
                                    where listTicket.totalRowTicketRating == 2 && listTicket.lastRatingTicket.CreatedAt.Year == year && listTicket.getValueCloseTicket > 180
                                    select new
                                    {
                                        ticketCreate = listTicket.lastRatingTicket
                                    }).ToList();

            var countTicket = countTicketClose.Count;
            return countTicket;
        }
        #endregion

        public List<Ticket> getTicketMonthlySummaryDetail(int userId, string type, int month, int year)
        {
            List<Ticket> result = _ctx.Ticket
                .Where(w => w.Submiter == userId && w.Status == 3 && w.CreatedAt.Value.Month == month && w.CreatedAt.Value.Year == year || w.Responder == userId && w.Status == 3 && w.CreatedAt.Value.Month == month && w.CreatedAt.Value.Year == year)
                .ToList();
            return result;
        }

        public decimal GetCountDppm(int userId)
        {
            var getData = (from ticket in _ctx.Ticket
                           where ticket.Submiter == userId && ticket.DPPMno != null || ticket.Responder == userId && ticket.DPPMno != null
                           select new
                           {
                               countTicket = ticket.DPPMno
                           }).ToList();
            var count = getData.Count;
            return count;
        }

        public List<TicketMonitoring> getDataDashboardMonitoring(int userId, string type, string filter, int column, string order, bool searchTicketNo, bool searchCategory, bool searchTitle, bool searcrSubmitter, bool searchResponder, bool searcrDateCreated, bool searchDateClose, bool searchDateUpdate, bool searchStatusTR, bool searchStatusUser, string valueSearch)
        {

            var dataList = (from ticket in _ctx.Ticket
                            where ticket.Submiter == userId || ticket.Responder == userId
                            select new
                            {
                                ticketCreate = ticket.CreatedAt,
                                ticketUpdate = ticket.UpdatedAt,
                                ticketNo = ticket.TicketNo,
                                ticketTitle = ticket.Title,
                                ticketStatus = ticket.Status,
                                idSubmitter = ticket.Submiter,
                                idResponder = ticket.Responder,
                                dueDateAnswert = ticket.DueDateAnswer,
                                getNameCategory = _ctx.TicketCategory.Where(w => w.TicketCategoryId == ticket.TicketCategoryId).FirstOrDefault(),
                                getNameSubmitter = _ctx.User.Where(w => w.UserId == ticket.Submiter).FirstOrDefault(),
                                getNameResponder = _ctx.User.Where(w => w.UserId == ticket.Responder).FirstOrDefault(),
                                getTicketClose = _ctx.Rating.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(odb => odb.UserRatingId).FirstOrDefault(),
                                getCountTicketClose = _ctx.Rating.Where(w => w.TicketId == ticket.TicketId).Count(),
                                getStatusUser = _ctx.User.Where(w => w.UserId == userId).FirstOrDefault(),
                                getTicketDueDateAnswer = ticket.DueDateAnswer,
                                ticketId = ticket.TicketId,
                                nextCommenter = ticket.NextCommenter,
                                getLastDiscuss = _ctx.TicketDiscussion.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(odb => odb.CreatedAt).FirstOrDefault(),
                                getLastNote = _ctx.TicketNote.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(odb => odb.CreatedAt).FirstOrDefault()
                            });

            if (type == "responder")
            {
                dataList = (from ticket in _ctx.Ticket
                            where ticket.Responder == userId
                            select new
                            {
                                ticketCreate = ticket.CreatedAt,
                                ticketUpdate = ticket.UpdatedAt,
                                ticketNo = ticket.TicketNo,
                                ticketTitle = ticket.Title,
                                ticketStatus = ticket.Status,
                                idSubmitter = ticket.Submiter,
                                idResponder = ticket.Responder,
                                dueDateAnswert = ticket.DueDateAnswer,
                                getNameCategory = _ctx.TicketCategory.Where(w => w.TicketCategoryId == ticket.TicketCategoryId).FirstOrDefault(),
                                getNameSubmitter = _ctx.User.Where(w => w.UserId == ticket.Submiter).FirstOrDefault(),
                                getNameResponder = _ctx.User.Where(w => w.UserId == ticket.Responder).FirstOrDefault(),
                                getTicketClose = _ctx.Rating.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(odb => odb.UserRatingId).FirstOrDefault(),
                                getCountTicketClose = _ctx.Rating.Where(w => w.TicketId == ticket.TicketId).Count(),
                                getStatusUser = _ctx.User.Where(w => w.UserId == userId).FirstOrDefault(),
                                getTicketDueDateAnswer = ticket.DueDateAnswer,
                                ticketId = ticket.TicketId,
                                nextCommenter = ticket.NextCommenter,
                                getLastDiscuss = _ctx.TicketDiscussion.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(odb => odb.CreatedAt).FirstOrDefault(),
                                getLastNote = _ctx.TicketNote.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(odb => odb.CreatedAt).FirstOrDefault()
                            });
            }

            var preDataListBuild = (from item in dataList
                                    select new
                                    {
                                        ticketCreate = item.ticketCreate,
                                        ticketUpdate = item.ticketUpdate,
                                        ticketNo = item.ticketNo,
                                        ticketTitle = item.ticketTitle,
                                        ticketStatus = item.ticketStatus,
                                        idSubmitter = item.idSubmitter,
                                        idResponder = item.idResponder,
                                        dueDateAnswer = item.dueDateAnswert,
                                        getNameCategory = item.getNameCategory,
                                        getNameSubmitter = item.getNameSubmitter,
                                        getNameResponder = item.getNameResponder,
                                        getTicketClose = (item.getCountTicketClose == 2) ? item.getTicketClose.CreatedAt : item.ticketCreate,
                                        getCountTicketClose = item.getCountTicketClose,
                                        getStatusUser = item.getStatusUser.RoleId,
                                        subtDayForStatusTicket = DbFunctions.DiffDays(item.ticketCreate, DateTime.Now),
                                        getTicketDueDateAnswer = item.getTicketDueDateAnswer,
                                        ticketId = item.ticketId,
                                        nextCommenter = item.nextCommenter,
                                        getEscelated = _ctx.EscalateLog.Count(q => q.TicketId == item.ticketId && q.EscalateFrom == userId) > 0,
                                        getLastDiscuss = (item.getLastDiscuss != null) ? item.getLastDiscuss.CreatedAt.Value : item.ticketCreate.Value,
                                        getLastNote = (item.getLastNote != null) ? item.getLastNote.CreatedAt.Value : item.ticketCreate.Value,
                                        getLastNoteUpdate = item.getLastNote,
                                        getLastDiscussUpdate = item.getLastDiscuss
                                    }).ToList();

            var dataListBuild = (from item in preDataListBuild
                                 select new
                                 {
                                     idSubmitter = item.idSubmitter,
                                     idResponder = item.idResponder,
                                     ticketCreate = item.ticketCreate,
                                     ticketUpdate = item.ticketUpdate,
                                     ticketNo = item.ticketNo,
                                     ticketTitle = item.ticketTitle,
                                     ticketStatus = item.ticketStatus,
                                     getEscelated = item.getEscelated,
                                     getNameCategory = item.getNameCategory,
                                     getNameSubmitter = item.getNameSubmitter,
                                     getNameResponder = item.getNameResponder,
                                     getTicketClose = item.getTicketClose,
                                     getStatusUser = item.getStatusUser,
                                     subtDayForStatusTicket =item.subtDayForStatusTicket,
                                     nextCommenter = item.nextCommenter,
                                     ticketId = item.ticketId,
                                     getCount = item.dueDateAnswer == null ? 0 : getCountDateBeforeThreeDays(item.dueDateAnswer.Value),
                                     lastNote = item.getLastNote,
                                     lastDiscuss = item.getLastDiscuss,
                                     lastDiscussUpdate = item.getLastDiscussUpdate,
                                     lastNoteUpdate = item.getLastNoteUpdate
                                 });

            var dataListBuildAgain = (from item in dataListBuild
                                      where item.getCount >= 1 && item.getCount <= 3 && item.getEscelated && item.ticketStatus == 2 && item.idResponder == userId || item.getCount >= 1 && item.getCount <= 3 && item.idResponder == item.nextCommenter && item.nextCommenter == userId && item.ticketStatus == 2 || item.getCount >= 1 && item.getCount <= 3 && item.idSubmitter == item.nextCommenter && item.nextCommenter == userId && item.ticketStatus == 2 || item.getCount >= 1 && item.getCount <= 3 && item.ticketStatus == 6 && item.idSubmitter == userId
                                      select new
                                      {
                                          idSubmitter = item.idSubmitter,
                                          idResponder = item.idResponder,
                                          ticketCreate = item.ticketCreate,
                                          ticketUpdate = item.ticketUpdate,
                                          ticketNo = item.ticketNo,
                                          ticketTitle = item.ticketTitle,
                                          ticketStatus = item.ticketStatus,
                                          getEscalated = item.getEscelated,
                                          getNameCategory = item.getNameCategory,
                                          getNameSubmitter = item.getNameSubmitter,
                                          getNameResponder = item.getNameResponder,
                                          getTicketClose = item.getTicketClose,
                                          getStatusUser = _ctx.UserRole.Where(w => w.UserRoleId == item.getStatusUser).FirstOrDefault(),
                                          nextCommenter = item.nextCommenter,
                                          ticketId = item.ticketId,
                                          lastNote = item.lastNote,
                                          lastDiscuss = item.lastDiscuss,
                                          lastDiscussUpdate = item.lastDiscussUpdate,
                                          lastNoteUpdate = item.lastDiscussUpdate
                                      });

            if (type == "submitter" && filter == "psa")
            {
                dataListBuildAgain = (from item in dataListBuild
                                      where item.getCount >= 0 && item.getCount <= 2 && item.ticketStatus == 6 && item.idSubmitter == userId || item.getCount >= 0 && item.getCount <= 2 && item.idSubmitter == item.nextCommenter && item.nextCommenter == userId && item.ticketStatus == 2
                                      select new
                                      {
                                          idSubmitter = item.idSubmitter,
                                          idResponder = item.idResponder,
                                          ticketCreate = item.ticketCreate,
                                          ticketUpdate = item.ticketUpdate,
                                          ticketNo = item.ticketNo,
                                          ticketTitle = item.ticketTitle,
                                          ticketStatus = item.ticketStatus,
                                          getEscalated = item.getEscelated,
                                          getNameCategory = item.getNameCategory,
                                          getNameSubmitter = item.getNameSubmitter,
                                          getNameResponder = item.getNameResponder,
                                          getTicketClose = item.getTicketClose,
                                          getStatusUser = _ctx.UserRole.Where(w => w.UserRoleId == item.getStatusUser).FirstOrDefault(),
                                          nextCommenter = item.nextCommenter,
                                          ticketId = item.ticketId,
                                          lastNote = item.lastNote,
                                          lastDiscuss = item.lastDiscuss,
                                          lastDiscussUpdate = item.lastDiscussUpdate,
                                          lastNoteUpdate = item.lastDiscussUpdate
                                      });
            }
            else if(type == "submitter" && filter == "escalated")
            {
                dataListBuildAgain = (from item in dataListBuild
                                      where item.getCount == 3 && item.ticketStatus == 6 && item.idSubmitter == item.nextCommenter && item.nextCommenter == userId || item.getCount == 3 && item.idSubmitter == item.nextCommenter && item.nextCommenter == userId && item.ticketStatus == 2
                                      select new
                                      {
                                          idSubmitter = item.idSubmitter,
                                          idResponder = item.idResponder,
                                          ticketCreate = item.ticketCreate,
                                          ticketUpdate = item.ticketUpdate,
                                          ticketNo = item.ticketNo,
                                          ticketTitle = item.ticketTitle,
                                          ticketStatus = item.ticketStatus,
                                          getEscalated = item.getEscelated,
                                          getNameCategory = item.getNameCategory,
                                          getNameSubmitter = item.getNameSubmitter,
                                          getNameResponder = item.getNameResponder,
                                          getTicketClose = item.getTicketClose,
                                          getStatusUser = _ctx.UserRole.Where(w => w.UserRoleId == item.getStatusUser).FirstOrDefault(),
                                          nextCommenter = item.nextCommenter,
                                          ticketId = item.ticketId,
                                          lastNote = item.lastNote,
                                          lastDiscuss = item.lastDiscuss,
                                          lastDiscussUpdate = item.lastDiscussUpdate,
                                          lastNoteUpdate = item.lastDiscussUpdate
                                      });
            }
            else if(type == "submitter" && filter == "other")
            {
                dataListBuildAgain = (from item in dataListBuild
                                      where item.getCount > 3 && item.idSubmitter == item.nextCommenter && item.nextCommenter == userId && item.ticketStatus == 2
                                      select new
                                      {
                                          idSubmitter = item.idSubmitter,
                                          idResponder = item.idResponder,
                                          ticketCreate = item.ticketCreate,
                                          ticketUpdate = item.ticketUpdate,
                                          ticketNo = item.ticketNo,
                                          ticketTitle = item.ticketTitle,
                                          ticketStatus = item.ticketStatus,
                                          getEscalated = item.getEscelated,
                                          getNameCategory = item.getNameCategory,
                                          getNameSubmitter = item.getNameSubmitter,
                                          getNameResponder = item.getNameResponder,
                                          getTicketClose = item.getTicketClose,
                                          getStatusUser = _ctx.UserRole.Where(w => w.UserRoleId == item.getStatusUser).FirstOrDefault(),
                                          nextCommenter = item.nextCommenter,
                                          ticketId = item.ticketId,
                                          lastNote = item.lastNote,
                                          lastDiscuss = item.lastDiscuss,
                                          lastDiscussUpdate = item.lastDiscussUpdate,
                                          lastNoteUpdate = item.lastDiscussUpdate
                                      });
            }

            if (type == "responder" && filter == "psa")
            {
                dataListBuildAgain = (from item in dataListBuild
                                      where item.getCount >= 0 && item.getCount <= 2 && item.getEscelated && item.idResponder == item.nextCommenter && item.nextCommenter == userId || item.getCount >= 0 && item.getCount <= 2 && item.idResponder == item.nextCommenter && item.nextCommenter == userId && item.ticketStatus == 2
                                      select new
                                      {
                                          idSubmitter = item.idSubmitter,
                                          idResponder = item.idResponder,
                                          ticketCreate = item.ticketCreate,
                                          ticketUpdate = item.ticketUpdate,
                                          ticketNo = item.ticketNo,
                                          ticketTitle = item.ticketTitle,
                                          ticketStatus = item.ticketStatus,
                                          getEscalated = item.getEscelated,
                                          getNameCategory = item.getNameCategory,
                                          getNameSubmitter = item.getNameSubmitter,
                                          getNameResponder = item.getNameResponder,
                                          getTicketClose = item.getTicketClose,
                                          getStatusUser = _ctx.UserRole.Where(w => w.UserRoleId == item.getStatusUser).FirstOrDefault(),
                                          nextCommenter = item.nextCommenter,
                                          ticketId = item.ticketId,
                                          lastNote = item.lastNote,
                                          lastDiscuss = item.lastDiscuss,
                                          lastDiscussUpdate = item.lastDiscussUpdate,
                                          lastNoteUpdate = item.lastDiscussUpdate
                                      });
            }
            else if (type == "responder" && filter == "escalated")
            {
                dataListBuildAgain = (from item in dataListBuild
                                      where item.getCount == 3 && item.getEscelated && item.idResponder == item.nextCommenter && item.nextCommenter == userId || item.getCount == 3 && item.idResponder == item.nextCommenter && item.nextCommenter == userId && item.ticketStatus == 2
                                      select new
                                      {
                                          idSubmitter = item.idSubmitter,
                                          idResponder = item.idResponder,
                                          ticketCreate = item.ticketCreate,
                                          ticketUpdate = item.ticketUpdate,
                                          ticketNo = item.ticketNo,
                                          ticketTitle = item.ticketTitle,
                                          ticketStatus = item.ticketStatus,
                                          getEscalated = item.getEscelated,
                                          getNameCategory = item.getNameCategory,
                                          getNameSubmitter = item.getNameSubmitter,
                                          getNameResponder = item.getNameResponder,
                                          getTicketClose = item.getTicketClose,
                                          getStatusUser = _ctx.UserRole.Where(w => w.UserRoleId == item.getStatusUser).FirstOrDefault(),
                                          nextCommenter = item.nextCommenter,
                                          ticketId = item.ticketId,
                                          lastNote = item.lastNote,
                                          lastDiscuss = item.lastDiscuss,
                                          lastDiscussUpdate = item.lastDiscussUpdate,
                                          lastNoteUpdate = item.lastDiscussUpdate
                                      });
            }
            else if (type == "responder" && filter == "other")
            {
                dataListBuildAgain = (from item in dataListBuild
                                      where item.getCount > 3 && item.getEscelated && item.idResponder == item.nextCommenter && item.nextCommenter == userId || item.getCount > 3 && item.idResponder == item.nextCommenter && item.nextCommenter == userId && item.ticketStatus == 2
                                      select new
                                      {
                                          idSubmitter = item.idSubmitter,
                                          idResponder = item.idResponder,
                                          ticketCreate = item.ticketCreate,
                                          ticketUpdate = item.ticketUpdate,
                                          ticketNo = item.ticketNo,
                                          ticketTitle = item.ticketTitle,
                                          ticketStatus = item.ticketStatus,
                                          getEscalated = item.getEscelated,
                                          getNameCategory = item.getNameCategory,
                                          getNameSubmitter = item.getNameSubmitter,
                                          getNameResponder = item.getNameResponder,
                                          getTicketClose = item.getTicketClose,
                                          getStatusUser = _ctx.UserRole.Where(w => w.UserRoleId == item.getStatusUser).FirstOrDefault(),
                                          nextCommenter = item.nextCommenter,
                                          ticketId = item.ticketId,
                                          lastNote = item.lastNote,
                                          lastDiscuss = item.lastDiscuss,
                                          lastDiscussUpdate = item.lastDiscussUpdate,
                                          lastNoteUpdate = item.lastDiscussUpdate
                                      });
            }

            var listData = (from item in dataListBuildAgain
                            select new
                            {
                                idSubmitter = item.idSubmitter,
                                idResponder = item.idResponder,
                                ticketCreate = item.ticketCreate,
                                ticketUpdate = item.ticketUpdate,
                                ticketId = item.ticketId,
                                ticketNo = item.ticketNo,
                                ticketTitle = item.ticketTitle,
                                ticketStatus = item.ticketStatus,
                                getEscalated = item.getEscalated,
                                getNamaCategory = item.getNameCategory.Name,
                                getNameSubmitter = item.getNameSubmitter.Name,
                                getNameResponder = item.getNameResponder.Name,
                                ticketClose = item.getTicketClose,
                                nextCommenter = item.nextCommenter,
                                getStatusUser = item.getStatusUser.Name,
                                lastNote = item.lastNote,
                                lastDiscuss = item.lastDiscuss,
                                orderDateUpdate = getDateUpdate(item.lastNote, item.lastDiscuss, item.ticketCreate.Value)
                            });
            if (!string.IsNullOrWhiteSpace(valueSearch))
            {
                if (valueSearch.ToLower().Contains("psa"))
                {
                    listData = listData.Where(w => w.idSubmitter == userId && w.idSubmitter == w.nextCommenter);
                }
                else if(valueSearch.ToLower().Contains("pra"))
                {
                    listData = listData.Where(w => w.idResponder == userId && w.idResponder == w.nextCommenter);
                }
                else if (valueSearch.ToLower().Contains("solved"))
                {
                    listData = listData.Where(w => w.idSubmitter == userId && w.ticketStatus == 6);
                }
                else if (valueSearch.ToLower().Contains("escalated"))
                {
                    listData = listData.Where(w => w.idResponder == userId && w.getEscalated);
                }
                else
                {
                    listData = listData.Where(w => w.ticketNo.ToLower().Contains(valueSearch) || w.getNamaCategory.ToLower().Contains(valueSearch) || w.ticketTitle.ToLower().Contains(valueSearch) || w.getNameSubmitter.ToLower().Contains(valueSearch) || w.getNameResponder.ToLower().Contains(valueSearch) || w.ticketClose.ToString().Contains(valueSearch) || w.ticketCreate.Value.ToString().Contains(valueSearch) || w.orderDateUpdate.ToString().Contains(valueSearch) || w.getStatusUser.ToLower().Contains(valueSearch));
                }
                
            }

            

            #region Order
            if (column == 0)
            {
                if (order == "asc")
                {
                    listData = listData.OrderBy(ord => ord.ticketNo);
                }
                else
                {
                    listData = listData.OrderByDescending(odb => odb.ticketNo);
                }
            }
            else if (column == 1)
            {
                if (order == "asc")
                {
                    listData = listData.OrderBy(ord => ord.getNamaCategory);
                }
                else
                {
                    listData = listData.OrderByDescending(odb => odb.getNamaCategory);
                }
            }
            else if (column == 2)
            {
                if (order == "asc")
                {
                    listData = listData.OrderBy(ord => ord.ticketTitle);
                }
                else
                {
                    listData = listData.OrderByDescending(odb => odb.ticketTitle);
                }
            }
            else if (column == 3)
            {
                if (order == "asc")
                {
                    listData = listData.OrderBy(ord => ord.getNameSubmitter);
                }
                else
                {
                    listData = listData.OrderByDescending(odb => odb.getNameSubmitter);
                }
            }
            else if (column == 4)
            {
                if (order == "asc")
                {
                    listData = listData.OrderBy(ord => ord.getNameResponder);
                }
                else
                {
                    listData = listData.OrderByDescending(odb => odb.getNameResponder);
                }
            }
            else if (column == 5)
            {
                if (order == "asc")
                {
                    listData = listData.OrderBy(ord => ord.ticketCreate);
                }
                else
                {
                    listData = listData.OrderByDescending(odb => odb.ticketCreate);
                }
            }
            else if (column == 6)
            {
                if (order == "asc")
                {
                    listData = listData.OrderBy(ord => ord.ticketClose);
                }
                else
                {
                    listData.OrderByDescending(odb => odb.ticketClose);
                }
            }
            else if (column == 7)
            {
                if (order == "asc")
                {
                    listData = listData.OrderBy(ord => ord.orderDateUpdate);
                }
                else
                {
                    listData = listData.OrderByDescending(odb => odb.orderDateUpdate);
                }
            }
            else if (column == 8)
            {
                if (order == "asc")
                {
                    listData = listData.OrderBy(ord => ord.ticketStatus);
                }
                else
                {
                    listData = listData.OrderByDescending(odb => odb.ticketStatus);
                }
            }
            else if (column == 9)
            {
                if (order == "asc")
                {
                    listData = listData.OrderBy(ord => ord.getStatusUser);
                }
                else
                {
                    listData = listData.OrderByDescending(odb => odb.getStatusUser);
                }
            }
            #endregion


            var listItem = new List<TicketMonitoring>();
            int ida = 0;
            foreach (var ticket in listData.ToList())
            {
                var strTicketCreate = Convert.ToDateTime(ticket.ticketCreate).ToString("yyyy-MM-dd");
                var strTicketClose = Convert.ToDateTime(ticket.ticketClose).ToString("yyyy-MM-dd");
                var strTicketUpdate = Convert.ToDateTime(ticket.ticketUpdate).ToString("yyyy-MM-dd");
                var list = new TicketMonitoring();
                list.Row = ida++;
                list.TicketId = ticket.ticketId;
                list.TicketNo = ticket.ticketNo;
                list.Category = Convert.ToString(ticket.getNamaCategory);
                list.Title = ticket.ticketTitle;
                list.Submitter = (ticket.getNameSubmitter != null) ? ticket.getNameSubmitter : "0";
                list.Responder = (ticket.getNameResponder != null) ? ticket.getNameResponder : "0";
                list.TicketCreated = (strTicketCreate != null) ? strTicketCreate : "";
                list.TicketClosed = getDateCloseTicket(ticket.ticketClose.Value, ticket.ticketCreate.Value);
                list.TicketUpdated = getDateUpdate(ticket.lastNote,ticket.lastDiscuss, ticket.ticketCreate.Value);
                list.TicketStatus = getStatusTicket(ticket.getEscalated, ticket.nextCommenter, ticket.ticketStatus, ticket.idSubmitter, ticket.idResponder, userId);
                list.UserStatus = ticket.getStatusUser;
                listItem.Add(list);
            }
            
            return listItem;

        }

        static string getStatusTicket(bool escalated, int nextCommenter, int ticketStatus, int submitter, int responder, int userId)
        {
            var value = "";
            if(ticketStatus == 2)
            {
                if (escalated)
                {
                    value = "Escalated";
                }
                else if(userId == submitter && submitter == nextCommenter)
                {
                    value = "PSA";
                }
                else if(userId == responder && responder == nextCommenter)
                {
                    value = "PRA";
                }
            }else if(ticketStatus == 6)
            {
                value = "Solved";
            }
            return value;
        }

        public List<int> getvalueSubmitter(int userId)
        {
            var dataList = (from ticket in _ctx.Ticket
                            where ticket.Submiter == userId
                            select new
                            {
                                createdTicket = ticket.CreatedAt,
                                ticketId = ticket.TicketId,
                                getEscelated = _ctx.EscalateLog.Count(q => q.TicketId == ticket.TicketId && q.EscalateFrom == userId) > 0,
                                ticketStatus = ticket.Status,
                                ticketNextCommenter = ticket.NextCommenter,
                                ticketSubmitter = ticket.Submiter,
                                dueDateAnswer = ticket.DueDateAnswer,
                                getLastNote = _ctx.TicketNote.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(odb => odb.CreatedAt).FirstOrDefault(),
                                getLastDiscussion = _ctx.TicketDiscussion.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(odb => odb.CreatedAt).FirstOrDefault(),
                            });

            var dataTemp = (from item in dataList
                            select new
                            {
                                createdTicket = item.createdTicket,
                                ticketId = item.ticketId,
                                getEscelated = _ctx.EscalateLog.Count(q => q.TicketId == item.ticketId && q.EscalateFrom == userId) > 0,
                                ticketStatus = item.ticketStatus,
                                ticketNextCommenter = item.ticketNextCommenter,
                                ticketSubmitter = item.ticketSubmitter,
                                dueDateAnswer = item.dueDateAnswer,
                                getLastNote = (item.getLastNote != null) ? item.getLastNote.CreatedAt : item.createdTicket,
                                getLastDiscussion = (item.getLastDiscussion != null) ? item.getLastDiscussion.CreatedAt : item.createdTicket,
                            }).ToList();

            var listData = (from item in dataTemp
                            select new
                            {
                                ticketId = item.ticketId,
                                ticketCreated = item.createdTicket,
                                getEscalated = item.getEscelated,
                                ticketStatus = item.ticketStatus,
                                ticketSubmitter = item.ticketSubmitter,
                                ticketNextCommenter = item.ticketNextCommenter,
                                getCount = item.dueDateAnswer == null ? 0:getCountDateBeforeThreeDays(item.dueDateAnswer.Value)
                            });

            var countTicketCloseLessThanTwoDay = (from item in listData
                                                  where item.getCount == 1 && item.ticketStatus == 6 && item.ticketSubmitter == userId || item.getCount == 1 && item.ticketSubmitter == item.ticketNextCommenter && item.ticketNextCommenter == userId && item.ticketStatus == 2
                                                  select new
                                                  {
                                                      ticketId = item.ticketId
                                                  }).ToList();
            var countTicketCloseLessThanThreeDay = (from item in listData
                                                    where item.getCount == 2 && item.ticketStatus == 6 && item.ticketSubmitter == userId || item.getCount == 2 && item.ticketSubmitter == item.ticketNextCommenter && item.ticketNextCommenter == userId && item.ticketStatus == 2
                                                    select new
                                                    {
                                                        ticketId = item.ticketId
                                                    }).ToList();
            var countTicketCloseMoreThanThreeDay = (from item in listData
                                                    where item.getCount == 3 && item.ticketSubmitter == item.ticketNextCommenter && item.ticketNextCommenter == userId && item.ticketStatus == 2
                                                    select new
                                                    {
                                                        ticketId = item.ticketId
                                                    }).ToList();

            var countData1 = countTicketCloseLessThanTwoDay.Count();
            var countData2 = countTicketCloseLessThanThreeDay.Count();
            var countData3 = countTicketCloseMoreThanThreeDay.Count();

            var resutList = new List<int>();
            resutList.Add(countData1);
            resutList.Add(countData2);
            resutList.Add(countData3);

            return resutList;
        }

        public List<int> getvalueSubmitterPSA(int userId)
        {
            var dataList = (from ticket in _ctx.Ticket
                            where ticket.Submiter == userId
                            select new
                            {
                                createdTicket = ticket.CreatedAt,
                                ticketId = ticket.TicketId,
                                getEscelated = _ctx.EscalateLog.Count(q => q.TicketId == ticket.TicketId && q.EscalateFrom == userId) > 0,
                                ticketStatus = ticket.Status,
                                ticketNextCommenter = ticket.NextCommenter,
                                ticketSubmitter = ticket.Submiter,
                                dueDateAnswer = ticket.DueDateAnswer,
                                getLastNote = _ctx.TicketNote.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(odb => odb.CreatedAt).FirstOrDefault(),
                                getLastDiscussion = _ctx.TicketDiscussion.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(odb => odb.CreatedAt).FirstOrDefault(),
                            });

            var dataTemp = (from item in dataList
                            select new
                            {
                                createdTicket = item.createdTicket,
                                ticketId = item.ticketId,
                                getEscelated = _ctx.EscalateLog.Count(q => q.TicketId == item.ticketId && q.EscalateFrom == userId) > 0,
                                ticketStatus = item.ticketStatus,
                                ticketNextCommenter = item.ticketNextCommenter,
                                ticketSubmitter = item.ticketSubmitter,
                                dueDateAnswer = item.dueDateAnswer,
                                getLastNote = (item.getLastNote != null) ? item.getLastNote.CreatedAt : item.createdTicket,
                                getLastDiscussion = (item.getLastDiscussion != null) ? item.getLastDiscussion.CreatedAt : item.createdTicket,
                            }).ToList();

            var listData = (from item in dataTemp
                            select new
                            {
                                ticketId = item.ticketId,
                                ticketCreated = item.createdTicket,
                                getEscalated = item.getEscelated,
                                ticketStatus = item.ticketStatus,
                                ticketSubmitter = item.ticketSubmitter,
                                ticketNextCommenter = item.ticketNextCommenter,
                                getCount = item.dueDateAnswer== null ? 0 :  getCountDateBeforeThreeDays(item.dueDateAnswer.Value)
                            });

            var countTicketCloseLessThanTwoDay = (from item in listData
                                                  where item.getCount == 1 && item.ticketSubmitter == item.ticketNextCommenter && item.ticketNextCommenter == userId && item.ticketStatus == 2
                                                  select new
                                                  {
                                                      ticketId = item.ticketId
                                                  }).ToList();
            var countTicketCloseLessThanThreeDay = (from item in listData
                                                    where item.getCount == 2 && item.ticketSubmitter == item.ticketNextCommenter && item.ticketNextCommenter == userId && item.ticketStatus == 2
                                                    select new
                                                    {
                                                        ticketId = item.ticketId
                                                    }).ToList();
            var countTicketCloseMoreThanThreeDay = (from item in listData
                                                    where item.getCount == 3 && item.ticketSubmitter == item.ticketNextCommenter && item.ticketNextCommenter == userId && item.ticketStatus == 2
                                                    select new
                                                    {
                                                        ticketId = item.ticketId
                                                    }).ToList();

            var countData1 = countTicketCloseLessThanTwoDay.Count();
            var countData2 = countTicketCloseLessThanThreeDay.Count();
            var countData3 = countTicketCloseMoreThanThreeDay.Count();

            var resutList = new List<int>();
            resutList.Add(countData1);
            resutList.Add(countData2);
            resutList.Add(countData3);

            return resutList;
        }

        public List<int> getvalueSubmitterSolved(int userId)
        {
            var dataList = (from ticket in _ctx.Ticket
                            where ticket.Submiter == userId
                            select new
                            {
                                createdTicket = ticket.CreatedAt,
                                ticketId = ticket.TicketId,
                                getEscelated = _ctx.EscalateLog.Count(q => q.TicketId == ticket.TicketId && q.EscalateFrom == userId) > 0,
                                ticketStatus = ticket.Status,
                                ticketNextCommenter = ticket.NextCommenter,
                                ticketSubmitter = ticket.Submiter,
                                dueDateAnswer = ticket.DueDateAnswer,
                                getLastNote = _ctx.TicketNote.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(odb => odb.CreatedAt).FirstOrDefault(),
                                getLastDiscussion = _ctx.TicketDiscussion.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(odb => odb.CreatedAt).FirstOrDefault(),
                            });

            var dataTemp = (from item in dataList
                            select new
                            {
                                createdTicket = item.createdTicket,
                                ticketId = item.ticketId,
                                getEscelated = _ctx.EscalateLog.Count(q => q.TicketId == item.ticketId && q.EscalateFrom == userId) > 0,
                                ticketStatus = item.ticketStatus,
                                ticketNextCommenter = item.ticketNextCommenter,
                                ticketSubmitter = item.ticketSubmitter,
                                dueDateAnswer = item.dueDateAnswer,
                                getLastNote = (item.getLastNote != null) ? item.getLastNote.CreatedAt : item.createdTicket,
                                getLastDiscussion = (item.getLastDiscussion != null) ? item.getLastDiscussion.CreatedAt : item.createdTicket,
                            }).ToList();

            var listData = (from item in dataTemp
                            select new
                            {
                                ticketId = item.ticketId,
                                ticketCreated = item.createdTicket,
                                getEscalated = item.getEscelated,
                                ticketStatus = item.ticketStatus,
                                ticketSubmitter = item.ticketSubmitter,
                                ticketNextCommenter = item.ticketNextCommenter,
                                getCount = item.dueDateAnswer == null ? 0 : getCountDateBeforeThreeDays(item.dueDateAnswer.Value)
                            });

            var countTicketCloseLessThanTwoDay = (from item in listData
                                                  where item.getCount == 1 && item.ticketStatus == 6 
                                                  select new
                                                  {
                                                      ticketId = item.ticketId
                                                  }).ToList();
            var countTicketCloseLessThanThreeDay = (from item in listData
                                                    where item.getCount == 2 && item.ticketStatus == 6 
                                                    select new
                                                    {
                                                        ticketId = item.ticketId
                                                    }).ToList();

            var countData1 = countTicketCloseLessThanTwoDay.Count();
            var countData2 = countTicketCloseLessThanThreeDay.Count();

            var resutList = new List<int>();
            resutList.Add(countData1);
            resutList.Add(countData2);

            return resutList;
        }

        public List<int> getvalueResponder(int userId)
        {
            var dataList = (from ticket in _ctx.Ticket
                            where ticket.Responder == userId
                            select new
                            {
                                createdTicket = ticket.CreatedAt,
                                ticketId = ticket.TicketId,
                                getEscelated = _ctx.EscalateLog.Count(q => q.TicketId == ticket.TicketId && q.EscalateFrom == userId) > 0,
                                ticketStatus = ticket.Status,
                                ticketNextCommenter = ticket.NextCommenter,
                                ticketResponder = ticket.Responder,
                                dueDateAnswer = ticket.DueDateAnswer,
                                getLastNote = _ctx.TicketNote.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(odb => odb.CreatedAt).FirstOrDefault(),
                                getLastDiscussion = _ctx.TicketDiscussion.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(odb => odb.CreatedAt).FirstOrDefault(),
                            });

            var dataTemp = (from item in dataList
                            select new
                            {
                                createdTicket = item.createdTicket,
                                ticketId = item.ticketId,
                                getEscelated = _ctx.EscalateLog.Count(q => q.TicketId == item.ticketId && q.EscalateFrom == userId) > 0,
                                ticketStatus = item.ticketStatus,
                                ticketNextCommenter = item.ticketNextCommenter,
                                ticketResponder = item.ticketResponder,
                                dueDateAnswer = item.dueDateAnswer,
                                getLastNote = (item.getLastNote != null) ? item.getLastNote.CreatedAt : item.createdTicket,
                                getLastDiscussion = (item.getLastDiscussion != null) ? item.getLastDiscussion.CreatedAt : item.createdTicket,
                            }).ToList();

            var listData = (from item in dataTemp
                            select new
                            {
                                ticketId = item.ticketId,
                                ticketCreated = item.createdTicket,
                                getEscalated = item.getEscelated,
                                ticketStatus = item.ticketStatus,
                                ticketResponder = item.ticketResponder,
                                ticketNextCommenter = item.ticketNextCommenter,
                                getCount = getCountDateBeforeThreeDays(item.dueDateAnswer.Value)
                            });

            var countTicketCloseLessThanTwoDay = (from item in listData
                                                  where item.getCount == 1 && item.getEscalated && item.ticketResponder == item.ticketNextCommenter && item.ticketNextCommenter == userId || item.getCount == 1 && item.ticketResponder == item.ticketNextCommenter && item.ticketNextCommenter == userId && item.ticketStatus == 2
                                                  select new
                                                  {
                                                      ticketId = item.ticketId
                                                  }).ToList();
            var countTicketCloseLessThanThreeDay = (from item in listData
                                                    where item.getCount == 2 && item.getEscalated && item.ticketResponder == item.ticketNextCommenter && item.ticketNextCommenter == userId || item.getCount == 2 && item.ticketResponder == item.ticketNextCommenter && item.ticketNextCommenter == userId && item.ticketStatus == 2
                                                    select new
                                                    {
                                                        ticketId = item.ticketId
                                                    }).ToList();
            var countTicketCloseMoreThanThreeDay = (from item in listData
                                                    where item.getCount == 3 && item.getEscalated && item.ticketResponder == item.ticketNextCommenter && item.ticketNextCommenter == userId || item.getCount == 3 && item.ticketResponder == item.ticketNextCommenter && item.ticketNextCommenter == userId && item.ticketStatus == 2
                                                    select new
                                                    {
                                                        ticketId = item.ticketId
                                                    }).ToList();

            var countData1 = countTicketCloseLessThanTwoDay.Count();
            var countData2 = countTicketCloseLessThanThreeDay.Count();
            var countData3 = countTicketCloseMoreThanThreeDay.Count();

            var resutList = new List<int>();
            resutList.Add(countData1);
            resutList.Add(countData2);
            resutList.Add(countData3);

            return resutList;
        }

        public List<int> getValueResponderPRA(int userId)
        {
            var dataList = (from ticket in _ctx.Ticket
                            where ticket.Responder == userId
                            select new
                            {
                                createdTicket = ticket.CreatedAt,
                                ticketId = ticket.TicketId,
                                getEscelated = _ctx.EscalateLog.Count(q => q.TicketId == ticket.TicketId && q.EscalateFrom == userId) > 0,
                                ticketStatus = ticket.Status,
                                ticketNextCommenter = ticket.NextCommenter,
                                ticketResponder = ticket.Responder,
                                dueDateAnswer = ticket.DueDateAnswer,
                                ticketSubmiter = ticket.Submiter,
                                getLastNote = _ctx.TicketNote.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(odb => odb.CreatedAt).FirstOrDefault(),
                                getLastDiscussion = _ctx.TicketDiscussion.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(odb => odb.CreatedAt).FirstOrDefault(),
                            });

            var dataTemp = (from item in dataList
                            select new
                            {
                                createdTicket = item.createdTicket,
                                ticketId = item.ticketId,
                                getEscelated = _ctx.EscalateLog.Count(q => q.TicketId == item.ticketId && q.EscalateFrom == userId) > 0,
                                ticketStatus = item.ticketStatus,
                                ticketNextCommenter = item.ticketNextCommenter,
                                ticketResponder = item.ticketResponder,
                                dueDateAnswer = item.dueDateAnswer,
                                ticketSubmitter = item.ticketSubmiter,
                                getLastNote = (item.getLastNote != null) ? item.getLastNote.CreatedAt : item.createdTicket,
                                getLastDiscussion = (item.getLastDiscussion != null) ? item.getLastDiscussion.CreatedAt : item.createdTicket,
                            }).ToList();

            var listData = (from item in dataTemp
                            select new
                            {
                                ticketId = item.ticketId,
                                ticketCreated = item.createdTicket,
                                getEscalated = item.getEscelated,
                                ticketStatus = item.ticketStatus,
                                ticketResponder = item.ticketResponder,
                                ticketSubmitter = item.ticketSubmitter,
                                ticketNextCommenter = item.ticketNextCommenter,
                                getCount = getCountDateBeforeThreeDays(item.dueDateAnswer.Value)
                            });
            var countTicketCloseLessThanTwoDay = (from item in listData
                                                  where item.getCount == 1 && item.ticketResponder == item.ticketNextCommenter && item.ticketNextCommenter == userId && item.ticketStatus == 2 && item.ticketNextCommenter != item.ticketSubmitter
                                                  select new
                                                  {
                                                      ticketId = item.ticketId
                                                  }).ToList();
            var countTicketCloseLessThanThreeDay = (from item in listData
                                                    where item.getCount == 2 && item.ticketResponder == item.ticketNextCommenter && item.ticketNextCommenter == userId && item.ticketStatus == 2 && item.ticketNextCommenter != item.ticketSubmitter
                                                    select new
                                                    {
                                                        ticketId = item.ticketId
                                                    }).ToList();
            var countTicketCloseMoreThanThreeDay = (from item in listData
                                                    where item.getCount == 3 && item.ticketResponder == item.ticketNextCommenter && item.ticketNextCommenter == userId && item.ticketStatus == 2 && item.ticketNextCommenter != item.ticketSubmitter
                                                    select new
                                                    {
                                                        ticketId = item.ticketId
                                                    }).ToList();

            var countData1 = countTicketCloseLessThanTwoDay.Count();
            var countData2 = countTicketCloseLessThanThreeDay.Count();
            var countData3 = countTicketCloseMoreThanThreeDay.Count();

            var resutList = new List<int>();
            resutList.Add(countData1);
            resutList.Add(countData2);
            resutList.Add(countData3);

            return resutList;
        }

        public List<int> getvalueResponderEscalated(int userId)
        {
            var dataList = (from ticket in _ctx.Ticket
                            where ticket.Responder == userId
                            select new
                            {
                                createdTicket = ticket.CreatedAt,
                                ticketId = ticket.TicketId,
                                getEscelated = _ctx.EscalateLog.Count(q => q.TicketId == ticket.TicketId && q.EscalateFrom == userId) > 0,
                                ticketStatus = ticket.Status,
                                ticketNextCommenter = ticket.NextCommenter,
                                ticketResponder = ticket.Responder,
                                dueDateAnswer = ticket.DueDateAnswer,
                                getLastNote = _ctx.TicketNote.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(odb => odb.CreatedAt).FirstOrDefault(),
                                getLastDiscussion = _ctx.TicketDiscussion.Where(w => w.TicketId == ticket.TicketId).OrderByDescending(odb => odb.CreatedAt).FirstOrDefault(),
                            });

            var dataTemp = (from item in dataList
                            select new
                            {
                                createdTicket = item.createdTicket,
                                ticketId = item.ticketId,
                                getEscelated = _ctx.EscalateLog.Count(q => q.TicketId == item.ticketId && q.EscalateFrom == userId) > 0,
                                ticketStatus = item.ticketStatus,
                                ticketNextCommenter = item.ticketNextCommenter,
                                ticketResponder = item.ticketResponder,
                                dueDateNaswer = item.dueDateAnswer,
                                getLastNote = (item.getLastNote != null) ? item.getLastNote.CreatedAt : item.createdTicket,
                                getLastDiscussion = (item.getLastDiscussion != null) ? item.getLastDiscussion.CreatedAt : item.createdTicket,
                            }).ToList();

            var listData = (from item in dataTemp
                            select new
                            {
                                ticketId = item.ticketId,
                                ticketCreated = item.createdTicket,
                                getEscalated = item.getEscelated,
                                ticketStatus = item.ticketStatus,
                                ticketResponder = item.ticketResponder,
                                ticketNextCommenter = item.ticketNextCommenter,
                                getCount = getCountDateBeforeThreeDays(item.dueDateNaswer.Value)
                            });

            var countTicketCloseLessThanTwoDay = (from item in listData
                                                  where item.getCount == 1 && item.getEscalated
                                                  select new
                                                  {
                                                      ticketId = item.ticketId
                                                  }).ToList();
            var countTicketCloseLessThanThreeDay = (from item in listData
                                                    where item.getCount == 2 && item.getEscalated
                                                    select new
                                                    {
                                                        ticketId = item.ticketId
                                                    }).ToList();
            var countTicketCloseMoreThanThreeDay = (from item in listData
                                                    where item.getCount == 3 && item.getEscalated
                                                    select new
                                                    {
                                                        ticketId = item.ticketId
                                                    }).ToList();

            var countData1 = countTicketCloseLessThanTwoDay.Count();
            var countData2 = countTicketCloseLessThanThreeDay.Count();
            var countData3 = countTicketCloseMoreThanThreeDay.Count();

            var resutList = new List<int>();
            resutList.Add(countData1);
            resutList.Add(countData2);
            resutList.Add(countData3);

            return resutList;
        }


        public int getCountDateBeforeThreeDays(DateTime dueDateAnswer)
        {
            int count = 0;
            // 1 = hijau 2= kuning 3=merah
            if(dueDateAnswer > DateTime.Today)
            {
                count = 1;
            }
            else if(dueDateAnswer == DateTime.Today)
            {
                count = 2;
            }
            else if(dueDateAnswer < DateTime.Today)
            {
                count = 3;
            }
            //TimeSpan data = new TimeSpan();
            //if(dueDateAnswer != null)
            //{
            //    data = DateTime.Today - dueDateAnswer;
            //    count = data.Days;
            //}
            return count;
        }

        public string getDateUpdate(DateTime lastNote, DateTime lastDiscuss, DateTime createTicket)
        {
            var date = "";
            if(lastDiscuss != null || lastNote != null)
            {
                if(lastDiscuss != createTicket || lastNote != createTicket)
                {
                    if (lastDiscuss > lastNote)
                    {
                        date = lastDiscuss.ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        date = lastNote.ToString("yyyy-MM-dd");
                    }
                }
                else
                {
                    date = "-";
                }
            }
            else
            {
                date = "-";
            }
            return date;
        }

        public string getDateCloseTicket(DateTime ticketClose, DateTime ticketCreate)
        {
            var date = "";
            if(ticketClose == ticketCreate)
            {
                date = "-";
            }
            else
            {
                date = ticketClose.ToString("yyyy-MM-dd");
            }
            return date;
        }
        
        public Tuple <List<TicketAPIDashboardOverview>,int> getListTicketBySerialNumber(string[]serialnumber)
        {
            List<TicketAPIDashboardOverview> ticketList = new List<TicketAPIDashboardOverview>();
            
            if (serialnumber.Length > 0)
            {
                int[] stat = { 2, 3, 4, 6 };

                var listticket = _ctx.Ticket.Where(item =>  stat.Contains(item.Status)).ToList();
                int countticket = listticket.Count();
               
                foreach (var data in listticket)
                {
                    if (serialnumber.Contains(data.SerialNumber))
                    {
                        TicketAPIDashboardOverview ticket = new TicketAPIDashboardOverview();
                        ticket.TicketNo = data.TicketNo;
                        ticket.TicketId = data.TicketId.ToString();
                        ticket.Title = data.Title;
                        ticket.Responder = data.Responder;
                        ticket.Submiter = data.Submiter;
                        ticket.NextCommenter = data.NextCommenter;
                        ticket.Description = data.Description;
                        string Resolution = _ctx.TicketResolution.Where(user => user.TicketId.Equals(data.TicketId)).Select(item => item.Description).FirstOrDefault();
                        ticket.Resolution = String.IsNullOrWhiteSpace(Resolution) ? "N/A" : Resolution;
                        ticket.CreatedAt = data.CreatedAt;
                        ticket.Status = data.Status;
                        ticketList.Add(ticket);
                    }
                }
               
                return Tuple.Create(ticketList.ToList(), ticketList.Count());
            }
            else
            {
                var getData = _ctx.Ticket.Where(item => true).Take(0);
                foreach (var data in getData)
                {
                    TicketAPIDashboardOverview ticket = new TicketAPIDashboardOverview();
                    ticket.TicketNo = data.TicketNo;
                    ticket.TicketId = data.TicketId.ToString();
                    ticket.Title = data.Title;
                    ticket.Responder = data.Responder;
                    ticket.Submiter = data.Submiter;
                    ticket.NextCommenter = data.NextCommenter;
                    ticket.Description = data.Description;
                    ticket.Resolution =  "N/A";
                    ticket.CreatedAt = null;
                    ticket.Status = data.Status;
                    ticketList.Add(ticket);
                }
                return Tuple.Create(ticketList.ToList(), 0);
            }
        }

        public Ticket CheckTicketReferences(int TicketId)
        {
            var result = _ctx.Ticket.Where(w => w.ReferenceTicket == TicketId).OrderByDescending(ob => ob.CreatedAt).FirstOrDefault();
            return result == null ? null : result;
        }

        public Ticket getTicketByTicketReferences(int TicketId)
        {
            var result = _ctx.Ticket.Where(w => w.TicketId == TicketId).FirstOrDefault();
            return result == null ? null : result;
        }
    } 
}
