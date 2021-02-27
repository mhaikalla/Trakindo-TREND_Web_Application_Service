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
        // GET: DashboardCompletionPsp
        public ActionResult DashboardCompletionPsp()
        {
            this.setViewBag();
            var getListArea = pslBS.GetListAreaPSLCompletionPSP();
            var getListStoreName = pslBS.GetListSalesOffice();
            ViewBag.ListArea = getListArea;
            ViewBag.ListStoreName = getListStoreName;

            var strDateFrom = "";
            var strDateEnd = "";
            var strInventory = "";
            var strRental = "";
            var strHid = "";
            var strOthers = "";
            var strArea = "";
            var strStoreName = "";

            ViewBag.GetDateFrom = strDateFrom;
            ViewBag.GetDateEnd = strDateEnd;

            var modelFormCollection = new GetFormCollectionPSLCompletionPSP();
            modelFormCollection.DateRangeFrom = strDateFrom;
            modelFormCollection.DateRangeEnd = strDateEnd;
            modelFormCollection.Inventory = strInventory;
            modelFormCollection.Rental = strRental;
            modelFormCollection.HID = strHid;
            modelFormCollection.Others = strOthers;
            modelFormCollection.Area = strArea;
            modelFormCollection.StoreName = strStoreName;
            ViewBag.ModelFormCollectionPSLCompletionPsp = modelFormCollection;
            return View();
        }

        [HttpPost]
        public ActionResult DashboardCompletionPsp(FormCollection fc)
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
            var strStoreName = (fc["storeName[]"] != null) ? fc["storeName[]"] : "";

            var modelFormCollection = new GetFormCollectionPSLCompletionPSP();
            modelFormCollection.DateRangeFrom = strDateFrom;
            modelFormCollection.DateRangeEnd = strDateEnd;
            modelFormCollection.Inventory = strInventory;
            modelFormCollection.Rental = strRental;
            modelFormCollection.HID = strHid;
            modelFormCollection.Others = strOthers;
            modelFormCollection.Area = strArea;
            modelFormCollection.StoreName = strStoreName;

            ViewBag.ModelFormCollectionPSLCompletionPsp = modelFormCollection;

            var getListArea = pslBS.GetListAreaPSLCompletionPSP();
            var getListStoreName = pslBS.GetListSalesOffice();
            ViewBag.ListArea = getListArea;
            ViewBag.ListStoreName = getListStoreName;
            return View();
        }
        [HttpPost]
        public JsonResult GetAreaForPSP(string[] psltype, string[] storename, string inventory, string rental, string hid, string others, string termDate)
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
            return Json(new SelectList(pslBS.GetAreaForPSP(psltype, storename, dateTermForm, dateTermEnd, inventory, rental, hid, others), "Area", "Area"));
        }

        [HttpPost]
        public JsonResult GetSalesOfficeForPSP(string[] psltype, string[] area, string inventory, string rental, string hid, string others, string termDate)
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
            return Json(new SelectList(pslBS.GetSalesOfficeForPSP(psltype, area, dateTermForm, dateTermEnd, inventory, rental, hid, others), "SalesOffice", "SalesOffice"));
        }
    }
}