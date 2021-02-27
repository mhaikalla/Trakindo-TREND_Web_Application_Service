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
        // GET: ChartContactDealerPPMUpdate
        public ActionResult ChartContactDealerPPMUpdate(string register, string status, string industry, string tech_reps, string dealer_contact)
        {

            var yAxes = new ChartContactDealerPPMUpdateModel.YAxes();
            yAxes.Max = 50;
            yAxes.Min = 0;
            yAxes.Step = 10;

            var getData = dppmBS.CountDealerContact(register, status, industry, tech_reps, dealer_contact);

            var dataSets = new ChartContactDealerPPMUpdateModel.DataSets();
            dataSets.Label = "";
            dataSets.Data = getData.Value;
            dataSets.BackgroundColor = "#fda953";
            dataSets.BorderColor = "transparent";

            var listDataSet = new List<ChartContactDealerPPMUpdateModel.DataSets>();
            listDataSet.Add(dataSets);

            var verticalBar = new ChartContactDealerPPMUpdateModel.VerticalBar();
            verticalBar.Label = getData.Name;
            verticalBar.BarSize = 0.7;
            verticalBar.YAxes = yAxes;
            verticalBar.BarLabel = true;
            verticalBar.HideLegend = true;
            verticalBar.DataSets = listDataSet;

            var data = new ChartContactDealerPPMUpdateModel.Data();
            data.VerticalBar = verticalBar;

            var statusNetwork = new ChartContactDealerPPMUpdateModel.Status();
            statusNetwork.Code = 200;
            statusNetwork.Message = "Success";

            var responseJson = new ChartContactDealerPPMUpdateModel.ResponseJson();
            responseJson.Data = data;
            responseJson.Status = statusNetwork;


            Response.ContentType = "application/json";
            Response.Write(JsonConvert.SerializeObject(responseJson));
            return new EmptyResult();
        }
    }
}