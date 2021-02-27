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
        // GET: ChartCompletionPSP
        public ActionResult ChartCompletionPSP(string dateRangeFrom, string dateRangeEnd, string inventory, string rental, string hid, string Area, string storeName, string others)
        { 
             
            var splitArea = new string[] { };
            var splitStoreName = new string[] { };
            var splitDateRangeFrom = new string[] { };
            var splitDateRangeEnd = new string[] { };
            var splitMonthFrom = 0;
            var splitMonthEnd = 0;
            var splitYearFrom = 0;
            var splitYearEnd = 0;

            if (!string.IsNullOrWhiteSpace(Area))
            {
                splitArea = Area.Split(',');
            }

            if (!string.IsNullOrWhiteSpace(storeName))
            {
                splitStoreName = storeName.Split(',');
            }

            if (!string.IsNullOrWhiteSpace(dateRangeFrom))
            {
                splitDateRangeFrom = dateRangeFrom.Split('-');
                splitMonthFrom = Convert.ToInt32(splitDateRangeFrom[1]);

            }

            if (!string.IsNullOrWhiteSpace(dateRangeEnd))
            {
                splitDateRangeEnd = dateRangeEnd.Split('-');
                splitMonthEnd = Convert.ToInt32(splitDateRangeEnd[1]);

            }

            if (!string.IsNullOrWhiteSpace(dateRangeFrom))
            {
                splitDateRangeFrom = dateRangeFrom.Split('-');
                splitYearFrom = Convert.ToInt32(splitDateRangeFrom[2]);

            }

            if (!string.IsNullOrWhiteSpace(dateRangeEnd))
            {
                splitDateRangeEnd = dateRangeEnd.Split('-');
                splitYearEnd = Convert.ToInt32(splitDateRangeEnd[2]);

            }

            var getListData = pslBS.GetListByAreaForCompletionPSP(splitArea, splitStoreName, splitMonthFrom, splitMonthEnd, splitYearFrom, splitYearEnd, dateRangeFrom, dateRangeEnd, hid, rental, inventory, others);
            var dataSetsCompleted = new List<decimal>();
            var rawDataCompleted = new List<int>();
            var dataSetsOutstanding = new List<decimal>();
            var rawDataOutstanding = new List<int>();
            var labels = new List<string>();
            foreach (var item in getListData)
            {
                labels.Add(item.Area);
                dataSetsCompleted.Add(item.RawDataStatusCompleted);
                dataSetsOutstanding.Add(item.RawDataStatusOutstanding);
                rawDataCompleted.Add(item.StatusCompleted);
                rawDataOutstanding.Add(item.StatusOutstanding);
            }

            var listDataSets = new List<ChartCompletionPSPModel.DataSets>();
            var dataSets1 = new ChartCompletionPSPModel.DataSets();
            dataSets1.Label = "Completed";
            dataSets1.yAxisID = "yAxis1";
            dataSets1.Data = dataSetsCompleted;
            dataSets1.RawData = rawDataCompleted;
            dataSets1.BorderColor = "transparent";
            dataSets1.BackgroundColor = "rgb(92, 155, 147)";
            var dataSets2 = new ChartCompletionPSPModel.DataSets();
            dataSets2.Label = "Outstanding";
            dataSets2.yAxisID = "yAxis1";
            dataSets2.Data = dataSetsOutstanding;
            dataSets2.RawData = rawDataOutstanding;
            dataSets2.BorderColor = "transparent";
            dataSets2.BackgroundColor = "rgb(233, 85, 107)";
            listDataSets.Add(dataSets1);
            listDataSets.Add(dataSets2);

            var anotation = new ChartCompletionPSPModel.Anotation();
            anotation.Min = 80;
            anotation.Title = "Minimum Target";

            var horizontalBarStacked = new ChartCompletionPSPModel.HorizBarStacked();
            horizontalBarStacked.Labels = labels;
            horizontalBarStacked.DataSets = listDataSets;
            horizontalBarStacked.Anotation = anotation;

            var data = new ChartCompletionPSPModel.Data();
            data.HorizBarStacked = horizontalBarStacked;

            var status = new ChartCompletionPSPModel.Status();
            status.Code = 200;
            status.Message = "Sukses";

            var responseJson = new ChartCompletionPSPModel.ResponseJson();
            responseJson.Data = data;
            responseJson.Status = status;

            Response.ContentType = "application/json";
            Response.Write(JsonConvert.SerializeObject(responseJson));
            return new EmptyResult();
        }
    }
}