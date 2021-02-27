using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Com.Trakindo.Framework;
using Com.Trakindo.TSICS.Business.Service;
using Com.Trakindo.TSICS.Data.Model;
using PagedList;
using TSICS.Helper;
namespace TSICS.Areas.Admin.Controllers
{

    public class DelegationController : Controller
    {
        private readonly UserBusinessService _userBService = Factory.Create<UserBusinessService>("User", ClassType.clsTypeBusinessService);
        private readonly UserRoleBusinessService _userRoleBService = Factory.Create<UserRoleBusinessService>("UserRole", ClassType.clsTypeBusinessService);
        private readonly TicketPreviewBusinessService _ticketPreviewBusinessService = Factory.Create<TicketPreviewBusinessService>("TicketPreview", ClassType.clsTypeBusinessService);
        private readonly TicketBusinessService _ticketBusinessService = Factory.Create<TicketBusinessService>("Ticket", ClassType.clsTypeBusinessService);
        private readonly TicketParcipantBusinessService _ticketParcipantBusinessService = Factory.Create<TicketParcipantBusinessService>("TicketParcipant", ClassType.clsTypeBusinessService);

        public ActionResult Index(int? page)
        {
            if (Common.CheckAdmin())
            {
                return RedirectToAction("Login", "Default");
            }
            var data = _userBService.GetListDelegation();

            List<User> UserFromData = new List<User>();
            List<User> UserToData = new List<User>();
            if (data != null)
            {
                foreach (var user in data)
                {
                    if (_userBService.GetDetail(user.FromUser) != null)
                    {
                        User userfromdata = new User()
                        {
                            UserId = user.DelegateId,
                            Name = _userBService.GetDetail(user.FromUser).Name
                        };
                        UserFromData.Add(userfromdata);
                    }
                    else
                    {
                        User userfromdata = new User()
                        {
                            UserId = user.DelegateId,
                            Name = ""
                        };
                        UserFromData.Add(userfromdata);
                    }

                    if (_userBService.GetDetail(user.ToUser) != null)
                    {
                        User usertodata = new User()
                        {
                            UserId = user.DelegateId,
                            Name = _userBService.GetDetail(user.ToUser).Name
                        };
                        UserToData.Add(usertodata);
                    }
                    else
                    {
                        User usertodata = new User()
                        {
                            UserId = user.DelegateId,
                            Name = ""
                        };
                        UserToData.Add(usertodata);
                    }


                }
            }
            ViewBag.FromUserData = UserFromData;
            ViewBag.ToUserData = UserToData;

            int PageSize = 999999999;
            int pageNumber = (page ?? 1);

            return View(data.ToPagedList(pageNumber, PageSize));
        }

        public ActionResult Details(int id)
        {
            if (Common.CheckAdmin())
            {
                return RedirectToAction("Login", "Default");
            }
            Delegation delegation = _userBService.GetDetailDelegation(id);

            ViewBag.FromUserData = _userBService.GetDetail(delegation.FromUser);
            ViewBag.ToUserData = _userBService.GetDetail(delegation.ToUser);


            return View(delegation);
        }

        public ActionResult Create()
        {
            ViewBag.Domain = WebConfigure.GetDomain();
            if (Common.CheckAdmin())
            {
                return RedirectToAction("Login", "Default");
            }
            var UserData = _userBService.GetListUserNonDelegate();
            ViewBag.Userlist = UserData;

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FormCollection formCollection)
        {
            Delegation delegation = new Delegation();

            if (Common.CheckAdmin())
            {
                return RedirectToAction("Login", "Default");
            }
            if (ModelState.IsValid)
            {
                User user = _userBService.GetDetail(Convert.ToInt32(formCollection["FromUser"]));
                user.MobileToken = Common.AccessToken() + user.UserId;
                user = _userBService.Edit(user);

                delegation.FromUser = Convert.ToInt32(formCollection["FromUser"]);
                delegation.ToUser = Convert.ToInt32(formCollection["ToUser"]);
                delegation.StartDate = Convert.ToDateTime(formCollection["StartDate"]);
                delegation.EndDate = Convert.ToDateTime(formCollection["EndDate"]);
               
                delegation.CreatedAt = DateTime.Now;
                delegation.Status = delegation.CreatedAt < delegation.StartDate ? 3 : 1;

                Delegation DelegationAddResult = _userBService.AddDelegation(delegation);
                int DelegateId = DelegationAddResult.DelegateId;
                if (Convert.ToInt32(Session["userid"]) == delegation.FromUser)
                {
                    Session["DelegateStatus"] = DelegationAddResult.Status;
                    Session["DelegateTo"] = _userBService.GetDetail(DelegationAddResult.ToUser).Name;
                    Session["DelegateUntil"] = delegation.EndDate;
                    Session["DelegateStart"] = delegation.StartDate;
                }
            }
            return RedirectToAction("Details/" + delegation.DelegateId, "Delegation");

        }

        public ActionResult Edit(int id)
        {
            ViewBag.Domain = WebConfigure.GetDomain();
            if (Common.CheckAdmin())
            {
                return RedirectToAction("Login", "Default");
            }
            Delegation delegation = _userBService.GetDetailDelegation(id);
            User fromUserData = _userBService.GetDetail(delegation.FromUser);
            User toUserData = _userBService.GetDetail(delegation.ToUser);

            ViewBag.FromUser = fromUserData;
            ViewBag.ToUser = toUserData;
            ViewBag.RoleName1 = _userRoleBService.GetDetail(fromUserData.RoleId).Name;
            ViewBag.RoleName2 = _userRoleBService.GetDetail(toUserData.RoleId).Name;
            var UserData = _userBService.GetListActive();
            ViewBag.Userlist = UserData;

            return View(delegation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, FormCollection formCollection)
        {
            if (Common.CheckAdmin())
            {
                return RedirectToAction("Login", "Default");
            }
           
            Delegation delegation = _userBService.GetDetailDelegation(id);
            User user = _userBService.GetDetail(delegation.FromUser);

            user.MobileToken = Common.AccessToken() + user.UserId;
            user = _userBService.Edit(user);

            User fromUserData = _userBService.GetDetail(delegation.FromUser);
            User toUserData = _userBService.GetDetail(delegation.ToUser);

            User fromuser = new User()
            {
                UserId = fromUserData.UserId,
                Name = fromUserData.Name
            };

            User touser = new User()
            {
                UserId = toUserData.UserId,
                Name = toUserData.Name
            };

            ViewBag.FromUser = fromuser;
            ViewBag.ToUser = touser;

            var UserData = _userBService.GetListActive();
            ViewBag.Userlist = UserData;

            var currentToUser = delegation.ToUser;
            delegation.DelegateId = id;
            delegation.FromUser = delegation.FromUser;
            delegation.ToUser = Convert.ToInt32(formCollection["ToUser"]);
            delegation.EndDate = Convert.ToDateTime(formCollection["EndDate"]);
            delegation.StartDate = Convert.ToDateTime(formCollection["StartDate"]);
            delegation.Status = DateTime.Now < delegation.StartDate ? 3 : 1;

            Delegation DelegationResult = _userBService.EditDelegation(delegation, id, currentToUser);
            if (Convert.ToInt32(Session["userid"]) == delegation.FromUser)
            {
                Session["DelegateStatus"] = DelegationResult.Status;
                Session["DelegateTo"] = _userBService.GetDetail(DelegationResult.ToUser).Name;
                Session["DelegateUntil"] = DelegationResult.EndDate;
                Session["DelegateStart"] = DelegationResult.StartDate;
            }

            return RedirectToAction("Details/" + DelegationResult.DelegateId, "Delegation");
        }


        public ActionResult Delete(int id)
        {
            if (Common.CheckAdmin())
            {
                return RedirectToAction("Login", "Default");
            }
            Delegation delegation = _userBService.GetDetailDelegation(id);
            ViewBag.FromUser = _userBService.GetDetail(delegation.FromUser);
            ViewBag.ToUser = _userBService.GetDetail(delegation.ToUser);
            if (delegation == null)
            {
                return HttpNotFound();
            }
            return View(delegation);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Common.CheckAdmin())
            {
                return RedirectToAction("Login", "Default");
            }
                Delegation delegation = _userBService.GetDetailDelegation(id);
                int from = delegation.FromUser;
                int to = delegation.ToUser;

                delegation.Status = 0;
                _userBService.DeleteDelegation(delegation, id);
                Session["DelegateStatus"] = 0;
                Session["DelegateTo"] = null;
                Session["DelegateUntil"] = null;
                Session["DelegateStart"] = null;

                User user = _userBService.GetDetail(delegation.FromUser);
                user.MobileToken = Common.AccessToken() + user.UserId;
                user = _userBService.Edit(user);

                return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult GetDetailUser(int UserId)
        {
            User userData = _userBService.GetDetail(UserId);
            User userJson = new User() {
                UserId = userData.UserId,
                Name = userData.Name,
                EmployeeId = userData.EmployeeId,
                Email = String.IsNullOrWhiteSpace(userData.Email) ? "-" : userData.Email,
                AreaName = String.IsNullOrWhiteSpace(userData.AreaName) ? "HEAD OFFICE" : userData.AreaName,
                BranchName = String.IsNullOrWhiteSpace(userData.BranchName) ? "-" : userData.BranchName,
                AreaCode = String.IsNullOrWhiteSpace(userData.AreaCode) ? "-" : userData.AreaCode,
                BranchCode = String.IsNullOrWhiteSpace(userData.BranchCode) ? "-" : userData.BranchCode,
                MobilePassword = userData.RoleId == 0 ? "GUEST" : _userRoleBService.GetDetail(userData.RoleId).Name,
                Position = String.IsNullOrWhiteSpace(userData.Position) ? "-" : userData.Position,
                TechnicalJobExperienceDuration = String.IsNullOrWhiteSpace(userData.TechnicalJobExperienceDuration) ? "-" : userData.TechnicalJobExperienceDuration,
                TechnicalJobTitle = String.IsNullOrWhiteSpace(userData.TechnicalJobTitle) ? "-" : userData.TechnicalJobTitle,
                Phone = String.IsNullOrWhiteSpace(userData.Phone) ? "-" : userData.Phone,
                PhotoProfile = String.IsNullOrWhiteSpace(userData.PhotoProfile) ? "avatar-default.jpg" : userData.PhotoProfile,
                POH_Name = String.IsNullOrWhiteSpace(userData.POH_Name) ? "-" : userData.POH_Name,
                RoleDescription = String.IsNullOrWhiteSpace(userData.RoleDescription) ? "-" : userData.RoleDescription,
                RoleId = userData.RoleId
            };
            return Json(userJson);
        }


    }
}
