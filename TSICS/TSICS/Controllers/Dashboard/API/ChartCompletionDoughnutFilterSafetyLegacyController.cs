﻿using System;
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
        // GET: ChartCompletionDoughnutFilterSafetyLegacy
        public ActionResult ChartCompletionDoughnutFilterSafetyLegacy(string dateRangeFrom, string dateRangeEnd, string inventory, string rental, string hid, string area, string storeName, string pslId, string psltype, string others)
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

            var getData = pslBS.GetDataForChartCompletionFilterSafetyLegacy(splitArea, splitPSLId, splitStoreName, splitPSLType, dateRangeFrom, dateRangeEnd, hid, rental, inventory, others);
            ViewBag.GetDataFilterSafetyLegacy = getData.ElementScoreFilterSafetyLegacy;
            ViewBag.GetDataSafetyLegacyPending = getData;

            var listLabel = new List<string>();
            listLabel.Add("Completed");
            listLabel.Add("Pending");

            var backgroundColor = new List<string>();
            backgroundColor.Add("#609d96");
            backgroundColor.Add("#cccccc");

            var data = new ChartCompletionDoughnutFilterSafetyLegacyModel.DataSets();
            data.Data = getData.DataFilterSafetyLegacy;
            data.BorderWidth = 0;
            data.BackgroundColor = backgroundColor;
            data.UsePercent = false;

            var dataList = new List<ChartCompletionDoughnutFilterSafetyLegacyModel.DataSets>();
            dataList.Add(data);

            var dataDoughnut = new ChartCompletionDoughnutFilterSafetyLegacyModel.Doughnut();
            dataDoughnut.DataSets = dataList;
            dataDoughnut.Labels = listLabel;
            dataDoughnut.UseChart = false;

            var dataFinal = new ChartCompletionDoughnutFilterSafetyLegacyModel.Data();
            dataFinal.Doughnut = dataDoughnut;

            var status = new ChartCompletionDoughnutFilterSafetyLegacyModel.Status();
            status.Code = 200;
            status.Message = "Sukses";

            var responseJson = new ChartCompletionDoughnutFilterSafetyLegacyModel.ResponseJson();
            responseJson.Data = dataFinal;
            responseJson.Status = status;


            Response.ContentType = "application/json";
            Response.Write(JsonConvert.SerializeObject(responseJson));
            return new EmptyResult();
        }
    }
}