using System.Linq;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using TSICS.Models.Dashboard;
using Com.Trakindo.TSICS.Business.Service;
using Com.Trakindo.Framework;
using System;

namespace TSICS.Controllers.Dashboard
{
    public partial class DashboardController
    {
        // GET: ChartVerticalBarPSLOutstanding
        [HttpPost]
        public ActionResult ChartVerticalBarPSLOutstanding(FormCollection fc,string PipSafety, string PipPriority, string PspProactive, string PspAfterFailure, string ReleaseDateFrom, string ReleaseDateEnd, string TerminationDateFrom, string TerminationDateEnd, string Inventory, string Rental, string HID, string Areass, string SalesOffice, string PSLStatus, string PSLNo, string AgeIndicator, string SerialNumber, string Model, string Priority, string area, string type, string others)
        {
            #region Split String
            var splitArea = new string[] { };
            var splitSalesOffice = new string[] { };
            var splitPSLStatus = new string[] { };
            var splitPSLNo = new string[] { };
            var splitAgeIndicator = new string[] { };
            var splitSerialNumber = new string[] { };
            var splitModel = new string[] { };
            var splitPriority = new string[] { };

            if (!string.IsNullOrWhiteSpace(Areass))
            {
                splitArea = Areass.Split(',');
            }

            if (!string.IsNullOrWhiteSpace(SalesOffice))
            {
                splitSalesOffice = SalesOffice.Split(',');
            }

            if (!string.IsNullOrWhiteSpace(PSLStatus))
            {
                splitPSLStatus = PSLStatus.Split(',');
            }

            if (!string.IsNullOrWhiteSpace(PSLNo))
            {
                splitPSLNo = PSLNo.Split(',');
            }

            if (!string.IsNullOrWhiteSpace(AgeIndicator))
            {
                splitAgeIndicator = AgeIndicator.Split(',');
            }

            if (!string.IsNullOrWhiteSpace(SerialNumber))
            {
                splitSerialNumber = SerialNumber.Split(',');
            }

            if (!string.IsNullOrWhiteSpace(Model))
            {
                splitModel = Model.Split(',');
            }

            if (!string.IsNullOrWhiteSpace(Priority))
            {
                splitPriority = Priority.Split(',');
            }
            #endregion

            if (area == "")
            {
                var getDataArea = pslBS.GetListByArea(splitArea, splitSalesOffice, splitPSLStatus, splitPSLNo, splitAgeIndicator, splitSerialNumber, splitModel, splitPriority,ReleaseDateFrom, ReleaseDateEnd, TerminationDateFrom, TerminationDateEnd, area, type, PipSafety, PipPriority, PspProactive, PspAfterFailure, HID, Rental, Inventory, others);
                var countGetArea = getDataArea.Count();
                var dataSetsOpen = new List<int>();
                var dataSetsRelease = new List<int>();
                var dataSetsOutstanding = new List<int>();
                var dataSetsInProgress = new List<int>();
                var dataLabels = new List<string>();
                var TotalPSL = new List<int>();
                int totalMaxData = 100;
                foreach (var item in getDataArea)
                {
                    dataLabels.Add(item.Area);
                    dataSetsOpen.Add(item.StatusOpen);
                    dataSetsRelease.Add(item.StatusRelease);
                    dataSetsOutstanding.Add(item.StatusOutstanding);
                    dataSetsInProgress.Add(item.StatusInProgress);
                    TotalPSL.Add(item.StatusOutstanding + item.StatusOpen + item.StatusInProgress + item.StatusRelease);
                }
                int sum = 0;
                for (int i = 0; i < TotalPSL.Count(); i++)
                {
                    sum += TotalPSL[i];
                }
                var dataSets = new ChartVerticalBarPSLOutstandingModel.DataSets();
                dataSets.Label = "Open";
                dataSets.Data = dataSetsOpen;
                dataSets.BackgroundColor = "rgba(255, 85, 85, 0.8)";
                dataSets.BorderColor = "transparent";

                var dataSets2 = new ChartVerticalBarPSLOutstandingModel.DataSets();
                dataSets2.Label = "Release";
                dataSets2.Data = dataSetsRelease;
                dataSets2.BackgroundColor = "rgba(255, 229, 85, 0.8)";
                dataSets2.BorderColor = "transparent";

                var dataSets3 = new ChartVerticalBarPSLOutstandingModel.DataSets();
                dataSets3.Label = "Outstanding";
                dataSets3.Data = dataSetsOutstanding;
                dataSets3.BackgroundColor = "rgba(100, 124, 68, 0.8)";
                dataSets3.BorderColor = "transparent";

                var dataSets4 = new ChartVerticalBarPSLOutstandingModel.DataSets();
                dataSets4.Label = "In Progress";
                dataSets4.Data = dataSetsInProgress;
                dataSets4.BackgroundColor = "rgba(3, 179, 32, 0.8)";
                dataSets4.BorderColor = "transparent";

                var dataSetsArray = new List<ChartVerticalBarPSLOutstandingModel.DataSets>();
                dataSetsArray.Add(dataSets);
                dataSetsArray.Add(dataSets2);
                dataSetsArray.Add(dataSets3);
                dataSetsArray.Add(dataSets4);

                var YAxes = new ChartVerticalBarPSLOutstandingModel.YAxes();
                YAxes.Min = 0;
                YAxes.Max = totalMaxData;
                YAxes.Step = 50;

                var verticalBar = new ChartVerticalBarPSLOutstandingModel.VerticalBar();
                verticalBar.TotalData = sum;
                verticalBar.YAxes = YAxes;
                verticalBar.Labels = dataLabels;
                verticalBar.DataSet = dataSetsArray;

                var data = new ChartVerticalBarPSLOutstandingModel.Data();
                data.VerticalBar = verticalBar;

                var status = new ChartVerticalBarPSLOutstandingModel.Status();
                status.Code = 200;
                status.Message = "Sukses";

                var responseJson = new ChartVerticalBarPSLOutstandingModel.ResponseJson();
                responseJson.Status = status;
                responseJson.Data = data;

                Response.ContentType = "application/json";
                Response.Write(JsonConvert.SerializeObject(responseJson));
                return new EmptyResult();
            }
            else
            {
                var getDataArea = pslBS.GetListByAreaPostArea(splitArea, splitSalesOffice, splitPSLStatus, splitPSLNo, splitSerialNumber, splitModel, splitPriority,splitAgeIndicator,ReleaseDateFrom, ReleaseDateEnd, TerminationDateFrom, TerminationDateEnd, area, type, PipSafety, PipPriority, PspProactive, PspAfterFailure, HID, Rental, Inventory, others);
                var countGetArea = getDataArea.Count();
                var listData = new List<int>();
                var listbackgroundColor = new List<string>();
                listbackgroundColor.Add("rgba(255, 85, 85, 0.8)");
                listbackgroundColor.Add("rgba(255, 229, 85, 0.8)");
                listbackgroundColor.Add("rgba(100, 124, 68, 0.8)");
                listbackgroundColor.Add("rgba(3, 179, 32, 0.8)");
                var borderColor = "transparent";
                var dataSets = new ChartverticalBarPSLOutstandingPostModel.DataSets();
                dataSets.BackgroundColor = listbackgroundColor;
                dataSets.BorderColor = borderColor;
                int TotalData = 0;
                int totalMaxData = 50;
                foreach (var item in getDataArea)
                {
                    dataSets.Label = item.Area;
                    dataSets.Data = item.Data;
                    TotalData = item.TotalData;
                }

                var listDataSets = new List<ChartverticalBarPSLOutstandingPostModel.DataSets>();
                listDataSets.Add(dataSets);

                var listLabels = new List<string>();
                listLabels.Add("Open");
                listLabels.Add("Released");
                listLabels.Add("Outstanding");
                listLabels.Add("In Progress");

                var YAxes = new ChartverticalBarPSLOutstandingPostModel.YAxes();
                YAxes.Min = 0;
                YAxes.Max = totalMaxData;
                YAxes.Step = 50;

                var verticalBar = new ChartverticalBarPSLOutstandingPostModel.VerticalBar();
                verticalBar.TotalData = TotalData;
                verticalBar.YAxes = YAxes;
                verticalBar.DataSets = listDataSets;
                verticalBar.Labels = listLabels;

                var status = new ChartverticalBarPSLOutstandingPostModel.Status();
                status.Code = 200;
                status.Message = "Sukses";

                var data = new ChartverticalBarPSLOutstandingPostModel.Data();
                data.VerticalBar = verticalBar;

                var responseJson = new ChartverticalBarPSLOutstandingPostModel.ResponseJson();
                responseJson.Status = status;
                responseJson.Data = data;

                Response.ContentType = "application/json";
                Response.Write(JsonConvert.SerializeObject(responseJson));
                return new EmptyResult();
            }

            
        }
    }
}