using Com.Trakindo.Framework;
using Com.Trakindo.TSICS.Business.Service;
using Com.Trakindo.TSICS.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Mvc;
using TSICS.Helper;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Configuration;

namespace TSICS.Controllers
{

    public class AccountController : Controller
    {
        private readonly UserBusinessService _userBService = Factory.Create<UserBusinessService>("User", ClassType.clsTypeBusinessService);
        private readonly UserMessageBusinessService _userMessageBService = Factory.Create<UserMessageBusinessService>("UserMessage", ClassType.clsTypeBusinessService);
        private readonly UserRoleBusinessService _userRoleBService = Factory.Create<UserRoleBusinessService>("UserRole", ClassType.clsTypeBusinessService);
        private readonly EmployeeMasterBusinessService _mstEmployeeBService = Factory.Create<EmployeeMasterBusinessService>("EmployeeMaster", ClassType.clsTypeBusinessService);
        private readonly MasterAreaBusinessService _masterAreaBs = Factory.Create<MasterAreaBusinessService>("MasterArea", ClassType.clsTypeBusinessService);
        private readonly MasterBranchBusinessService _masterBranchBs = Factory.Create<MasterBranchBusinessService>("MasterBranch", ClassType.clsTypeBusinessService);
        private readonly LogErrorBusinessService _logErrorBService = Factory.Create<LogErrorBusinessService>("LogError", ClassType.clsTypeBusinessService);
        private readonly LogReportBusinessService _logReportBService = Factory.Create<LogReportBusinessService>("LogReport", ClassType.clsTypeBusinessService);
        private readonly ArticleBusinessService _articleBs = Factory.Create<ArticleBusinessService>("Article", ClassType.clsTypeBusinessService);
        private readonly UserTsManagerBusinessService _userTsManagerBusinessService = Factory.Create<UserTsManagerBusinessService>("UserTsManager", ClassType.clsTypeBusinessService);
        private readonly TicketBusinessService _ticketBs = Factory.Create<TicketBusinessService>("Ticket", ClassType.clsTypeBusinessService);

        // GET: Account
        public ActionResult Index()
        {
            ViewBag.Download = WebConfigure.GetDomain() + "/Upload/Document/" + WebConfigure.GetUserGuideNameFileWithExtention();
            if (Session["userid"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                //if (Common.GetUserXupj().ToLower() != Session["username"].ToString().ToLower())
                //{
                //    return RedirectToAction("Login", "Account");
                //}
                var userT = _userBService.GetDetail(Convert.ToInt32(Session["userid"]));

                ViewBag.MsgReject = _userMessageBService.GetListByEmployeeId(userT.EmployeeId);
                User app1 = userT.Approval1 == 0 ? null : _userBService.GetDetailByEmployeeId(userT.Approval1);
                User app2 = userT.Approval2 == 0 ? null : _userBService.GetDetailByEmployeeId(userT.Approval2);

                EmployeeMaster userEmpMst = _mstEmployeeBService.GetDetailbyUserName(userT.Username);
                EmployeeMaster empMstApp1 = userEmpMst == null ? null : _mstEmployeeBService.GetDetail(userEmpMst.Superior_ID);
                EmployeeMaster empMstApp2 = empMstApp1 == null ? null : _mstEmployeeBService.GetDetail(empMstApp1.Superior_ID);
                ViewBag.App1 = app1 == null ? null : app1;
                ViewBag.App2 = app2 == null ? null : app2;
                ViewBag.empMstApp1 = empMstApp1 == null ? null : empMstApp1;
                ViewBag.empMstApp2 = empMstApp2 == null ? null : empMstApp2;
                UserTsManager tsmanager = userT.UserTsManager1Id == 0 ? null : _userTsManagerBusinessService.GetDetail(userT.UserTsManager1Id);
                ViewBag.TSManager = tsmanager == null ? null : _userBService.GetDetailByEmployeeId(tsmanager.EmployeeId);
                ViewBag.Recent = _ticketBs.GetRecent();
                var Employemst = _mstEmployeeBService.GetDetail(userT.EmployeeId);
                if (Employemst == null)
                {
                    ViewBag.EmployeeMaster_empId = String.Empty;
                    ViewBag.EmployeeMaster_POH_Id = String.Empty;
                    ViewBag.EmployeeMaster_POH_Name = "-";
                }
                else
                {
                    ViewBag.EmployeeMaster_empId = Convert.ToString(Employemst.Employee_Id);
                    ViewBag.EmployeeMaster_POH_Id = string.IsNullOrWhiteSpace(Employemst.Business_Area) ? "-" : "ID : " + Employemst.Business_Area;
                    ViewBag.EmployeeMaster_POH_Name = string.IsNullOrWhiteSpace(Employemst.Business_Area_Desc) ? "-" : Employemst.Business_Area_Desc;
                }
                return View(userT);

            }
        }
        public ActionResult ApproveList(string sortOrder)
        {
            if (Session["role"] == null)
                return RedirectToAction("Index", "Home");

            List<User> lUser;

            if (Session["role"].ToString() == "1") //approval 2
            {
                lUser = _userBService.GetListApprove(5);
                ViewBag.App = "1";
            }
            else if (Session["role"].ToString() == "3")
            {
                lUser = _userBService.GetListApprove(3);
                ViewBag.App = "2";
            }
            else
            {
                lUser = _userBService.GetListApprove(2);
                ViewBag.App = "";
            }


            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            var students = lUser;

            switch (sortOrder)
            {
                case "name_desc":
                    students = students.OrderByDescending(s => s.Username).ToList();
                    break;
                case "Date":
                    students = students.OrderBy(s => s.Name).ToList();
                    break;
                case "date_desc":
                    students = students.OrderByDescending(s => s.Position).ToList();
                    break;
                default:
                    students = students.OrderBy(s => s.Name).ToList();
                    break;
            }

            return View(students);
        }
        [AllowAnonymous]
        public ActionResult Login()
        {
            string userPortal = Common.GetUserXupj().ToLower();
            try
            {
                if (Session["userid"] != null)
                {
                    ClearSessionData();
                }
                ViewBag.Download = WebConfigure.GetDomain() + "/Upload/Document/" + WebConfigure.GetUserGuideNameFileWithExtention();
                ViewBag.UrlApp = WebConfigure.GetDomain();
                ViewBag.CheckLogin = WebConfigure.GetLoginPortal();

                if (WebConfigure.GetLoginPortal() == "false")
                {

                    ViewBag.ListUser = _userBService.GetList();
                    return View();
                }
                else
                {
                    if (userPortal != "")
                    {
                        var user = _userBService.GetDetailbyUsername(userPortal);

                        if (user != null)// User registered at TREND system
                        {

                            if (user.Status.Equals(7))
                            {
                                Session["userid"] = user.UserId;
                                Session["role"] = user.RoleId;
                                Session["roleColor"] = _userRoleBService.GetDetail(user.RoleId).Description;
                                Session["username"] = user.Username;
                                Session["name"] = user.Name;
                                Session["status"] = user.Status;
                                Session["photo"] = user.PhotoProfile;
                                Session["loginPortal"] = WebConfigure.GetLoginPortal();

                                if (user.IsDelegate != 0)
                                {
                                    Delegation delegationData = _userBService.GetDetailDelegation(user.IsDelegate);
                                    Session["DelegateStatus"] = delegationData.Status;
                                    Session["DelegateTo"] = _userBService.GetDetail(delegationData.ToUser).Name;
                                    Session["DelegateUntil"] = delegationData.EndDate;
                                    Session["DelegateStart"] = delegationData.StartDate;
                                }
                                else if (user.IsDelegate == 0)
                                {
                                    Session["DelegateStatus"] = null;
                                    Session["DelegateUntil"] = null;
                                    Session["DelegateTo"] = null;
                                    Session["DelegateStart"] = null;
                                }
                                _logReportBService.WriteLog("[SUCCESS] Account Login", MethodBase.GetCurrentMethod().Name, "User Portal :" + userPortal + " has Login");
                                return RedirectToAction("Index", "Library");
                            }
                            else
                            {
                                Session["userid"] = user.UserId;
                                Session["role"] = user.RoleId;
                                Session["roleColor"] = "Guest";
                                Session["username"] = user.Username;
                                Session["name"] = user.Name;
                                Session["status"] = user.Status;
                                Session["photo"] = user.PhotoProfile;
                                Session["loginPortal"] = WebConfigure.GetLoginPortal();
                                Session["DelegateStatus"] = 0;
                                Session["DelegateUntil"] = null;
                                Session["DelegateTo"] = null;
                                Session["DelegateStart"] = null;
                                _logReportBService.WriteLog("[SUCCESS] Account Login", MethodBase.GetCurrentMethod().Name, "User Portal :" + userPortal + " has Login");
                                return RedirectToAction("Index", "Library");
                            }
                        }
                        else //As Guest
                        {
                            EmployeeMaster UserfromPortal = _mstEmployeeBService.GetDetailbyUserName(userPortal);
                            if (UserfromPortal != null)
                            {
                                User model = new User();
                                model.CreatedAt = DateTime.Now;
                                model.Status = 0;
                                var mEmployee = userPortal.Equals("ict.development") ? _mstEmployeeBService.GetDetailbyUserName("XUPJ21IYN") : UserfromPortal;
                                model.Name = mEmployee.Employee_Name;
                                model.EmployeeId = mEmployee.Employee_Id;
                                model.Username = userPortal.Equals("ict.development") ? "ict.development" : mEmployee.Employee_xupj;
                                model.Position = mEmployee.Position_Name;
                                model.Email = mEmployee.Email;
                                model.AreaCode = mEmployee.Location_Id;
                                model.AreaName = mEmployee.Location_Name;
                                model.BranchCode = mEmployee.Branch_Id;
                                model.Dob = DateTime.ParseExact(mEmployee.Birth_date, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
                                model.RoleId = 0;
                                model = _userBService.Add(model);

                                var userguest = _userBService.GetDetailbyUsername(userPortal);
                                Session["userid"] = userguest.UserId;
                                Session["role"] = userguest.RoleId;
                                Session["roleColor"] = "Guest";
                                Session["username"] = userPortal;
                                Session["name"] = userguest.Name;
                                Session["status"] = userguest.Status;
                                Session["photo"] = userguest.PhotoProfile;
                                Session["loginPortal"] = WebConfigure.GetLoginPortal();
                                Session["DelegateStatus"] = 0;
                                Session["DelegateUntil"] = null;
                                Session["DelegateTo"] = null;
                                Session["DelegateStart"] = null;
                                _logReportBService.WriteLog("[SUCCESS] Account Login", MethodBase.GetCurrentMethod().Name, "User Portal :" + userPortal + " has Login");
                                return RedirectToAction("Index", "Library");
                            }
                            else
                            {
                                var userguest = _userBService.GetDetailbyUsername("Guest");
                                Session["userid"] = userguest.UserId;
                                Session["role"] = userguest.RoleId;
                                Session["roleColor"] = "Guest";
                                Session["username"] = userguest.Username;
                                Session["name"] = userPortal;
                                Session["status"] = userguest.Status;
                                Session["photo"] = userguest.PhotoProfile;
                                Session["loginPortal"] = WebConfigure.GetLoginPortal();
                                Session["DelegateStatus"] = 0;
                                Session["DelegateUntil"] = null;
                                Session["DelegateTo"] = null;
                                Session["DelegateStart"] = null;
                                return Content("<script language='javascript' type='text/javascript'>alert('Your Portal Account Has Not Registered in Employee Master Database'); location.href = '" + WebConfigure.GetDomain() + "/Library';</script>");
                                // return RedirectToAction("Index", "Library");
                            }
                        }
                    }
                    else
                    {
                        Uri uri = new Uri(ConfigurationManager.AppSettings["Domain"]);
                        string requested = uri.Scheme + Uri.SchemeDelimiter + uri.Host + ":" + uri.Port;
                        string protokol = Common.GetProtocol();
                        string portalUrl = WebConfigure.GetLoginHost() + "/_layouts/15/Trakindo/Authentication/Login.aspx?ReturnUrl=" + protokol + uri.Host + uri.AbsolutePath + "/Library";
                        ViewBag.portalDomain = portalUrl;
                        return View("RedirectToPortal");
                    }
                }
            }
            catch (Exception er)
            {
                _logErrorBService.WriteLog("[FAILED] Account Login", MethodBase.GetCurrentMethod().Name, er.ToString(), userPortal);
                return RedirectToAction("Index", "Library");
                //ClearSessionData();
                //Uri uri = new Uri(ConfigurationManager.AppSettings["Domain"]);
                //string requested = uri.Scheme + Uri.SchemeDelimiter + uri.Host + ":" + uri.Port;
                //string protokol = Common.GetProtocol();
                //string portalUrl = WebConfigure.GetLoginHost() + "/_layouts/15/Trakindo/Authentication/Login.aspx?ReturnUrl=" + protokol + uri.Host + uri.AbsolutePath + "/Library";
                //ViewBag.portalDomain = portalUrl;
                //return View("RedirectToPortal");
            }
        }

        private void ClearSessionData()
        {
            Session.Remove("userid");
            Session.Remove("role");
            Session.Remove("roleColor");
            Session.Remove("username");
            Session.Remove("name");
            Session.Remove("status");
            Session.Remove("photo");
            Session.Remove("userstatus");
            Session.Remove("loginPortal");
            Session.Remove("Delegation");
            Session.Remove("DelegateStatus");
            Session.Remove("DelegateTo");
            Session.Remove("DelegateUntil");
            Session.Remove("DelegateStart");
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
#pragma warning disable 1998
        public async Task<ActionResult> Login(User model)
#pragma warning restore 1998
        {
            ViewBag.Download = WebConfigure.GetDomain() + "/Upload/Document/" + WebConfigure.GetUserGuideNameFileWithExtention();
            ViewBag.Domain = WebConfigure.GetDomain();
            try
            {
                if (ModelState.IsValid)
                {
                    #region cek EmployeeID
                    if (LoginValidate(model))
                    {
                        #region session User

                        var user = _userBService.GetDetailbyUsername(model.Username);

                        if (user != null)// User registered at TREND system
                        {
                            if (user.Status.Equals(7))
                            {
                                var userT = _userBService.GetDetailbyUsername(model.Username);
                                Session["userid"] = userT.UserId;
                                Session["role"] = userT.RoleId;
                                Session["roleColor"] = _userRoleBService.GetDetail(userT.RoleId).Description;
                                Session["username"] = userT.Username;
                                Session["name"] = userT.Name;
                                Session["status"] = userT.Status;
                                Session["photo"] = userT.PhotoProfile;
                                Session["loginPortal"] = WebConfigure.GetLoginPortal();

                                if (userT.IsDelegate != 0)
                                {
                                    Delegation delegationData = _userBService.GetDetailDelegation(userT.IsDelegate);
                                    Session["DelegateStatus"] = delegationData.Status;
                                    Session["DelegateTo"] = _userBService.GetDetail(delegationData.ToUser).Name;
                                    Session["DelegateUntil"] = delegationData.EndDate;
                                    Session["DelegateStart"] = delegationData.StartDate;
                                }
                                else if (user.IsDelegate == 0)
                                {
                                    Session["DelegateStatus"] = null;
                                    Session["DelegateUntil"] = null;
                                    Session["DelegateTo"] = null;
                                    Session["DelegateStart"] = null;
                                }
                                return RedirectToAction("Summary", "TechnicalRequest");

                                #endregion
                            }
                            else//user Registered but Inactive
                            {
                                Session["userid"] = user.UserId;
                                Session["role"] = user.RoleId;
                                Session["roleColor"] = "Guest";
                                Session["username"] = user.Username;
                                Session["name"] = user.Name;
                                Session["status"] = user.Status;
                                Session["photo"] = user.PhotoProfile;
                                Session["loginPortal"] = WebConfigure.GetLoginPortal();
                                Session["DelegateStatus"] = null;
                                Session["DelegateUntil"] = null;
                                Session["DelegateTo"] = null;
                                Session["DelegateStart"] = null;
                                return RedirectToAction("Index", "Library");
                            }
                        }
                        else if (user == null)
                        {//As Guest
                         //return RedirectToAction("Register", "Account");
                            var userguest = _userBService.GetDetailbyUsername("Guest");
                            Session["userid"] = userguest.UserId;
                            Session["role"] = userguest.RoleId;
                            Session["roleColor"] = "Guest";
                            Session["username"] = userguest.Username;
                            Session["name"] = userguest.Name;
                            Session["status"] = userguest.Status;
                            Session["photo"] = userguest.PhotoProfile;
                            Session["loginPortal"] = WebConfigure.GetLoginPortal();
                            Session["DelegateStatus"] = null;
                            Session["DelegateUntil"] = null;
                            Session["DelegateTo"] = null;
                            Session["DelegateStart"] = null;
                            return RedirectToAction("Index", "Library");
                        }


                    }
                    #endregion
                }

                return View(model);
            }
            catch (Exception er)
            {
                _logErrorBService.WriteLog("Account", MethodBase.GetCurrentMethod().Name, er.ToString());
                return View("Login", "Default");
            }
        }
        [AllowAnonymous]
        public ActionResult Register()
        {
            SetListDropdown();

            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
#pragma warning disable 1998
        public async Task<ActionResult> Register(User model)
#pragma warning restore 1998
        {
            SetListDropdown();
            ModelState.Remove("TechnicalJobExperienceDuration");
            ModelState.Remove("TechnicalJobTitle");
            ModelState.Remove("RoleDescription");
            ModelState.Remove("Phone");
            ModelState.Remove("MasterAreaId");
            ModelState.Remove("MasterBranchId");
            if (ModelState.IsValid)
            {
                #region cek EmployeeID
                if (RegisterValidate(model))
                {
                    if (_userBService.CheckUserReject(model.EmployeeId))
                    {
                        return RedirectToAction("Index", "Library");
                    }
                    //insert User
                    string token = Common.AccessToken() + model.EmployeeId;

                    var employeeM = _mstEmployeeBService.GetDetail(model.EmployeeId);

                    model = this.ParsingUser(model);
                    model.CreatedAt = DateTime.Now;

                    model.Status = 2;
                    token = token.Replace("/", "");
                    token = token.Replace("+", "");

                    model.MobileToken = token;
                    model.Approval1 = employeeM.Superior_ID;

                    if (model.MasterAreaId == 0)
                    {
                        model.AreaName = null;
                        model.MasterAreaId = 0;
                    }
                    else
                    {
                        model.MasterAreaId = _masterAreaBs.GetDetail(model.MasterAreaId).MasterAreaId;
                        model.AreaName = _masterAreaBs.GetDetail(model.MasterAreaId).Name;
                    }
                    if (model.MasterBranchId == 0)
                    {
                        model.BranchName = null;
                        model.MasterBranchId = 0;
                    }
                    else
                    {
                        model.MasterBranchId = _masterBranchBs.GetDetail(model.MasterBranchId).MasterBranchId;
                        model.BranchName = _masterBranchBs.GetDetail(model.MasterBranchId).Name;
                    }

                    var addDay = Common.NumberOfWorkDays(DateTime.Now, WebConfigure.GetApprovalSpvDay());
                    model.DueDateApproval1 = DateTime.Now.AddDays(addDay);
                    model.POH_Name = employeeM.POH_Name;

                    var userModel = _userBService.GetByEmployeeId(model.EmployeeId);
                    if (userModel != null)
                    {
                        if (userModel.UserId != 0)
                        {
                            //UserModel = model;
                            var userModelEdit = _userBService.GetDetail(userModel.UserId);
                            //UserModelEdit = this.ParsingUser(model);
                            userModelEdit.CreatedAt = DateTime.Now;
                            userModelEdit.UpdatedAt = DateTime.Now;
                            userModelEdit.Status = 2;
                            userModelEdit.Phone = model.Phone;
                            userModelEdit.MobileToken = token;
                            userModelEdit.Approval1 = employeeM.Superior_ID;
                            userModelEdit.AreaName = model.MasterAreaId != 0 ? _masterAreaBs.GetDetail(model.MasterAreaId).Name : null;
                            userModelEdit.BranchName = model.MasterBranchId != 0 ? _masterBranchBs.GetDetail(model.MasterBranchId).Name : null;
                            userModelEdit.MasterAreaId = model.MasterAreaId != 0 ? _masterAreaBs.GetDetail(model.MasterAreaId).MasterAreaId : 0;
                            userModelEdit.MasterBranchId = model.MasterBranchId != 0 ? _masterBranchBs.GetDetail(model.MasterBranchId).MasterBranchId : 0;
                            userModelEdit.RoleId = model.RoleId;
                            userModelEdit.DueDateApproval1 = model.DueDateApproval1;
                            userModelEdit.RoleDescription = model.RoleDescription;
                            userModelEdit.TechnicalJobExperienceDuration = model.TechnicalJobExperienceDuration;
                            userModelEdit.TechnicalJobTitle = model.TechnicalJobTitle;
                            userModelEdit.Email = _mstEmployeeBService.GetDetail(model.EmployeeId).Email;
                            model = _userBService.Edit(userModelEdit);
                        }
                    }
                    else
                    {
                        model = _userBService.Add(model);
                    }

                    Email.SendMailRegister(model);

                    //send email to approval 1 Superior_ID
                    Email.SendMailNeedApp1(model);

                    return RedirectToAction("Login", "Account");
                }
            }
            #endregion
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public ActionResult Approve(int id)
        {
            var userT = _userBService.GetDetail(id);
            return View(userT);
        }

        [HttpPost]
        public ActionResult Approve(User user)
        {
            //next to approval 2
            var userT = _userBService.GetDetail(user.UserId);
            userT.Status = 3;
            _userBService.Edit(userT);

            return RedirectToAction("ApproveList");
        }
        public ActionResult Approve2(int id)
        {
            var userT = _userBService.GetDetail(id);
            return View(userT);
        }

        [HttpPost]
        public ActionResult Approve2(User user)
        {
            //next to admin
            var userT = _userBService.GetDetail(user.UserId);
            userT.Status = 5;
            _userBService.Edit(userT);

            return RedirectToAction("ApproveList");

        }
        public ActionResult Approve1(int id)
        {
            var userT = _userBService.GetDetail(id);
            return View(userT);
        }
        [HttpPost]
        public ActionResult Approve1(User user)
        {
            //next to active
            var userT = _userBService.GetDetail(user.UserId);
            userT.Status = 7;
            _userBService.Edit(userT);

            return RedirectToAction("ApproveList");
        }
        [HttpPost]
        public ActionResult Reject1(FormCollection collect)
        {
            if (!string.IsNullOrWhiteSpace(collect["rejectMsg"]) || !string.IsNullOrEmpty(collect["rejectMsg"]))
            {
                var token = Common.AccessToken();
                token = token.Replace("/", "");
                token = token.Replace("+", "");

                //next to reject
                var userT = _userBService.GetDetail(Convert.ToInt32(collect["UserId"]));
                userT.Status = 4;
                userT.UpdatedAt = DateTime.Now;
                userT.MobileToken = token;
                User userData = _userBService.Edit(userT);

                EmployeeMaster SuperiorData = _mstEmployeeBService.GetDetail(_mstEmployeeBService.GetDetail(userData.EmployeeId).Superior_ID);
                //insert UserMessage
                UserMessage userM = Factory.Create<UserMessage>("UserMessage", ClassType.clsTypeDataModel);
                userM.Message = collect["rejectMsg"];
                userM.MessageType = "Reject 1";
                userM.ToUserId = Convert.ToInt32(collect["UserId"]);
                //UserM.FromUserId = Convert.ToInt32(Session["userid"]);
                userM.CreatedAt = DateTime.Now;
                userM.ToEmployeeId = userT.EmployeeId;
                userM.FromEmployeeId = userT.Approval1;

                _userMessageBService.Add(userM);


                Email.SendMailReject(userT, userM, SuperiorData);
                Email.SendMailResApp1(userT, userM.Message, 1, 0);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Reject2(FormCollection collect)
        {
            if (!string.IsNullOrWhiteSpace(collect["rejectMsg"]) || !string.IsNullOrEmpty(collect["rejectMsg"]))
            {
                string token = Common.AccessToken();
                token = token.Replace("/", "");
                token = token.Replace("+", "");

                //next to reject
                var userT = _userBService.GetDetail(Convert.ToInt32(collect["UserId"]));
                userT.Status = 6;
                userT.UpdatedAt = DateTime.Now;
                userT.MobileToken = token;
                _userBService.Edit(userT);
                User userData = _userBService.Edit(userT);

                EmployeeMaster SuperiorData1 = _mstEmployeeBService.GetDetail(_mstEmployeeBService.GetDetail(userData.EmployeeId).Superior_ID);
                EmployeeMaster SuperiorData2 = _mstEmployeeBService.GetDetail(SuperiorData1.Superior_ID);
                //insert UserMessage
                UserMessage userM = Factory.Create<UserMessage>("UserMessage", ClassType.clsTypeDataModel);
                userM.Message = collect["rejectMsg"];
                userM.MessageType = "Reject 2";
                userM.ToUserId = Convert.ToInt32(collect["UserId"]);
                //UserM.FromUserId = Convert.ToInt32(Session["userid"]);
                userM.CreatedAt = DateTime.Now;
                userM.ToEmployeeId = userT.EmployeeId;
                userM.FromEmployeeId = userT.Approval2;

                _userMessageBService.Add(userM);
                Email.SendMailResApp1(userT, collect["rejectMsg"], 2, 0);
                //send email
                Email.SendMailReject(userT, userM, SuperiorData2);

            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult LogOff(User user)
        {
            Session.Remove("userid");
            Session.Remove("role");
            Session.Remove("roleColor");
            Session.Remove("username");
            Session.Remove("name");
            Session.Remove("status");
            Session.Remove("photo");
            Session.Remove("loginPortal");
            Session.Remove("Delegation");
            Session.Remove("DelegateTo");
            return RedirectToAction("Index", "Home");
        }

        private User ParsingUser(User model)
        {
            //string dob;
            //get master employee
            var mEmployee = _mstEmployeeBService.GetDetail(model.EmployeeId);

            model.Name = mEmployee.Employee_Name;
            model.Username = mEmployee.Employee_xupj;
            model.Position = mEmployee.Position_Name;
            model.Email = mEmployee.Email;
            model.AreaCode = mEmployee.Location_Id;
            model.AreaName = mEmployee.Location_Name;
            model.BranchCode = mEmployee.Branch_Id;
            //dob = MEmployee.Birth_date.Substring(0, 4) + "-" + MEmployee.Birth_date.Substring(4, 2) + "-" + MEmployee.Birth_date.Substring(6, 2);
            model.Dob = DateTime.ParseExact(mEmployee.Birth_date, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);
            return model;
        }
        private bool LoginValidate(User model)
        {
            bool ret;

            if (WebConfigure.GetLoginPortal() == "false")
            {
                return true;
            }

            if (Common.GetUserXupj().ToLower() != model.Username.ToLower())
            {
                ModelState.AddModelError("Username", "Username and Login portal not match");
                ret = false;
            }
            else if (_userBService.CheckUserName(model.Username))
            {
                ret = true;
            }
            else
            {
                ModelState.AddModelError("Username", "Username not active or not registered");
                ret = false;
            }
            return ret;
        }
        private bool RegisterValidate(User model)
        {
            bool ret;
            if (!_mstEmployeeBService.CheckEmployee(model.EmployeeId))
            {
                ModelState.AddModelError("EmployeeId", "EmployeeId not found");
                ret = false;
            }
            else if (_userBService.CheckUser(model.EmployeeId))
            {
                ModelState.AddModelError("EmployeeId", "EmployeeId already registered");
                ret = false;
            }
            else
            {
                ret = true;
            }
            return ret;
        }

        private void SetListDropdown()
        {
            ViewBag.ListRole = _userRoleBService.GetList();
            ViewBag.ListMasterArea = _masterAreaBs.GetListActive();
            ViewBag.ListMasterBranch = _masterBranchBs.GetList();
        }

        public ActionResult Approve1Mail(string id)
        {
            ViewBag.UseFullLink = null;
            var userT = _userBService.GetByToken(id);
            var employeeApproval = _mstEmployeeBService.GetDetail(userT.Approval1);

            ViewBag.Level = _userRoleBService.GetDetail(userT.RoleId).Name;
            ViewBag.Approval = employeeApproval;
            ViewBag.RecentArticle = _articleBs.GetRecent();
            return View(userT);
        }

        [HttpPost]
        public ActionResult Approve1Mail(User user, FormCollection collect)
        {
            string token = Common.AccessToken() + user.EmployeeId;
            token = token.Replace("/", "");
            token = token.Replace("+", "");

            var employeeM = _mstEmployeeBService.GetDetail(user.Approval1);

            //next to approval 2
            var userT = _userBService.GetDetail(user.UserId);
            userT.Status = 3;
            userT.UpdatedAt = DateTime.Now;
            userT.MobileToken = token;
            userT.Approval2 = employeeM.Superior_ID;
            userT.NameApproval1 = employeeM.Employee_Name;
            userT.DateApproval1 = DateTime.Now;

            var addDay = Common.NumberOfWorkDays(DateTime.Now, WebConfigure.GetApprovalSpvDay());
            userT.DueDateApproval2 = DateTime.Now.AddDays(addDay);

            userT = _userBService.Edit(userT);


            //insert UserMessage
            UserMessage userM = Factory.Create<UserMessage>("UserMessage", ClassType.clsTypeDataModel);
            userM.Message = String.IsNullOrWhiteSpace(collect["ApproveMsg"]) ? "" : collect["ApproveMsg"];
            userM.MessageType = "Approve 1";
            userM.ToUserId = Convert.ToInt32(collect["UserId"]);
            //UserM.FromUserId = Convert.ToInt32(Session["userid"]);
            userM.CreatedAt = DateTime.Now;
            userM.ToEmployeeId = userT.EmployeeId;
            userM.FromEmployeeId = userT.Approval1;


            _userMessageBService.Add(userM);

            //Send Email to Approval 1 Status Approve
            Email.SendMailResApp1(userT, userM.Message, 1, 1);

            //Send Email to Approval 2 Need Approve
            Email.SendMailNeedApp2(userT, userM.Message);

            return RedirectToAction("ApproveList");
        }
        public ActionResult Approve2Mail(string id)
        {
            var userT = _userBService.GetByToken(id);
            var employeeApproval = _mstEmployeeBService.GetDetail(userT.Approval1);
            var employeeApproval2 = _mstEmployeeBService.GetDetail(userT.Approval2);

            ViewBag.Level = _userRoleBService.GetDetail(userT.RoleId).Name;
            ViewBag.Approval = employeeApproval;
            ViewBag.Approval2 = employeeApproval2;
            ViewBag.RecentArticle = _articleBs.GetRecent();
            ViewBag.MessageResponse = _userMessageBService.GetListByEmployeeId(userT.EmployeeId).FirstOrDefault();
            return View(userT);
        }

        [HttpPost]
        public ActionResult Approve2Mail(User user, FormCollection collect)
        {
            string token = Common.AccessToken() + user.EmployeeId;
            token = token.Replace("/", "");
            token = token.Replace("+", "");

            var employeeM = _mstEmployeeBService.GetDetail(user.Approval2);

            var userTsManagerModel = _userTsManagerBusinessService.GetDetail(1);
            var Spv1 = _mstEmployeeBService.GetDetail(_mstEmployeeBService.GetDetail(user.EmployeeId).Superior_ID);
            var Spv2 = _mstEmployeeBService.GetDetail(Spv1.Superior_ID);
            //next to approval 2
            var userT = _userBService.GetDetail(user.UserId);
            userT.Status = 5;
            userT.UpdatedAt = DateTime.Now;
            userT.MobileToken = token;
            userT.NameApproval2 = employeeM.Employee_Name;
            userT.DateApproval2 = DateTime.Now;

            userT.UserTsManager1Id = userTsManagerModel.UserTsManagerId;
            userT.UserTsManager1Name = userTsManagerModel.Name;
            var addDay = Common.NumberOfWorkDays(DateTime.Now, WebConfigure.GetApprovalTsManagerDay());
            userT.UserTsManager1DueDate = DateTime.Now.AddDays(addDay);
            userT.UserTsManagerDueFlag = 1;

            _userBService.Edit(userT);


            //insert UserMessage
            UserMessage userM = Factory.Create<UserMessage>("UserMessage", ClassType.clsTypeDataModel);
            userM.Message = String.IsNullOrWhiteSpace(collect["approveMsg"]) ? "" : collect["approveMsg"];
            userM.MessageType = "Approve 2";
            userM.ToUserId = Convert.ToInt32(collect["UserId"]);
            userM.CreatedAt = DateTime.Now;
            userM.ToEmployeeId = userT.EmployeeId;
            userM.FromEmployeeId = userT.Approval2;

            _userMessageBService.Add(userM);

            Dictionary<String, EmployeeMaster> ApprovalData = new Dictionary<string, EmployeeMaster>();
            Dictionary<String, String> Message = new Dictionary<String, String>();
            ApprovalData.Add("Approval1", Spv1);
            ApprovalData.Add("Approval2", Spv2);
            ApprovalData.Add("ApprovalTS", _mstEmployeeBService.GetDetail(userTsManagerModel.EmployeeId));

            Message.Add("Approval1", _userMessageBService.GetDetail(Spv1.Employee_Id, 0, "Approve 1 - Registering").Message);
            Message.Add("Approval2", _userMessageBService.GetDetail(Spv2.Employee_Id, 0, "Approve 2 - Registering").Message);
            Message.Add("ApprovalTS", collect["ApproveMsg"]);


            //Send Email TsManager
            Email.SendMailNeedAppTs1(userT, 1, userM.Message);
            Email.SendMailResApp1(userT, userM.Message, 2, 1);
            return RedirectToAction("ApproveList");
        }

        [HttpPost]
        public JsonResult GetBranch(int id)
        {
            return Json(new SelectList(_masterBranchBs.GetListbyArea(id), "MasterBranchId", "Name"));
        }

        public ActionResult ApproveTsMail(string id)
        {
            var userT = _userBService.GetByToken(id);
            var employeeApproval = _mstEmployeeBService.GetDetail(userT.Approval1);
            var employeeApproval2 = _mstEmployeeBService.GetDetail(userT.Approval2);

            var userTsManagerModel = _userTsManagerBusinessService.GetDetail(userT.UserTsManager1Id);

            ViewBag.Level = _userRoleBService.GetDetail(userT.RoleId).Name;
            ViewBag.Approval = employeeApproval;
            ViewBag.Approval2 = employeeApproval2;
            ViewBag.ApprovalTs = userTsManagerModel;
            ViewBag.RecentArticle = _articleBs.GetRecent();
            ViewBag.MessageResponse = _userMessageBService.GetListByEmployeeId(userT.EmployeeId).FirstOrDefault();
            return View(userT);
        }

        [HttpPost]
        public ActionResult ApproveTsMail(User user, FormCollection collect)
        {
            string token = Common.AccessToken() + user.EmployeeId;
            token = token.Replace("/", "");
            token = token.Replace("+", "");
            var userTsManagerModel = _userTsManagerBusinessService.GetDetail(1);
            var Spv1 = _mstEmployeeBService.GetDetail(_mstEmployeeBService.GetDetail(user.EmployeeId).Superior_ID);
            var Spv2 = _mstEmployeeBService.GetDetail(Spv1.Superior_ID);
            //next to approval Admin
            var userT = _userBService.GetDetail(user.UserId);

            userT.Status = 9;
            userT.UpdatedAt = DateTime.Now;
            userT.MobileToken = token;
            userT.UserTsManagerDueFlag = 0;

            _userBService.Edit(userT);

            if (collect["approveMsg"] != null)
            {
                //insert UserMessage
                UserMessage userM = Factory.Create<UserMessage>("UserMessage", ClassType.clsTypeDataModel);
                userM.Message = collect["approveMsg"];
                userM.MessageType = "Approve TS";
                userM.ToUserId = Convert.ToInt32(collect["UserId"]);
                userM.CreatedAt = DateTime.Now;
                userM.ToEmployeeId = userT.EmployeeId;
                userM.FromEmployeeId = userTsManagerModel.EmployeeId;

                _userMessageBService.Add(userM);
            }

            Dictionary<String, EmployeeMaster> ApprovalData = new Dictionary<string, EmployeeMaster>();
            Dictionary<String, String> Message = new Dictionary<String, String>();
            ApprovalData.Add("Approval1", Spv1);
            ApprovalData.Add("Approval2", Spv2);
            ApprovalData.Add("ApprovalTS", _mstEmployeeBService.GetDetail(userTsManagerModel.EmployeeId));

            Message.Add("Approval1", _userMessageBService.GetDetail(Spv1.Employee_Id, 0, "Approve 1 - Registering").Message);
            Message.Add("Approval2", _userMessageBService.GetDetail(Spv2.Employee_Id, 0, "Approve 2 - Registering").Message);
            Message.Add("ApprovalTS", collect["ApproveMsg"]);


            //Send Email Admin
            Email.SendMailNeedActivedAdmin(userT, ApprovalData, Message);
            Email.SendMailResApp1(userT, collect["approveMsg"], 3, 1);
            return RedirectToAction("Login");
        }
        [HttpPost]
        public ActionResult RejectTs(FormCollection collect)
        {
            if (!string.IsNullOrWhiteSpace(collect["rejectMsg"]) || !string.IsNullOrEmpty(collect["rejectMsg"]))
            {
                string token = Common.AccessToken();
                token = token.Replace("/", "");
                token = token.Replace("+", "");

                var userTsManagerModel = _userTsManagerBusinessService.GetDetail(1);

                //next to reject
                var userT = _userBService.GetDetail(Convert.ToInt32(collect["UserId"]));
                userT.Status = 10;
                userT.UpdatedAt = DateTime.Now;
                userT.MobileToken = token;
                userT.UserTsManagerDueFlag = 0;
                User userData = _userBService.Edit(userT);

                //insert UserMessage
                UserMessage userM = Factory.Create<UserMessage>("UserMessage", ClassType.clsTypeDataModel);
                userM.Message = collect["rejectMsg"];
                userM.MessageType = "Reject Ts Manager";
                userM.ToUserId = Convert.ToInt32(collect["UserId"]);
                userM.CreatedAt = DateTime.Now;
                userM.ToEmployeeId = userT.EmployeeId;
                userM.FromEmployeeId = userTsManagerModel.EmployeeId;

                _userMessageBService.Add(userM);


                Email.SendMailResApp1(userT, collect["rejectMsg"], 3, 0);
                //send email
                Email.SendMailReject(userT, userM, _mstEmployeeBService.GetDetail(userM.FromEmployeeId));

            }
            return RedirectToAction("Index");
        }
        public string CekEmployee(int employeeId)
        {
            string result = "";

            var employeeM = _mstEmployeeBService.GetDetail(employeeId);

            if (employeeM != null)
            {
                result = "<span>" + employeeM.Employee_Name + "</span><br>";
                result += "<span>" + employeeM.Position_Name + "</span><br>";
            }


            if (string.IsNullOrEmpty(result))
            {
                // ReSharper disable once StringLiteralTypo
                // ReSharper disable once StringLiteralTypo
                result = "Data Not Found";
            }
            return result;
        }


    }
}