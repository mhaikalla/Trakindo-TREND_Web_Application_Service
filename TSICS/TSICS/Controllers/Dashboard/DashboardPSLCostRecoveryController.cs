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
        // GET: DashboardPSLCostRecovery
        public ActionResult DashboardPSLCostRecovery()
        {
            this.setViewBag();
            var getListArea = pslBS.GetListArea();
            var getListPSLType = pslBS.GetlListPSLType();
            var getListPSLStatus = pslBS.GetListPSLStatus();
            var getListSalesOffice = pslBS.GetListSalesOffice();
            var getListModel = pslBS.GetListModel();
            ViewBag.ListArea = getListArea;
            ViewBag.ListPSLType = getListPSLType;
            ViewBag.ListPSLStatus = getListPSLStatus;
            ViewBag.ListSalesOffice = getListSalesOffice;
            ViewBag.ListModel = getListModel;
            
            var strDateFrom = "";
            var strDateEnd = "";
            var strInventory = "";
            var strRental = "";
            var strHid = "";
            var strOthers = "";
            var strArea = "";
            var strSalesOffice = "";
            var strModel = "";
            var strPslType = "";
            var strPSLStatus = "";

            ViewBag.GetDateFrom = strDateFrom;
            ViewBag.GetDateEnd = strDateEnd;

            var modelFormCollection = new GetFormCollectionTablePSLCostRecovery();
            modelFormCollection.DateRangeFrom = strDateFrom;
            modelFormCollection.DateRangeEnd = strDateEnd;
            modelFormCollection.Inventory = strInventory;
            modelFormCollection.Rental = strRental;
            modelFormCollection.HID = strHid;
            modelFormCollection.Others = strOthers;
            modelFormCollection.Area = strArea;
            modelFormCollection.SalesOffice = strSalesOffice;
            modelFormCollection.Model = strModel;
            modelFormCollection.PSLType = strPslType;
            modelFormCollection.PSLStatus = strPSLStatus;
            ViewBag.ModelFormCollectionPSLCostRecovery = modelFormCollection;
            return View();
        }

        [HttpPost]
        public ActionResult DashboardPSLCostRecovery(FormCollection fc)
        {
            this.setViewBag();
            var dateRangeFrom = "";
            var dateRangeEnd = "";
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

            var strDateFrom = (!string.IsNullOrWhiteSpace(dateRangeFrom)) ? dateRangeFrom : "";
            var strDateEnd = (!string.IsNullOrWhiteSpace(dateRangeEnd)) ? dateRangeEnd : "";
            ViewBag.GetDateFrom = strDateFrom;
            ViewBag.GetDateEnd = strDateEnd;
            var strInventory = (fc["inventory"] != null) ? fc["inventory"] : "";
            var strRental = (fc["rental"] != null) ? fc["rental"] : "";
            var strHid = (fc["hid"] != null) ? fc["hid"] : "";
            var strOthers = (fc["others"] != null) ? fc["others"] : "";
            var strArea = (fc["Area[]"] != null) ? fc["Area[]"] : "";
            var strSalesOffice = (fc["SalesOffice[]"] != null) ? fc["SalesOffice[]"] : "";
            var strModel = (fc["Model[]"] != null) ? fc["Model[]"] : "";
            var strPSLType = (fc["pslType[]"] != null) ? fc["pslType[]"] : "";
            var strPslStatus = (fc["pslStatus[]"] != null) ? fc["pslStatus[]"] : "";

            var modelFormCollection = new GetFormCollectionTablePSLCostRecovery();
            modelFormCollection.DateRangeFrom = strDateFrom;
            modelFormCollection.DateRangeEnd = strDateEnd;
            modelFormCollection.Inventory = strInventory;
            modelFormCollection.Rental = strRental;
            modelFormCollection.HID = strHid;
            modelFormCollection.Others = strOthers;
            modelFormCollection.Area = strArea;
            modelFormCollection.Model = strModel;
            modelFormCollection.SalesOffice = strSalesOffice;
            modelFormCollection.PSLType = strPSLType;
            modelFormCollection.PSLStatus = strPslStatus;

            ViewBag.ModelFormCollectionPSLCostRecovery = modelFormCollection;

            var getListArea = pslBS.GetListArea();
            var getListPSLType = pslBS.GetlListPSLType();
            var getListPSLStatus = pslBS.GetListPSLStatus();
            var getListSalesOffice = pslBS.GetListSalesOffice();
            var getListModel = pslBS.GetListModel();
            ViewBag.ListArea = getListArea;
            ViewBag.ListPSLType = getListPSLType;
            ViewBag.ListPSLStatus = getListPSLStatus;
            ViewBag.ListSalesOffice = getListSalesOffice;
            ViewBag.ListModel = getListModel;
            return View();
        }

        [HttpPost]
        public JsonResult GetAreaForCostRecovery(string[] salesoffice, string[] model, string[] psltype, string[] pslstatus, string inventory, string rental, string hid, string others, string termDate)
        {
            var dateTermForm = "";
            var dateTermEnd = "";
            if (termDate != null)
            {
                var strlist = termDate.Split(' ', 't', 'o', ' ');
                if (strlist.Count() == 5)
                {
                    dateTermForm = strlist[0];
                    dateTermEnd = strlist[4];
                }
                else
                {
                    dateTermForm = strlist[0];
                }
            }
            else
            {
                dateTermForm = "";
                dateTermEnd = "";
            }
            return Json(new SelectList(pslBS.GetAreaForCostRecovery(salesoffice, model, psltype, pslstatus, dateTermForm, dateTermEnd, inventory, rental, hid, others), "Area", "Area"));
        }

        [HttpPost]
        public JsonResult GetSalesOfficeForCostRecovery(string[] area, string[] model, string[] psltype, string[] pslstatus, string inventory, string rental, string hid, string others, string termDate)
        {
            var dateTermForm = "";
            var dateTermEnd = "";
            if (termDate != null)
            {
                var strlist = termDate.Split(' ', 't', 'o', ' ');
                if (strlist.Count() == 5)
                {
                    dateTermForm = strlist[0];
                    dateTermEnd = strlist[4];
                }
                else
                {
                    dateTermForm = strlist[0];
                }
            }
            else
            {
                dateTermForm = "";
                dateTermEnd = "";
            }
            return Json(new SelectList(pslBS.GetSalesOfficeForCostRecovery(area, model, psltype, pslstatus, dateTermForm, dateTermEnd, inventory, rental, hid, others), "SalesOffice", "SalesOffice"));
        }

        [HttpPost]
        public JsonResult GetModelForCostRecovery(string[] area, string[] salesoffice, string[] psltype, string[] pslstatus, string inventory, string rental, string hid, string others, string termDate)
        {
            var dateTermForm = "";
            var dateTermEnd = "";
            if (termDate != null)
            {
                var strlist = termDate.Split(' ', 't', 'o', ' ');
                if (strlist.Count() == 5)
                {
                    dateTermForm = strlist[0];
                    dateTermEnd = strlist[4];
                }
                else
                {
                    dateTermForm = strlist[0];
                }
            }
            else
            {
                dateTermForm = "";
                dateTermEnd = "";
            }
            return Json(new SelectList(pslBS.GetModelForCostRecovery(area, salesoffice, psltype, pslstatus, dateTermForm, dateTermEnd, inventory, rental, hid, others), "Model", "Model"));
        }

        [HttpPost]
        public JsonResult GetPSLTypeForCostRecovery(string[] area, string[] salesoffice, string[] model, string[] pslstatus, string inventory, string rental, string hid, string others, string termDate)
        {
            var dateTermForm = "";
            var dateTermEnd = "";
            if (termDate != null)
            {
                var strlist = termDate.Split(' ', 't', 'o', ' ');
                if (strlist.Count() == 5)
                {
                    dateTermForm = strlist[0];
                    dateTermEnd = strlist[4];
                }
                else
                {
                    dateTermForm = strlist[0];
                }
            }
            else
            {
                dateTermForm = "";
                dateTermEnd = "";
            }
            return Json(new SelectList(pslBS.GetPSLTypeForCostRecovery(area, salesoffice, model, pslstatus, dateTermForm, dateTermEnd, inventory, rental, hid, others), "PSLType", "PSLType"));
        }

        [HttpPost]
        public JsonResult GetPSLStatusForCostRecovery(string[] area, string[] salesoffice, string[] model, string[] psltype, string inventory, string rental, string hid, string others, string termDate)
        {
            var dateTermForm = "";
            var dateTermEnd = "";
            if (termDate != null)
            {
                var strlist = termDate.Split(' ', 't', 'o', ' ');
                if (strlist.Count() == 5)
                {
                    dateTermForm = strlist[0];
                    dateTermEnd = strlist[4];
                }
                else
                {
                    dateTermForm = strlist[0];
                }
            }
            else
            {
                dateTermForm = "";
                dateTermEnd = "";
            }
            return Json(new SelectList(pslBS.GetPSLStatusForCostRecovery(area, salesoffice, model, psltype, dateTermForm, dateTermEnd, inventory, rental, hid, others), "SapPSLStatus", "SapPSLStatus"));
        }
    }
}