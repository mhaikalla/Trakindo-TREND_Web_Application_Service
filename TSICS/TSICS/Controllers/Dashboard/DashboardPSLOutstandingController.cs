using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Com.Trakindo.Framework;
using Com.Trakindo.TSICS.Business.Service;
using Com.Trakindo.TSICS.Data.Model;
using Newtonsoft.Json;
using TSICS.Models.Dashboard;

namespace TSICS.Controllers.Dashboard
{
    public partial class DashboardController
    {
        private readonly PSLMasterBusinessService pslBS = Factory.Create<PSLMasterBusinessService>("PSLMaster", ClassType.clsTypeBusinessService);
        // GET: DashboardPSLOutstanding
        public ActionResult DashboardPSLOutstanding()
        {
            this.setViewBag();
            var getListArea = pslBS.GetListArea();
            var getListSalesOffice = pslBS.GetListSalesOffice();
            var getListPSLStatus = pslBS.GetListPSLStatus();
            var getListPSLNo = pslBS.GetListPSLNo();
            var getListAgeIndicator = pslBS.GetListAgeIndicator();
            var getListSerialNumber = pslBS.GetListSerialNumber();
            var getListModel = pslBS.GetListModel();
            var getListPriority = pslBS.GetListPriority();
            ViewBag.ListArea = getListArea;
            ViewBag.ListSalesOffice = getListSalesOffice;
            ViewBag.ListPSLStatus = getListPSLStatus;
            ViewBag.ListPSLNo = getListPSLNo;
            ViewBag.ListAgeIndicator = getListAgeIndicator;
            ViewBag.ListSerialNumber = getListSerialNumber;
            ViewBag.ListModel = getListModel;
            ViewBag.ListPriority = getListPriority;

            var strPipSafety = "on";
            var strPipPriority = "on";
            var strPspProactive = string.Empty;
            var strPspAfterFailure = string.Empty;
            var strReleaseDateFrom = string.Empty;
            var strReleaseDateEnd = string.Empty;
            var strTerminationDateFrom = string.Empty;
            var strTerminationDateEnd = string.Empty;
            var strInventory = string.Empty;
            var strRental = string.Empty;
            var strHid = string.Empty;
            var strOther = string.Empty;
            var strArea = string.Empty;
            var strSalesOffice = string.Empty;
            var strPSLStatus = string.Empty;
            var strPSLNo = string.Empty;
            var strAgeIndicator = string.Empty;
            var strSerialNumber = string.Empty;
            var strModel = string.Empty;
            var strPrioroty = string.Empty;
            var checklistArea = string.Empty;
            var checklistSalesOffice = string.Empty;
            var checklistPSLStatus = string.Empty;
            var checklistPSLNo = string.Empty;
            var checklistAgeIndicator = string.Empty;
            var checklistSerialNumber = string.Empty;
            var checklistModel = string.Empty;
            var checklistPriority = string.Empty;
            var strAreaFilter = string.Empty;
            var strTypefilter = string.Empty;

            var splitArea = new string[] { };
            var splitSalesOffice = new string[] { };
            var splitPSLStatus = new string[] { };
            var splitPSLNo = new string[] { };
            var splitSerialNumber = new string[] { };
            var splitModel = new string[] { };
            var splitAgeIndicator = new string[] { };
            var splitPriority = new string[] { };
            var ReleaseDateFrom = string.Empty;
            var ReleaseDateEnd = string.Empty;
            var TerminationDateFrom = string.Empty;
            var TerminationDateEnd = string.Empty;


            var listOutstanding = new List<int>();
            var getDataArea = pslBS.GetListByArea(splitArea, splitSalesOffice, splitPSLStatus, splitPSLNo, splitAgeIndicator, splitSerialNumber, splitModel, splitPriority, ReleaseDateFrom, ReleaseDateEnd, TerminationDateFrom, TerminationDateEnd, strAreaFilter, strTypefilter, strPipSafety, strPipPriority, strPspProactive, strPspAfterFailure, strHid, strRental, strInventory, strOther);

            foreach(var item in getDataArea)
            {
                listOutstanding.Add(item.StatusOutstanding);
            }

            int sum = 0;
            for(int i = 0; i < listOutstanding.Count(); i++)
            {
                sum += listOutstanding[i];
            }
            ViewBag.TotalOutstanding = sum;
            var modelFormCollection = new GetFormCollectionPSLOutstanding();
            modelFormCollection.PipSafety = strPipSafety;
            modelFormCollection.PipPriority = strPipPriority;
            modelFormCollection.PspProactive = strPspProactive;
            modelFormCollection.PspAfterFailure = strPspAfterFailure;
            modelFormCollection.ReleaseDateFrom = strReleaseDateFrom;
            modelFormCollection.ReleaseDateEnd = strReleaseDateEnd;
            modelFormCollection.TerminationDateFrom = strTerminationDateFrom;
            modelFormCollection.TerminationDateEnd = strTerminationDateEnd;
            modelFormCollection.Inventory = strInventory;
            modelFormCollection.Rental = strRental;
            modelFormCollection.HID = strHid;
            modelFormCollection.Area = strArea;
            modelFormCollection.SalesOffice = strSalesOffice;
            modelFormCollection.PSLStatus = strPSLStatus;
            modelFormCollection.PSLNo = strPSLNo;
            modelFormCollection.AgeIndicator = strAgeIndicator;
            modelFormCollection.SerialNumber = strSerialNumber;
            modelFormCollection.Model = strModel;
            modelFormCollection.Priority = strPrioroty;
            modelFormCollection.ChecklistArea = checklistArea;
            modelFormCollection.ChecklistSalesOffice = checklistSalesOffice;
            modelFormCollection.ChecklistPSLStatus = checklistPSLStatus;
            modelFormCollection.ChecklistPSLNo = checklistPSLNo;
            modelFormCollection.ChecklistAgeIndicator = checklistAgeIndicator;
            modelFormCollection.ChecklistSerialNumber = checklistSerialNumber;
            modelFormCollection.ChecklistModel = checklistModel;
            modelFormCollection.ChecklistPriority = checklistPriority;
            ViewBag.ModelGetFormCollection = modelFormCollection;

            return View();
        }

        [HttpPost]
        public ActionResult DashboardPSLOutstanding(FormCollection fc)
        {
            var dateReleaseForm = "";
            var dateReleaseEnd = "";
            var dateTerminationFrom = "";
            var dateTermitaionEnd = "";

            var strPipSafety = (fc["pipSafety"] != null) ? fc["pipSafety"] : "";
            var strPipPriority = (fc["pipPriority"] != null) ? fc["pipPriority"] : "";
            var strPspProactive = (fc["pspProactive"] != null) ? fc["pspProactive"] : "";
            var strPspAfterFailure = (fc["pspAfterfailure"] != null) ? fc["pspAfterfailure"] : "";
            if(fc["releaseDate"] != null)
            {
                var strlist = fc["releaseDate"].Split(' ', 't', 'o', ' ');
                if (strlist.Count() == 5)
                {
                    dateReleaseForm = strlist[0];
                    dateReleaseEnd = strlist[4];
                }
                else
                {
                    dateReleaseForm = strlist[0];
                }
            }
            else
            {
                dateReleaseForm = "";
                dateReleaseEnd = "";
            }
            if (fc["terminationDate"] != null)
            {
                var strlist = fc["terminationDate"].Split(' ', 't', 'o', ' ');
                if (strlist.Count() == 5)
                {
                    dateTerminationFrom = strlist[0];
                    dateTermitaionEnd = strlist[4];
                }
                else
                {
                    dateTerminationFrom = strlist[0];
                }
            }
            else
            {
                dateTerminationFrom = "";
                dateTermitaionEnd = "";
            }
            var strReleaseDateFrom = (dateReleaseForm != null) ? dateReleaseForm : "";
            var strReleaseDateEnd = (dateReleaseEnd != null) ? dateReleaseEnd : "";
            var strTerminationFrom = (dateTerminationFrom != null) ? dateTerminationFrom : "";
            var strTerminationEnd = (dateTermitaionEnd != null) ? dateTermitaionEnd : "";
            var strInventory = (fc["inventory"] != null) ? fc["inventory"] : "";
            var strRental = (fc["rental"] != null) ? fc["rental"] : "";
            var strHid = (fc["hid"] != null) ? fc["hid"] : "";
            var strOther = (fc["others"] != null) ? fc["others"] : "";
            var strArea = (fc["Area[]"] != null) ? fc["Area[]"] : "";
            var strSalesOffice = (fc["salesOffice[]"] != null) ? fc["salesOffice[]"] : "";
            var strPSLStatus = (fc["pslStatus[]"] != null) ? fc["pslStatus[]"] : "";
            var strPSLNo = (fc["pslNo[]"] != null) ? fc["pslNo[]"] : "";
            var strAgeIndicator = (fc["ageIndicator[]"] != null) ? fc["ageIndicator[]"] : "";
            var strSerialNumber = (fc["serialNo[]"] != null) ? fc["serialNo[]"] : "";
            var strModel = (fc["model[]"] != null) ? fc["model[]"] : "";
            var strPrioroty = (fc["priority[]"] != null) ? fc["priority[]"] : "";
            var checklistArea = (fc["checklistArea"] != null) ? fc["checklistArea"] : "";
            var checklistSalesOffice = (fc["checklistSalesOffice"] != null) ? fc["checklistSalesOffice"] : "";
            var checklistPSLStatus = (fc["checklistPSLStatus"] != null) ? fc["checklistPSLStatus"] : "";
            var checklistPSLNo = (fc["checklistPSLNo"] != null) ? fc["checklistPSLNo"] : "";
            var checklistAgeIndicator = (fc["checklistAgeIndicator"] != null) ? fc["checklistAgeIndicator"] : "";
            var checklistSerialNumber = (fc["checklistSerialNumber"] != null) ? fc["checklistSerialNumber"] : "";
            var checklistModel = (fc["checklistModel"] != null) ? fc["checklistModel"] : "";
            var checklistPriority = (fc["checklistPriority"] != null) ? fc["checklistPriority"] : "";
            var strAreaFilter = (fc["area"] != null) ? fc["area"] : "";
            var strTypeFilter = (fc["type"] != null) ? fc["type"] : "";
            var modelFormCollection = new GetFormCollectionPSLOutstanding();

            #region Split String
            var splitArea = new string[] { };
            var splitSalesOffice = new string[] { };
            var splitPSLStatus = new string[] { };
            var splitPSLNo = new string[] { };
            var splitAgeIndicator = new string[] { };
            var splitSerialNumber = new string[] { };
            var splitModel = new string[] { };
            var splitPriority = new string[] { };

            if (!string.IsNullOrWhiteSpace(strArea))
            {
                splitArea = strArea.Split(',');
            }

            if (!string.IsNullOrWhiteSpace(strSalesOffice))
            {
                splitSalesOffice = strSalesOffice.Split(',');
            }

            if (!string.IsNullOrWhiteSpace(strPSLStatus))
            {
                splitPSLStatus = strPSLStatus.Split(',');
            }

            if (!string.IsNullOrWhiteSpace(strPSLNo))
            {
                splitPSLNo = strPSLNo.Split(',');
            }

            if (!string.IsNullOrWhiteSpace(strAgeIndicator))
            {
                splitAgeIndicator = strAgeIndicator.Split(',');
            }

            if (!string.IsNullOrWhiteSpace(strSerialNumber))
            {
                splitSerialNumber = strSerialNumber.Split(',');
            }

            if (!string.IsNullOrWhiteSpace(strModel))
            {
                splitModel = strModel.Split(',');
            }

            if (!string.IsNullOrWhiteSpace(strPrioroty))
            {
                splitPriority = strPrioroty.Split(',');
            }
            #endregion

            var listOutstanding = new List<int>();
            var getDataArea = pslBS.GetListByArea(splitArea, splitSalesOffice, splitPSLStatus, splitPSLNo, splitAgeIndicator, splitSerialNumber, splitModel, splitPriority, strReleaseDateFrom, strReleaseDateEnd, strTerminationFrom, strTerminationEnd, strAreaFilter, strTypeFilter , strPipSafety, strPipPriority, strPspProactive, strPspAfterFailure, strHid, strRental, strInventory, strOther);

            foreach (var item in getDataArea)
            {
                listOutstanding.Add(item.StatusOutstanding+item.StatusOpen+item.StatusInProgress+item.StatusRelease);
            }

            int sum = 0;
            for (int i = 0; i < listOutstanding.Count(); i++)
            {
                sum += listOutstanding[i];
            }
            ViewBag.TotalOutstanding = sum;

            modelFormCollection.PipSafety = strPipSafety;
            modelFormCollection.PipPriority = strPipPriority;
            modelFormCollection.PspProactive = strPspProactive;
            modelFormCollection.PspAfterFailure = strPspAfterFailure;
            modelFormCollection.ReleaseDateFrom = strReleaseDateFrom;
            modelFormCollection.ReleaseDateEnd = strReleaseDateEnd;
            modelFormCollection.TerminationDateFrom = strTerminationFrom;
            modelFormCollection.TerminationDateEnd = strTerminationEnd;
            modelFormCollection.Inventory = strInventory;
            modelFormCollection.Rental = strRental;
            modelFormCollection.HID = strHid;
            modelFormCollection.Area = strArea;
            modelFormCollection.SalesOffice = strSalesOffice;
            modelFormCollection.PSLStatus = strPSLStatus;
            modelFormCollection.PSLNo = strPSLNo;
            modelFormCollection.AgeIndicator = strAgeIndicator;
            modelFormCollection.SerialNumber = strSerialNumber;
            modelFormCollection.Model = strModel;
            modelFormCollection.Priority = strPrioroty;
            modelFormCollection.ChecklistArea = checklistArea;
            modelFormCollection.ChecklistSalesOffice = checklistSalesOffice;
            modelFormCollection.ChecklistPSLStatus = checklistPSLStatus;
            modelFormCollection.ChecklistPSLNo = checklistPSLNo;
            modelFormCollection.ChecklistAgeIndicator = checklistAgeIndicator;
            modelFormCollection.ChecklistSerialNumber = checklistSerialNumber;
            modelFormCollection.ChecklistModel = checklistModel;
            modelFormCollection.ChecklistPriority = checklistPriority;
            modelFormCollection.Others = strOther;

            ViewBag.ModelGetFormCollection = modelFormCollection;

            var getListArea = pslBS.GetListArea();
            var getListSalesOffice = pslBS.GetListSalesOffice();
            var getListPSLStatus = pslBS.GetListPSLStatus();
            var getListPSLNo = pslBS.GetListPSLNo();
            var getListAgeIndicator = pslBS.GetListAgeIndicator();
            var getListSerialNumber = pslBS.GetListSerialNumber();
            var getListModel = pslBS.GetListModel();
            var getListPriority = pslBS.GetListPriority();
            ViewBag.ListArea = getListArea;
            ViewBag.ListSalesOffice = getListSalesOffice;
            ViewBag.ListPSLStatus = getListPSLStatus;
            ViewBag.ListPSLNo = getListPSLNo;
            ViewBag.ListAgeIndicator = getListAgeIndicator;
            ViewBag.ListSerialNumber = getListSerialNumber;
            ViewBag.ListModel = getListModel;
            ViewBag.ListPriority = getListPriority;
            ViewBag.UseFullLink = _articleBs.getUseFullLink().Count > 0 ? _articleBs.getUseFullLink() : null;
            return View();
        }

        [HttpPost]
        public JsonResult GetArea(string[] salesOffice, string[] pslStatus, string[] pslNo, int[] ageIndicator, string[] serialNumber, string[] model, string[] priority, string[] type, string terminationDate, string releaseDate, string inventory, string rental, string hid, string others)
        {
            var dateReleaseForm = "";
            var dateReleaseEnd = "";
            var dateTerminationFrom = "";
            var dateTermitaionEnd = "";
            if (releaseDate != null)
            {
                var strlist = releaseDate.Split(' ', 't', 'o', ' ');
                if (strlist.Count() == 5)
                {
                    dateReleaseForm = strlist[0];
                    dateReleaseEnd = strlist[4];
                }
                else
                {
                    dateReleaseForm = strlist[0];
                }
            }
            else
            {
                dateReleaseForm = "";
                dateReleaseEnd = "";
            }
            if (terminationDate != null)
            {
                var strlist = terminationDate.Split(' ', 't', 'o', ' ');
                if (strlist.Count() == 5)
                {
                    dateTerminationFrom = strlist[0];
                    dateTermitaionEnd = strlist[4];
                }
                else
                {
                    dateTerminationFrom = strlist[0];
                }
            }
            else
            {
                dateTerminationFrom = "";
                dateTermitaionEnd = "";
            }
            return Json(new SelectList(pslBS.GetArea(salesOffice, pslStatus, pslNo, ageIndicator, serialNumber, model, priority, type, dateTerminationFrom, dateTermitaionEnd, dateReleaseForm, dateReleaseEnd, inventory, rental, hid, others), "Area", "Area"));
        }

        [HttpPost]
        public JsonResult GetSalesOffice(string[] areachild, string[] pslStatus, string[] pslNo, int[] ageIndicator, string[] serialNumber, string[] model, string[] priority, string[] type, string terminationDate, string releaseDate, string inventory, string rental, string hid, string others)
        {
            var dateReleaseForm = "";
            var dateReleaseEnd = "";
            var dateTerminationFrom = "";
            var dateTermitaionEnd = "";
            if (releaseDate != null)
            {
                var strlist = releaseDate.Split(' ', 't', 'o', ' ');
                if (strlist.Count() == 5)
                {
                    dateReleaseForm = strlist[0];
                    dateReleaseEnd = strlist[4];
                }
                else
                {
                    dateReleaseForm = strlist[0];
                }
            }
            else
            {
                dateReleaseForm = "";
                dateReleaseEnd = "";
            }
            if (terminationDate != null)
            {
                var strlist = terminationDate.Split(' ', 't', 'o', ' ');
                if (strlist.Count() == 5)
                {
                    dateTerminationFrom = strlist[0];
                    dateTermitaionEnd = strlist[4];
                }
                else
                {
                    dateTerminationFrom = strlist[0];
                }
            }
            else
            {
                dateTerminationFrom = "";
                dateTermitaionEnd = "";
            }
            return Json(new SelectList(pslBS.GetSalesOffice(areachild, pslStatus, pslNo, ageIndicator, serialNumber, model, priority, type, dateTerminationFrom, dateTermitaionEnd, dateReleaseForm, dateReleaseEnd, inventory, rental, hid, others), "SalesOffice", "SalesOffice"));
        }

        [HttpPost]
        public JsonResult GetPSLStatus(string[] areachild, string[] salesOffice, string[] pslNo, int[] ageIndicator, string[] serialNumber, string[] model, string[] priority, string[] type, string terminationDate, string releaseDate, string inventory, string rental, string hid, string others)
        {
            var dateReleaseForm = "";
            var dateReleaseEnd = "";
            var dateTerminationFrom = "";
            var dateTermitaionEnd = "";
            if (releaseDate != null)
            {
                var strlist = releaseDate.Split(' ', 't', 'o', ' ');
                if (strlist.Count() == 5)
                {
                    dateReleaseForm = strlist[0];
                    dateReleaseEnd = strlist[4];
                }
                else
                {
                    dateReleaseForm = strlist[0];
                }
            }
            else
            {
                dateReleaseForm = "";
                dateReleaseEnd = "";
            }
            if (terminationDate != null)
            {
                var strlist = terminationDate.Split(' ', 't', 'o', ' ');
                if (strlist.Count() == 5)
                {
                    dateTerminationFrom = strlist[0];
                    dateTermitaionEnd = strlist[4];
                }
                else
                {
                    dateTerminationFrom = strlist[0];
                }
            }
            else
            {
                dateTerminationFrom = "";
                dateTermitaionEnd = "";
            }
            return Json(new SelectList(pslBS.GetPSLStatus(areachild, salesOffice, pslNo, ageIndicator, serialNumber, model, priority, type, dateTerminationFrom, dateTermitaionEnd, dateReleaseForm, dateReleaseEnd, inventory, rental, hid, others), "SapPSLStatus", "SapPSLStatus"));
        }

        [HttpPost] 
        public JsonResult GetPSLNo(string[] areachild, string[] salesOffice, string[] pslStatus, int[] ageIndicator, string[] serialNumber, string[] model, string[] priority, string[] type, string terminationDate, string releaseDate, string inventory, string rental, string hid, string others)
        {
            var dateReleaseForm = "";
            var dateReleaseEnd = "";
            var dateTerminationFrom = "";
            var dateTermitaionEnd = "";
            if (releaseDate != null)
            {
                var strlist = releaseDate.Split(' ', 't', 'o', ' ');
                if (strlist.Count() == 5)
                {
                    dateReleaseForm = strlist[0];
                    dateReleaseEnd = strlist[4];
                }
                else
                {
                    dateReleaseForm = strlist[0];
                }
            }
            else
            {
                dateReleaseForm = "";
                dateReleaseEnd = "";
            }
            if (terminationDate != null)
            {
                var strlist = terminationDate.Split(' ', 't', 'o', ' ');
                if (strlist.Count() == 5)
                {
                    dateTerminationFrom = strlist[0];
                    dateTermitaionEnd = strlist[4];
                }
                else
                {
                    dateTerminationFrom = strlist[0];
                }
            }
            else
            {
                dateTerminationFrom = "";
                dateTermitaionEnd = "";
            }
            return Json(new SelectList(pslBS.GetPSLNo(areachild, salesOffice, pslStatus, ageIndicator, serialNumber, model, priority, type, dateTerminationFrom, dateTermitaionEnd, dateReleaseForm, dateReleaseEnd, inventory, rental, hid, others), "PSLId", "PSLId"));
        }

        [HttpPost]
        public JsonResult GetAgeIndicator(string[] areachild, string[] salesOffice, string[] pslStatus, string[] pslNo, string[] serialNumber, string[] model, string[] priority, string[] type, string terminationDate, string releaseDate, string inventory, string rental, string hid, string others)
        {
            var dateReleaseForm = "";
            var dateReleaseEnd = "";
            var dateTerminationFrom = "";
            var dateTermitaionEnd = "";
            if (releaseDate != null)
            {
                var strlist = releaseDate.Split(' ', 't', 'o', ' ');
                if (strlist.Count() == 5)
                {
                    dateReleaseForm = strlist[0];
                    dateReleaseEnd = strlist[4];
                }
                else
                {
                    dateReleaseForm = strlist[0];
                }
            }
            else
            {
                dateReleaseForm = "";
                dateReleaseEnd = "";
            }
            if (terminationDate != null)
            {
                var strlist = terminationDate.Split(' ', 't', 'o', ' ');
                if (strlist.Count() == 5)
                {
                    dateTerminationFrom = strlist[0];
                    dateTermitaionEnd = strlist[4];
                }
                else
                {
                    dateTerminationFrom = strlist[0];
                }
            }
            else
            {
                dateTerminationFrom = "";
                dateTermitaionEnd = "";
            }
            return Json(new SelectList(pslBS.GetAgeIndicator(areachild, salesOffice, pslStatus, pslNo, serialNumber, model, priority, type, dateTerminationFrom, dateTermitaionEnd, dateReleaseForm, dateReleaseEnd, inventory, rental, hid, others), "PslAge", "PslAge"));
        }

        [HttpPost]
        public JsonResult GetSerialNumber(string[] areachild, string[] salesOffice, string[] pslStatus, string[] pslNo, int[] ageIndicator, string[] model, string[] priority, string[] type, string terminationDate, string releaseDate, string inventory, string rental, string hid, string others)
        {
            var dateReleaseForm = "";
            var dateReleaseEnd = "";
            var dateTerminationFrom = "";
            var dateTermitaionEnd = "";
            if (releaseDate != null)
            {
                var strlist = releaseDate.Split(' ', 't', 'o', ' ');
                if (strlist.Count() == 5)
                {
                    dateReleaseForm = strlist[0];
                    dateReleaseEnd = strlist[4];
                }
                else
                {
                    dateReleaseForm = strlist[0];
                }
            }
            else
            {
                dateReleaseForm = "";
                dateReleaseEnd = "";
            }
            if (terminationDate != null)
            {
                var strlist = terminationDate.Split(' ', 't', 'o', ' ');
                if (strlist.Count() == 5)
                {
                    dateTerminationFrom = strlist[0];
                    dateTermitaionEnd = strlist[4];
                }
                else
                {
                    dateTerminationFrom = strlist[0];
                }
            }
            else
            {
                dateTerminationFrom = "";
                dateTermitaionEnd = "";
            }
            return Json(new SelectList(pslBS.GetSerialNumber(areachild, salesOffice, pslStatus, pslNo, ageIndicator, model, priority, type, dateTerminationFrom, dateTermitaionEnd, dateReleaseForm, dateReleaseEnd, inventory, rental, hid, others), "SerialNumber", "SerialNumber"));
        }

        [HttpPost]
        public JsonResult GetModel(string[] areachild, string[] salesOffice, string[] pslStatus, string[] pslNo, int[] ageIndicator, string[] serialNumber, string[] priority, string[] type, string terminationDate, string releaseDate, string inventory, string rental, string hid, string others)
        {
            var dateReleaseForm = "";
            var dateReleaseEnd = "";
            var dateTerminationFrom = "";
            var dateTermitaionEnd = "";
            if (releaseDate != null)
            {
                var strlist = releaseDate.Split(' ', 't', 'o', ' ');
                if (strlist.Count() == 5)
                {
                    dateReleaseForm = strlist[0];
                    dateReleaseEnd = strlist[4];
                }
                else
                {
                    dateReleaseForm = strlist[0];
                }
            }
            else
            {
                dateReleaseForm = "";
                dateReleaseEnd = "";
            }
            if (terminationDate != null)
            {
                var strlist = terminationDate.Split(' ', 't', 'o', ' ');
                if (strlist.Count() == 5)
                {
                    dateTerminationFrom = strlist[0];
                    dateTermitaionEnd = strlist[4];
                }
                else
                {
                    dateTerminationFrom = strlist[0];
                }
            }
            else
            {
                dateTerminationFrom = "";
                dateTermitaionEnd = "";
            }
            return Json(new SelectList(pslBS.GetModel(areachild, salesOffice, pslStatus, pslNo, ageIndicator, serialNumber, priority, type, dateTerminationFrom, dateTermitaionEnd, dateReleaseForm, dateReleaseEnd, inventory, rental, hid, others), "Model", "Model"));
        }

        [HttpPost]  
        public JsonResult GetPriorityLevel(string[] areachild, string[] salesOffice, string[] pslStatus, string[] pslNo, int[] ageIndicator, string[] serialNumber, string[] model, string[] type, string terminationDate, string releaseDate, string inventory, string rental, string hid, string others)
        {
            var dateReleaseForm = "";
            var dateReleaseEnd = "";
            var dateTerminationFrom = "";
            var dateTermitaionEnd = "";
            if (releaseDate != null)
            {
                var strlist = releaseDate.Split(' ', 't', 'o', ' ');
                if (strlist.Count() == 5)
                {
                    dateReleaseForm = strlist[0];
                    dateReleaseEnd = strlist[4];
                }
                else
                {
                    dateReleaseForm = strlist[0];
                }
            }
            else
            {
                dateReleaseForm = "";
                dateReleaseEnd = "";
            }
            if (terminationDate != null)
            {
                var strlist = terminationDate.Split(' ', 't', 'o', ' ');
                if (strlist.Count() == 5)
                {
                    dateTerminationFrom = strlist[0];
                    dateTermitaionEnd = strlist[4];
                }
                else
                {
                    dateTerminationFrom = strlist[0];
                }
            }
            else
            {
                dateTerminationFrom = "";
                dateTermitaionEnd = "";
            }
            return Json(new SelectList(pslBS.GetPriorityLevel(areachild, salesOffice, pslStatus, pslNo, ageIndicator, serialNumber, model, type, dateTerminationFrom, dateTermitaionEnd, dateReleaseForm, dateReleaseEnd, inventory, rental, hid, others), "PriorityLevel", "PriorityLevel"));
        }
    }
}