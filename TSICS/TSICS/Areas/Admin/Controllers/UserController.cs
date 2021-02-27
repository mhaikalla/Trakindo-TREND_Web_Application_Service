using Com.Trakindo.Framework;
using Com.Trakindo.TSICS.Business.Service;
using Com.Trakindo.TSICS.Data.Model;
using System;
using System.Web.Mvc;
using PagedList;
using TSICS.Helper;
using System.IO;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable IdentifierTypo

namespace TSICS.Areas.Admin.Controllers
{

    public class UserController : Controller
    {
        private readonly UserBusinessService _userBService = Factory.Create<UserBusinessService>("User", ClassType.clsTypeBusinessService);
        private readonly EmployeeMasterBusinessService _employeeMasterBs = Factory.Create<EmployeeMasterBusinessService>("EmployeeMaster", ClassType.clsTypeBusinessService);
        private readonly UserRoleBusinessService _userRoleBService = Factory.Create<UserRoleBusinessService>("UserRole", ClassType.clsTypeBusinessService);
        private readonly UserMessageBusinessService _userMessageBService = Factory.Create<UserMessageBusinessService>("UserMessage", ClassType.clsTypeBusinessService);
        private readonly TicketBusinessService _ticketBs = Factory.Create<TicketBusinessService>("Ticket", ClassType.clsTypeBusinessService);
        private readonly MasterAreaBusinessService _masterAreaBs = Factory.Create<MasterAreaBusinessService>("MasterArea", ClassType.clsTypeBusinessService);
        private readonly MasterBranchBusinessService _masterBranchBs = Factory.Create<MasterBranchBusinessService>("MasterBranch", ClassType.clsTypeBusinessService);
        private readonly UserTsManagerBusinessService _userTsManagerBusinessService = Factory.Create<UserTsManagerBusinessService>("UserTsManager", ClassType.clsTypeBusinessService);
        
        // GET: Admin/User
        public ActionResult Index(int?  page)
        {
            if (Common.CheckAdmin())
                return RedirectToAction("Login", "Default");
            
            int pageSize = 999999999;
            int pageNumber = (page ?? 1);
            
            var userLs = _userBService.GetList();

            List<User> delegationData = new List<User>();
            List<User> SalesOfficeData = new List<User>(); 
            if (userLs != null)
            {
                foreach(var item in userLs)
                {
                    Delegation delegation = _userBService.GetDetailDelegationByUserFrom(item.UserId);
                    EmployeeMaster empMaster = _employeeMasterBs.GetDetail(item.EmployeeId);
                    if(delegation != null)
                    {
                        User delegationdata = new User()
                        {
                            UserId = item.UserId,
                            Name = _userBService.GetDetail(delegation.ToUser).Name
                        };
                        delegationData.Add(delegationdata);
                    }
                    else
                    {
                        User delegationdata = new User()
                        {
                            UserId = item.UserId,
                            Name = null
                        };
                        delegationData.Add(delegationdata);
                    }

                    if (empMaster != null)
                    {
                        User SalesOffice = new User()
                        {
                            UserId = item.UserId,
                            POH_Name = empMaster.Business_Area
                        };
                        SalesOfficeData.Add(SalesOffice);
                    }
                    else
                    {
                        User SalesOffice = new User()
                        {
                            UserId = item.UserId,
                            POH_Name = "-"
                        };
                        SalesOfficeData.Add(SalesOffice);
                    }
                }
            }
            ViewBag.SalesOffice = SalesOfficeData;
            ViewBag.Delegation = delegationData;
            return View(userLs.ToPagedList(pageNumber, pageSize));
        }

        // GET: Admin/User/Details/5
        public ActionResult Detail(int id)
        {
            if (Common.CheckAdmin())
                return RedirectToAction("Login", "Default");
     
            var userT = _userBService.GetDetail(id);
            ViewBag.MsgReject = _userMessageBService.GetListByEmployeeId(userT.EmployeeId);
            User app1 = userT.Approval1 == 0 ? null : _userBService.GetDetailByEmployeeId(userT.Approval1);
            User app2 = userT.Approval2 == 0 ? null : _userBService.GetDetailByEmployeeId(userT.Approval2);


            EmployeeMaster userEmpMst = _employeeMasterBs.GetDetailbyUserName(userT.Username);
            EmployeeMaster empMstApp1 = userEmpMst == null ? null : _employeeMasterBs.GetDetail(userEmpMst.Superior_ID);
            EmployeeMaster empMstApp2 = empMstApp1 == null ? null : _employeeMasterBs.GetDetail(empMstApp1.Superior_ID);
            ViewBag.userEmpMst = userEmpMst;
            ViewBag.App1 =  app1;
            ViewBag.App2 =  app2;
            ViewBag.empMstApp1 = empMstApp1 == null ? null : empMstApp1;
            ViewBag.empMstApp2 = empMstApp2 == null ? null : empMstApp2;
            User Tsmanager = userT.UserTsManager1Id == 0 ? null : _userBService.GetDetailByEmployeeId(_userTsManagerBusinessService.GetDetail(userT.UserTsManager1Id).EmployeeId);
            ViewBag.TSManager = Tsmanager;

            ViewBag.Message1 = app1 == null ? null : _userMessageBService.GetDetail(app1.EmployeeId);
            ViewBag.Message2 = app2 == null ? null :_userMessageBService.GetDetail(app2.EmployeeId);
            ViewBag.MessageTsManager = Tsmanager == null ? null: _userMessageBService.GetDetail(Tsmanager.EmployeeId);
            var EmpMaster = _employeeMasterBs.GetDetail(userT.EmployeeId);
            if(EmpMaster == null)
            {
                ViewBag.EmployeeMaster_empId = String.Empty;
                ViewBag.EmployeeMaster_POH_Name = "-";
                ViewBag.EmployeeMaster_POH_Id = String.Empty;
            }
            else
            {
                ViewBag.EmployeeMaster_empId = Convert.ToString(EmpMaster.Employee_Id);
                ViewBag.EmployeeMaster_POH_Name = string.IsNullOrWhiteSpace(EmpMaster.Business_Area_Desc) ? "-" : EmpMaster.Business_Area_Desc;
                ViewBag.EmployeeMaster_POH_Id = string.IsNullOrWhiteSpace(EmpMaster.Business_Area) ? String.Empty : "ID : " + EmpMaster.Business_Area;
            }
            if (userT.Status == 7)
            {
                ViewBag.RoleColor = _userRoleBService.GetDetail(userT.RoleId).Description;
            }
            else
            {
                ViewBag.RoleColor = "Guest";
            }
            return View(userT);
        }

        // GET: Admin/User/Create
        public ActionResult Create()
        {
            if (Common.CheckAdmin())
                return RedirectToAction("Login", "Default");
            SetListDropdown();
           
            return View();
        }

        // POST: Admin/User/Create
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User user)
        {
            SetListDropdown();
            
            if (Common.CheckAdmin())
                return RedirectToAction("Login", "Default");
            ModelState.Remove("TechnicalJobExperienceDuration");
            ModelState.Remove("TechnicalJobTitle");
            ModelState.Remove("RoleDescription");
            ModelState.Remove("Phone");
            ModelState.Remove("MasterAreaId");
            ModelState.Remove("MasterBranchId");
            if (ModelState.IsValid)
            {
                if (RegisterValidate(user))
                {
                    string token = Common.AccessToken() + user.EmployeeId;

                    var employeeM = _employeeMasterBs.GetDetail(user.EmployeeId);
                    user = this.ParsingUser(user);
                    user.CreatedAt = DateTime.Now;
                    user.Status = 2;
                    token = token.Replace("/", "");
                    token = token.Replace("+", "");
                    user.MobileToken = token;
                    user.Approval1 = employeeM.Superior_ID;
                    user.POH_Name = employeeM.POH_Name;
                    user.Email = employeeM.Email;
                    if (user.MasterAreaId == 0)
                    {
                        user.AreaName = null;
                        user.MasterAreaId = 0;
                    }
                    else
                    {
                        user.MasterAreaId = _masterAreaBs.GetDetail(user.MasterAreaId).MasterAreaId;
                        user.AreaName = _masterAreaBs.GetDetail(user.MasterAreaId).Name;
                    }
                    if (user.MasterBranchId == 0)
                    {
                        user.BranchName = null;
                        user.MasterBranchId = 0;
                    }
                    else
                    {
                        user.MasterBranchId = _masterBranchBs.GetDetail(user.MasterBranchId).MasterBranchId;
                        user.BranchName = _masterBranchBs.GetDetail(user.MasterBranchId).Name;
                    }

                    var addDay = Common.NumberOfWorkDays(DateTime.Now, WebConfigure.GetApprovalSpvDay());
                    user.DueDateApproval1 = DateTime.Now.AddDays(addDay);

                    if(Request.Files.Count > 0)
                    {
                        for (int i = 0, iLen = Request.Files.Count; i < iLen; i++)
                        {
                            var file = Request.Files[i];
                            string response = Common.ValidateFileUpload(file);

                            if (response.Equals("true"))
                            {
                                if (file != null)
                                {
                                    var fileName = user.EmployeeId + Path.GetExtension(file.FileName);

                                    var path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Upload/UserProfile/"), fileName);
                                    file.SaveAs(path);

                                    user.PhotoProfile = fileName;
                                }
                            }
                        }
                    }

                    var userModel = _userBService.GetByEmployeeId(user.EmployeeId);
                    if(userModel != null && userModel.UserId != 0)
                    {
                        var userModelEdit = _userBService.GetDetail(userModel.UserId);
                        userModelEdit.CreatedAt = DateTime.Now;
                        userModelEdit.UpdatedAt = DateTime.Now;
                        userModelEdit.Status = 2;
                        userModelEdit.Phone = user.Phone;
                        userModelEdit.MobileToken = token;
                        userModelEdit.Approval1 = employeeM.Superior_ID;
                        userModelEdit.AreaName = user.MasterAreaId != 0 ? _masterAreaBs.GetDetail(user.MasterAreaId).Name : null;
                        userModelEdit.BranchName = user.MasterBranchId != 0 ? _masterBranchBs.GetDetail(user.MasterBranchId).Name : null;
                        userModelEdit.MasterAreaId = user.MasterAreaId != 0 ? _masterAreaBs.GetDetail(user.MasterAreaId).MasterAreaId : 0;
                        userModelEdit.MasterBranchId = user.MasterBranchId != 0 ? _masterBranchBs.GetDetail(user.MasterBranchId).MasterBranchId : 0;
                        userModelEdit.RoleId = user.RoleId;
                        userModelEdit.DueDateApproval1 = user.DueDateApproval1;
                        userModelEdit.RoleDescription = user.RoleDescription;
                        userModelEdit.TechnicalJobExperienceDuration = user.TechnicalJobExperienceDuration;
                        userModelEdit.TechnicalJobTitle = user.TechnicalJobTitle;
                         _userBService.Edit(userModelEdit);
                    }
                    else
                    {
                        _userBService.Add(user);
                    }
                    Email.SendMailRegister(user);

                    //send email to approval 1 Superior_ID
                    Email.SendMailNeedApp1(user);
                    return RedirectToAction("Index","User");
                }
            }

            return RedirectToAction("Create", "User");
        }

        private bool RegisterValidate(User model)
        {
            bool ret;
            if (!_employeeMasterBs.CheckEmployee(model.EmployeeId))
            {

                ModelState.AddModelError("EmployeeId", "EmployeeId not found");
                return false;
            }

            if (_userBService.CheckUser(model.EmployeeId))
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

        private User ParsingUser(User model)
        {
            //string dob;
            //get master employee
            var mEmployee = _employeeMasterBs.GetDetail(model.EmployeeId);

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
        public ActionResult Approval(int id)
        {
            if (Common.CheckAdmin())
                return RedirectToAction("Login", "Default");

            var userT = _userBService.GetDetail(id);
            ViewBag.Level = _userRoleBService.GetDetail(userT.RoleId).Name;
            ViewBag.ReplyTo = _userBService.GetDetail(userT.UserId).Email;
            return View(userT);
        }
        [HttpPost]
        public ActionResult Approval(User user)
        {
            if (Common.CheckAdmin())
                return RedirectToAction("Login", "Default");

            //next to active
            var userT = _userBService.GetDetail(user.UserId);
            userT.Status = 7;
            _userBService.Edit(userT);

            //sendEmail to User and approval
            Email.SendMailActived(userT);

            return RedirectToAction("Index");
        }
        // GET: Admin/User/Edit/5
        public ActionResult Edit(int id)
        {
            if (Common.CheckAdmin())
                return RedirectToAction("Login", "Default");

            var userT = _userBService.GetDetail(id);
            var employeeM = _employeeMasterBs.GetDetail(userT.EmployeeId);
            if (employeeM != null)
            {
                ViewBag.EmployeeMaster_empId = Convert.ToString(employeeM.Employee_Id);
                ViewBag.EmpMasterEmail = employeeM.Email == null ? "<span class='text-danger'>This Employee hasn't an employee email</span>" : "<span>Email Registered on Employee Master : <strong>"+ employeeM.Email + "</strong></span>" ;
                ViewBag.OriginBranch = employeeM.Business_Area_Desc == null ? null : employeeM.Business_Area_Desc;
            }
            else
            {
                ViewBag.EmployeeMaster_empId = String.Empty;
                ViewBag.EmpMasterEmail = "<span class='text-danger'>This Employee is not registered in the employee master database</span>";
                ViewBag.OriginBranch = null;
            }
            ViewBag.Email = userT.Email;
            ViewBag.MobilePassword = String.IsNullOrWhiteSpace(userT.MobilePassword) || String.IsNullOrEmpty(userT.MobilePassword) ? "" :Common.Base64Decode(userT.MobilePassword);
            SetListDropdown();
            ViewBag.ListMasterBranch = _masterBranchBs.GetListbyArea(userT.MasterAreaId);
            return View(userT);
        }

        // POST: Admin/User/Edit/5
        [HttpPost]
        public ActionResult Edit(User userM)
        {
            if (Common.CheckAdmin())
                return RedirectToAction("Login", "Default");

            try
            {
                // TODO: Add update logic here
                SetListDropdown();
                ViewBag.ListMasterBranch = _masterBranchBs.GetListbyArea(userM.MasterAreaId);
                var userT = _userBService.GetDetail(userM.UserId);
                var statusBefore = userT.Status;
                userT.Name = userM.Name;
                userT.Position = userM.Position;
                userT.Email = userM.Email;
                userT.Phone = userM.Phone;
                
             
                if (userM.MasterAreaId == 0)
                {
                    userT.MasterAreaId = 0;
                    userT.AreaName = null;
                }
                else
                {
                    userT.MasterAreaId = userM.MasterAreaId;
                    userT.AreaName = _masterAreaBs.GetDetail(userM.MasterAreaId).Name;
                }
                if (userM.MasterBranchId == 0 )
                {
                    userT.MasterBranchId = 0;
                    userT.BranchName = null;
                }
                else
                {
                    userT.MasterBranchId = userM.MasterBranchId;
                    userT.BranchName = _masterBranchBs.GetDetail(userM.MasterBranchId).Name;
                }
                userT.RoleId = userM.RoleId;
                userT.RoleDescription = userM.RoleDescription;
                userT.Status = userM.Status;
                userT.IsAdmin = userM.IsAdmin;
                userT.TechnicalJobExperienceDuration = userM.TechnicalJobExperienceDuration;
                userT.TechnicalJobTitle = userM.TechnicalJobTitle;
                if (!string.IsNullOrWhiteSpace(userM.MobilePassword))
                {
                    userT.MobilePassword = Common.Base64Encode(userM.MobilePassword);
                }

                if (userM.Status == 8)
                {
                    userT.Approval2 = 0;
                }

                if (Request.Files.Count > 0)
                {
                    for (int i = 0, iLen = Request.Files.Count; i < iLen; i++)
                    {
                        var file = Request.Files[i];
                        string response = Common.ValidateFileUpload(file);

                        if (response.Equals("true"))
                        {
                            if (file != null)
                            {
                                var fileName = userT.EmployeeId + "-"+DateTime.Now.ToString("ddMMyyyyHHmm") + Path.GetExtension(file.FileName);

                                var path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Upload/UserProfile/"), fileName);
                                file.SaveAs(path);

                                userT.PhotoProfile = fileName;
                            }
                        }
                    }
                }

                userT.UpdatedAt = DateTime.Now;

                User userdata = _userBService.Edit(userT);
                if (userdata.Status == 7 && statusBefore == 9)
                {
                    Email.SendMailActived(userdata);
                }
                if(Convert.ToInt32( Session["userid"]).Equals(userdata.UserId))
                {
                    Session["photo"] = userdata.PhotoProfile;
                    Session["name"] = userdata.Name;
                    Session["status"] = userdata.Status;
                    Session["role"] = userdata.RoleId;
                    Session["roleColor"] = _userRoleBService.GetDetail(userdata.RoleId).Description;

                }
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return View(userM);
            }
        }

        // GET: Admin/User/Delete/5
        public ActionResult Delete(int id)
        {
            if (Common.CheckAdmin())
                return RedirectToAction("Login", "Default");

            var userT = _userBService.GetDetail(id);

            return View(userT);
        }

        // POST: Admin/User/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            if (Common.CheckAdmin())
                return RedirectToAction("Login", "Default");

            try
            {
                // TODO: Add delete logic here

                var userT = _userBService.GetDetail(id);
                userT.Status = 8;
                userT.UpdatedAt = DateTime.Now;

                _userBService.Edit(userT);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        private void SetListDropdown()
        {
            ViewBag.ListRole = _userRoleBService.GetList();
            ViewBag.ListMasterArea = _masterAreaBs.GetListActive();
        }

        [HttpPost]
        public JsonResult GetBranchs(int id)
        {
            return Json(new SelectList(_masterBranchBs.GetListbyArea(id), "MasterBranchId", "Name"));
        }

        public string CekEmployee(int uxupj)
        {
            string result = "";

            var employeeM = _employeeMasterBs.GetDetail(uxupj);

            if (employeeM != null)
            {
                result = "<span>" + employeeM.Employee_Name + "</span><br>";
                result += "<span>" + employeeM.Position_Name + "</span><br>";
            }


            if (string.IsNullOrEmpty(result))
            {
                result = "Data Not Found";
            }
            return result;
        }

    }
}
