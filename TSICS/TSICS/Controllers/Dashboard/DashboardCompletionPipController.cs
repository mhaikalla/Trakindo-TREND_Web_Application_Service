using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TSICS.Helper;
using Com.Trakindo.Framework;
using Com.Trakindo.TSICS.Business.Service;
using Com.Trakindo.TSICS.Data.Model;
using Newtonsoft.Json;
using TSICS.Models.Dashboard;

namespace TSICS.Controllers.Dashboard
{
    partial class DashboardController
    {
        // GET: DashboardCompletionPip 
        public ActionResult DashboardCompletionPip()
        {
            this.setViewBag();
            var getListArea = pslBS.GetListArea();
            var getListSalesOffice = pslBS.GetListSalesOffice();
            var getListPSLId = pslBS.GetListPSLNo();
            ViewBag.ListArea = getListArea;
            ViewBag.ListSalesOffice = getListSalesOffice;
            ViewBag.ListPSLId = getListPSLId;
             
            var strArea = new string[] { };
            var strSalesOffice = new string[] { };
            var strPSLId = new string[] { };
            var strPslType = new string[] { };
            var strDateFrom = "";
            var strDateEnd = "";
            ViewBag.DateFrom = strDateFrom;
            ViewBag.DateEnd = strDateEnd;
            var strInventory ="";
            var strRental = "";
            var strHid = "";
            var strOthers = "";
            var stringArea = "";
            var strStoreName = "";
            var stringPSLId = "";
            var stringPSLType = "";
            var hid = "";
            var rental = "";
            var inventory = "";

            var getData = pslBS.GetDataForChartCompletionFilterSafetyCompletion(strArea, strPSLId, strSalesOffice, strPslType, strDateFrom, strDateEnd, hid, rental, inventory, strOthers);

            decimal scoreSafety = 0;
            decimal scoreSafetyLegacy = 0;
            decimal scorePriority = 0;
            if (getData.Completed >= 88 && getData.Completed < 93)
            {
                scoreSafety = 1;
            }
            else if (getData.Completed >= 93 && getData.Completed < 98)
            {
                scoreSafety = 2;
            }
            else if (getData.Completed >= 98)
            {
                scoreSafety = 3;
            }
            else
            {
                scoreSafety = 0;
            }
            ViewBag.ScorePipSafety = scoreSafety;
            ViewBag.CountPendingData = getData.CountPending;

            var getDataPriority = pslBS.GetDataForChartCompletionFilterPriority(strArea, strPSLId, strSalesOffice, strPslType, strDateFrom, strDateEnd, hid, rental, inventory, strOthers);
            if (getDataPriority.Completed >= 88 && getDataPriority.Completed < 93)
            {
                scorePriority = 1;
            }
            else if (getDataPriority.Completed >= 93 && getDataPriority.Completed < 98)
            {
                scorePriority = 2;
            }
            else if (getDataPriority.Completed >= 98)
            {
                scorePriority = 3;
            }
            else
            {
                scorePriority = 0;
            }
            ViewBag.ScorePriority = scorePriority;
            ViewBag.CountPendingPriority = getDataPriority.CountPending;

            var getDataSafetyLegacy = pslBS.GetDataForChartCompletionFilterSafetyLegacy(strArea, strPSLId, strSalesOffice, strPslType, strDateFrom, strDateEnd, hid, rental, inventory, strOthers);

            if (getDataSafetyLegacy.Completed == 0)
            {
                scoreSafetyLegacy = 3;
            }
            else if (getDataSafetyLegacy.Completed >= 1 && getDataSafetyLegacy.Completed <= 2)
            {
                scoreSafetyLegacy = 2;
            }
            else if (getDataSafetyLegacy.Completed > 2 && getDataSafetyLegacy.Completed <= 25)
            {
                scoreSafetyLegacy = 1;
            }
            else if (getDataSafetyLegacy.Completed > 25)
            {
                scoreSafetyLegacy = 0;
            }
            ViewBag.ScoreSafetyLegacy = scoreSafetyLegacy;
            ViewBag.CountDataPendingLegacy = getDataSafetyLegacy.Completed;
            var averageScore = (scoreSafety + scorePriority + scoreSafetyLegacy) / 3;
            ViewBag.AverageScore = Math.Round(averageScore, 2);


            var getDataFilterSafetyCompletion = pslBS.GetDataForChartCompletionFilterSafetyCompletion(strArea, strPSLId, strSalesOffice, strPslType, strDateFrom, strDateEnd, hid, rental, inventory, strOthers);
            ViewBag.GetDataElementSafetyCompletion = getDataFilterSafetyCompletion.ElementScoreFilterSafetyCompletion;
            ViewBag.GetDataAreaSafetyCompletion = getDataFilterSafetyCompletion.Area;
            ViewBag.GetDataPSLIdSafetyCompletion = getDataFilterSafetyCompletion.PSLId;
            ViewBag.GetDataSalesOfficeSafetyCompletion = getDataFilterSafetyCompletion.SalesOffice;
            ViewBag.GetDataFilterSafetyCompletion = getDataFilterSafetyCompletion;

            var getDataFilterPriority = pslBS.GetDataForChartCompletionFilterPriority(strArea, strPSLId, strSalesOffice, strPslType, strDateFrom, strDateEnd, hid, rental, inventory, strOthers);
            ViewBag.GetDataElementPriority = getDataFilterPriority.ElementScoreFilterPriority;
            ViewBag.GetDataAreaPriority = getDataFilterPriority.Area;
            ViewBag.GetDataPSLIdPriority = getDataFilterPriority.PSLId;
            ViewBag.GetDataSalesOffice = getDataFilterPriority.SalesOffice;

            var getDataFilterSafetyLegacy = pslBS.GetDataForChartCompletionFilterSafetyLegacy(strArea, strPSLId, strSalesOffice, strPslType, strDateFrom, strDateEnd, hid, rental, inventory, strOthers);
            ViewBag.GetDataElementSafetyLegacy = getDataFilterSafetyLegacy.ElementScoreFilterSafetyLegacy;
            ViewBag.GetDataAreaSafetyLegacy = getDataFilterSafetyLegacy.Area;
            ViewBag.GetDataPSLIdSafetyLegacy = getDataFilterSafetyLegacy.PSLId;
            ViewBag.GetDataSalesOfficeSafetyLegacy = getDataFilterSafetyLegacy.SalesOffice;



            var modelFormCollection = new GetFormCollectionPSLCompletionPip();
            modelFormCollection.DateRangeFrom = strDateFrom;
            modelFormCollection.DateRangeEnd = strDateEnd;
            modelFormCollection.Inventory = strInventory;
            modelFormCollection.Rental = strRental;
            modelFormCollection.HID = strHid;
            modelFormCollection.Others = strOthers;
            modelFormCollection.Area = stringArea;
            modelFormCollection.StoreName = strStoreName;
            modelFormCollection.PSLId = stringPSLId;
            modelFormCollection.PSLType = stringPSLType;
            ViewBag.ModelFormCollectionPSLCompletionPip = modelFormCollection;

            return View();
        }

        [HttpPost]
        public ActionResult DashboardCompletionPip(FormCollection fc)
        {
            ViewBag.Domain =WebConfigure.GetDomain();
            var dateRangeFrom = "";
            var dateRangeEnd = "";
            if(fc["dateRange"] != null)
            {
                var strList = fc["dateRange"].Split(' ', 't', 'o', ' ');
                if(strList.Count() == 5)
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
            ViewBag.DateFrom = strDateFrom;
            ViewBag.DateEnd = strDateEnd;
            var strInventory = (fc["inventory"] != null) ? fc["inventory"] : "";
            var strRental = (fc["rental"] != null) ? fc["rental"] : "";
            var strHid = (fc["hid"] != null) ? fc["hid"] : "";
            var strOthers = (fc["others"] != null) ? fc["others"] : "";
            var strArea = (fc["Area[]"] != null) ? fc["Area[]"] : "";
            var strStoreName = (fc["storeName[]"] != null) ? fc["storeName[]"] : "";
            var strPSLId = (fc["pslId[]"] != null) ? fc["pslId[]"] : "";
            var strPslType = (fc["PSLType[]"] != null) ? fc["PSLType[]"] : ""; 

            var splitArea = new string[] { };
            var splitSalesOffice = new string[] { };
            var splitPSLId = new string[] { };
            var splitPSLType = new string[] { };

            if (!string.IsNullOrWhiteSpace(strArea))
            {
                splitArea = strArea.Split(',');
            }

            if (!string.IsNullOrWhiteSpace(strStoreName))
            {
                splitSalesOffice = strStoreName.Split(',');
            }

            if (!string.IsNullOrWhiteSpace(strPSLId))
            {
                splitPSLId = strPSLId.Split(',');
            }

            if (!string.IsNullOrWhiteSpace(strPslType))
            {
                splitPSLType = strPslType.Split(',');
            }

            var getData = pslBS.GetDataForChartCompletionFilterSafetyCompletion(splitArea, splitPSLId, splitSalesOffice, splitPSLType, strDateFrom, strDateEnd, strHid, strRental, strInventory, strOthers);

            decimal scoreSafety = 0;
            decimal scoreSafetyLegacy = 0;
            decimal scorePriority = 0;
            if (getData.Completed >= 88 && getData.Completed < 93)
            {
                scoreSafety = 1;
            }
            else if (getData.Completed >= 93 && getData.Completed < 98)
            {
                scoreSafety = 2;
            }
            else if(getData.Completed >= 98)
            {
                scoreSafety = 3;
            }
            ViewBag.ScorePipSafety = scoreSafety;
            ViewBag.CountPendingData = getData.CountPending;

            var getDataPriority = pslBS.GetDataForChartCompletionFilterPriority(splitArea, splitPSLId, splitSalesOffice, splitPSLType, strDateFrom, strDateEnd, strHid, strRental, strInventory, strOthers);
            if (getDataPriority.Completed >= 88 && getDataPriority.Completed < 93)
            {
                scorePriority = 1;
            }
            else if (getDataPriority.Completed >= 93 && getDataPriority.Completed < 98)
            {
                scorePriority = 2;
            }
            else if(getDataPriority.Completed >= 98)
            {
                scorePriority = 3;
            }
            ViewBag.ScorePriority = scorePriority;
            ViewBag.CountPendingPriority = getDataPriority.CountPending;

            var getDataSafetyLegacy = pslBS.GetDataForChartCompletionFilterSafetyLegacy(splitArea, splitPSLId, splitSalesOffice, splitPSLType, strDateFrom, strDateEnd, strHid, strRental, strInventory, strOthers);

            if (getDataSafetyLegacy.Completed == 0)
            {
                scoreSafetyLegacy = 3;
            }
            else if (getDataSafetyLegacy.Completed >= 1 && getDataSafetyLegacy.Completed <= 2)
            {
                scoreSafetyLegacy = 2;
            }
            else if(getDataSafetyLegacy.Completed > 2 && getDataSafetyLegacy.Completed <= 25)
            {
                scoreSafetyLegacy = 1;
            }
            else if(getDataSafetyLegacy.Completed > 25)
            {
                scoreSafetyLegacy = 0;
            }
            ViewBag.ScoreSafetyLegacy = scoreSafetyLegacy;
            ViewBag.CountDataPendingLegacy = getDataSafetyLegacy.Completed;
            var averageScore = (scoreSafety + scorePriority + scoreSafetyLegacy) / 3;
            ViewBag.AverageScore = Math.Round(averageScore, 2);

            var getDataFilterSafetyCompletion = pslBS.GetDataForChartCompletionFilterSafetyCompletion(splitArea, splitPSLId, splitSalesOffice, splitPSLType, strDateFrom, strDateEnd, strHid, strRental, strInventory, strOthers);
            ViewBag.GetDataElementSafetyCompletion = getDataFilterSafetyCompletion.ElementScoreFilterSafetyCompletion;
            ViewBag.GetDataAreaSafetyCompletion = getDataFilterSafetyCompletion.Area;
            ViewBag.GetDataPSLIdSafetyCompletion = getDataFilterSafetyCompletion.PSLId;
            ViewBag.GetDataSalesOfficeSafetyCompletion = getDataFilterSafetyCompletion.SalesOffice;
            ViewBag.GetDataFilterSafetyCompletion = getDataFilterSafetyCompletion;

            var getDataFilterPriority = pslBS.GetDataForChartCompletionFilterPriority(splitArea, splitPSLId, splitSalesOffice, splitPSLType, strDateFrom, strDateEnd, strHid, strRental, strInventory, strOthers);
            ViewBag.GetDataElementPriority = getDataFilterPriority.ElementScoreFilterPriority;
            ViewBag.GetDataAreaPriority = getDataFilterPriority.Area;
            ViewBag.GetDataPSLIdPriority = getDataFilterPriority.PSLId;
            ViewBag.GetDataSalesOffice = getDataFilterPriority.SalesOffice;

            var getDataFilterSafetyLegacy = pslBS.GetDataForChartCompletionFilterSafetyLegacy(splitArea, splitPSLId, splitSalesOffice, splitPSLType, strDateFrom, strDateEnd, strHid, strRental, strInventory, strOthers);
            ViewBag.GetDataElementSafetyLegacy = getDataFilterSafetyLegacy.ElementScoreFilterSafetyLegacy;
            ViewBag.GetDataAreaSafetyLegacy = getDataFilterSafetyLegacy.Area;
            ViewBag.GetDataPSLIdSafetyLegacy = getDataFilterSafetyLegacy.PSLId;
            ViewBag.GetDataSalesOfficeSafetyLegacy = getDataFilterSafetyLegacy.SalesOffice;

            var modelFormCollection = new GetFormCollectionPSLCompletionPip();
            modelFormCollection.DateRangeFrom = strDateFrom;
            modelFormCollection.DateRangeEnd = strDateEnd;
            modelFormCollection.Inventory = strInventory;
            modelFormCollection.Rental = strRental;
            modelFormCollection.HID = strHid;
            modelFormCollection.Others = strOthers;
            modelFormCollection.Area = strArea;
            modelFormCollection.PSLId = strPSLId;
            modelFormCollection.PSLType = strPslType;
            modelFormCollection.StoreName = strStoreName;

            ViewBag.ModelFormCollectionPSLCompletionPip = modelFormCollection;

            var getListArea = pslBS.GetListArea();
            var getListSalesOffice = pslBS.GetListSalesOffice();
            var getListPSLId = pslBS.GetListPSLNo();
            ViewBag.ListArea = getListArea;
            ViewBag.ListSalesOffice = getListSalesOffice;
            ViewBag.ListPSLId = getListPSLId;


            return View();
        }

        [HttpGet]
        public ActionResult GetSalesOfficePIPSafetyLegacy(string parent, string dateRangeFrom, string dateRangeEnd, string inventory, string rental, string hid, string others, string pslType, string storeName, string area)
        {
            var splitPSLType = new string[] { };
            var splitArea = new string[] { };
            var splitStoreName = new string[] { };

            if (!string.IsNullOrWhiteSpace(pslType))
            {
                splitPSLType = pslType.Split(',');
            }
            if (!string.IsNullOrWhiteSpace(storeName))
            {
                splitStoreName = storeName.Split(',');
            }
            if (!string.IsNullOrWhiteSpace(area))
            {
                splitArea = area.Split(',');
            }
            var PSLData = pslBS.GetListSalesOfficeForCompletionPipSafetyLegacy(parent, dateRangeFrom, dateRangeEnd, inventory, rental, hid, others, splitPSLType, splitArea, splitStoreName);
            List<Ticket> PSLDataTemp = new List<Ticket>();
            foreach (var item in PSLData)
            {

                Ticket tempPSL = new Ticket()
                {
                    Title = item.Replace(" ", ""),
                    TicketNo = item
                };
                PSLDataTemp.Add(tempPSL);
            }
            var modelFormCollection = new GetFormCollectionPSLCompletionPip();
            modelFormCollection.DateRangeFrom = dateRangeFrom;
            modelFormCollection.DateRangeEnd = dateRangeEnd;
            modelFormCollection.Inventory = inventory;
            modelFormCollection.Rental = rental;
            modelFormCollection.HID = hid;
            modelFormCollection.Others = others;
            modelFormCollection.Area = parent;
            modelFormCollection.PSLId = pslType;
            modelFormCollection.PSLType = "";
            modelFormCollection.StoreName = "";
            ViewBag.ModelFormCollectionPSLCompletionPip = modelFormCollection;
            ViewBag.ListSalesOfficeSafetyLegacy = PSLDataTemp;
            ViewBag.AreaSafetyLegacy = parent;
            ViewBag.Domain =WebConfigure.GetDomain();
            return View();
        }

        [HttpGet]
        public ActionResult GetPSLIdPIPSafetyLegacy(string parentarea, string salesOffice, string dateRangeFrom, string dateRangeEnd, string inventory, string rental, string hid, string others, string pslType, string storeName, string area)
        {
            var splitPSLType = new string[] { };
            var splitArea = new string[] { };
            var splitStoreName = new string[] { };

            if (!string.IsNullOrWhiteSpace(pslType))
            {
                splitPSLType = pslType.Split(',');
            }
            if (!string.IsNullOrWhiteSpace(storeName))
            {
                splitStoreName = storeName.Split(',');
            }
            if (!string.IsNullOrWhiteSpace(area))
            {
                splitArea = area.Split(',');
            }
            var modelFormCollection = new GetFormCollectionPSLCompletionPip();
            modelFormCollection.DateRangeFrom = dateRangeFrom;
            modelFormCollection.DateRangeEnd = dateRangeEnd;
            modelFormCollection.Inventory = inventory;
            modelFormCollection.Rental = rental;
            modelFormCollection.HID = hid;
            modelFormCollection.Others = others;
            modelFormCollection.Area = area;
            modelFormCollection.PSLId = "";
            modelFormCollection.PSLType = pslType;
            modelFormCollection.StoreName = "";
            ViewBag.ModelFormCollectionPSLCompletionPip = modelFormCollection;
            ViewBag.ListPSLIdSafetyLegacy = pslBS.GetListPSLIdForCompletionPipSafetyLegacy(parentarea, salesOffice, dateRangeFrom, dateRangeEnd, inventory, rental, hid, others, splitPSLType, splitArea, splitStoreName);
            ViewBag.Domain =WebConfigure.GetDomain();
            return View();

        }

        [HttpGet]
        public ActionResult GetSalesOfficePIPSafety (string parent, string dateRangeFrom, string dateRangeEnd, string inventory, string rental, string hid, string others, string pslType, string storeName, string area)
        {
            var splitPSLType = new string[] { };
            var splitArea = new string[] { };
            var splitStoreName = new string[] { };

            if (!string.IsNullOrWhiteSpace(pslType))
            {
                splitPSLType = pslType.Split(',');
            }
            if (!string.IsNullOrWhiteSpace(storeName))
            {
                splitStoreName = storeName.Split(',');
            }
            if (!string.IsNullOrWhiteSpace(area))
            {
                splitArea = area.Split(',');
            }
            var PSLData = pslBS.GetListSalesOfficeForCompletionPipSafety(parent, dateRangeFrom, dateRangeEnd, inventory, rental, hid, others, splitPSLType, splitArea, splitStoreName);
            List<Ticket> PSLDataTemp = new List<Ticket>();
            foreach (var item in PSLData)
            {

                Ticket tempPSL = new Ticket()
                {
                    Title = item.Replace(" ", ""),
                    TicketNo = item
                };
                PSLDataTemp.Add(tempPSL);
            }
            var modelFormCollection = new GetFormCollectionPSLCompletionPip();
            modelFormCollection.DateRangeFrom = dateRangeFrom;
            modelFormCollection.DateRangeEnd = dateRangeEnd;
            modelFormCollection.Inventory = inventory;
            modelFormCollection.Rental = rental;
            modelFormCollection.HID = hid;
            modelFormCollection.Others = others;
            modelFormCollection.Area = parent;
            modelFormCollection.PSLId = pslType;
            modelFormCollection.PSLType = "";
            modelFormCollection.StoreName = "";
            ViewBag.ModelFormCollectionPSLCompletionPip = modelFormCollection;
            ViewBag.ListSalesOffice = PSLDataTemp;
            ViewBag.Area = parent;
            ViewBag.Domain =WebConfigure.GetDomain();
            return View();
        }

        [HttpGet]
        public ActionResult GetPSLIdPIPSafety(string parentarea, string salesOffice, string dateRangeFrom, string dateRangeEnd, string inventory, string rental, string hid, string others, string pslType, string storeName, string area)
        {
            var splitPSLType = new string[] { };
            var splitArea = new string[] { };
            var splitStoreName = new string[] { };

            if (!string.IsNullOrWhiteSpace(pslType))
            {
                splitPSLType = pslType.Split(',');
            }
            if (!string.IsNullOrWhiteSpace(storeName))
            {
                splitStoreName = storeName.Split(',');
            }
            if (!string.IsNullOrWhiteSpace(area))
            {
                splitArea = area.Split(',');
            }
            var modelFormCollection = new GetFormCollectionPSLCompletionPip();
            modelFormCollection.DateRangeFrom = dateRangeFrom;
            modelFormCollection.DateRangeEnd = dateRangeEnd;
            modelFormCollection.Inventory = inventory;
            modelFormCollection.Rental = rental;
            modelFormCollection.HID = hid;
            modelFormCollection.Others = others;
            modelFormCollection.Area = area;
            modelFormCollection.PSLId = "";
            modelFormCollection.PSLType = pslType;
            modelFormCollection.StoreName = "";
            ViewBag.ModelFormCollectionPSLCompletionPip = modelFormCollection;
            ViewBag.ListPSLId = pslBS.GetListPSLIdForCompletionPipSafety(parentarea, salesOffice, dateRangeFrom, dateRangeEnd, inventory, rental, hid, others, splitPSLType,splitArea, splitStoreName);
            ViewBag.Domain =WebConfigure.GetDomain();
            return View();
            
        }


        [HttpGet]
        public ActionResult GetSalesOfficePIPPriority(string parent, string dateRangeFrom, string dateRangeEnd, string inventory, string rental, string hid, string others, string pslType, string storeName, string area)
        {
            var splitPSLType = new string[] { };
            var splitArea = new string[] { };
            var splitStoreName = new string[] { };

            if (!string.IsNullOrWhiteSpace(pslType))
            {
                splitPSLType = pslType.Split(',');
            }
            if (!string.IsNullOrWhiteSpace(storeName))
            {
                splitStoreName = storeName.Split(',');
            }
            if (!string.IsNullOrWhiteSpace(area))
            {
                splitArea = area.Split(',');
            }
            var PSLData = pslBS.GetListSalesOfficeForCompletionPipPriority(parent, dateRangeFrom, dateRangeEnd, inventory, rental, hid, others, splitPSLType, splitArea, splitStoreName);
            List<Ticket> PSLDataTemp = new List<Ticket>();
            foreach (var item in PSLData)
            {

                Ticket tempPSL = new Ticket()
                {
                    Title = item.Replace(" ", ""),
                    TicketNo = item
                };
                PSLDataTemp.Add(tempPSL);
            }
            var modelFormCollection = new GetFormCollectionPSLCompletionPip();
            modelFormCollection.DateRangeFrom = dateRangeFrom;
            modelFormCollection.DateRangeEnd = dateRangeEnd;
            modelFormCollection.Inventory = inventory;
            modelFormCollection.Rental = rental;
            modelFormCollection.HID = hid;
            modelFormCollection.Others = others;
            modelFormCollection.Area = parent;
            modelFormCollection.PSLId = pslType;
            modelFormCollection.PSLType = "";
            modelFormCollection.StoreName = "";
            ViewBag.ModelFormCollectionPSLCompletionPip = modelFormCollection;
            ViewBag.ListSalesOfficePipPriority = PSLDataTemp;
            ViewBag.AreaPriority = parent;
            ViewBag.Domain =WebConfigure.GetDomain();
            return View();
        }

        [HttpGet]
        public ActionResult GetPSLIdPIPPriority(string parentarea, string salesOffice, string dateRangeFrom, string dateRangeEnd, string inventory, string rental, string hid, string others, string pslType, string storeName, string area)
        {
            var splitPSLType = new string[] { };
            var splitArea = new string[] { };
            var splitStoreName = new string[] { };

            if (!string.IsNullOrWhiteSpace(pslType))
            {
                splitPSLType = pslType.Split(',');
            }
            if (!string.IsNullOrWhiteSpace(storeName))
            {
                splitStoreName = storeName.Split(',');
            }
            if (!string.IsNullOrWhiteSpace(area))
            {
                splitArea = area.Split(',');
            }
            var modelFormCollection = new GetFormCollectionPSLCompletionPip();
            modelFormCollection.DateRangeFrom = dateRangeFrom;
            modelFormCollection.DateRangeEnd = dateRangeEnd;
            modelFormCollection.Inventory = inventory;
            modelFormCollection.Rental = rental;
            modelFormCollection.HID = hid;
            modelFormCollection.Others = others;
            modelFormCollection.Area = area;
            modelFormCollection.PSLId = "";
            modelFormCollection.PSLType = pslType;
            modelFormCollection.StoreName = "";
            ViewBag.ModelFormCollectionPSLCompletionPip = modelFormCollection;
            ViewBag.ListPSLIdPipPriority = pslBS.GetListPSLIdForCompletionPipPriority(parentarea, salesOffice, dateRangeFrom, dateRangeEnd, inventory, rental, hid, others, splitPSLType, splitArea, splitStoreName);
            ViewBag.Domain =WebConfigure.GetDomain();
            return View();
        }




        [HttpPost]
        public JsonResult GetPSLTypeForPIP(string[] area, string[] storename, string inventory, string rental, string hid, string others, string issueDate)
        {
            return Json(new SelectList(pslBS.GetPSLTypeForPIP(area, storename), "PSLType", "PSLType"));
        }

        [HttpPost]
        public JsonResult GetAreaForPIP(string[] psltype, string[] storename, string inventory, string rental, string hid, string others, string issueDate)
        {
            var dateReleaseForm = "";
            var dateReleaseEnd = "";
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
            return Json(new SelectList(pslBS.GetAreaForPIP(psltype, storename, dateReleaseForm, dateReleaseEnd, inventory, rental, hid, others), "Area", "Area"));
        }

        [HttpPost]
        public JsonResult GetSalesOfficeForPIP(string[] psltype, string[] area, string inventory, string rental, string hid, string others, string issueDate)
        {
            var dateReleaseForm = "";
            var dateReleaseEnd = "";
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
            return Json(new SelectList(pslBS.GetSalesOfficeForPIP(psltype, area, dateReleaseForm, dateReleaseEnd, inventory, rental, hid, others), "SalesOffice", "SalesOffice"));
        }





    }
}