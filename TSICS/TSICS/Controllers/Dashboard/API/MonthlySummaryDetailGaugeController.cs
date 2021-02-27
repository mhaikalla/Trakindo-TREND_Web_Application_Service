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
        // GET: MonthlySummaryDetailGauge
        public ActionResult MonthlySummaryDetailGauge(string type, int m, int y)
        {
            var getCount = ticketBS.GetCountTicketDashboardMonthSubmitter(Convert.ToInt32(Session["userid"]), type, m, y);

            var backgroundColorValue = new List<string>();
            backgroundColorValue.Add("red");
            backgroundColorValue.Add("yellow");
            backgroundColorValue.Add("yellow");
            backgroundColorValue.Add("green");

            var gaugeLimits = new List<int>();
            gaugeLimits.Add(1);
            gaugeLimits.Add(2);
            gaugeLimits.Add(3);
            gaugeLimits.Add(4);
            gaugeLimits.Add(5);

            var data = new MonthlySummaryDetailGaugeModel.DoughnutGauge();
            data.Value = Convert.ToDecimal(getCount);
            data.BackgroundColor = backgroundColorValue;
            data.GaugeLimits = gaugeLimits;

            var dataValue = new MonthlySummaryDetailGaugeModel.Data();
            dataValue.DoughnutGauge = data;

            var status = new MonthlySummaryDetailGaugeModel.Status();
            status.Message = "Sukses";
            status.Code = 200;

            var responseJson = new MonthlySummaryDetailGaugeModel.ResponseJson();
            responseJson.Data = dataValue;
            responseJson.Status = status;

            Response.ContentType = "application/json";
            Response.Write(JsonConvert.SerializeObject(responseJson));
            return new EmptyResult();
        }
    }
}