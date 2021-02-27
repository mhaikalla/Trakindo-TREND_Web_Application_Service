using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using TSICS.Models.Dashboard;

namespace TSICS.Controllers.Dashboard
{
    public partial class DashboardController
    {
        // GET: ChartCompletionPip
        public ActionResult ChartCompletionPip(string dateRangeFrom, string dateRangeEnd, string inventory, string rental, string hid, string area, string storeName, string others, string psltype)
        { 
            var splitArea = new string[] { };
            var splitStoreName = new string[] { };
            var splitPSLType = new string[] { };

            if (!string.IsNullOrWhiteSpace(area))
            {
                splitArea = area.Split(',');
            }

            if (!string.IsNullOrWhiteSpace(storeName))
            {
                splitStoreName = storeName.Split(',');
            }

            if (!string.IsNullOrWhiteSpace(psltype))
            {
                splitPSLType = psltype.Split(',');
            }

            var listDataDataSetsCompleted = new List<decimal>();
            var listDataDataSetsOutstanding = new List<decimal>();
            var listRawDataDataSetsCompleted = new List<int>();
            var listRawDataDataSetsOutstanding = new List<int>();
            var listMonth = new List<string>();
            var listData = pslBS.GetDataForChartCompletionPip(splitArea, splitStoreName, dateRangeFrom, dateRangeEnd, hid, rental, inventory, others, splitPSLType);
            foreach (var item in listData)
            {
                listMonth = item.MonthName;
                listRawDataDataSetsCompleted = item.StatusCompleted;
                listRawDataDataSetsOutstanding = item.StatusOutstanding;
                listDataDataSetsCompleted = item.RawDataStatusCompleted;
                listDataDataSetsOutstanding = item.RawDataStatusOutstanding;
            }

            var listDataSets = new List<CharCompletionPipModel.DataSets>();
            var dataSetsCompleted = new CharCompletionPipModel.DataSets();
            dataSetsCompleted.Label = "Completed";
            dataSetsCompleted.Data = listDataDataSetsCompleted;
            dataSetsCompleted.RawData = listRawDataDataSetsCompleted;
            dataSetsCompleted.BackgroundColor = "rgb(92, 155, 147)";
            dataSetsCompleted.BorderColor = "transparent";
            var dataSetsOutstanding = new CharCompletionPipModel.DataSets();
            dataSetsOutstanding.Label = "Outstanding";
            dataSetsOutstanding.Data = listDataDataSetsOutstanding;
            dataSetsOutstanding.RawData = listRawDataDataSetsOutstanding;
            dataSetsOutstanding.BackgroundColor = "rgb(233, 85, 107)";
            dataSetsOutstanding.BorderColor = "transparent";
            listDataSets.Add(dataSetsCompleted);
            listDataSets.Add(dataSetsOutstanding);

            var verticalBarStacked = new CharCompletionPipModel.VerticalBarStacked();
            verticalBarStacked.Label = listMonth;
            verticalBarStacked.DataSets = listDataSets;

            var data = new CharCompletionPipModel.Data();
            data.VerticalBarStacked = verticalBarStacked;

            var status = new CharCompletionPipModel.Status();
            status.Code = 200;
            status.Message = "Sukses";

            var responseJson = new CharCompletionPipModel.ResponseJson();
            responseJson.Data = data;
            responseJson.Status = status;

            Response.ContentType = "application/json";
            Response.Write(JsonConvert.SerializeObject(responseJson));
            return new EmptyResult();
        }
    }
}