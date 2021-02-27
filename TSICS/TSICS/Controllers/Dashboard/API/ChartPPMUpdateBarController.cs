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
        // GET: ChartPPMUpdateBar
        public ActionResult ChartPPMUpdateBar(string register, string status, string industry, string tech_reps, string dealer_contact)
        {
            var getData = dppmBS.CountBarUpdatePPM(register, status, industry, tech_reps, dealer_contact);

            var dataSets = new ChartPPMUpdateBarModel.DataSets();
            dataSets.Label = "Affected Unit";
            dataSets.Data = getData.ValueModel;
            dataSets.BackgroundColor = "rgba(206, 129, 149, 0.8)";
            dataSets.BorderColor = "transparent";

            var listDataSets = new List<ChartPPMUpdateBarModel.DataSets>();
            listDataSets.Add(dataSets);

            var verticalBar = new ChartPPMUpdateBarModel.VerticalBar();
            verticalBar.Label = getData.Model;
            verticalBar.DataSets = listDataSets;

            var data = new ChartPPMUpdateBarModel.Data();
            data.VerticalBar = verticalBar;
            
            var statusNetwork = new ChartPPMUpdateBarModel.Status();
            statusNetwork.Code = 200;
            statusNetwork.Message = "Success";

            var responseJson = new ChartPPMUpdateBarModel.ResponseJson();
            responseJson.Data = data;
            responseJson.Status = statusNetwork;

            Response.ContentType = "application/json";
            Response.Write(JsonConvert.SerializeObject(responseJson));
            return new EmptyResult();
        }
    }
}