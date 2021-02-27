using Com.Trakindo.Framework;
using Com.Trakindo.TSICS.Data.Context;
using Com.Trakindo.TSICS.Data.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Net;
using System.Reflection;
using System.Diagnostics;
namespace Com.Trakindo.TSICS.Business.Service
{
    public class UserBusinessService
    {
        private readonly TsicsContext _dBtsics = Factory.Create<TsicsContext>("TsicsContext", ClassType.clsTypeDataContext);
      
        public List<User> GetList()
        {
            List<User> result = _dBtsics.User
                .Where(r => r.Status != 8 && r.EmployeeId != 0)
                .OrderByDescending(o => o.CreatedAt)
                .ToList();

            return result;
        }
        public List<User> GetListApprove(int ap)
        {
            List<User> result = _dBtsics.User
                .Where(a => a.Status == ap)
                .ToList();

            return result;
        }
        public List<User> GetListAdmin()
        {
            List<User> result = _dBtsics.User
                .Where(a => a.IsAdmin == 1)
                .ToList();

            return result;
        }
        public List<User> GetListOtherAdmin()
        {
            List<User> result = _dBtsics.User
                .Where(a => a.IsAdmin == 1 && a.Username != "Technical.c.admin1".ToUpper())
                .ToList();

            return result;
        }
        public User GetListFirstAdmin(String username)
        {
           return _dBtsics.User
                .Where(a => a.IsAdmin == 1 && a.Username.Contains(username))
                .FirstOrDefault();
        }
        public User Add(User usr)
        {
            _dBtsics.User.Add(usr);
            _dBtsics.SaveChanges();
            return usr;
        }
        public User Edit(User usr)
        {
            _dBtsics.Entry(usr).State = EntityState.Modified;
            _dBtsics.SaveChanges();
            return usr;
        }

        public User GetDetail(int id)
        {
            User usr = _dBtsics.User.Find(id);
            return (usr);
        }
        public bool CheckName(string name)
        {
            return _dBtsics.User.Count(user => user.Name == name) > 0;
        }
        public User GetDetailbyUsername(string usrnm)
        {
            if (usrnm == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User usr = _dBtsics.User.SingleOrDefault(u => u.Username.Contains(usrnm));
            if (usr == null)
            {
                //return HttpNotFound();
            }
            return (usr);
        }
        public User GetDetailByEmployeeId(int id)
        {
            User usr = _dBtsics.User.FirstOrDefault(u => u.EmployeeId.Equals(id));
            return (usr);
        }
        public bool CheckUser(int id)
        {
            int[] reject = {0,4, 6, 8, 10 };
            return _dBtsics.User.Count(e => e.EmployeeId == id && (!reject.Contains(e.Status))) > 0;
        }
        public bool CheckUserReject(int id)
        {
            int[] reject = { 4, 6, 8, 10 };
            return _dBtsics.User.Count(e => e.EmployeeId == id && (reject.Contains(e.Status))) > 0;
        }
        public bool CheckUserName(string usrnm)
        {
            return _dBtsics.User.Count(e => e.Username == usrnm && e.Status == 7) > 0;
        }
        public bool CheckDefaultAdmin(String username)
        {
            return _dBtsics.User.Count(e => e.Username.Contains(username) && e.Status == 7 && e.IsAdmin == 1) > 0;
        }
        public User CheckLoginMobile(MobileLogin login)
        {
            return _dBtsics.User
                .FirstOrDefault(q => q.Username.Equals(login.Xupj) && q.MobilePassword.Equals(login.Password));
        }

        public UserDevices CheckUserDevices(int iduser, string playerid)
        {
            return _dBtsics.UserDevices.FirstOrDefault(s => s.Status == 1 && s.UserId == iduser && s.PlayerId.Equals(playerid));
        }

        public User GetByToken(string token)
        {
            return _dBtsics.User.FirstOrDefault(user => user.MobileToken.Equals(token));
        }


        public List<User> GetSuggestion(int requesterUserId, string userNameToSearch="")
        {
            
            List<User> userList = new List<User>();
            var allUsersMatchHierarchyRole = GetUserListByHierarchy(requesterUserId);

            foreach (var userItem in allUsersMatchHierarchyRole)
            {

                //if (userItem.Username.ToUpper().Contains(userNameToSearch.ToUpper()))
                //{
                    userList.Add(userItem);
                //}
                //else if()
            }

            return userList;
        }
        public List<User> GetSuggestionParticipant(int requesterUserId, string userNameToSearch)
        {
            List<User> userList = new List<User>();
            var allUsersMatchHierarchyRole = GetListActive();

            foreach (var userItem in allUsersMatchHierarchyRole)
            {
                if (userItem.Username.ToUpper().Contains(userNameToSearch.ToUpper()))
                {
                    userList.Add(userItem);
                }
            }
            return userList;
        }
        public User GetByXupj(string xupj)
        {
            return _dBtsics.User.FirstOrDefault(user => user.Username.Equals(xupj));
        }
        public bool CheckIsAdmin(string usrnm)
        {
            return _dBtsics.User.Count(e => e.Username == usrnm && e.Status == 0 && e.IsAdmin == 1) > 0;
        }

        public bool CheckUsernamePassword(string usrnm, string password)
        {
            return _dBtsics.User.Count(u => u.Username.Equals(usrnm) && u.AdminPassword.Equals(password)) > 0;
        }
        public List<User> GetListActive()
        {
            List<User> result = _dBtsics.User
                .Where(r => r.Status == 7)
                .OrderBy(u => u.Name).ToList();

            return result;
        }
        public List<User> GetListUserNonDelegate()
        {
            List<User> result = _dBtsics.User
                .Where(r => r.Status == 7 && r.IsDelegate.Equals(0))
                .OrderBy(u => u.Name).ToList();

            return result;
        }
        public string GetListEmailParticipant(List<int> ipd, bool forAdmin = false)
        {
            string result = "";
            if (!forAdmin)
            {
                List<User> lUser = _dBtsics.User
                    .Where(r => ipd.Contains(r.UserId))
                    .Where(s => s.Status == 7)
                    .ToList();
                foreach (var item in lUser)
                {
                    result += item.Email + ";";
                }
            }
            else
            {
                List<User> listAdmin = _dBtsics.User
                   .Where(r => ipd.Contains(r.UserId))
                   .Where(s => s.IsAdmin == 1)
                   .ToList();
                foreach (var item in listAdmin)
                {
                    result += item.Email + ";";
                }
            }
            
            return result;
        }
      
        public User GetByEmployeeId(int employeeId)
        {
            return _dBtsics.User.FirstOrDefault(user => user.EmployeeId.Equals(employeeId));
        }
        public List<int> GetListSearchUser(string searchString)
        {
            var result = _dBtsics.User.Where(e => e.Name.Contains(searchString)).Select(u => u.UserId).ToList();
            return result;
        }
        public List<User> GetListNeedApproveTsManager()
        {
            DateTime dt = DateTime.Now.Date;

            List<User> result = _dBtsics.User
                .Where(a => a.Status == 5 && a.UserTsManagerDueFlag != 0)
                .Where(q => DbFunctions.TruncateTime(q.UserTsManager1DueDate) <= dt || DbFunctions.TruncateTime(q.UserTsManager2DueDate) <= dt || DbFunctions.TruncateTime(q.UserTsManager3DueDate) <= dt)
                .ToList();

            return result;
        }

        public List<User> GetListNeedRejectAuto()
        {
            int[] spv = { 2, 3 };

            DateTime dt = DateTime.Now.Date;

            List<User> result = _dBtsics.User
                .Where(a => spv.Contains(a.Status))
                .Where(q => DbFunctions.TruncateTime(q.DueDateApproval1) <= dt || DbFunctions.TruncateTime(q.DueDateApproval2) <= dt)
                .ToList();

            return result;
        }

        public List<string> GetSuggestionRoleDesc(string spesification)
        {
            List<string> suggestion = new List<string>();
            List<User> listRoleDesc = _dBtsics.User
                .Where(q => q.RoleDescription.Contains(spesification))
                .ToList();

            foreach (var item in listRoleDesc)
            {
                suggestion.Add(item.RoleDescription);
            }

            suggestion = suggestion.Distinct().ToList();
            return suggestion;
        }
        
        public List<User> GetUserListByHierarchy(int userId)
        {
            var submiter = _dBtsics.User.Find(userId);
            List<User> userList = new List<User>();
            if (submiter != null)
            {
                User submiterdata = new User()
                {
                    UserId = userId,
                    RoleId = submiter.RoleId,
                    MasterAreaId = submiter.MasterAreaId,
                    MasterBranchId = submiter.MasterBranchId
                };
                switch (submiterdata.RoleId)
                {
                    case 1:// Level 1
                        if (submiterdata.MasterAreaId == 0 && submiterdata.MasterBranchId == 0) //Level 1 - HO
                        {
                          return Hierarchy_Level1_HO(submiterdata);
                        }
                        else if (submiterdata.MasterAreaId != 0 && submiterdata.MasterBranchId == 0)  //Level 1 - Area
                        {
                            userList = Hierarchy_Level1_Area(submiterdata);
                            if (userList.Count == 0)
                            {
                                return Hierarchy_Level1_HO(submiterdata);
                            }
                            return userList;
                        }
                        else if (submiterdata.MasterAreaId != 0 && submiterdata.MasterBranchId != 0)  // Level 1 - Branch
                        {
                            userList = Hierarchy_Level1_Branch(submiterdata);
                            if (userList.Count == 0)
                            {
                                userList = Hierarchy_Level1_Area(submiterdata);
                                if (userList.Count == 0)
                                {
                                    return Hierarchy_Level1_HO(submiterdata);
                                }
                                return userList;
                            }
                            return userList;
                        }
                        return null;
                    case 2:// Level 2

                        if (submiterdata.MasterAreaId == 0 && submiterdata.MasterBranchId == 0) //Level2 - HO
                        {
                            return Hierarchy_Level2_HO(submiterdata);
                        }
                        else if (submiterdata.MasterAreaId != 0 && submiterdata.MasterBranchId == 0) //Level 2 - Area
                        {
                            userList = Hierarchy_Level2_Area(submiterdata);
                            if (userList.Count == 0)
                            {
                                return Hierarchy_Level2_HO(submiterdata);
                            }
                            return userList;
                        }
                        else if (submiterdata.MasterAreaId != 0 && submiterdata.MasterBranchId != 0) //Level 2 - Branch
                        {
                            userList = Hierarchy_Level2_Branch(submiterdata);
                            if(userList.Count == 0)
                            {
                                userList = Hierarchy_Level2_Area(submiterdata);
                                if (userList.Count == 0)
                                {
                                    return Hierarchy_Level2_HO(submiterdata);
                                }
                                return userList;
                            }
                            return userList;
                        }
                        return null;

                    case 3://TC Area
                        return Hierarchy_TCArea(submiterdata);
                    case 4://TC HO
                        return Hierarchy_TCHO(submiterdata);
                }
            }

            return userList;
        }

        private List<User> Hierarchy_Level1_HO(User userdata)
        {
            return _dBtsics.User
                           .Where(q => (q.RoleId == 2 && q.MasterAreaId == 0 && q.MasterBranchId == 0) ||
                           (q.RoleId == 4))

                               .Where(q => q.Status == 7 && q.UserId != userdata.UserId && q.IsDelegate.Equals(0))
                               .OrderBy(u => u.Name)
                           .ToList();
        }
        private List<User> Hierarchy_Level1_Area(User userdata)
        {
            return  _dBtsics.User
                           .Where(q => (q.RoleId == 2 && q.MasterAreaId.Equals(userdata.MasterAreaId) && q.MasterBranchId == 0) ||
                           (q.RoleId == 3 && q.MasterAreaId.Equals(userdata.MasterAreaId) && q.MasterBranchId == 0))
                               .Where(q => q.Status == 7 && q.UserId != userdata.UserId && q.IsDelegate.Equals(0))
                               .OrderBy(u => u.Name)
                           .ToList();
        }
        private List<User> Hierarchy_Level1_Branch(User userdata)
        {
            return _dBtsics.User
                          .Where(q => q.RoleId.Equals(2) && q.MasterBranchId.Equals(userdata.MasterBranchId) && q.MasterAreaId.Equals(userdata.MasterAreaId))
                              .Where(q => q.Status == 7 && q.UserId != userdata.UserId && q.IsDelegate.Equals(0))
                              .OrderBy(u => u.Name)
                          .ToList();
        }
        private List<User> Hierarchy_Level2_HO(User userdata)
        {
           return _dBtsics.User
                          .Where(q => (q.RoleId.Equals(4)) ||
                          (q.RoleId.Equals(2) && q.MasterAreaId != 0 && q.MasterBranchId == 0) ||
                          (q.RoleId.Equals(3) && q.MasterAreaId != 0 && q.MasterBranchId == 0))
                              .Where(q => q.Status == 7 && q.UserId != userdata.UserId && q.IsDelegate.Equals(0))
                              .OrderBy(u => u.Name)
                          .ToList();
        }
        private List<User> Hierarchy_Level2_Area(User userdata)
        {
           return _dBtsics.User
                          .Where(q => ((q.RoleId.Equals(3) && q.MasterAreaId.Equals(userdata.MasterAreaId) && q.MasterBranchId == 0) ||
                          (q.RoleId.Equals(2) && q.MasterAreaId == 0 && q.MasterBranchId == 0) ||
                          (q.RoleId.Equals(2) && q.MasterAreaId.Equals(userdata.MasterAreaId) && q.MasterBranchId != 0)))
                          .Where(q => q.Status == 7 && q.UserId != userdata.UserId && q.IsDelegate.Equals(0))
                          .OrderBy(u => u.Name)
                          .ToList();
        }
        private List<User> Hierarchy_Level2_Branch(User userdata)
        {
            return _dBtsics.User
                          .Where(q => (q.RoleId == 2 && q.MasterAreaId.Equals(userdata.MasterAreaId) && q.MasterBranchId == 0) ||
                          (q.RoleId == 3 && q.MasterAreaId.Equals(userdata.MasterAreaId) && q.MasterBranchId == 0) ||
                          (q.RoleId == 1 && q.MasterAreaId.Equals(userdata.MasterAreaId) && q.MasterBranchId.Equals(userdata.MasterBranchId)))
                              .Where(q => q.Status == 7 && q.UserId != userdata.UserId && q.IsDelegate.Equals(0))
                              .OrderBy(u => u.Name)
                          .ToList();
        }
        private List<User> Hierarchy_TCArea(User userdata)
        {
            return _dBtsics.User
                      .Where(q => (q.RoleId.Equals(2) && q.MasterAreaId.Equals(userdata.MasterAreaId) && q.MasterBranchId == 0 ||
                      (q.RoleId.Equals(2) && q.MasterAreaId == 0 && q.MasterBranchId == 0) ||
                      (q.RoleId.Equals(4) && q.MasterAreaId == 0 && q.MasterBranchId == 0) ||
                      (q.RoleId.Equals(3) && q.MasterAreaId != 0 && q.MasterBranchId == 0) ||
                      (q.RoleId.Equals(2) && q.MasterAreaId.Equals(userdata.MasterAreaId) && q.MasterBranchId != 0)))
                          .Where(q => q.Status == 7 && q.UserId != userdata.UserId && q.IsDelegate.Equals(0))
                          .OrderBy(u => u.Name)
                      .ToList();
        }
        private List<User> Hierarchy_TCHO(User userdata)
        {
           return _dBtsics.User
                           .Where(q => (q.RoleId.Equals(2) && q.MasterAreaId.Equals(0) && q.MasterBranchId.Equals(0)) ||
                           (q.RoleId.Equals(3) && q.MasterAreaId != 0 && q.MasterBranchId == 0) ||
                           (q.RoleId.Equals(2) && q.MasterAreaId != 0 && q.MasterBranchId == 0) ||
                           (q.RoleId.Equals(4)))
                           .Where(q => q.Status == 7 && q.UserId != userdata.UserId && q.IsDelegate.Equals(0))
                           .OrderBy(u => u.Name)
                           .ToList();
        }


        public void InsertData(UserDevices userDevices)
        {
            var cek = GetPlayerId(userDevices.UserId, userDevices.PlayerId);
            if (cek == null)
            {
                userDevices.Status = 1;
                _dBtsics.UserDevices.Add(userDevices);
                _dBtsics.SaveChanges();

            }
        }

        public UserDevices GetPlayerId(int userid, string playerId)
        {
            UserDevices result = _dBtsics.UserDevices.
                Where(uid => uid.UserId == userid).
                SingleOrDefault(pid => pid.PlayerId == playerId);
            return result;
        }

        #region Delegation Function

        public List<Delegation> GetListDelegation()
        {
            List<Delegation> result = _dBtsics.Delegation.Where(d => d.Status != 0)
                .OrderByDescending(d => d.CreatedAt).ToList();
            return result;
        }

        public Delegation GetDetailDelegation(int id)
        {

            Delegation delegateData = _dBtsics.Delegation.Find(id);
            return delegateData;
        }
        public Delegation GetDetailDelegationByUserFrom(int id)
        {
            return _dBtsics.Delegation.OrderByDescending(d =>d.CreatedAt).FirstOrDefault(d => d.FromUser.Equals(id));
        }
        public Delegation GetDetailDelegationByUserTo(int id)
        {
            return _dBtsics.Delegation.OrderByDescending(d => d.CreatedAt).FirstOrDefault(d => d.ToUser.Equals(id));
        }

        public Delegation AddDelegation(Delegation delegation)
        {
            _dBtsics.Delegation.Add(delegation);
            _dBtsics.SaveChanges();
            using (TsicsContext db = new TsicsContext())
            {
                db.Database.ExecuteSqlCommand("UPDATE UserTsics set IsDelegate = " + delegation.DelegateId + "where UserId = " + delegation.FromUser);
            }
            if (delegation.CreatedAt >= delegation.StartDate)
            {
                DelegateUser(delegation);
            }
            return delegation;
        }
        public void DelegateUser(Delegation delegation)
        { 
            using (TsicsContext db = new TsicsContext())
            {
                var DelegationData = _dBtsics.Delegation.OrderByDescending(d => d.CreatedAt).FirstOrDefault().DelegateId;

                db.Database.ExecuteSqlCommand("update ticket set ticket.Responder = " + delegation.ToUser + ",ticket.NextCommenter = " + delegation.ToUser + ", DelegateId = " + DelegationData + " where (ticket.Responder = " + delegation.FromUser + ") and (ticket.Status != 3 and ticket.Status != 5) and (ticket.DelegateId = 0 ) and (ticket.NextCommenter = ticket.Responder)");

                db.Database.ExecuteSqlCommand("update ticket set ticket.Responder = " + delegation.ToUser + ", DelegateId = " + DelegationData + " where (ticket.Responder = " + delegation.FromUser + ") and (ticket.Status != 3 and ticket.Status != 5) and (ticket.DelegateId = 0 ) and (ticket.NextCommenter != ticket.Responder)");

                db.Database.ExecuteSqlCommand("update ticket set ticket.Submiter = " + delegation.ToUser + ", ticket.NextCommenter = " + delegation.ToUser + ", DelegateId = " + DelegationData + " where(ticket.Submiter = " + delegation.FromUser + ") and (ticket.Status != 3 and ticket.Status != 5) and (ticket.DelegateId = 0 ) and (ticket.NextCommenter = ticket.Submiter)");

                db.Database.ExecuteSqlCommand("update ticket set ticket.Submiter = " + delegation.ToUser + ", DelegateId = " + DelegationData + " where(ticket.Submiter = " + delegation.FromUser + ") and (ticket.Status != 3 and ticket.Status != 5) and (ticket.DelegateId = 0 ) and (ticket.NextCommenter != ticket.Submiter)");

                db.Database.ExecuteSqlCommand("update ticketparcipant set ticketparcipant.userid = " + delegation.ToUser + " where (ticketparcipant.userid = " + delegation.FromUser + ")");

            }

            
        }

        public Delegation EditDelegation(Delegation delegation, int id, int currentuser)
        {
            using (TsicsContext db = new TsicsContext())
            {
                db.Database.ExecuteSqlCommand("UPDATE UserTsics set IsDelegate = " + id + " where UserId = " + delegation.FromUser);

                db.Database.ExecuteSqlCommand("update ticket set ticket.Responder = " + delegation.ToUser + ", ticket.NextCommenter = " + delegation.ToUser + " where (ticket.Responder = " + currentuser + ") and (ticket.Status != 3 and ticket.Status != 5) and (ticket.DelegateId = " + id + " ) and (ticket.Responder = Ticket.NextCommenter)");
                db.Database.ExecuteSqlCommand("update ticket set ticket.Responder = " + delegation.ToUser + " where (ticket.Responder = " + currentuser + ") and (ticket.Status != 3 and ticket.Status != 5) and (ticket.DelegateId = " + id + " ) and (ticket.Responder != Ticket.NextCommenter)");

                db.Database.ExecuteSqlCommand("update ticket set ticket.Submiter = " + delegation.ToUser + ", ticket.NextCommenter = " + delegation.ToUser + " where (ticket.Submiter = " + currentuser + ") and (ticket.Status != 3 and ticket.Status != 5) and (ticket.DelegateId = " + id + " ) and (ticket.Submiter = Ticket.NextCommenter)");

                db.Database.ExecuteSqlCommand("update ticket set ticket.Submiter = " + delegation.ToUser + " where (ticket.Submiter = " + currentuser + ") and (ticket.Status != 3 and ticket.Status != 5) and (ticket.DelegateId = " + id + " ) and (ticket.Submiter != Ticket.NextCommenter)");

                db.Database.ExecuteSqlCommand("update ticketparcipant set ticketparcipant.userid = " + delegation.ToUser + " where (ticketparcipant.userid = " + currentuser + ")");

            }
            _dBtsics.Entry(delegation).State = EntityState.Modified;
            _dBtsics.SaveChanges();
            return delegation;
        }


        public Delegation DeleteDelegation(Delegation delegation, int id)
        {
           
                using (TsicsContext db = new TsicsContext())
                {
                db.Database.ExecuteSqlCommand("UPDATE UserTsics set IsDelegate = 0 where UserId = " + delegation.FromUser);

                db.Database.ExecuteSqlCommand("update ticket set ticket.Responder = " + delegation.FromUser + ", ticket.DelegateId = 0 where (ticket.Responder = " + delegation.ToUser + ") and (ticket.Status != 3 and ticket.Status != 5) and (ticket.DelegateId = " + id + " ) and (ticket.Responder != ticket.NextCommenter)");

                db.Database.ExecuteSqlCommand("update ticket set ticket.Responder = " + delegation.FromUser + ", ticket.NextCommenter = " + delegation.FromUser + ", ticket.DelegateId = 0 where (ticket.Responder = " + delegation.ToUser + ") and (ticket.Status != 3 and ticket.Status != 5) and (ticket.DelegateId = " + id + " ) and (ticket.Responder = ticket.NextCommenter)");

                db.Database.ExecuteSqlCommand("update ticket set ticket.Submiter = " + delegation.FromUser + ", ticket.DelegateId = 0 where(ticket.Submiter = " + delegation.ToUser + ") and (ticket.Status != 3 and ticket.Status != 5) and (ticket.DelegateId = " + id + " ) and (ticket.Submiter != ticket.NextCommenter)");

                db.Database.ExecuteSqlCommand("update ticket set ticket.Submiter = " + delegation.FromUser + ", ticket.NextCommenter = " + delegation.FromUser + ", ticket.DelegateId = 0 where (ticket.Submiter = " + delegation.ToUser + ") and (ticket.Status != 3 and ticket.Status != 5) and (ticket.DelegateId = " + id + " ) and (ticket.Submiter = ticket.NextCommenter)");

                db.Database.ExecuteSqlCommand("update ticketparcipant set ticketparcipant.userid = " + delegation.FromUser + " where (ticketparcipant.userid = " + delegation.ToUser + ")");

                }
            _dBtsics.Entry(delegation).State = EntityState.Modified;
            _dBtsics.SaveChanges();
            return delegation;
        }
        public void EndDelegate()
        {
            List<Delegation> result = _dBtsics.Delegation.Where(d => DateTime.Now > d.EndDate && d.Status.Equals(1)).ToList();
            if (result.Count > 0)
            {
                using (TsicsContext db = new TsicsContext())
                {
                    foreach (var list in result)
                    {
                        db.Database.ExecuteSqlCommand("UPDATE UserTsics set IsDelegate = 0 where UserId = " + list.FromUser);
                        db.Database.ExecuteSqlCommand("UPDATE Delegation set Status = 2 where DelegateId = " + list.DelegateId);
                    }
                }
            }
        }
        public void StartDelegate()
        {
            List<Delegation> result = _dBtsics.Delegation.Where(d => DateTime.Now == d.StartDate && d.Status.Equals(3)).ToList();
            if (result.Count > 0)
            {
                using (TsicsContext db = new TsicsContext())
                {
                    foreach (var list in result)
                    {
                        db.Database.ExecuteSqlCommand("UPDATE UserTsics set IsDelegate = "+ list.DelegateId + " where UserId = " + list.FromUser);
                        db.Database.ExecuteSqlCommand("UPDATE Delegation set Status = 1 where DelegateId = " + list.DelegateId);
                        DelegateUser(list);
                    }
                }
            }
        }
      
        #endregion
    }
}
