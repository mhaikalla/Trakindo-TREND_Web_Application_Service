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
        // GET: ChartVerticalPSP
        public ActionResult ChartVerticalPSP(string dateRangeFrom, string dateRangeEnd, string inventory, string rental, string hid, string area, string storeName, string others)
        { 
            var splitArea = new string[] { };
            var splitStoreName = new string[] { };
             
            if (!string.IsNullOrWhiteSpace(area))
            {
                splitArea = area.Split(',');
            }
            if (!string.IsNullOrWhiteSpace(storeName))
            {
                splitStoreName = storeName.Split(',');
            }

            var labels = new List<string>();
            var dataCount = new List<int>();

            var listData = pslBS.GetDataForChartVerticalCompletionPSP(splitArea, splitStoreName,dateRangeFrom, dateRangeEnd, hid, rental, inventory, others);
            foreach(var item in listData)
            {
                labels.Add(item.Month);
                dataCount.Add(item.CountData);

            }
            var yAxes = new ChartVerticalPSPModel.YAxes();
            yAxes.Step = 20;
            yAxes.Min = 0;
            yAxes.Max = 100;

            var dataSets = new ChartVerticalPSPModel.DataSets();
            dataSets.Label = "";
            dataSets.Data = dataCount;
            dataSets.BackgroundColor = "#5d9c94";
            dataSets.BorderColor = "transparent";

            var listDataSets = new List<ChartVerticalPSPModel.DataSets>();
            listDataSets.Add(dataSets);

            var verticalBar = new ChartVerticalPSPModel.VerticalBar();
            verticalBar.Labels = labels;
            verticalBar.BarSize = 0.7;
            verticalBar.YAxes = yAxes;
            verticalBar.BarLabel = true;
            verticalBar.DataSets = listDataSets;

            var data = new ChartVerticalPSPModel.Data();
            data.VerticalBar = verticalBar;

            var status = new ChartVerticalPSPModel.Status();
            status.Code = 200;
            status.Message = "Sukses";

            var responseJson = new ChartVerticalPSPModel.ResponseJson();
            responseJson.Status = status;
            responseJson.Data = data;

            Response.ContentType = "application/json";
            Response.Write(JsonConvert.SerializeObject(responseJson));
            return new EmptyResult();
        }
    }
}