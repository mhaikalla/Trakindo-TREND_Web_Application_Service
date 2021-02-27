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
        // GET: DashboardPSLExecution
        public ActionResult DashboardPSLExecution()
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
            var getListSOStatus = pslBS.GetListSOStatus();
            ViewBag.ListArea = getListArea;
            ViewBag.ListSalesOffice = getListSalesOffice;
            ViewBag.ListPSLStatus = getListPSLStatus;
            ViewBag.ListPSLNo = getListPSLNo;
            ViewBag.ListAgeIndicator = getListAgeIndicator;
            ViewBag.ListSerialNumber = getListSerialNumber;
            ViewBag.ListModel = getListModel;
            ViewBag.ListPriority = getListPriority;
            ViewBag.ListSOStatus = getListSOStatus;
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
            var strSRNo = string.Empty;
            var strQuotNo = string.Empty;
            var strSONo = string.Empty;
            var strSOStatus = string.Empty;
            var checklistPSLStatus = string.Empty;
            var checklistSOStatus = string.Empty;

            var modelFormCollectionPSLExecution = new GetFormCollectionPSLExecution();
            modelFormCollectionPSLExecution.Inventory = strInventory;
            modelFormCollectionPSLExecution.Rental = strRental;
            modelFormCollectionPSLExecution.HID = strHid;
            modelFormCollectionPSLExecution.Others = strOther;
            modelFormCollectionPSLExecution.Area = strArea;
            modelFormCollectionPSLExecution.SalesOffice = strSalesOffice;
            modelFormCollectionPSLExecution.PSLStatus = strPSLStatus;
            modelFormCollectionPSLExecution.PSLNo = strPSLNo;
            modelFormCollectionPSLExecution.AgeIndicator = strAgeIndicator;
            modelFormCollectionPSLExecution.SerialNumber = strSerialNumber;
            modelFormCollectionPSLExecution.Model = strModel;
            modelFormCollectionPSLExecution.Priority = strPrioroty;
            modelFormCollectionPSLExecution.SONo = strSONo;
            modelFormCollectionPSLExecution.SRNo = strSRNo;
            modelFormCollectionPSLExecution.QuotationNo = strQuotNo;
            modelFormCollectionPSLExecution.SOStatus = strSOStatus;
            modelFormCollectionPSLExecution.ChecklistPSLStatus = checklistPSLStatus;
            modelFormCollectionPSLExecution.ChecklistSOStatus = checklistSOStatus;

            ViewBag.ModelGetFormCollectionPSLExecution = modelFormCollectionPSLExecution;
            return View();
        }

        [HttpPost]
        public ActionResult DashboardPSLExecution(FormCollection fc)
        {
            this.setViewBag();
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
            var strSRNo = (fc["SRNo"] != null) ? fc["SRNo"] : "";
            var strQuotNo = (fc["QuotNo"] != null) ? fc["QuotNo"] : "";
            var strSONo = (fc["SONo"] != null) ? fc["SONo"] : "";
            var strSOStatus = (fc["soStatus[]"] != null) ? fc["soStatus[]"] : "";
            var checklistPSLStatus = (fc["checklistPSLStatus"] != null) ? fc["checklistPSLStatus"] : "";
            var checklistSOStatus = (fc["checklistSOStatus"] != null) ? fc["checklistSOStatus"] : "";

            var modelFormCollectionPSLExecution = new GetFormCollectionPSLExecution();
            modelFormCollectionPSLExecution.Inventory = strInventory;
            modelFormCollectionPSLExecution.Rental = strRental;
            modelFormCollectionPSLExecution.HID = strHid;
            modelFormCollectionPSLExecution.Others = strOther;
            modelFormCollectionPSLExecution.Area = strArea;
            modelFormCollectionPSLExecution.SalesOffice = strSalesOffice;
            modelFormCollectionPSLExecution.PSLStatus = strPSLStatus;
            modelFormCollectionPSLExecution.PSLNo = strPSLNo;
            modelFormCollectionPSLExecution.AgeIndicator = strAgeIndicator;
            modelFormCollectionPSLExecution.SerialNumber = strSerialNumber;
            modelFormCollectionPSLExecution.Model = strModel;
            modelFormCollectionPSLExecution.Priority = strPrioroty;
            modelFormCollectionPSLExecution.SRNo = strSRNo;
            modelFormCollectionPSLExecution.QuotationNo = strQuotNo;
            modelFormCollectionPSLExecution.SONo = strSONo;
            modelFormCollectionPSLExecution.SOStatus = strSOStatus;
            modelFormCollectionPSLExecution.ChecklistPSLStatus = checklistPSLStatus;
            modelFormCollectionPSLExecution.ChecklistSOStatus = checklistSOStatus;

            ViewBag.ModelGetFormCollectionPSLExecution = modelFormCollectionPSLExecution;

            var getListArea = pslBS.GetListArea();
            var getListSalesOffice = pslBS.GetListSalesOffice();
            var getListPSLStatus = pslBS.GetListPSLStatus();
            var getListPSLNo = pslBS.GetListPSLNo();
            var getListAgeIndicator = pslBS.GetListAgeIndicator();
            var getListSerialNumber = pslBS.GetListSerialNumber();
            var getListModel = pslBS.GetListModel();
            var getListPriority = pslBS.GetListPriority();
            var getListSOStatus = pslBS.GetListSOStatus();
            ViewBag.ListArea = getListArea;
            ViewBag.ListSalesOffice = getListSalesOffice;
            ViewBag.ListPSLStatus = getListPSLStatus;
            ViewBag.ListPSLNo = getListPSLNo;
            ViewBag.ListAgeIndicator = getListAgeIndicator;
            ViewBag.ListSerialNumber = getListSerialNumber;
            ViewBag.ListModel = getListModel;
            ViewBag.ListPriority = getListPriority;
            ViewBag.ListSOStatus = getListSOStatus;
            
            return View();
        }
        [HttpPost]
        public JsonResult GetAreaForPSLExecution(string[] model, string[] pslid, string[] pslstatus, string[] sostatus, string inventory, string rental, string hid, string others)
        {
            return Json(new SelectList(pslBS.GetAreaForPSLExecution(model, pslid, pslstatus, sostatus, inventory, rental, hid, others), "Area", "Area"));
        }

        [HttpPost]
        public JsonResult GetPSLNoForPSLExecution(string[] area, string[] model, string[] pslstatus, string[] sostatus, string inventory, string rental, string hid, string others)
        {
            return Json(new SelectList(pslBS.GetPSLNoForPSLExecution(area, model, pslstatus, sostatus, inventory, rental, hid, others), "PSLId", "PSLId"));
        }

        [HttpPost]
        public JsonResult GetModelForPSLExecution(string[] area, string[] pslid, string[] pslstatus, string[] sostatus, string inventory, string rental, string hid, string others)
        {
            return Json(new SelectList(pslBS.GetModelForPSLExecution(area, pslid, pslstatus, sostatus, inventory, rental, hid, others), "Model", "Model"));
        }

        [HttpPost]
        public JsonResult GetPSLStatusForPSLExecution(string[] area, string[] model, string[] pslid, string[] sostatus, string inventory, string rental, string hid, string others)
        {
            return Json(new SelectList(pslBS.GetPSLStatusForPSLExecution(area, model, pslid, sostatus, inventory, rental, hid, others), "SapPSLStatus", "SapPSLStatus"));
        }

        [HttpPost]
        public JsonResult GetSoStatusForPSLExecution(string[] area, string[] model, string[] pslid, string[] pslstatus, string inventory, string rental, string hid, string others)
        {
            return Json(new SelectList(pslBS.GetSoStatusForPSLExecution(area, model, pslid, pslstatus, inventory, rental, hid, others), "ServiceOrderStatusDesc", "ServiceOrderStatusDesc"));
        }
    }
}