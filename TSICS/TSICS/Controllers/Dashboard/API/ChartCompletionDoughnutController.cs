using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using TSICS.Models.Dashboard;

namespace TSICS.Controllers.Dashboard
{
    partial class DashboardController
    {
        // GET: ChartCompletionDoughnut (filter safety completion)
        public ActionResult ChartCompletionDoughnut(string dateRangeFrom, string dateRangeEnd, string inventory, string rental, string hid, string area, string storeName, string pslId, string psltype, string others) 
        {

            var splitArea = new string[] { };
            var splitStoreName = new string[] { };
            var splitPSLId = new string[] { };
            var splitPSLType = new string[] { };

            if (!string.IsNullOrWhiteSpace(area))
            {
                splitArea = area.Split(',');
            }

            if (!string.IsNullOrWhiteSpace(storeName))
            {
                splitStoreName = storeName.Split(',');
            }

            if (!string.IsNullOrWhiteSpace(pslId))
            {
                splitPSLId = pslId.Split(',');
            }

            if (!string.IsNullOrWhiteSpace(psltype))
            {
                splitPSLType = psltype.Split(',');
            }

            var getData = pslBS.GetDataForChartCompletionFilterSafetyCompletion(splitArea, splitPSLId, splitStoreName, splitPSLType, dateRangeFrom, dateRangeEnd, hid, rental, inventory, others);
            ViewBag.GetDataFilterSafetyCompletion = getData.ElementScoreFilterSafetyCompletion;
            ViewBag.GetListDataSafetyCompletionPending = getData;


            var listLabel = new List<string>();
            listLabel.Add("Completed");
            listLabel.Add("Pending");

            var backgroundColor = new List<string>();
            backgroundColor.Add("#609d96");
            backgroundColor.Add("#cccccc");

            var data = new ChartCompletionDoughnutModel.DataSets();
            data.Data = getData.DataFilterSafetyCompletion;
            data.BorderWidth = 0;
            data.BackgroundColor = backgroundColor;
            data.UsePercent = true;

            var dataList = new List<ChartCompletionDoughnutModel.DataSets>();
            dataList.Add(data);

            var dataDoughnut = new ChartCompletionDoughnutModel.Doughnut();
            dataDoughnut.DataSets = dataList;
            dataDoughnut.Labels = listLabel;
            dataDoughnut.UseChart = true;

            var dataFinal = new ChartCompletionDoughnutModel.Data();
            dataFinal.Doughnut = dataDoughnut;

            var status = new ChartCompletionDoughnutModel.Status();
            status.Code = 200;
            status.Message = "Sukses";

            var responseJson = new ChartCompletionDoughnutModel.ResponseJson();
            responseJson.Data = dataFinal;
            responseJson.Status = status;


            Response.ContentType = "application/json";
            Response.Write(JsonConvert.SerializeObject(responseJson));
            return new EmptyResult();
        }
    }
}