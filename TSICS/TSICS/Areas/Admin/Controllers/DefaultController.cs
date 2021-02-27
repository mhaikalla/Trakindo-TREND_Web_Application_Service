using Com.Trakindo.Framework;
using Com.Trakindo.TSICS.Business.Service;
using Com.Trakindo.TSICS.Data.Model;
using System;
using System.Data;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Mvc;
using TSICS.Helper;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;


namespace TSICS.Areas.Admin.Controllers
{
    public class DefaultController : Controller
    {
        private readonly UserBusinessService _userBService = Factory.Create<UserBusinessService>("User", ClassType.clsTypeBusinessService);
        private readonly LogErrorBusinessService _logErrorBService = Factory.Create<LogErrorBusinessService>("LogError", ClassType.clsTypeBusinessService);
        private readonly MepBusinessService _mepBusinessService = Factory.Create<MepBusinessService>("Mep", ClassType.clsTypeBusinessService);
        private readonly ArticleBusinessService _ArticleBS = Factory.Create<ArticleBusinessService>("Article", ClassType.clsTypeBusinessService);
        private readonly PISBusinessServices pisBS = Factory.Create<PISBusinessServices>("PIS", ClassType.clsTypeBusinessService);
        private readonly PSLMasterBusinessService pslMasterBS = Factory.Create<PSLMasterBusinessService>("PSLMaster", ClassType.clsTypeBusinessService);
        private readonly CalcPISBusinessService calcPisBS = Factory.Create<CalcPISBusinessService>("CalcPIS", ClassType.clsTypeBusinessService);

        // GET: Admin/Default
        public ActionResult Index(String alert = "")
        {
            if (Common.CheckAdmin())
                return RedirectToAction("Login");
            ViewBag.Result = alert;
            ViewBag.DownloadTelematics = WebConfigure.GetDomain() + "/Upload/telematics.xlsx";
            ViewBag.DownloadWarranty = WebConfigure.GetDomain() + "/Upload/Document/WarrantyList_File.csv";
            ViewBag.DownloadJCode = WebConfigure.GetDomain() + "/Upload/Document/Location_File.csv";
            ViewBag.DownloadMEP = WebConfigure.GetDomain() + "/Upload/MEP.xlsx";
            ViewBag.DownloadPIS = WebConfigure.GetDomain() + "/Upload/Document/PIS_File.csv";
            ViewBag.DownloadComment = WebConfigure.GetDomain() + "/Upload/Document/Comment_File.csv";
            ViewBag.DownloadUserGuide = WebConfigure.GetDomain() + "/Upload/Document/" + WebConfigure.GetUserGuideNameFileWithExtention();
            ViewBag.DownloadDPPM = WebConfigure.GetDomain() + "/Upload/Document/DPPMSummary_FIle.csv";
            ViewBag.DownloadDPPMAffectedUnit = WebConfigure.GetDomain() + "/Upload/Document/DPPMAffectedUnit_File.csv";
            ViewBag.DownloadDPPMAffectedPart = WebConfigure.GetDomain() + "/Upload/Document/DPPMAffectedPart_File.csv";
            ViewBag.DownloadSIMSErrorSummary = WebConfigure.GetDomain() + "/Upload/Document/SIMSErrorSummary_File.csv";
            ViewBag.DownloadOrganization = WebConfigure.GetDomain() + "/Upload/Document/Organization_File.csv";
            //return View();
            return RedirectToAction("Index","User");
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            ViewBag.connmain = SharepointHelper.PushConf()["primaindb"];
            ViewBag.connsec = SharepointHelper.PushConf()["secdbempmst"];
            try
            {
                ViewBag.UrlApp = WebConfigure.GetDomain();
                ViewBag.CheckLogin = WebConfigure.GetLoginPortal();

                if (Common.CheckAdmin())
                {
                    _userBService.EndDelegate();
                    return View();
                }
                else
                {
                    return RedirectToAction("Index", "Default");
                }
            }
            catch (Exception er)
            {
                _logErrorBService.WriteLog("Account", MethodBase.GetCurrentMethod().Name, er.ToString());

                throw;
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
#pragma warning disable 1998
        public async Task<ActionResult> Login(User model)
#pragma warning restore 1998
        {
           
            if (ModelState.IsValid)
            {
                if (LoginValidate(model))
                {
                    //session User
                    var userT = _userBService.GetDetailbyUsername(model.Username);
                    Session["useridbackend"] = userT.UserId;
                    Session["rolebackend"] = userT.RoleId;
                    Session["usernamebackend"] = userT.Username;
                    Session["namebackend"] = userT.Name;
                    Session["photobackend"] = userT.PhotoProfile == null ? null : userT.PhotoProfile;
                    Session["emailbackend"] = userT.Email;
                    _userBService.EndDelegate();
                    return RedirectToAction("Index", "Default");
                }
            }

            return View(model);
        }
        private bool LoginValidate(User model)
        {
            bool ret;

            if (_userBService.CheckIsAdmin(model.Username))
            {
                if (_userBService.CheckUsernamePassword(model.Username, model.AdminPassword))
                {
                    ret = true;
                }
                else
                {
                    ModelState.AddModelError("AdminPassword", "Password Is Not Valid");
                    ret = false;
                }
            }
            else
            {
                ModelState.AddModelError("Username", "Username not get permission here");
                ret = false;
            }
            return ret;
        }

        [HttpPost]
        public ActionResult LogOff(User user)
        {
            Session.Remove("useridbackend");
            Session.Remove("rolebackend");
            Session.Remove("usernamebackend");
            Session.Remove("namebackend");
            Session.Remove("photobackend");
            Session.Remove("loginPortal");
            Session.Remove("emailbackend");
            return RedirectToAction("Index");
        }


        [HttpPost]
        public ActionResult UploadTelematicsExcel()
        {
            try
            {
                if (Request.Files.Count > 0)
                {
                    var file = Request.Files["file"];
                    string response = Common.ValidateFileUpload(file);

                    if (response.Equals("true"))
                    {
                        if (file != null)
                        {
                            var fileName = "telematics" + Path.GetExtension(file.FileName);

                            var path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Upload/"), fileName);

                            file.SaveAs(path);
                        }
                    }
                }
                return Content("<script language='javascript' type='text/javascript'>alert('Upload Telematics Excel was Successfull'); location.href = '" + WebConfigure.GetDomain() + "/Admin/Default/Index';</script>");
            }
            catch
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Upload Telematics Excel was Failed, Please Make to Sure About Format File and then upload it'); location.href = '" + WebConfigure.GetDomain() + "/Admin/Default/Index';</script>");
            }
        }

        public ActionResult UploadMepExcel()
        {
            try
            {
                if (Request.Files.Count > 0)
                {
                    var file = Request.Files["file"];
                    string response = Common.ValidateFileUpload(file);

                    if (response.Equals("true"))
                    {
                        if (file != null)
                        {
                            var fileName = "MEP" + Path.GetExtension(file.FileName);

                            var path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Upload/"), fileName);

                            file.SaveAs(path);

                            string oleDbConnectionString =
                                $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={path};Extended Properties=""Excel 12.0 Xml;HDR=YES""";
                            DataTable dataTable = _mepBusinessService.CreateDataTable(oleDbConnectionString);
                            _mepBusinessService.BulkUpdateMep(dataTable, WebConfigure.GetMainConnectionString());
                        }
                    }
                }
                return Content("<script language='javascript' type='text/javascript'>alert('Upload MEP Excel was Successfull'); location.href = '" + WebConfigure.GetDomain() + "/Admin/Default/Index';</script>");
            }
            catch 
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Upload MEP Excel was Failed, Please Make to Sure about format file to upload then'); location.href = '" + WebConfigure.GetDomain() + "/Admin/Default/Index';</script>");
            }
        }
        public ActionResult UploadPisData()
        {
            try
            {
                if (Request.Files.Count > 0)
                {
                    var file = Request.Files["file"];
                    string response = Common.ValidateFileUpload(file);

                    if (response.Equals("true"))
                    {
                        if (file != null)
                        {
                            var fileName = "PIS_File" + Path.GetExtension(file.FileName);
                            var path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Upload/Document/"), fileName);
                            file.SaveAs(path);

                            string csvConnectionString = "Driver={Microsoft Text Driver (*.txt; *.csv)};Dbq=" + path +
                                 ";Extensions=asc,csv,tab,txt;Persist Security Info=False";
                            string oleDbConnectionString =
                               $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={path};Extended Properties=""Excel 12.0 Xml;HDR=YES""";
                            DataTable dataTable = _mepBusinessService.CreateDataTablePIS(path);
                            _mepBusinessService.BulkUpdatePIS(dataTable, WebConfigure.GetMainConnectionString());
                        }
                    }
                }
                _mepBusinessService.SendToXalcPis();
                return Content("<script language='javascript' type='text/javascript'>alert('Upload PIS Excel Success'); location.href = '" + WebConfigure.GetDomain() + "/Admin/Default/Index';</script>");
            }
            catch (Exception ex)
            {
                _logErrorBService.WriteLog("Admin PIS", MethodBase.GetCurrentMethod().Name, ex.ToString());
                return Content("<script language='javascript' type='text/javascript'>alert('Upload PIS Excel was Failed, Please Make to Sure About Format File and then upload again'); location.href = '" + WebConfigure.GetDomain() + "/Admin/Default/Index';</script>");
            }
            //try {
            //if (Request.Files.Count > 0)
            //{
            //    var file = Request.Files["file"];
            //    string response = Common.ValidateFileUpload(file);

            //    if (response.Equals("true"))
            //    {
            //        if (file != null)
            //        {
            //            var fileName = "PIS_File" + Path.GetExtension(file.FileName);
            //            var path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Upload/Document/"), fileName);
            //            file.SaveAs(path);

            //            string csvConnectionString = "Driver={Microsoft Text Driver (*.txt; *.csv)};Dbq=" + path +
            //                 ";Extensions=asc,csv,tab,txt;Persist Security Info=False";
            //            string oleDbConnectionString =
            //               $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={path};Extended Properties=""Excel 12.0 Xml;HDR=YES""";
            //            DataTable dataTable = _mepBusinessService.CreateDataTablePIS(path);
            //            _mepBusinessService.BulkUpdatePIS(dataTable, WebConfigure.GetMainConnectionString());
            //        }

            //    }
            //}
            //_mepBusinessService.SendToXalcPis();
            //return Content("<script language='javascript' type='text/javascript'>alert('Upload PIS Data Excel is Successfull'); location.href = '" + WebConfigure.GetDomain() + "/Admin/Default/Index';</script>");

            //catch (Exception ex)
            //{
            //    return Content("<script language='javascript' type='text/javascript'>alert('Upload PIS Data Excel was Failed, Please make to sure about format file or content file'); location.href = '" + WebConfigure.GetDomain() + "/Admin/Default/Index';</script>");
            //}
        }

        public ActionResult UploadLocationExcel()
        {
            try
            {
                if (Request.Files.Count > 0)
                {
                    var file = Request.Files["file"];
                    string response = Common.ValidateFileUpload(file);

                    if (response.Equals("true"))
                    {
                        if (file != null)
                        {
                            var fileName = "Location_File" + Path.GetExtension(file.FileName);
                            var path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Upload/Document/"), fileName);
                            file.SaveAs(path);

                            string csvConnectionString = "Driver={Microsoft Text Driver (*.txt; *.csv)};Dbq=" + path +
                                 ";Extensions=asc,csv,tab,txt;Persist Security Info=False";
                            string oleDbConnectionString =
                               $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={path};Extended Properties=""Excel 12.0 Xml;HDR=YES""";
                            DataTable dataTable = _mepBusinessService.CreateTableLocation(path);
                            _mepBusinessService.BulkUpdateLocation(dataTable, WebConfigure.GetMainConnectionString());
                        }
                    }
                }

                return Content("<script language='javascript' type='text/javascript'>alert('Upload J-Code Data Excel is Successfull'); location.href = '" + WebConfigure.GetDomain() + "/Admin/Default/Index';</script>");
            }
            catch(Exception ex)
            {
                _logErrorBService.WriteLog("Admin Upload location", MethodBase.GetCurrentMethod().Name, ex.ToString());
                return Content("<script language='javascript' type='text/javascript'>alert('Upload J-COde Data was Failed, Please Make to Sure About Format File and then upload again'); location.href = '" + WebConfigure.GetDomain() + "/Admin/Default/Index';</script>");
            }
        }

        public ActionResult UploadCommentExcel()
        {
            try
            {
                if (Request.Files.Count > 0)
                {
                    var file = Request.Files["file"];
                    string response = Common.ValidateFileUpload(file);

                    if (response.Equals("true"))
                    {
                        if (file != null)
                        {
                            var fileName = "Comment_File" + Path.GetExtension(file.FileName);
                            var path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Upload/Document/"), fileName);
                            file.SaveAs(path);

                            string csvConnectionString = "Driver={Microsoft Text Driver (*.txt; *.csv)};Dbq=" + path +
                                 ";Extensions=asc,csv,tab,txt;Persist Security Info=False";
                            string oleDbConnectionString =
                               $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={path};Extended Properties=""Excel 12.0 Xml;HDR=YES""";
                            DataTable dataTable = _mepBusinessService.CreateTableComment(path);
                            _mepBusinessService.BulkUpdateComment(dataTable, WebConfigure.GetMainConnectionString());
                        }
                    }
                }
                return Content("<script language='javascript' type='text/javascript'>alert('Upload Comment Data Excel Successful'); location.href = '" + WebConfigure.GetDomain() + "/Admin/Default/Index';</script>");
            }
            catch (Exception e)
            {

                _logErrorBService.WriteLog("Admin UploadCommentExcel", MethodBase.GetCurrentMethod().Name, e.ToString());

                return Content("<script language='javascript' type='text/javascript'>alert('Upload Comment was Failed, Please Make to Sure About Format File and then upload again'); location.href = '" + WebConfigure.GetDomain() + "/Admin/Default/Index';</script>");
            }
        }

        public ActionResult UploadWarrantyListExcel()
        {
            try{
                if (Request.Files.Count > 0)
                {
                    var file = Request.Files["file"];
                    string response = Common.ValidateFileUpload(file);

                    if (response.Equals("true"))
                    {
                        if (file != null)
                        {
                            var fileName = "WarrantyList_File" + Path.GetExtension(file.FileName);
                            var path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Upload/Document/"), fileName);
                            file.SaveAs(path);

                            string csvConnectionString = "Driver={Microsoft Text Driver (*.txt; *.csv)};Dbq=" + path +
                                 ";Extensions=asc,csv,tab,txt;Persist Security Info=False";
                            string oleDbConnectionString =
                               $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={path};Extended Properties=""Excel 12.0 Xml;HDR=YES""";
                            DataTable dataTable = _mepBusinessService.CreateTableWarrantyList(path);
                            _mepBusinessService.BulkUpdateWarrantyList(dataTable, WebConfigure.GetMainConnectionString());
                        }
                    }
                }
                return Content("<script language='javascript' type='text/javascript'>alert('Upload Warranty Excel Success'); location.href = '" + WebConfigure.GetDomain() + "/Admin/Default/Index';</script>");
            }
            catch (Exception ex)
            {
                _logErrorBService.WriteLog("Admin Warranty List", MethodBase.GetCurrentMethod().Name, ex.ToString());
                return Content("<script language='javascript' type='text/javascript'>alert('Upload Warranty Excel was Failed, Please Make to Sure About Format File and then upload again'); location.href = '" + WebConfigure.GetDomain() + "/Admin/Default/Index';</script>");
            }

        }

        public ActionResult UploadDPPMSummaryExcel()
        {
            try
            {
                if (Request.Files.Count > 0)
                {
                    var file = Request.Files["file"];
                    string response = Common.ValidateFileUpload(file);

                    if (response.Equals("true"))
                    {
                        if (file != null)
                        {
                            var fileName = "DPPMSummary_FIle" + Path.GetExtension(file.FileName);
                            var path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Upload/Document/"), fileName);
                            file.SaveAs(path);

                            string csvConnectionString = "Driver={Microsoft Text Driver (*.txt; *.csv)};Dbq=" + path +
                                 ";Extensions=asc,csv,tab,txt;Persist Security Info=False";
                            string oleDbConnectionString =
                               $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={path};Extended Properties=""Excel 12.0 Xml;HDR=YES""";
                            DataTable dataTable = _mepBusinessService.CreateTableDPPMSummary(path);
                            _mepBusinessService.BulkUpdateDPPMSummary(dataTable, WebConfigure.GetMainConnectionString());
                        }
                    }
                }
                return Content("<script language='javascript' type='text/javascript'>alert('Upload DPPM Summary Excel Success'); location.href = '" + WebConfigure.GetDomain() + "/Admin/Default/Index';</script>");
            }
            catch (Exception ex)
            {
                _logErrorBService.WriteLog("Admin DPPM SUmmary", MethodBase.GetCurrentMethod().Name, ex.ToString());
                return Content("<script language='javascript' type='text/javascript'>alert('Upload DPPM Summary Excel was Failed, Please Make to Sure About Format File and then upload again'); location.href = '" + WebConfigure.GetDomain() + "/Admin/Default/Index';</script>");
            }

        }

        public ActionResult UploadDPPMAffectedUnit()
        {
            try
            {
                if (Request.Files.Count > 0)
                {
                    var file = Request.Files["file"];
                    string response = Common.ValidateFileUpload(file);

                    if (response.Equals("true"))
                    {
                        if (file != null)
                        {
                            var fileName = "DPPMAffectedUnit_File" + Path.GetExtension(file.FileName);
                            var path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Upload/Document/"), fileName);
                            file.SaveAs(path);

                            string csvConnectionString = "Driver={Microsoft Text Driver (*.txt; *.csv)};Dbq=" + path +
                                 ";Extensions=asc,csv,tab,txt;Persist Security Info=False";
                            string oleDbConnectionString =
                               $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={path};Extended Properties=""Excel 12.0 Xml;HDR=YES""";
                            DataTable dataTable = _mepBusinessService.CreateTableDPPMAffectedUnit(path);
                            _mepBusinessService.BulkUpdateDPPMAffectedUnit(dataTable, WebConfigure.GetMainConnectionString());
                        }
                    }
                }
                return Content("<script language='javascript' type='text/javascript'>alert('Upload DPPM Affected Unit Excel Success'); location.href = '" + WebConfigure.GetDomain() + "/Admin/Default/Index';</script>");
            }
            catch (Exception ex)
            {
                _logErrorBService.WriteLog("Admin Warranty List", MethodBase.GetCurrentMethod().Name, ex.ToString());
                return Content("<script language='javascript' type='text/javascript'>alert('Upload DPPM Affected Unit Excel was Failed, Please Make to Sure About Format File and then upload again'); location.href = '" + WebConfigure.GetDomain() + "/Admin/Default/Index';</script>");
            }

        }

        public ActionResult UploadDPPMAffectedPart()
        {
            try
            {
                if (Request.Files.Count > 0)
                {
                    var file = Request.Files["file"];
                    string response = Common.ValidateFileUpload(file);

                    if (response.Equals("true"))
                    {
                        if (file != null)
                        {
                            var fileName = "DPPMAffectedPart_File" + Path.GetExtension(file.FileName);
                            var path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Upload/Document/"), fileName);
                            file.SaveAs(path);

                            string csvConnectionString = "Driver={Microsoft Text Driver (*.txt; *.csv)};Dbq=" + path +
                                 ";Extensions=asc,csv,tab,txt;Persist Security Info=False";
                            string oleDbConnectionString =
                               $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={path};Extended Properties=""Excel 12.0 Xml;HDR=YES""";
                            DataTable dataTable = _mepBusinessService.CreateTableDPPMAffectedPart(path);
                            _mepBusinessService.BulkUpdateDPPMAffectedPart(dataTable, WebConfigure.GetMainConnectionString());
                        }
                    }
                }
                return Content("<script language='javascript' type='text/javascript'>alert('Upload DPPM Affected Part Excel Success'); location.href = '" + WebConfigure.GetDomain() + "/Admin/Default/Index';</script>");
            }
            catch (Exception ex)
            {
                _logErrorBService.WriteLog("Admin Warranty List", MethodBase.GetCurrentMethod().Name, ex.ToString());
                return Content("<script language='javascript' type='text/javascript'>alert('Upload DPPM Affected Part Excel was Failed, Please Make to Sure About Format File and then upload again'); location.href = '" + WebConfigure.GetDomain() + "/Admin/Default/Index';</script>");
            }

        }

        public ActionResult UploadSIMSErrorSummary()
        {
            try
            {
                if (Request.Files.Count > 0)
                {
                    var file = Request.Files["file"];
                    string response = Common.ValidateFileUpload(file);

                    if (response.Equals("true"))
                    {
                        if (file != null)
                        {
                            var fileName = "SIMSErrorSummary_File" + Path.GetExtension(file.FileName);
                            var path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Upload/Document/"), fileName);
                            file.SaveAs(path);

                            string csvConnectionString = "Driver={Microsoft Text Driver (*.txt; *.csv)};Dbq=" + path +
                                 ";Extensions=asc,csv,tab,txt;Persist Security Info=False";
                            string oleDbConnectionString =
                               $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={path};Extended Properties=""Excel 12.0 Xml;HDR=YES""";
                            DataTable dataTable = _mepBusinessService.CreateTableSIMSErrorSummary(path);
                            _mepBusinessService.BulkUpdateSIMSErrorSummary(dataTable, WebConfigure.GetMainConnectionString());
                        }
                    }
                }
                return Content("<script language='javascript' type='text/javascript'>alert('Upload SIMS Error Summary Excel Success'); location.href = '" + WebConfigure.GetDomain() + "/Admin/Default/Index';</script>");
            }
            catch (Exception ex)
            {
                _logErrorBService.WriteLog("Admin Warranty List", MethodBase.GetCurrentMethod().Name, ex.ToString());
                return Content("<script language='javascript' type='text/javascript'>alert('Upload SIMS Error Summary Excel was Failed, Please Make to Sure About Format File and then upload again'); location.href = '" + WebConfigure.GetDomain() + "/Admin/Default/Index';</script>");
            }

        }

        public ActionResult UploadOrganization()
        {
            try
            {
                if (Request.Files.Count > 0)
                {
                    var file = Request.Files["file"];
                    string response = Common.ValidateFileUpload(file);

                    if (response.Equals("true"))
                    {
                        if (file != null)
                        {
                            var fileName = "Organization_File" + Path.GetExtension(file.FileName);
                            var path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Upload/Document/"), fileName);
                            file.SaveAs(path);

                            string csvConnectionString = "Driver={Microsoft Text Driver (*.txt; *.csv)};Dbq=" + path +
                                 ";Extensions=asc,csv,tab,txt;Persist Security Info=False";
                            string oleDbConnectionString =
                               $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={path};Extended Properties=""Excel 12.0 Xml;HDR=YES""";
                            DataTable dataTable = _mepBusinessService.CreateTableOrganization(path);
                            _mepBusinessService.BulkUpdateOrganization(dataTable, WebConfigure.GetMainConnectionString());
                        }
                    }
                }
                return Content("<script language='javascript' type='text/javascript'>alert('Upload Organization Excel Success'); location.href = '" + WebConfigure.GetDomain() + "/Admin/Default/Index';</script>");
            }
            catch (Exception ex)
            {
                _logErrorBService.WriteLog("Admin Warranty List", MethodBase.GetCurrentMethod().Name, ex.ToString());
                return Content("<script language='javascript' type='text/javascript'>alert('Upload Organization Excel was Failed, Please Make to Sure About Format File and then upload again'); location.href = '" + WebConfigure.GetDomain() + "/Admin/Default/Index';</script>");
            }

        }

        public ActionResult UploadPSLPartExcel()
        {

            try
            {
                if (Request.Files.Count > 0)
                {
                    var file = Request.Files["file"];
                    string response = Common.ValidateFileUpload(file);

                    if (response.Equals("true"))
                    {
                        if (file != null)
                        {
                            var fileName = "PSLPart_File" + Path.GetExtension(file.FileName);
                            var path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Upload/Document/"), fileName);
                            file.SaveAs(path);

                            string csvConnectionString = "Driver={Microsoft Text Driver (*.txt; *.csv)};Dbq=" + path +
                                 ";Extensions=asc,csv,tab,txt;Persist Security Info=False";
                            string oleDbConnectionString =
                               $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={path};Extended Properties=""Excel 12.0 Xml;HDR=YES""";
                            DataTable dataTable = _mepBusinessService.CreateTablePSLPart(path);
                            _mepBusinessService.BulkUpdatePSLPart(dataTable, WebConfigure.GetMainConnectionString());
                        }
                    }
                }

                return Content("<script language='javascript' type='text/javascript'>alert('Upload PSLPart Data Excel is Successfull'); location.href = '" + WebConfigure.GetDomain() + "/Admin/Default/Index';</script>");
            }
            catch (Exception ex)
            {
                _logErrorBService.WriteLog("Admin Upload PSLPart", MethodBase.GetCurrentMethod().Name, ex.ToString());
                return Content("<script language='javascript' type='text/javascript'>alert('Upload PSLPart Data was Failed, Please Make to Sure About Format File and then upload again'); location.href = '" + WebConfigure.GetDomain() + "/Admin/Default/Index';</script>");
            }
        }

        public ActionResult MyAccount()
        {
            if (Session["useridbackend"] == null)
                return RedirectToAction("Index");

            return View(_userBService.GetDetail(Convert.ToInt32(Session["useridbackend"])));
        }
        [HttpGet]
        public ActionResult Edit()
        {
            if (Session["useridbackend"] == null)
                return RedirectToAction("Index");

            return View(_userBService.GetDetail(Convert.ToInt32(Session["useridbackend"])));
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(User UserAdmin)
        {
            if (Session["useridbackend"] == null)
                return RedirectToAction("Index");
            //if (!_userBService.CheckName(UserAdmin.Name))
            //{
            //    ModelState.AddModelError("Name", "This Name was already used");
            //    return RedirectToAction("Edit", "Default");
            //}//Fixed PhotoProfile
            User user = _userBService.GetDetail(UserAdmin.UserId);

           
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
                            var fileName = UserAdmin.UserId +"-"+ DateTime.Now.ToString("ddMMyyyyHHmm") + Path.GetExtension(file.FileName);

                            var path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Upload/UserProfile/"), fileName);
                            file.SaveAs(path);

                            user.PhotoProfile = fileName;
                        }
                    }
                }
            }
            user.Name = UserAdmin.Name;
            user.Email= UserAdmin.Email;
            user.IsAdmin = 1;
            var Result = _userBService.Edit(user);
            Session["namebackend"] = Result.Name;
            Session["photobackend"] = Result.PhotoProfile;
            return RedirectToAction("Edit", "Default");

        }
        [HttpGet]
        public ActionResult RemovePhoto()
        {
            if (Session["useridbackend"] == null)
                return RedirectToAction("Index");
            User user = _userBService.GetDetail(Convert.ToInt32(Session["useridbackend"]));

            if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(Path.Combine("~/Upload/UserProfile/", user.PhotoProfile))))
            {
                System.IO.File.Delete(System.Web.HttpContext.Current.Server.MapPath(Path.Combine("~/Upload/UserProfile/", user.PhotoProfile)));
            }
            user.PhotoProfile = null;
            _userBService.Edit(user);
            Session["photobackend"] = null;
            return RedirectToAction("Edit", "Default");
        }
        [HttpPost]
        public ActionResult UploadUserGuide()
        {
            try
            {
                if (Request.Files.Count > 0)
                {
                    var file = Request.Files["file"];
                    string response = Common.ValidateFileUpload(file);

                    if (response.Equals("true"))
                    {
                        if (file != null)
                        {
                            var fileName = Path.GetFileNameWithoutExtension(WebConfigure.GetUserGuideNameFileWithExtention()) + Path.GetExtension(file.FileName);

                            var path = Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Upload/Document"), fileName);

                            file.SaveAs(path);
                        }
                    }
                }
                return Content("<script language='javascript' type='text/javascript'>alert('Upload User Guide was Successfull'); location.href = '" + WebConfigure.GetDomain() + "/Admin/Default/Index';</script>");
            }
            catch
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Upload User Guide was Failed, Please Make to Sure About Format File and then upload it'); location.href = '" + WebConfigure.GetDomain() + "/Admin/Default/Index';</script>");
            }
        }
    }
}
