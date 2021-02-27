using Com.Trakindo.Framework;
using Com.Trakindo.TSICS.Data.Context;
using Com.Trakindo.TSICS.Data.Model;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Com.Trakindo.TSICS.Business.Service
{
    public class TicketParcipantBusinessService
    {
        private readonly TsicsContext _dBtsics = Factory.Create<TsicsContext>("TsicsContext", ClassType.clsTypeDataContext);
        public List<TicketParcipant> GetList()
        {
            List<TicketParcipant> result = _dBtsics.TicketParcipant
                .ToList();

            return result;
        }

        public List<UserDevices> Getuserdevice(List<string> uid)
        {
            List<UserDevices> result = _dBtsics.UserDevices.Where(i => uid.Contains(i.UserId.ToString())).ToList();
            return result;
        }
        public UserDevices GetuserdeviceResponder(int UserId)
        {
            return _dBtsics.UserDevices.FirstOrDefault(i => i.UserId == UserId);
        }
        public List<UserDevices> getuserdevicesresponder(int idu)
        {
            List<UserDevices> Result = _dBtsics.UserDevices.Where(i => i.UserId == idu).ToList();
            return Result;
        }
        public List<UserDevices> Getuserdeviceforasnote(List<int> uid)
        {
            List<UserDevices> result = _dBtsics.UserDevices.Where(i => uid.Contains(i.UserId)).ToList();
            return result;
        }


        public TicketParcipant Add(TicketParcipant ticketParcipant)
        {
            _dBtsics.TicketParcipant.Add(ticketParcipant);
            _dBtsics.SaveChanges();
            return ticketParcipant;
        }
        public TicketParcipant Edit(TicketParcipant ticketParcipant)
        {
            _dBtsics.Entry(ticketParcipant).State = EntityState.Modified;
            _dBtsics.SaveChanges();
            return ticketParcipant;
        }
        public int Delete(TicketParcipant ticketParcipant)
        { 
                _dBtsics.TicketParcipant.Remove(ticketParcipant);
                var deleteParticipant = _dBtsics.SaveChanges();
                return deleteParticipant;
        }

        public TicketParcipant GetDetail(int id)
        {
            TicketParcipant ticketParcipant = _dBtsics.TicketParcipant.Find(id);
            return (ticketParcipant);
        }
        public void DeleteAllParticipant(int tickedId)
        {
            using (TsicsContext db = new TsicsContext())
            {
                db.Database.ExecuteSqlCommand("Delete from TicketParcipant where TicketId = '" + tickedId + "'");
            }
        }
        public List<TicketParcipant> GetByTicket(int ticketId)
        {
            return _dBtsics.TicketParcipant.Where(q => q.TicketId == ticketId).ToList();
        }
        public List<int> GetTicketIdBySearchUserId(string search)
        {
            int[] userData = _dBtsics.User.Where(q => q.Name.Contains(search)).Select(q => q.UserId).ToArray(); 
            return _dBtsics.TicketParcipant.Where(q => userData.Contains(q.UserId)).Select(q => q.TicketId).ToList();
            
        }
        public List<TicketParticipantWithName> GetParticipantWithName(List<TicketParcipant> dataList)
        {
            var resultList = (
                from ticketParticipant in dataList
                from user in _dBtsics.User.Where(w => w.UserId == ticketParticipant.UserId).DefaultIfEmpty()
                where user != null
                select new TicketParticipantWithName
                {
                    TicketParcipant = ticketParticipant,
                    Name = user.Name,
                    UserId = ticketParticipant.UserId
                }
            ).ToList();

            return resultList;
        }

        public void CheckExistingParticipants(int ticketId, int userId)
        {
            List<TicketParcipant> existingParticipants = _dBtsics.TicketParcipant.Where(q => q.TicketId == ticketId && q.UserId == userId).ToList();
            if(existingParticipants.Count != 0)
            {
                foreach(TicketParcipant participant in existingParticipants)
                {
                    Delete(participant);
                }
            }
        }

        public void BulkDeleteByTicket(int ticketId)
        {
            using (TsicsContext db = new TsicsContext())
            {
                db.Database.ExecuteSqlCommand("DELETE FROM TicketParcipant WHERE TicketId = {0}", ticketId);
            }
        }

    }
}
