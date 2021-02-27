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
        // GET: DashboardCompletionSummary
        public ActionResult DashboardCompletionSummary()
        {
            this.setViewBag();
            var getListArea = pslBS.GetListArea();
            var getListPSLType = pslBS.GetlListPSLType();
            var getListSalesOffice = pslBS.GetListSalesOffice();
            var getListModel = pslBS.GetListModel();
            ViewBag.ListArea = getListArea;
            ViewBag.ListPSLType = getListPSLType;
            ViewBag.ListSalesOffice = getListSalesOffice;
            ViewBag.ListModel = getListModel;

            var strDateFrom = "";
            var strDateEnd = "";
            var strDateFromIssue = "";
            var strDateEndIssue = "";
            var strInventory = "";
            var strRental = "";
            var strHid = "";
            var strOthers = "";
            var strArea = "";
            var strSalesOffice = "";
            var strModel = "";
            var strPSLType = "";

            ViewBag.GetDateFrom = strDateFrom;
            ViewBag.GetDateEnd = strDateEnd;
            ViewBag.GetDateFromIssue = strDateFromIssue;
            ViewBag.GetDateEndIssue = strDateEndIssue;

            var modelFormCollection = new GetFormCollectionPSLCompletionSummary();
            modelFormCollection.DateRangeFrom = strDateFrom;
            modelFormCollection.DateRangeEnd = strDateEnd;
            modelFormCollection.DateRangeFromIssue = strDateFromIssue;
            modelFormCollection.DateRangeEndIssue = strDateEndIssue;
            modelFormCollection.Inventory = strInventory;
            modelFormCollection.Rental = strRental;
            modelFormCollection.HID = strHid;
            modelFormCollection.Others = strOthers;
            modelFormCollection.PSLType = strPSLType;
            modelFormCollection.Area = strArea;
            modelFormCollection.SalesOffice = strSalesOffice;
            modelFormCollection.Model = strModel;
            ViewBag.ModelFormCollectionPSLCompletionSummary = modelFormCollection;
            return View();
        }

        [HttpPost]
        public ActionResult DashboardCompletionSummary(FormCollection fc)
        {
            this.setViewBag();
            var dateRangeFrom = "";
            var dateRangeEnd = "";
            var dateRangeFromIssue = "";
            var dateRangeEndIssue = "";
            if (fc["dateRange"] != null)
            {
                var strList = fc["dateRange"].Split(' ', 't', 'o', ' ');
                if (strList.Count() == 5)
                {
                    dateRangeFrom = strList[0];
                    dateRangeEnd = strList[4];
                }
                else
                {
                    dateRangeFrom = strList[0];
                }
            }
            else
            {
                dateRangeFrom = "";
                dateRangeEnd = "";
            }

            if (fc["issueDateRange"] != null)
            {
                var strList = fc["issueDateRange"].Split(' ', 't', 'o', ' ');
                if (strList.Count() == 5)
                {
                    dateRangeFromIssue = strList[0];
                    dateRangeEndIssue = strList[4];
                }
                else
                {
                    dateRangeFromIssue = strList[0];
                }
            }
            else
            {
                dateRangeFromIssue = "";
                dateRangeEndIssue = "";
            }

            var strDateFrom = (!string.IsNullOrWhiteSpace(dateRangeFrom)) ? dateRangeFrom : "";
            var strDateEnd = (!string.IsNullOrWhiteSpace(dateRangeEnd)) ? dateRangeEnd : "";
            var strDateFromIssue = (!string.IsNullOrWhiteSpace(dateRangeFromIssue)) ? dateRangeFromIssue : "";
            var strDateEndIssue = (!string.IsNullOrWhiteSpace(dateRangeEndIssue)) ? dateRangeEndIssue : "";
            ViewBag.GetDateFrom = strDateFrom;
            ViewBag.GetDateEnd = strDateEnd;
            ViewBag.GetDateFromIssue = strDateFromIssue;
            ViewBag.GetDateEndIssue = strDateEndIssue;
            var strInventory = (fc["inventory"] != null) ? fc["inventory"] : "";
            var strRental = (fc["rental"] != null) ? fc["rental"] : "";
            var strHid = (fc["hid"] != null) ? fc["hid"] : "";
            var strOthers = (fc["others"] != null) ? fc["others"] : "";
            var strArea = (fc["Area[]"] != null) ? fc["Area[]"] : "";
            var strSalesOffice = (fc["SalesOffice[]"] != null) ? fc["SalesOffice[]"] : "";
            var strModel = (fc["Model[]"] != null) ? fc["Model[]"] : "";
            var strPSLType = (fc["PSLType[]"] != null) ? fc["PSLType[]"] : "";
            var modelFormCollection = new GetFormCollectionPSLCompletionSummary();
            modelFormCollection.DateRangeFrom = strDateFrom;
            modelFormCollection.DateRangeEnd = strDateEnd;
            modelFormCollection.DateRangeFromIssue = strDateFromIssue;
            modelFormCollection.DateRangeEndIssue = strDateEndIssue;
            modelFormCollection.Inventory = strInventory;
            modelFormCollection.Rental = strRental;
            modelFormCollection.HID = strHid;
            modelFormCollection.Others = strOthers;
            modelFormCollection.Area = strArea;
            modelFormCollection.PSLType = strPSLType;
            modelFormCollection.SalesOffice = strSalesOffice;
            modelFormCollection.Model = strModel;

            ViewBag.ModelFormCollectionPSLCompletionSummary = modelFormCollection;

            var listPSLType = new List<string>();

            var getListArea = pslBS.GetListArea();
            var getListPSLType = pslBS.GetlListPSLType();
            var getListSalesOffice = pslBS.GetListSalesOffice();
            var getListModel = pslBS.GetListModel();
            ViewBag.ListArea = getListArea;
            ViewBag.ListPSLType = getListPSLType;
            ViewBag.ListSalesOffice = getListSalesOffice;
            ViewBag.ListModel = getListModel;
            return View();
        }

        [HttpPost]
        public JsonResult GetAreaForSummary(string[] salesoffice, string[] model, string[] psltype, string termDate, string issueDate, string inventory, string rental, string hid, string others)
        {
            var dateReleaseForm = "";
            var dateReleaseEnd = "";
            var dateTerminationFrom = "";
            var dateTermitaionEnd = "";
            if (issueDate != null)
            {
                var strlist = issueDate.Split(' ', 't', 'o', ' ');
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
            if (termDate != null)
            {
                var strlist = termDate.Split(' ', 't', 'o', ' ');
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
            return Json(new SelectList(pslBS.GetAreaForSummary(salesoffice, model, psltype, dateTerminationFrom, dateTermitaionEnd, dateReleaseForm, dateReleaseEnd, inventory, rental, hid, others), "Area", "Area"));
        }

        [HttpPost]
        public JsonResult GetSalesOfficeForSummary(string[] area, string[] model, string[] psltype, string termDate, string issueDate, string inventory, string rental, string hid, string others)
        {
            var dateReleaseForm = "";
            var dateReleaseEnd = "";
            var dateTerminationFrom = "";
            var dateTermitaionEnd = "";
            if (issueDate != null)
            {
                var strlist = issueDate.Split(' ', 't', 'o', ' ');
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
            if (termDate != null)
            {
                var strlist = termDate.Split(' ', 't', 'o', ' ');
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
            return Json(new SelectList(pslBS.GetSalesOfficeForSummary(area, model, psltype, dateTerminationFrom, dateTermitaionEnd, dateReleaseForm, dateReleaseEnd, inventory, rental, hid, others), "SalesOffice", "SalesOffice"));
        }

        [HttpPost]
        public JsonResult GetModelForSummary(string[] area, string[] salesoffice, string[] psltype, string termDate, string issueDate, string inventory, string rental, string hid, string others)
        {
            var dateReleaseForm = "";
            var dateReleaseEnd = "";
            var dateTerminationFrom = "";
            var dateTermitaionEnd = "";
            if (issueDate != null)
            {
                var strlist = issueDate.Split(' ', 't', 'o', ' ');
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
            if (termDate != null)
            {
                var strlist = termDate.Split(' ', 't', 'o', ' ');
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
            return Json(new SelectList(pslBS.GetModelForSummary(area, salesoffice, psltype, dateTerminationFrom, dateTermitaionEnd, dateReleaseForm, dateReleaseEnd, inventory, rental, hid, others), "Model", "Model"));
        }

        [HttpPost]
        public JsonResult GetPSLTypeForSummary(string[] area, string[] salesoffice, string[] model, string termDate, string issueDate, string inventory, string rental, string hid, string others)
        {
            var dateReleaseForm = "";
            var dateReleaseEnd = "";
            var dateTerminationFrom = "";
            var dateTermitaionEnd = "";
            if (issueDate != null)
            {
                var strlist = issueDate.Split(' ', 't', 'o', ' ');
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
            if (termDate != null)
            {
                var strlist = termDate.Split(' ', 't', 'o', ' ');
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
            return Json(new SelectList(pslBS.GetPSLTypeForSummary(area, salesoffice, model, dateTerminationFrom, dateTermitaionEnd, dateReleaseForm, dateReleaseEnd, inventory, rental, hid, others), "PSLType", "PSLType"));
        }
    }
}